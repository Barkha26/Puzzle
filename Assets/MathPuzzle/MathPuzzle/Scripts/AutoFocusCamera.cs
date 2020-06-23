using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class AutoFocusCamera : MonoBehaviour
{
	void Start()
	{
		StartCoroutine ("ContinuesFocus");
	}
	IEnumerator ContinuesFocus() {
		yield return new WaitForSeconds (1.0f);
		CameraDevice.Instance.SetFocusMode (
			CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
	}

}
