using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Movement Member Variables
    private const int NEGATIVE = -1;
    public Vector3 m_acceleration;
    public Vector3 m_velocity;
    public Vector3 m_position;
    public float m_maxSpeed;
    public float m_mass;
    public float m_seekWeight;
    public float m_fleeWeight;
    #endregion

    #region Movement Methods
    protected void AddForce(Vector3 force)
    {
        m_acceleration += force / m_mass;
    }

    protected void CalculateForces()
    {
        m_position = transform.position;
        m_velocity += m_acceleration * Time.deltaTime;
        m_position += m_velocity * Time.deltaTime;
        transform.position = m_position;

        m_acceleration = Vector3.zero;
    }

    protected void FleeNoVert(Vector3 position)
    {
        Vector3 desiredVelocity = position - m_position;

        desiredVelocity.Normalize();
        desiredVelocity *= m_maxSpeed * NEGATIVE;
        desiredVelocity.y = 0f;

        AddForce(desiredVelocity - m_velocity * (1 / Vector3.Distance(position, m_position)) * m_fleeWeight);
    }

    protected void SeekNoVert(Vector3 position)
    {
        Vector3 desiredVelocity = position - m_position;

        desiredVelocity.Normalize();
        desiredVelocity *= m_maxSpeed;
        desiredVelocity.y = 0f;

        AddForce(desiredVelocity - m_velocity * (1 / Vector3.Distance(position, m_position)) * m_seekWeight);
    }

    protected void Flee(Vector3 position)
    {
        Vector3 desiredVelocity = position - m_position;

        desiredVelocity.Normalize();
        desiredVelocity *= m_maxSpeed * NEGATIVE;

        AddForce(desiredVelocity - m_velocity * (1/Vector3.Distance(position, m_position)) * m_fleeWeight);
    }

    protected void Seek(Vector3 position)
    {
        Vector3 desiredVelocity = position - m_position;

        desiredVelocity.Normalize();
        desiredVelocity *= m_maxSpeed;

        AddForce(desiredVelocity - m_velocity * (1 / Vector3.Distance(position, m_position)) * m_seekWeight);
    }

    protected void Friction()
    {

    }

    protected void ClampAngle(float angle)
    {
        transform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(transform.eulerAngles.z, -angle, angle));
    }
    #endregion
}
