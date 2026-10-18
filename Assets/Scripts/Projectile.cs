using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileParams m_params;
    [SerializeField] private float m_raycastLength = 0.5f;
    [SerializeField] private float m_raycastRadius = 0.15f;
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private ParticleDetach m_smokeDetach;
    [SerializeField] private ParticleDetach m_explosionDetach;
    [SerializeField] private AudioSource m_explosionAudio;
    public Teams m_targetTeam;
    private Transform m_transform;
    private int m_numTimesBounced = 0;
    private Ray m_ray = new();
    private RaycastHit[] m_hits = new RaycastHit[10];

    private void Start()
    {
        m_transform = transform;        
    }

    private void FixedUpdate()
    {
        m_rb.MovePosition(m_transform.position +
            m_transform.forward * m_params.ForwardVelocity);
    }

    private void Update()
    {
        m_ray.origin = m_transform.position;
        m_ray.direction = m_transform.forward;
        int numHits = Physics.SphereCastNonAlloc(
            ray: m_ray,
            radius: m_raycastRadius,
            results: m_hits,
            maxDistance: m_raycastLength,
            layerMask: m_layerMask);
        
        if (numHits > 0)
            OnCollision(numHits);        
    }

    private void OnCollision(int numHits)
    {
        bool bounce = false;
        Vector3 bounceNormal = Vector3.zero;
        GameObject self = gameObject;
        for (int i = 0; i < numHits; i++)
        {
            GameObject collider = m_hits[i].collider.gameObject;

            if (collider == null || collider == self)
            {
                continue;
            }

            if (collider.TryGetComponent(out Projectile other))
            {
                other.Explode();
                Explode();
                return;
            }

            if (collider.TryGetComponent(out Team teamComponent))
            {
                if (teamComponent.GetTeam() == m_targetTeam)
                {
                    if (collider.TryGetComponent(out Health healthComponent))
                    {
                        healthComponent.ChangeHealth(-1);
                        Explode();
                        return;
                    }
                }
            }

            bounce = true;
            bounceNormal = m_hits[i].normal;
        }

        if (!bounce)        
            return;
        

        // If not a valid target, bounce.

        if (m_numTimesBounced >= m_params.NumBounces)
        {
            Explode();
            return;
        }

        ++m_numTimesBounced;
        Vector3 newDirection = Vector3.Reflect(
            m_transform.forward, bounceNormal);
        m_transform.rotation = Quaternion.LookRotation(
            newDirection.normalized);
    }

    public void Explode()
    {
        SFXPlayer.Play(m_explosionAudio);
        DetachParticles();
        Destroy(gameObject);
    }

    private void DetachParticles()
    {
        m_smokeDetach.StopAndDetach();
        m_explosionDetach.StartAndDetach();
    }
}
