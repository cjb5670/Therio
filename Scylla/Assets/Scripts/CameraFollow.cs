using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject player;
	public Vector2 offset;

	float properZ;
	float properY;
	float properX;

	private Vector2 playerOldPos;
	private Vector2 playerNewPos;
	// Use this for initialization
	void Start()
	{
		playerOldPos = playerPos2D();
		properZ = transform.position.z;
		properY = transform.position.y - offset.y;

	}

	// Update is called once per frame
	void Update()
	{
		playerNewPos = playerPos2D();
		if (player.transform.position.x - offset.x < 0)
		{
			// do nothing
		}
		else
		{
			properX = playerPos2D().x + (playerNewPos.x - playerOldPos.x) + offset.x;
		}

		transform.position = new Vector3(properX, properY, properZ);

		playerOldPos = playerPos2D();
	}

	Vector2 playerPos2D ()
	{
		return new Vector2(player.transform.position.x, player.transform.position.y);
	}
}
