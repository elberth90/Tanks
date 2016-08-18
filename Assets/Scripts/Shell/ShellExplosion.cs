using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
		Collider[] colliders = Physics.OverlapSphere (transform.position, this.m_ExplosionRadius, this.m_TankMask);
		for (int i = 0; i < colliders.Length; i++) {
			Rigidbody targetRigidbody = colliders [i].GetComponent<Rigidbody> ();
			if (!targetRigidbody) {
				continue;
			}
			targetRigidbody.AddExplosionForce (this.m_ExplosionForce, transform.position, this.m_ExplosionRadius);

			TankHealth targetHealth = colliders [i].GetComponent<TankHealth> ();
			if (!targetHealth) {
				continue;
			}

			float damage = this.CalculateDamage (targetHealth.transform.position);
			targetHealth.TakeDamage (damage);
		}

		this.m_ExplosionParticles.transform.parent = null;
		this.m_ExplosionParticles.Play ();
		this.m_ExplosionAudio.Play ();

		Destroy (this.m_ExplosionParticles.gameObject, this.m_ExplosionParticles.duration);
		Destroy (gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
		Vector3 explosionToTarget = targetPosition - transform.position;
		float explosionDistance = explosionToTarget.magnitude;
		float relativeDistance = (this.m_ExplosionRadius - explosionDistance) / this.m_ExplosionRadius;
		float damage =  relativeDistance * this.m_MaxDamage;
		return Mathf.Max (0f, damage);
    }
}