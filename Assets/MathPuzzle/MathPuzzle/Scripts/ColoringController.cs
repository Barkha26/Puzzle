//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//
//
//[System.Serializable]
//public class MarkerInfo
//{
//	private string name = null;
//	//private Vector3 postion = Vector3.zero;
//	//private int count = 0;
//	private GameObject obj = null;
//	private GameObject modelObj = null;
//	private Material material = null;
//	private bool meshShown = false;
//
//	public string Name{ set { name = value; } get { return name; } }
//
//	//public Vector3 Postion{ set { postion = value; } get { return postion; } }
//
//	//public int ChildCount{ set { count = value; } get { return count; } }
//
//	// image target gameobject which hold all properties of imagetarget
//	public GameObject ImageTargetObject{ set { obj = value; } get { return obj; } }
//
//
//	public GameObject ModelObject{ set { modelObj = value; } get { return modelObj; } }
//
//	public Material ModelMaterialName{ set { material = value; } get { return material; } }
//
//	public bool ObjectMeshVisible{ set { meshShown = value; } get { return meshShown; } }
//}
//
//public class ColoringController : MonoBehaviour
//{
//
//
//	#region PRIVATE_VARIABLES
//
//
//	private MarkerInfo markerInfoObject;
//	private MarkerInfo markerObjectHold;
//	private List <string> collideObjectList = new List<string> ();
//	private float width = 100f;
//	private float height = 100f;
//	private int sameColorCount = 0;
//	private string operatorSign = null;
//	private string blueDigitHolder = null;
//	private string greenDigitHolder = null;
//	public int maxMarkerDetectLimit = 5;
//	//	private string lastPostionMarker = "ImageTarget_equal";
//
//	#endregion
//
//	#region Public_Variable
//
//	public Text resultText;
//	public Text textOne;
//	public Text textTwo;
//	public Text textThree;
//	public Text textFour;
//	public Material redMaterial;
//	public Material blueMaterial;
//	public Material greenMaterial;
//
//	public List<MarkerInfo> markerlList = new List<MarkerInfo> ();
//
//	#endregion
//
//	#region  UNTIY_MONOBEHAVIOUR_METHODS
//
//	// Use this for initialization
//	void Start ()
//	{
//		
//		DontDestroyOnLoad (this);
//		Debug.Log ("width " + width);
//		Vuforia.DefaultTrackableEventHandler.onImageTargetDetected += onImageTargetDetected;
//		CollisionDetection.onCollision += onCollision;
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//		// when first card detect then its color change in blue
//		if (markerlList.Count != 0) {
//			SortList ();
//
//			for (int i = 0; i < markerlList.Count; i++) {
//
//				if (i == 0 && !OperatorTypeCard (markerlList [i].ImageTargetObject.name.ToLower ())) {
//					// change the color of gameobject by using material
//					markerlList [0].ModelObject.GetComponent<MeshRenderer> ().material = blueMaterial;
//					textTwo.text = FindDigit (markerlList [0].Name).ToString ();
//					textTwo.color = Color.blue;
//				} 
//				Debug.Log ("marker position " + markerlList [i].ImageTargetObject.transform.localPosition.x);
//				Debug.Log ("marker color " + markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name);
//			}
//			sameColorCount = 0;
//			for (int j = 0; j < markerlList.Count - 1; j++) {
//				if (!OperatorTypeCard (markerlList [j].ImageTargetObject.name.ToLower ())) {
//
//					for (int k = j; k < markerlList.Count - 1; k++) {
//					     
//						#region marker overlap handling 
//						if (markerlList [k + 1].ImageTargetObject != null) {
//							if (markerlList [j].Name.Equals (markerlList [k + 1].Name)) {
//								// in this condition when one or more marker/model place in same position. when model detected but we change the marker position.
//								if (markerlList [j].ImageTargetObject.transform.localPosition.x >= markerlList [k + 1].ImageTargetObject.transform.localPosition.x) {
//									if (markerlList [j].ImageTargetObject.transform.localPosition.x - markerlList [k + 1].ImageTargetObject.transform.localPosition.x <= width / 1.5f) {
//										markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = false;
//										markerlList [k + 1].ObjectMeshVisible = false;
//										markerlList.RemoveAt (k + 1);
//										return;
//										Debug.Log ("two model place same position 1");
//									}
//								}
//					      //in this condition when one or more marker/model place in same position.when second model have large position as compare to one model
//					       else if (markerlList [k + 1].ImageTargetObject.transform.localPosition.x - markerlList [j].ImageTargetObject.transform.localPosition.x <= width / 1.5f) {
//									markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = false;
//									markerlList [k + 1].ObjectMeshVisible = false;
//									markerlList.RemoveAt (k + 1);
//									return;
//									Debug.Log ("two model  place same position 2");
//								}
//
//					    // when two model are separate 
//					   else {
//
//									markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = true;
//									markerlList [k + 1].ObjectMeshVisible = true;
//									//markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
//									Debug.Log ("two model are separate");
//								}
//								#endregion
//							}
//							#region if condition is used for coloring the cards
//							// In this condition when marker are overlap then its show red color .
//							Debug.Log ("become red " + (markerlList [k + 1].ImageTargetObject.transform.localPosition.x + " j value " + markerlList [j].ImageTargetObject.transform.localPosition.x));
//							if (markerlList [k + 1].ImageTargetObject.transform.localPosition.x - markerlList [j].ImageTargetObject.transform.localPosition.x <= width / 2) {
//								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material = redMaterial;
//
//								Debug.Log ("material become red");
//							}
//					              // when two marker are collide with each other then first two model show blue color .
//
//						       else if (markerlList [k + 1].ImageTargetObject.transform.localPosition.x - markerlList [j].ImageTargetObject.transform.localPosition.x <= width * 1.8f && sameColorCount < 1 && !markerlList [j].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("red")) {
//						
//								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material = markerlList [j].ModelObject.GetComponent<MeshRenderer> ().material;
//								textOne.text = FindDigit (markerlList [j].Name).ToString ();
//								textOne.color = Color.blue;
//								textTwo.text = FindDigit (markerlList [k + 1].Name).ToString ();
//								textTwo.color = Color.blue;
//								Debug.Log ("material become same ");
//							} else if (markerlList [k + 1].ImageTargetObject.transform.localPosition.x - markerlList [j].ImageTargetObject.transform.localPosition.x <= width * 1.8f && sameColorCount >= 1) {
//								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
//								//	if (markerlList [j].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("red"))
//								//		markerlList [j].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
//								textThree.text = FindDigit (markerlList [j].Name).ToString ();
//								textThree.color = Color.green;
//								textFour.text = FindDigit (markerlList [k + 1].Name).ToString ();
//								textFour.color = Color.green;
//								Debug.Log ("material become green same ");
//							} else if (markerlList.Count <= 3) {
//								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
//								textFour.text = FindDigit (markerlList [k + 1].Name).ToString ();
//								textFour.color = Color.green;
//							} else if (markerlList [2] != null) {
//								//markerlList [2].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
//								
//							} else {
//								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material = redMaterial;
//							}
//							#endregion
//
//							// compare coloring of two objects
//							if (markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.Equals (markerlList [j].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name)) {
//								sameColorCount++;
//							} 
//
//						}
//					}
//
//				}
//			}
//
//			// for calculation of app
//			blueDigitHolder = null;
//			greenDigitHolder = null;
//			if (markerlList.Count > 1 && operatorSign != null) {
//				for (int l = 0; l < markerlList.Count; l++) {
//					if (markerlList [l].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("blue")) {
//						blueDigitHolder = blueDigitHolder + FindDigit (markerlList [l].Name).ToString ();
//						Debug.Log (" blue value name " + blueDigitHolder);
//
//					} else if (markerlList [l].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("green")) {
//						greenDigitHolder = greenDigitHolder + FindDigit (markerlList [l].Name).ToString ();
//						Debug.Log (" green value name " + greenDigitHolder);
//					}
//
//					if (l == markerlList.Count - 1) {
//						ResultCalculate (operatorSign);
//					}
//
//				}
//			}
//
//
//
//
//
//
//
//		} else {
//			textOne.text = "0";
//			textTwo.text = "0";
//			textThree.text = "0";
//			textFour.text = "0";
//			textOne.color = Color.black;
//			textTwo.color = Color.black;
//			textThree.color = Color.black;
//			textFour.color = Color.black;
//
//		}
//	}
//
//	#endregion
//
//	#region EVENTS
//
//	// subscribed with event in defaultTrackableEventHandler
//	void onImageTargetDetected (GameObject arg0, bool isfound)
//	{
//		if (isfound) {
//			
//			Debug.Log ("found object name " + arg0.name); 
//
//			string[] name = arg0.name.Split ('_');
//			Debug.Log ("name " + name [name.Length - 1]);
//			if (!OperatorTypeCard (name [name.Length - 1])) {
//				markerInfoObject = new MarkerInfo ();
//				markerInfoObject.ImageTargetObject = arg0;
//				markerInfoObject.Name = name [name.Length - 1];
//				markerInfoObject.ModelObject = arg0.transform.GetChild (0).gameObject.transform.GetChild (0).gameObject;
//				markerInfoObject.ModelMaterialName = arg0.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material;
//				markerInfoObject.ObjectMeshVisible = true;
//
//				markerlList.Add (markerInfoObject);
//			} else {
//				operatorSign = name [name.Length - 1];
//			}
//
//
//			if (markerlList.Count <= maxMarkerDetectLimit && markerlList.Count > 1) {
//				int listCount = markerlList.Count;
//				for (int i = 0; i < listCount - 1; i++) {
//					//	Debug.Log ("value of i " + i + " marker count " + markerlList.Count);
//					for (int j = i; j < listCount - 1; j++) {
//						Debug.Log ("one marker name" + markerlList [i].ImageTargetObject.name + " another name " + markerlList [j + 1].ImageTargetObject.name);
//						if (!markerlList [i].ImageTargetObject.name.Equals (markerlList [j + 1].ImageTargetObject.name)) {
//							//Debug.Log ("no repeated marker name" + markerlList [j].ImageTargetObject.name + " another name " + markerlList [j + 1].ImageTargetObject.name);
//						} else {
//							//	Debug.Log ("repeated marker name" + markerlList [j].ImageTargetObject.name);
//							markerlList.Remove (markerlList.Find (item => item.ImageTargetObject.name == markerlList [i].ImageTargetObject.name));
//							//	Debug.Log ("after remove list count " + markerlList.Count);
//						}
//						//Debug.Log ("value of j " + j);
//					}
//					if (i == markerlList.Count - 1 && markerlList.Count > 1)
//						SortList ();
//				}
//
//
//			}
//
//
//
//		} else {
//			if (operatorSign != null && OperatorTypeCard (arg0.name.ToLower ())) {
//				
//
//				operatorSign = null;
//
//
//			}
//			if (markerlList.Count != 0) {
//				
//				int index = markerlList.FindIndex (item => item.ImageTargetObject.name == arg0.name); 
//
//				//Debug.Log ("index value " + index);
//				if (index != -1) {
//					markerlList [index].ModelObject.GetComponent<MeshRenderer> ().material = markerlList [index].ModelMaterialName;
//					markerlList.RemoveAt (index);
//				}
//			}
//			//GetComponent<SoundManager> ().stopAllMusic ();
//			Debug.Log ("list count " + markerlList.Count);
//		}
//	}
//
//	#endregion
//
//	public void SortList ()
//	{
//		
//		if (markerlList != null) {
//			
//			markerlList.Sort (delegate(MarkerInfo x, MarkerInfo y) {
//				if (x.ImageTargetObject.transform.localPosition == null && y.ImageTargetObject.transform.localPosition == null)
//					return 0;
//				else if (x.ImageTargetObject.transform.localPosition == null)
//					return -1;
//				else if (y.ImageTargetObject.transform.localPosition == null)
//					return 1;
//				else {
//					return  x.ImageTargetObject.transform.localPosition.x.CompareTo (y.ImageTargetObject.transform.localPosition.x);
//				} 
//				y = x;
//			});
//
//
//
//		}
//
//	}
//
//
//
//	private int FindDigit (string digit)
//	{
//		string value = digit.ToLower ();
//		if (value.Contains ("zero")) {
//			return 0;
//		} else if (value.Contains ("one")) {
//			return 1;
//		} else if (value.Contains ("two")) {
//			return 2;
//		} else if (value.Contains ("three")) {
//			return 3;
//		} else if (value.Contains ("four")) {
//			return 4;
//		} else if (value.Contains ("five")) {
//			return 5;
//		} else if (value.Contains ("six")) {
//			return 6;
//		} else if (value.Contains ("seven")) {
//			return 7;
//		} else if (value.Contains ("eight")) {
//			return 8;
//		} else if (value.Contains ("nine")) {
//			return 9;
//		}
//		return -1;
//
//	}
//
//	//  This method check detect card is digit or operator.
//	private bool OperatorTypeCard (string cardName)
//	{
//		cardName = cardName.ToLower ();
//		if (cardName.Contains ("multiply"))
//			return true;
//		else if (cardName.Contains ("add"))
//			return true;
//		else if (cardName.Contains ("subtract"))
//			return true;
//		else if (cardName.Contains ("divide"))
//			return true;
//		else if (cardName.Contains ("equal"))
//			return true;
//			
//		return false;
//	}
//
//
//
//	void onCollision (string name, bool status)
//	{
//		if (status) {
//			collideObjectList.Add (name);
//		} else {
//			collideObjectList.Remove (name);
//
//		}
//	}
//
//	int firstValue = 0;
//	int secondValue = 0;
//	int thirdValue = 0;
//	int fourValue = 0;
//
//
//	private void ResultCalculate (string sign)
//	{
//		sign = sign.ToLower ();
//		Debug.Log ("sign " + sign);
//		if (blueDigitHolder != null && greenDigitHolder != null) {
//			float result = 0;
//			if (sign.Equals ("divide"))
//				result = (float.Parse (blueDigitHolder) / float.Parse (greenDigitHolder));
//			else if (sign.Equals ("multiply"))
//				result = (float.Parse (blueDigitHolder) * float.Parse (greenDigitHolder));
//			else if (sign.Equals ("add"))
//				result = (float.Parse (blueDigitHolder) + float.Parse (greenDigitHolder));
//			else if (sign.Equals ("subtract"))
//				result = (float.Parse (blueDigitHolder) - float.Parse (greenDigitHolder));
//			resultText.text = result.ToString ();
//		} else {
//			resultText.text = "";
//		}
//	}
//
//}
