using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        
    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    public GameObject m_ExplosionPrefab;
    
    private AudioSource m_ExplosionAudio;          
    private ParticleSystem m_ExplosionParticles;   
    private float m_CurrentHealth;  
    private bool m_Dead;            


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
		this.m_CurrentHealth -= amount;
		this.SetHealthUI ();
		if (this.m_CurrentHealth <= 0f && !this.m_Dead) {
			this.OnDeath ();
		}
    }


    private void SetHealthUI()
    {
		this.m_Slider.value = this.m_CurrentHealth;
		this.m_FillImage.color = Color.Lerp (this.m_ZeroHealthColor, this.m_FullHealthColor, this.m_CurrentHealth / this.m_StartingHealth);
    }


    private void OnDeath()
    {
		this.m_Dead = true;

		this.m_ExplosionParticles.transform.position = transform.position;
		this.m_ExplosionParticles.gameObject.SetActive (true);
		this.m_ExplosionParticles.Play ();
		this.m_ExplosionAudio.Play ();

		gameObject.SetActive (false);
    }
}