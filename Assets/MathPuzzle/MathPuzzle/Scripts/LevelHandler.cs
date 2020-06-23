using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
	
	// Use this for initialization
	void OnEnable ()
	{
		LoadLevel (MainHandler.instance.levelIndex);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void LoadLevel (int index)
	{

		if (index == 1) {
			gameObject.transform.GetChild (0).gameObject.SetActive (true);
			gameObject.transform.GetChild (1).gameObject.SetActive (false);
			gameObject.transform.GetChild (2).gameObject.SetActive (false);
			gameObject.transform.GetChild (3).gameObject.SetActive (false);
		} else if (index == 2) {
			gameObject.transform.GetChild (0).gameObject.SetActive (false);
			gameObject.transform.GetChild (1).gameObject.SetActive (true);
			gameObject.transform.GetChild (2).gameObject.SetActive (false);
			gameObject.transform.GetChild (3).gameObject.SetActive (false);

		} else if (index == 3) {
			gameObject.transform.GetChild (0).gameObject.SetActive (false);
			gameObject.transform.GetChild (1).gameObject.SetActive (false);
			gameObject.transform.GetChild (2).gameObject.SetActive (true);
			gameObject.transform.GetChild (3).gameObject.SetActive (false);
		} else if (index == 4) {
			gameObject.transform.GetChild (0).gameObject.SetActive (false);
			gameObject.transform.GetChild (1).gameObject.SetActive (false);
			gameObject.transform.GetChild (2).gameObject.SetActive (false);
			gameObject.transform.GetChild (3).gameObject.SetActive (true);
		}

//		if (index == 1) {
//			GetComponent<LevelOneController> ().enabled = true;
//			GetComponent<LevelTwoController> ().enabled = false;
//			GetComponent<LevelThreeController> ().enabled = false;
//			GetComponent<LevelFourController> ().enabled = false;
//		} else if (index == 2) {
//			GetComponent<LevelTwoController> ().enabled = true;
//			GetComponent<LevelOneController> ().enabled = false;
//			GetComponent<LevelThreeController> ().enabled = false;
//			GetComponent<LevelFourController> ().enabled = false;
//
//		} else if (index == 3) {
//			GetComponent<LevelThreeController> ().enabled = true;
//			GetComponent<LevelTwoController> ().enabled = false;
//			GetComponent<LevelOneController> ().enabled = false;
//			GetComponent<LevelFourController> ().enabled = false;
//		} else if (index == 4) {
//			GetComponent<LevelFourController> ().enabled = true;
//			GetComponent<LevelThreeController> ().enabled = false;
//			GetComponent<LevelTwoController> ().enabled = false;
//			GetComponent<LevelOneController> ().enabled = false;
//		}
	}


}
