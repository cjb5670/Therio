using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTypes
{
    #region WaveTypes Methods
    public static float SinXWave(
        Vector3 position,
        float speed,
        float scale,
        float waveDistance,
        float noiseStrength,
        float noiseWalk,
        float timeSinceStart)
    {
        float x = position.x;
        float z = position.z;
        float y = 0f;

        // Using only x or z will produce straight waves
        // Using only y will produce an up/down movement
        // x + y + z rolling waves
        // x * z produces a moving sea without rolling waves
        float waveType = z;

        y += Mathf.Sin((timeSinceStart * speed + waveType) / waveDistance) * scale;
        y += Mathf.PerlinNoise(x + noiseWalk, y + Mathf.Sin(timeSinceStart * 0.1f)) * noiseStrength;
        return y; 
    }
    #endregion
}
