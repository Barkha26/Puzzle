using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithPlane : MonoBehaviour
{

	public delegate void OnCollisionAction (string name,bool status);

	public static event OnCollisionAction onCollisionWithPlane;

	public bool isPlaneCollide = false;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}


	void OnTriggerEnter (Collider col)
	{
		if (col.transform.name != null) {
			//	Debug.Log ("name of trigger method " + col.transform.name);
			if (onCollisionWithPlane != null) {
				onCollisionWithPlane (col.transform.name, true);
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.transform.name != null) {
			//	Debug.Log ("name of trigger method exit  " + col.transform.name);
			if (onCollisionWithPlane != null) {
				isPlaneCollide = false;
				onCollisionWithPlane (col.transform.name, false);
			}
		}
	}



	void OnTriggerStay (Collider col)
	{
		if (col.transform.name != null) {
			//	Debug.Log ("collsion stay called " + col.gameObject.name);
			if (onCollisionWithPlane != null) {
				isPlaneCollide = true;
				onCollisionWithPlane (col.transform.name, true);
			} else {
				isPlaneCollide = false;
			}
		} else {
			isPlaneCollide = false;
			onCollisionWithPlane (col.transform.name, false);
		}
	}
}
