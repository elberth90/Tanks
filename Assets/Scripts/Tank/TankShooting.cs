using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;    
    public Slider m_AimSlider;           
    public AudioSource m_ShootingAudio;  
    public AudioClip m_ChargingClip;     
    public AudioClip m_FireClip;         
    public float m_MinLaunchForce = 15f; 
    public float m_MaxLaunchForce = 30f; 
    public float m_MaxChargeTime = 0.75f;
	public float firePushPower = 100f;
	public float reloadTime = 1f;

    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;
	private float lastShot;


    private void OnEnable()
    {
        this.m_CurrentLaunchForce = this.m_MinLaunchForce;
        this.m_AimSlider.value = this.m_MinLaunchForce;
    }


    private void Start()
    {
        this.m_FireButton = "Fire" + m_PlayerNumber;

        this.m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
		if (!this.CanShoot ()) {
			return;
		}

		this.m_AimSlider.value = this.m_MinLaunchForce;

		if (this.m_CurrentLaunchForce >= this.m_MaxLaunchForce && !this.m_Fired && Input.GetButtonUp (this.m_FireButton)) {
			this.m_CurrentLaunchForce = this.m_MaxLaunchForce;
			this.m_AimSlider.value = this.m_CurrentLaunchForce;
			this.Fire ();
		} else if (Input.GetButtonDown (this.m_FireButton)) {
			this.m_Fired = false;
			this.m_CurrentLaunchForce = this.m_MinLaunchForce;
			this.m_ShootingAudio.clip = this.m_ChargingClip;
			this.m_ShootingAudio.Play ();
		} else if (Input.GetButton (this.m_FireButton) && !this.m_Fired) {
			this.m_CurrentLaunchForce += this.m_ChargeSpeed * Time.deltaTime;
			this.m_AimSlider.value = this.m_CurrentLaunchForce;
		} else if (Input.GetButtonUp (this.m_FireButton) && !this.m_Fired) {
			this.Fire ();
		}

    }

	private bool CanShoot()
	{
		if (this.lastShot == null)
			return true;
		return (Time.time - this.reloadTime > this.lastShot);
	}


    private void Fire()
    {
		this.m_Fired = true;
		Rigidbody shellInstance = Instantiate (this.m_Shell, this.m_FireTransform.position, this.m_FireTransform.rotation) as Rigidbody;
		shellInstance.velocity = this.m_CurrentLaunchForce * this.m_FireTransform.forward;
		this.m_ShootingAudio.clip = this.m_FireClip;
		this.m_ShootingAudio.Play ();
		this.m_CurrentLaunchForce = this.m_MinLaunchForce;
		this.lastShot = Time.time;
		Rigidbody tankRigidbody = this.gameObject.GetComponent<Rigidbody>();
	}
}