using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1: MonoBehaviour
{


	public int x = 4;
	public int y = 1;
	int count = 0;
	public int[] array;
	public int xHolder = 0;


	// Use this for initialization
	void Start ()
	{
		


		for (int i = 0; i < array.Length; i++) {
			//count = 0;
			Calculation (i);
		}

	}
	// Update is called once per frame
	void Update ()
	{
		
	}

	void Calculation (int index)
	{

		if (array [index] <= x) {
			count++;
			Debug.Log (" count " + count);
			return;
		} else {
			xHolder = x - y;
			array [index] = array [index] - xHolder;
			count++;
			Calculation (index);
		}

	}
}
