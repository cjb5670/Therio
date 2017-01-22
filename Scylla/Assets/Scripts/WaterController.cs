using UnityEngine;

public class WaterController : MonoBehaviour
{
    #region WaterController Member Variables
    public static WaterController current;
    public bool m_isMoving;
    public float m_scale = 0.1f;
    public float m_speed = 1.0f;
    public float m_waveDistance = 1f;
    public float m_noiseStrength = 1f;
    public float m_noiseWalk = 1f;
    #endregion

    #region WaterController Methods
    void Start()
    {
        current = this;
    }

    void Update()
    {
        Shader.SetGlobalFloat("_WaterScale", m_scale);
        Shader.SetGlobalFloat("_WaterSpeed", m_speed);
        Shader.SetGlobalFloat("_WaterDistance", m_waveDistance);
        Shader.SetGlobalFloat("_WaterTime", Time.time);
        Shader.SetGlobalFloat("_WaterNoiseStrength", m_noiseStrength);
        Shader.SetGlobalFloat("_WaterNoiseWalk", m_noiseWalk);
    }

    public float GetWaveYPos(Vector3 position, float timeSinceStart)
    {
        /*
        if (isMoving)
        {
            return WaveTypes.SinXWave(position, m_speed, m_scale, m_waveDistance, m_noiseStrength, m_noiseWalk, timeSinceStart);
        }
        else
        {
            return 0f;
        }
        */

        return 0f;
    }

    // Find the distance from a vertice to water
    // Make sure the position is in global coordinates
    // Positive if above water
    // Negative if below water
    public float DistanceToWater(Vector3 position, float timeSinceStart)
    {
        float waterHeight = GetWaveYPos(position, timeSinceStart);
        return position.y - waterHeight;
    }
    #endregion
}