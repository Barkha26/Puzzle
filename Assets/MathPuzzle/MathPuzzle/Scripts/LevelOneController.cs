using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneController : MonoBehaviour
{


	#region PRIVATE_VARIABLES


	public MarkerInfo markerInfoObject;
	private MarkerInfo markerObjectHold;
	private List <string> collideObjectList = new List<string> ();
	private float width = 1f;
	private float height = 1.399f;
	private int sameColorCount = 0;
	private string operatorSign = null;
	private string blueDigitHolder = null;
	private string greenDigitHolder = null;
	
	private bool isCollision = false;
	private bool isLevelActivated = false;
	//	private string lastPostionMarker = "ImageTarget_equal";

	#endregion

	#region Public_Variable

	public Text resultText;
	public int maxMarkerDetectLimit = 2;
	public Material redMaterial;
	public Material blueMaterial;
	public Material greenMaterial;
	public GameObject arCamera;
	public GameObject cube;
	[SerializeField]public List<MarkerInfo> markerlList = new List<MarkerInfo> ();

	#endregion




	#region  UNTIY_MONOBEHAVIOUR_METHODS

	// Use this for initialization
	void Start ()
	{
		
		//	DontDestroyOnLoad (this);
		CollisionDetection.onCollision += onCollision;
		Debug.Log ("width " + width);
		Vuforia.DefaultTrackableEventHandler.onImageTargetDetected += onImageTargetDetected;
	}

	void OnEnable ()
	{
		isLevelActivated = true;

	}
	// Update is called once per frame
	void Update ()
	{
		// when first card detect then its color change in blue
		if (markerlList.Count != 0) {
			SortList ();

			if (!isCollision) {
				
				for (int i = 0; i < markerlList.Count; i++) {
					
					if (i == 0) {
						markerlList [0].ModelObject.GetComponent<MeshRenderer> ().material = blueMaterial;
						//Debug.Log ("marker 0 " + markerlList [0].ImageTargetObject.transform.position);

					} else if (i == 1) {
						markerlList [1].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
						//Debug.Log ("marker 1 " + markerlList [1].ImageTargetObject.transform.position);

					} else {
						markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material = redMaterial;
						//Debug.Log ("marker  " + markerlList [i].ImageTargetObject.transform.position);
					}

				}
			}
			sameColorCount = 0;
			for (int j = 0; j < markerlList.Count - 1; j++) {

				for (int k = j; k < markerlList.Count - 1; k++) {

					#region marker overlap handling 
					if (markerlList [k + 1].ImageTargetObject != null) {
						if (markerlList [j].Name.Equals (markerlList [k + 1].Name)) {
							// in this condition when one or more marker/model place in same position. when model detected but we change the marker position.
							if (markerlList [j].ImageTargetObject.transform.localPosition.x >= markerlList [k + 1].ImageTargetObject.transform.localPosition.x) {
								if (-markerlList [k + 1].ImageTargetObject.transform.localPosition.x <= width / 1.5f) {
									markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = false;
									markerlList [k + 1].ObjectMeshVisible = false;
									markerlList.RemoveAt (k + 1);
									return;
									//Debug.Log ("two model place same position 1");
								}
							}
								//in this condition when one or more marker/model place in same position.when second model have large position as compare to one model
								else if (markerlList [k + 1].ImageTargetObject.transform.localPosition.x - markerlList [j].ImageTargetObject.transform.localPosition.x <= width / 1.5f) {
								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = false;
								markerlList [k + 1].ObjectMeshVisible = false;
								markerlList.RemoveAt (k + 1);
								return;
								//Debug.Log ("two model  place same position 2");
							}

								// when two model are separate 
								else {

								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = true;
								markerlList [k + 1].ObjectMeshVisible = true;
								//Debug.Log ("two model are separate");
							}

						}
						#endregion


					}
				}

				
			}

			// for calculation of app
			blueDigitHolder = null;
			greenDigitHolder = null;
			if (markerlList.Count > 1 && operatorSign != null) {
				for (int l = 0; l < markerlList.Count; l++) {
					if (markerlList [l].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("blue")) {
						blueDigitHolder = blueDigitHolder + FindDigit (markerlList [l].Name).ToString ();
						//Debug.Log (" blue value name " + blueDigitHolder);

					} else if (markerlList [l].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("green")) {
						greenDigitHolder = greenDigitHolder + FindDigit (markerlList [l].Name).ToString ();
						//Debug.Log (" green value name " + greenDigitHolder);
					}

					if (l == markerlList.Count - 1) {
						ResultCalculate (operatorSign);
					}

				}
			} else
				resultText.text = "";

		} 
	}

	#endregion

	#region EVENTS

	// subscribed with event in defaultTrackableEventHandler
	void onImageTargetDetected (GameObject arg0, bool isfound)
	{
		if (isLevelActivated) {
			if (isfound) {
				
				Debug.Log ("found object name " + arg0.name); 

				string[] name = arg0.name.Split ('_');
				Debug.Log ("name " + name [name.Length - 1]);
				if (!OperatorTypeCard (name [name.Length - 1])) {
					markerInfoObject = new MarkerInfo ();
					markerInfoObject.ImageTargetObject = arg0;
					markerInfoObject.Name = name [name.Length - 2] + "_" + name [name.Length - 1];
					markerInfoObject.ModelObject = arg0.transform.GetChild (0).gameObject.transform.GetChild (0).gameObject;
					markerInfoObject.ModelMaterialName = arg0.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material;
					markerInfoObject.ObjectMeshVisible = true;

					markerlList.Add (markerInfoObject);
				} else if (OperatorTypeCard (name [name.Length - 1])) {
					operatorSign = name [name.Length - 1];
				}


				if (markerlList.Count <= maxMarkerDetectLimit && markerlList.Count > 1) {
					int listCount = markerlList.Count;
					for (int i = 0; i < listCount - 1; i++) {
						//	Debug.Log ("value of i " + i + " marker count " + markerlList.Count);
						for (int j = i; j < listCount - 1; j++) {
							Debug.Log ("one marker name" + markerlList [i].ImageTargetObject.name + " another name " + markerlList [j + 1].ImageTargetObject.name);
							if (!markerlList [i].ImageTargetObject.name.Equals (markerlList [j + 1].ImageTargetObject.name)) {
								Debug.Log ("no repeated marker name" + markerlList [j].ImageTargetObject.name + " another name " + markerlList [j + 1].ImageTargetObject.name);
							} else {
								//	Debug.Log ("repeated marker name" + markerlList [j].ImageTargetObject.name);
								markerlList.Remove (markerlList.Find (item => item.ImageTargetObject.name == markerlList [i].ImageTargetObject.name));
								Debug.Log ("after remove list count " + markerlList.Count);
							}

						}
						if (i == markerlList.Count - 1 && markerlList.Count > 1)
							SortList ();
					}


				} 
			
				if (markerlList.Count <= 1) {
					isCollision = false;
				}


			} else {
				
				if (operatorSign != null && OperatorTypeCard (arg0.name.ToLower ())) {


					operatorSign = null;
					if (resultText != null)
						resultText.text = "";


				}
				if (markerlList.Count != 0) {

					int index = markerlList.FindIndex (item => item.ImageTargetObject.name == arg0.name); 

					if (index != -1) {
						markerlList [index].ModelObject.GetComponent<MeshRenderer> ().material = markerlList [index].ModelMaterialName;
						markerlList.RemoveAt (index);
					}
				}
				//GetComponent<SoundManager> ().stopAllMusic ();
				Debug.Log ("list count " + markerlList.Count);
			}
		}
	}

	#endregion

	public void SortList ()
	{

		if (markerlList != null) {

			markerlList.Sort (delegate(MarkerInfo x, MarkerInfo y) {
				if (x.ImageTargetObject.transform.localPosition == null && y.ImageTargetObject.transform.localPosition == null)
					return 0;
				else if (x.ImageTargetObject.transform.localPosition == null)
					return -1;
				else if (y.ImageTargetObject.transform.localPosition == null)
					return 1;
				else {
					return  Gyroscope.instance.MarkerPositionCalculate (x.ImageTargetObject).CompareTo (Gyroscope.instance.MarkerPositionCalculate (y.ImageTargetObject));
				} 
				y = x;
			});



		}

		//if (markerlList != null && Gyroscope.instance.cube.transform.position.x > 0)
		//	markerlList.Reverse ();

	}



	private int FindDigit (string digit)
	{
		string[] digitName = digit.Split ('_');
		string value = digitName [digitName.Length - 1].ToLower ();
		Debug.Log ("value is " + value);
		if (value.Contains ("zero")) {
			return 0;
		} else if (value.Contains ("one")) {
			return 1;
		} else if (value.Contains ("two")) {
			return 2;
		} else if (value.Contains ("three")) {
			return 3;
		} else if (value.Contains ("four")) {
			return 4;
		} else if (value.Contains ("five")) {
			return 5;
		} else if (value.Contains ("six")) {
			return 6;
		} else if (value.Contains ("seven")) {
			return 7;
		} else if (value.Contains ("eight")) {
			return 8;
		} else if (value.Contains ("nine")) {
			return 9;
		}
		return -1;

	}

	//  This method check detect card is digit or operator.
	private bool OperatorTypeCard (string cardName)
	{
		cardName = cardName.ToLower ();
		if (cardName.Contains ("multiply"))
			return true;
		else if (cardName.Contains ("add"))
			return true;
		else if (cardName.Contains ("subtract"))
			return true;
		else if (cardName.Contains ("divide"))
			return true;
		else if (cardName.Contains ("equal"))
			return true;

		return false;
	}

	private void ResultCalculate (string sign)
	{
		sign = sign.ToLower ();
		Debug.Log ("sign " + sign);
		if (blueDigitHolder != null && greenDigitHolder != null) {
			float result = 0;
			if (sign.Equals ("divide"))
				result = (float.Parse (blueDigitHolder) / float.Parse (greenDigitHolder));
			else if (sign.Equals ("multiply"))
				result = (float.Parse (blueDigitHolder) * float.Parse (greenDigitHolder));
			else if (sign.Equals ("add"))
				result = (float.Parse (blueDigitHolder) + float.Parse (greenDigitHolder));
			else if (sign.Equals ("subtract"))
				result = (float.Parse (blueDigitHolder) - float.Parse (greenDigitHolder));
			resultText.text = result.ToString ();
		} else {
			resultText.text = "";
		}
	}

	void onCollision (string name, bool status)
	{
		if (isLevelActivated) {
			Debug.Log (status);
			isCollision = status;
			MarkerInfo obj = markerlList.Find (item => item.ImageTargetObject.name == name); 
			if (status && obj != null) {
				obj.ImageTargetObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material = redMaterial;
			} 
		}
	}

	void OnDisable ()
	{
		isLevelActivated = false;
	}
}
