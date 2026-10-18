using UnityEngine;

public struct TankTurretInput
{
    public float Turning;
    public bool Shooting;
}

public class TankTurret : MonoBehaviour
{
    [SerializeField] private TankController m_controller;
    [SerializeField] private Transform m_projectileSpawnTransform;
    [SerializeField] private TankTurretParameters m_params;
    [SerializeField] private Projectile m_projectilePrefab;
    [SerializeField] private AudioSource m_shootAudio;
    [HideInInspector] public TankTurretInput m_input = new();
    private Transform m_transform;

    private float m_shootingCooldown = 0;

    public Transform GetProjectileSpawn()
    {
        return m_projectileSpawnTransform;
    }

    void Start()
    {
        m_transform = transform;
    }

    private void Update()
    {
        if (!Level.m_gameStarted)
            return;

        if (m_shootingCooldown < m_params.ReloadSpeed)
            m_shootingCooldown += Time.deltaTime;
    }

    public float GetNormalizedReloadTime()
    {
        return m_shootingCooldown / m_params.ReloadSpeed;
    }

    private void FixedUpdate()
    {
        float turningInput = Mathf.Clamp(m_input.Turning, -1, 1);
        float rotation = turningInput * m_params.TurnSpeed;
        m_transform.Rotate(m_transform.up, rotation * Time.fixedDeltaTime);

        if (m_input.Shooting)
            TryShoot();
    }

    private void TryShoot()
    {
        if (m_shootingCooldown >= m_params.ReloadSpeed)
        {
            Projectile projectile = Instantiate(m_projectilePrefab,
                m_projectileSpawnTransform.position,
                m_projectileSpawnTransform.rotation);
            m_shootingCooldown = 0;
            SFXPlayer.Play(m_shootAudio);
        }
    }
}
