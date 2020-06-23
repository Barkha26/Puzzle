using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Gyroscope : MonoBehaviour
{

	public GameObject plane, cube, rotateCamera;
	public static Gyroscope instance;
	// Use this for initialization
	void Start ()
	{
		instance = this;
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (plane != null)
			plane.transform.rotation = rotateCamera.transform.rotation;

		//plane.transform.rotation = Axis (Input.gyro.attitude);
		

	}

	Quaternion Axis (Quaternion q)
	{
		return new Quaternion (q.x, q.y, -q.z, -q.w);
	}


	public  float value = 0;

	public  float  MarkerPositionCalculate (GameObject obj)
	{

		if (obj != null) {
			value = Vector3.Distance (cube.transform.position, obj.transform.position);
			//value = (cube.transform.position.x) - obj.transform.position.x;

		}

		return value;
	}

	public	void Cube ()
	{
		Debug.Log ("cuvbe value " + cube.transform.position);
	}

}
