using UnityEngine;

public struct TankInput
{
    public float Forward;
    public float Turning;
}

public class Tank : MonoBehaviour
{
    [HideInInspector] public TankInput m_input = new();
    [SerializeField] private Rigidbody m_rb;
    [SerializeField] private TankParameters m_params;
    [SerializeField] private TankTurret m_turret;
    [SerializeField] private GameObject m_targetingObject;
    [SerializeField] private GameObject m_wrapper = null;
    private Transform m_transform;
    private Transform m_targetingObjectTransform = null;

    private void Start()
    {
        m_transform = transform;
        if (m_targetingObject != null)
            m_targetingObjectTransform = m_targetingObject.transform;
    }

    private void FixedUpdate()
    {
        float forwardInput = Mathf.Clamp(m_input.Forward, -1, 1);
        Vector3 force = forwardInput * m_params.Speed * m_transform.forward;
        m_rb.AddForce(force * Time.fixedDeltaTime, ForceMode.Force);
        
        float turningInput = Mathf.Clamp(m_input.Turning, -1, 1);
        Vector3 torque = turningInput * m_params.TurnSpeed * m_transform.up;
        m_rb.AddTorque(torque * Time.fixedDeltaTime, ForceMode.Force);
    }

    public void StopAngularVelocity()
    {
        m_rb.angularVelocity = Vector3.zero;
    }

    public void SetTargetingObjectPos(float secondsInFuture)
    {
        if (m_targetingObjectTransform == null)
            return;

        m_targetingObjectTransform.position =
            m_transform.position + (m_rb.linearVelocity * secondsInFuture);
    }

    public GameObject GetTargetingObject()
    {
        return m_targetingObject;
    }

    private void OnDestroy()
    {
        if (m_wrapper != null)
            Destroy(m_wrapper);
    }

    public void TankSetActive(bool state)
    {
        if (m_wrapper != null)
            m_wrapper.SetActive(state);
        else
            gameObject.SetActive(state);
    }
}
