using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chatterGen : MonoBehaviour
{
	static bool AudioBegin = false;
	public AudioClip[] idleChatter;
	public float volume;
	int i;

	private AudioSource source;

	// Use this for initialization
	void Start()
	{
		source = GetComponent<AudioSource>();
		
		if (!AudioBegin)
		{
			DontDestroyOnLoad(gameObject);
			AudioBegin = true;
		}
		 i = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (SceneManager.GetActiveScene().name == "Scylla")
		{
			source.Stop();
		}

		if (!source.isPlaying) 
		{
			source.PlayOneShot(idleChatter[i], volume);
			if (i == 3)
			{
				i = 0;
			}
			else i++;	
		}
	}
}