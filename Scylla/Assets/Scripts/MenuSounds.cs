
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSounds : MonoBehaviour
{
	static bool AudioBegin = false;
	private AudioSource source;

	// Use this for initialization
	void Awake()
	{
		source = GetComponent<AudioSource>();

		if (!AudioBegin)
		{
			source.Play();
			DontDestroyOnLoad(gameObject);
			AudioBegin = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (SceneManager.GetActiveScene().name == "Scylla")
		{
			source.Stop();
			AudioBegin = false;
		}
	}
}