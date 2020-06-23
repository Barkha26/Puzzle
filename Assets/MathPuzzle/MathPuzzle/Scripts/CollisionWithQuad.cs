using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWithQuad : MonoBehaviour
{

	public delegate void OnCollisionAction (string name,bool status);

	public static event OnCollisionAction onCollisionWithQuad;

	public bool isQuadCollide = false;
	private  bool isTriggerStayCalled = false;
	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void LateUpdate ()
	{
		isTriggerStayCalled = false;
		//InvokeRepeating ("CheckTriggerStayCalled", 0.5f, 0.5f);
	}

	void CheckTriggerStayCalled ()
	{
		if (onCollisionWithQuad != null && !isTriggerStayCalled)
			onCollisionWithQuad (null, false);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.transform.name != null) {
			Debug.Log ("name of trigger method " + col.transform.name);
			if (onCollisionWithQuad != null) {
				
				onCollisionWithQuad (col.transform.name, true);
			}
		}
	}

	void OnTriggerExit (Collider col)
	{
		if (col.transform.name != null) {
			Debug.Log ("name of trigger method exit  " + col.transform.name);
			if (onCollisionWithQuad != null) {
				isQuadCollide = false;
				onCollisionWithQuad (col.transform.name, false);
			}
		}
	}



	void OnTriggerStay (Collider col)
	{
		if (col.transform.name != null) {

			if (onCollisionWithQuad != null) {
				isTriggerStayCalled = true;
				isQuadCollide = true;
				onCollisionWithQuad (col.transform.name, isQuadCollide);
			} else {
				isQuadCollide = false;
			}
		} else {
			isQuadCollide = false;
			onCollisionWithQuad (col.transform.name, isQuadCollide);
		}
	}
}
