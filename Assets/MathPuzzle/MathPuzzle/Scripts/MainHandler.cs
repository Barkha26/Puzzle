using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHandler : MonoBehaviour
{
	public int levelIndex = 0;
	public static MainHandler instance;
	// Use this for initialization
	void Start ()
	{
		//DontDestroyOnLoad (this);
		instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void LevelOne ()
	{
		levelIndex = 1;
		Invoke ("SceneLoad", 0.5f);
	}

	public void LevelTwo ()
	{
		levelIndex = 2;
		Invoke ("SceneLoad", 0.5f);
	}

	public void LevelThree ()
	{
		levelIndex = 3;
		Invoke ("SceneLoad", 0.5f);
	}

	public void LevelFour ()
	{
		levelIndex = 4;
		Invoke ("SceneLoad", 0.5f);
	}

	void SceneLoad ()
	{
		SceneManager.LoadScene (1);
	}
}
