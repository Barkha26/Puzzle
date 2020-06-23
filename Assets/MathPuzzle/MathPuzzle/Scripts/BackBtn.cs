using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBtn : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		//DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void BackButtonClick ()
	{
		SceneManager.UnloadSceneAsync (1);
		SceneManager.LoadScene (0);

	}
}
