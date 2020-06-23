using UnityEngine;
using System.Collections;
using Vuforia;

public class ImageTargetPoints : MonoBehaviour
{

	private ImageTargetBehaviour mImageTargetBehaviour = null;

	// Use this for initialization
	void Start ()
	{
		// We retrieve the ImageTargetBehaviour component
		// Note: This only works if this script is attached to an ImageTarget
		mImageTargetBehaviour = GetComponent<ImageTargetBehaviour> ();

		if (mImageTargetBehaviour == null) {
			Debug.Log ("ImageTargetBehaviour not found ");
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (mImageTargetBehaviour == null) {
			Debug.Log ("ImageTargetBehaviour not found");
			return;
		} else {
	
			Vector2 targetSize = mImageTargetBehaviour.GetSize ();
			float targetAspect = targetSize.x / targetSize.y;

			// We define a point in the target local reference 
			// we take the bottom-left corner of the target, 
			// just as an example
			// Note: the target reference plane in Unity is X-Z, 
			// while Y is the normal direction to the target plane
			Vector3 pointOnTarget = new Vector3 (-0.5f, 0, -0.5f / targetAspect); 

			// We convert the local point to world coordinates
			Vector3 targetPointInWorldRef = transform.TransformPoint (pointOnTarget);

			// We project the world coordinates to screen coords (pixels)
			Vector3 screenPoint = Camera.main.WorldToScreenPoint (targetPointInWorldRef);

			Debug.Log ("target point in screen coords: " + screenPoint.x + ", " + screenPoint.y + " name " + mImageTargetBehaviour.name);
		}
	}
}
