using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSet : MonoBehaviour
{
	// int that is the average y value we want the terrain at MUST BE BETWEEN 0 AND 1
	public float terrainAverage;

	// int that determines how far down the path until another spike.
	public int distBetweenPoints;

	// float for low angle min/max value MUST BE BETWEEN 0 AND 1
	public float lowAngleDif;

	// high angle min/max saved as from 0 to average
	public float highAngleDif;

	// diminishing value static value
	public float diminshingValue;
	// diminishing value using value
	public float currentDiminishing;
	// diminishing value decrease rate
	public float diminshingDecrease;

	// Array of heights
	public float[,] heights;

	// List of KeyPoints
	List<float> KeyPoints = new List<float>();

	// Use this for initialization
	void Start()
	{
		ResetHeights();
        
		// Saves heights
		heights = Terrain.activeTerrain.terrainData.GetHeights(0, 0, Terrain.activeTerrain.terrainData.heightmapWidth, Terrain.activeTerrain.terrainData.heightmapHeight);
        
		// finds the larger angle
		if (terrainAverage <= 0.5)
		{
			highAngleDif = terrainAverage * 2;
		}
		else
		{
			highAngleDif = 1 - terrainAverage;
		}

		// Y value at position 0 is 1 unit above the average.
		for (int i = 1; i < Terrain.activeTerrain.terrainData.heightmapHeight; i++)
		{
			heights[i - 1, 0] = terrainAverage + 0.1f;                              // HEIGHTMAP ADJUSTMENT
		}

		// While between 1 and the maximum x value of the terrain
		for (int i = 1; i * distBetweenPoints < Terrain.activeTerrain.terrainData.heightmapWidth; i++)
		{
			SetKeypoints(currentDiminishing, i);
		}

		// to each point, add it's position in the list * slope to heights.
		SetBetweenPoints();

		// Bring the point closest to the camera to 0
		BringDownFront();

		Terrain.activeTerrain.terrainData.SetHeights(0, 0, heights);

        var c = this.GetComponent<PolygonCollider2D>();

        var test = new Vector2[KeyPoints.Count];
        for (int i = 1; i < KeyPoints.Count; i++)
        {
            test[i] = new Vector2(i,  KeyPoints[i] * 10);
        }
        c.points = test;
    }

    // Update is called once per frame
    void Update()
	{

	}

	void SetKeypoints(float currentChance, int loopValue)
	{
		if (ChnCh(currentChance))
		{
			SetLowAngle(loopValue);
			if (currentDiminishing - diminshingDecrease > 0)
			{
				currentDiminishing -= diminshingDecrease;
			}
		}
		else
		{
			SetHighAngle(loopValue);
			currentDiminishing = diminshingValue;
		}
	}

	void SetBetweenPoints()
	{
		for (int j = 1; j < KeyPoints.Count; j++)
		{
			// Find the slope between the keypoint and the one before it
			float slope = (KeyPoints[j] - KeyPoints[j - 1]) / distBetweenPoints;

			// Change each point between the keypoints to an increase of [first keypoint] + (slope * iteration)
			for (int k = 1; k < distBetweenPoints; k ++)
			{
				for (int m = 1; m < Terrain.activeTerrain.terrainData.heightmapHeight; m++)
				{
					heights[m, (j - 1) * distBetweenPoints + k] =                                       // HEIGHTMAP EDITING
						KeyPoints[j - 1] + (slope * k);
				}

				// heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, (j - 1) * distBetweenPoints + k] =
				//	   KeyPoints[j - 1] + (slope * k);

			}
		}
	}

	void SetLowAngle(int loopValue)
	{
		// get the point down the x track that we are currently at and find a new low angle height
		float prevPosition = heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, (loopValue - 1) * distBetweenPoints];
		float CalcRandom = Random.Range(prevPosition - lowAngleDif, prevPosition + lowAngleDif);
		// find a new height
		for (int i = 1; i < Terrain.activeTerrain.terrainData.heightmapHeight; i++)
		{
			heights[i, loopValue * distBetweenPoints] = CalcRandom;                     // HEIGHTMAP EDITING
		}
		// heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, loopValue * distBetweenPoints] = CalcRandom;

		KeyPoints.Add(heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, loopValue * distBetweenPoints]);
	}

	void SetHighAngle(int loopValue)
	{
		// if previous point is below the average
		if (heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, (loopValue - 1) * distBetweenPoints] <= terrainAverage)
		{
			float CalcRandom = Random.Range(terrainAverage, highAngleDif);
			// Set height to a value between the average and the max height.
			for (int i = 1; i < Terrain.activeTerrain.terrainData.heightmapHeight; i++)
			{
				heights[i, loopValue * distBetweenPoints] = CalcRandom;                 // HEIGHTMAP EDITING
			}
			// heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, loopValue * distBetweenPoints] = CalcRandom;


			KeyPoints.Add(heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, loopValue * distBetweenPoints]);
		}

		else
		{
			float CalcRandom = Random.Range(0, terrainAverage);
			// Set height to a value between the lowest value and the average.
			//for (int i = 1; i < Terrain.activeTerrain.terrainData.heightmapHeight; i++)
			//{
			//	heights[i, loopValue * distBetweenPoints] = CalcRandom;		// HEIGHTMAP EDITING
			//}
			heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, loopValue * distBetweenPoints] = CalcRandom;

			KeyPoints.Add(heights[Terrain.activeTerrain.terrainData.heightmapHeight - 1, loopValue * distBetweenPoints]);
		}
	}

	/// <summary>
	/// Generates a number between 1 and 100 and sees if it falls BELOW the given number
	/// </summary>
	/// <param name="chance">0 = will never happen 100= will always happen</param>
	/// <returns>true if below given number</returns>
	bool ChnCh(float chance)
	{
		float calculatedChance = Random.Range(0f, 100f);
		return (calculatedChance <= chance);
	}

	void ResetHeights()
	{
		// Starts currentDiminishing at max value
		currentDiminishing = diminshingValue;

		heights = new float[Terrain.activeTerrain.terrainData.heightmapWidth, Terrain.activeTerrain.terrainData.heightmapHeight];
		for (int i = 0; i < Terrain.activeTerrain.terrainData.heightmapWidth; i++)
		{
			for (int j = 0; j < Terrain.activeTerrain.terrainData.heightmapHeight; j++)
			{
				heights[i, j] = 0;
			}
		}
		Terrain.activeTerrain.terrainData.SetHeights(0, 0, heights);
	}

	void BringDownFront()
	{
		for (int i = 0; i < Terrain.activeTerrain.terrainData.heightmapWidth; i++)
		{
			heights[0, i] = 0;
		}
	}

}

// Distance between points seems to be working off of total length of the terrain
