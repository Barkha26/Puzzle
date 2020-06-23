using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
	public delegate void OnCollisionAction (string name,bool status);

	public static event OnCollisionAction onCollision;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
//		if (onCollision != null)
//			onCollision (null, false);
	}


	void OnTriggerEnter (Collider col)
	{
		if (col.transform.name != null) {
			//	Debug.Log ("name of trigger method " + col.transform.name);
			if (onCollision != null) {
				onCollision (col.transform.name, true);
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.transform.name != null) {
			//	Debug.Log ("name of trigger method exit  " + col.transform.name);
			if (onCollision != null) {
				onCollision (col.transform.name, false);
			}
		}
	}

	void OnTriggerStay (Collider col)
	{
		if (col.transform.name != null) {
			Debug.Log ("collsion stay called " + col.gameObject.name);
			if (onCollision != null) {
				onCollision (col.transform.name, true);
			}
		} 
	}
}
