using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDemo : MonoBehaviour
{

	//	private Camera cam;
	//	private Plane[] planes;
	//
	//	void Start ()
	//	{
	//		cam = Camera.main;
	//		planes = GeometryUtility.CalculateFrustumPlanes (cam);
	//		Debug.Log (cam.GetComponent<Camera> ().projectionMatrix);
	//	}
	//
	//	void Update ()
	//	{
	//	}
	//
	//	void OnBecameVisible ()
	//	{
	//		enabled = true;
	//		Debug.Log ("visible object");
	//	}


	public GameObject anObject;
	private Collider anObjCollider;
	private Camera cam;
	private Plane[] planes;

	void Start ()
	{
		cam = Camera.main;
		planes = GeometryUtility.CalculateFrustumPlanes (cam);
		anObjCollider = GetComponent<Collider> ();
		for (int i = 0; i < planes.Length; i++) {
			GameObject p = GameObject.CreatePrimitive (PrimitiveType.Plane);
			p.name = "Plane " + i.ToString ();
			p.transform.position = -planes [i].normal * planes [i].distance;
			p.transform.rotation = Quaternion.FromToRotation (Vector3.up, planes [i].normal);
		}
	}

	void Update ()
	{
//		if (GeometryUtility.TestPlanesAABB (planes, anObjCollider.bounds))
//			Debug.Log (anObject.name + " has been detected!");
//		else
//			Debug.Log ("Nothing has been detected");
	}

	void OnDrawGizmosSelected ()
	{
		Camera camera = GetComponent<Camera> ();
		Vector3 p = camera.ViewportToWorldPoint (new Vector3 (1, 1, camera.nearClipPlane));
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (p, 0.1F);
	}


}
