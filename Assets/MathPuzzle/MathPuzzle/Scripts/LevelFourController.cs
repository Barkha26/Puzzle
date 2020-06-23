using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFourController : MonoBehaviour
{

	#region PRIVATE_VARIABLES


	private MarkerInfo markerInfoObject;
	private MarkerInfo markerObjectHold;
	private List <string> collideObjectList = new List<string> ();
	private float width = 72.83073f;
	private float height = 100f;
	private int sameColorCount = 0;
	private string operatorSign = null;
	private string blueDigitHolder = null;
	private string greenDigitHolder = null;
	public int maxMarkerDetectLimit = 3;
	private bool isQuadCollision = false;
	private bool isCardCollision = false;
	private bool isPlaneCollision = false;
	private GameObject quadObject = null;
	private GameObject planeObject = null;
	private int blueCount = 0;
	private int greenCount = 0;
	private int isPlaneIndex = -1;
	private bool isPlaneCreated = false;
	private bool isLevelActivated = false;
	//private string lastPostionMarker = "ImageTarget_equal";

	#endregion

	#region Public_Variable

	public Text resultText;

	public Material redMaterial;
	public Material blueMaterial;
	public Material greenMaterial;
	public Material yellowMaterial;
	public GameObject planePrefab;
	public List<MarkerInfo> markerlList = new List<MarkerInfo> ();
	public static LevelFourController instance;

	#endregion


	#region  UNTIY_MONOBEHAVIOUR_METHODS

	// Use this for initialization
	void Start ()
	{

		//	DontDestroyOnLoad (this);
		isCardCollision = false;
		isPlaneCollision = false;
		instance = this;

		CollisionDetection.onCollision += onCollision;
		CollisionWithQuad.onCollisionWithQuad += onCollisionWithQuad;
		CollisionWithPlane.onCollisionWithPlane += onCollisionWithPlane;
		Debug.Log ("width " + width);
		
	}

	void OnEnable ()
	{
		isLevelActivated = true;
		Vuforia.DefaultTrackableEventHandler.onImageTargetDetected += onImageTargetDetected;
	}
	// Update is called once per frame
	void LateUpdate ()
	{
		// when first card detect then its color change in blue
	
		if (markerlList.Count > 0) {
			SortList ();

			blueCount = 0;
			greenCount = 0;

			for (int i = 0; i < markerlList.Count; i++) {
				
				// change the color of model acc. to their psition

				markerlList [0].ModelObject.GetComponent<MeshRenderer> ().material = blueMaterial;

				// for quad creation and destroy
				if (quadObject != null && markerlList [i].ImageTargetObject.transform.Find ("Quad") && i != 0) {
					
					Destroy (markerlList [i].ImageTargetObject.transform.Find ("Quad").gameObject);

				} else if (!markerlList [i].ImageTargetObject.transform.Find ("Quad") && i == 0) {
					
					CreateQuad ();
				} else if (markerlList [i].ImageTargetObject.transform.Find ("Quad") && !isQuadCollision && quadObject != null) {
					quadObject.GetComponent<MeshRenderer> ().enabled = true;
				}

				/*
				 * if quad is collide then quad not visible else visible
				*/
				if (GameObject.Find ("Quad").GetComponent<CollisionWithQuad> ().isQuadCollide && quadObject != null) {
					quadObject.GetComponent<MeshRenderer> ().enabled = false;
				} else if (quadObject != null) {
					quadObject.GetComponent<MeshRenderer> ().enabled = true;
				}



				if (markerlList.Count > 1) {
					if (markerlList [i].ImageTargetObject.transform.Find ("Quad") && i == 0) {
						if (GameObject.Find ("Quad").GetComponent<CollisionWithQuad> ().isQuadCollide && markerlList.Count > 1) {
							markerlList [i + 1].ModelObject.GetComponent<MeshRenderer> ().material = blueMaterial;
						} else {
							markerlList [i + 1].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
						}
						isPlaneIndex = -1;
						isPlaneCreated = false;
					
					} else if (markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("blue") && GameObject.Find ("Quad").GetComponent<CollisionWithQuad> ().isQuadCollide && i == 1) {
						
						markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material = blueMaterial;
					} else if (isPlaneIndex == -1 && !isPlaneCreated) {
						
						isPlaneIndex = i;
						markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
					} else if (markerlList [i - 1].ImageTargetObject.transform.Find ("Plane")) {
						
						if (GameObject.Find ("Plane").GetComponent<CollisionWithPlane> ().isPlaneCollide) {
							isPlaneIndex = -1;
							isPlaneCreated = true;
							markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material = greenMaterial;
						} else {
							markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material = redMaterial;
						}
					} else {
						
						markerlList [i].ModelObject.GetComponent<MeshRenderer> ().material = redMaterial;
					}

				
					if (!markerlList [i].ImageTargetObject.transform.Find ("Plane") && isPlaneIndex != -1 && !isPlaneCreated) {
						if (GameObject.Find ("Quad").GetComponent<CollisionWithQuad> ().isQuadCollide && markerlList.Count > 2) {
							isPlaneCreated = true;
							Destroy (planeObject);
							CreatePLane (isPlaneIndex);
						} else if (!GameObject.Find ("Quad").GetComponent<CollisionWithQuad> ().isQuadCollide && markerlList.Count > 1) {
							isPlaneCreated = true;
							Destroy (planeObject);
							CreatePLane (isPlaneIndex);
						}

					} else if (markerlList [i].ImageTargetObject.transform.Find ("Plane") && i != isPlaneIndex) {
						
						Destroy (markerlList [i].ImageTargetObject.transform.Find ("Plane").gameObject);
					} 

					if (markerlList [i].ImageTargetObject.transform.Find ("Plane")) {
						
						if (GameObject.Find ("Plane").GetComponent<CollisionWithPlane> ().isPlaneCollide) {
							planeObject.GetComponent<MeshRenderer> ().enabled = false;
						} else {
							planeObject.GetComponent<MeshRenderer> ().enabled = true;
						}
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
								if (markerlList [j].ImageTargetObject.transform.localPosition.x - markerlList [k + 1].ImageTargetObject.transform.localPosition.x <= width / 1.5f) {
									markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = false;
									markerlList [k + 1].ObjectMeshVisible = false;
									markerlList.RemoveAt (k + 1);
									return;
									Debug.Log ("two model place same position 1");
								}
							}
							//in this condition when one or more marker/model place in same position.when second model have large position as compare to one model
							else if (markerlList [k + 1].ImageTargetObject.transform.localPosition.x - markerlList [j].ImageTargetObject.transform.localPosition.x <= width / 1.5f) {
								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = false;
								markerlList [k + 1].ObjectMeshVisible = false;
								markerlList.RemoveAt (k + 1);
								return;
								Debug.Log ("two model  place same position 2");
							}

							// when two model are separate 
							else {

								markerlList [k + 1].ModelObject.GetComponent<MeshRenderer> ().enabled = true;
								markerlList [k + 1].ObjectMeshVisible = true;
								Debug.Log ("two model are separate");
							}

						}

					}


					#endregion
				}

				// for calculation of app
				blueDigitHolder = null;
				greenDigitHolder = null;
				if (markerlList.Count > 1 && operatorSign != null) {
					for (int l = 0; l < markerlList.Count; l++) {
						if (markerlList [l].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("blue")) {
							blueDigitHolder = blueDigitHolder + FindDigit (markerlList [l].Name).ToString ();
							Debug.Log (" blue value name " + blueDigitHolder);

						} else if (markerlList [l].ModelObject.GetComponent<MeshRenderer> ().material.mainTexture.name.ToLower ().Equals ("green")) {
							greenDigitHolder = greenDigitHolder + FindDigit (markerlList [l].Name).ToString ();
							Debug.Log (" green value name " + greenDigitHolder);
						}

						if (l == markerlList.Count - 1) {
							ResultCalculate (operatorSign);
						}

					}
				} else
					resultText.text = "";

			}

		} 

	}

	#endregion

	#region EVENTS

	// subscribed with event in defaultTrackableEventHandler
	void onImageTargetDetected (GameObject arg0, bool isfound)
	{
		if (isLevelActivated) {
			if (isfound) {
				string[] name = arg0.name.Split ('_');

				if (!OperatorTypeCard (name [name.Length - 1]) && markerlList.Count <= 3) {
					markerInfoObject = new MarkerInfo ();
					markerInfoObject.ImageTargetObject = arg0;
					markerInfoObject.Name = name [name.Length - 1];
					markerInfoObject.ModelObject = arg0.transform.GetChild (0).gameObject.transform.GetChild (0).gameObject;
					markerInfoObject.ModelMaterialName = arg0.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material;
					markerInfoObject.ObjectMeshVisible = true;
					markerlList.Add (markerInfoObject);
					markerlList [0].ModelObject.GetComponent<MeshRenderer> ().material = blueMaterial;
				} else {
					operatorSign = name [name.Length - 1];
				}


				if (markerlList.Count <= maxMarkerDetectLimit && markerlList.Count > 1) {
					int listCount = markerlList.Count;
					for (int i = 0; i < listCount - 1; i++) {

						for (int j = i; j < listCount - 1; j++) {
							
							if (markerlList [i].ImageTargetObject.name.Equals (markerlList [j + 1].ImageTargetObject.name)) {
								
								markerlList.Remove (markerlList.Find (item => item.ImageTargetObject.name == markerlList [i].ImageTargetObject.name));

							}

						}
						if (i == markerlList.Count - 1 && markerlList.Count > 1)
							SortList ();
					}


				}


				// when first marker is detect then it show quad to the user.
				if (markerlList.Count <= 4) {
					
					if (markerlList.Count == 1) {
						Destroy (quadObject);

						CreateQuad ();
					} 
				}
				if (markerlList.Count <= 1) {
					isQuadCollision = false;
					isPlaneCollision = false;
				}
			} else {
				if (operatorSign != null && OperatorTypeCard (arg0.name.ToLower ())) {

					operatorSign = null;
					resultText.text = "";

				}
				if (markerlList.Count != 0) {

					int index = markerlList.FindIndex (item => item.ImageTargetObject.name == arg0.name); 

			
					if (index != -1) {
						markerlList [index].ModelObject.GetComponent<MeshRenderer> ().material = markerlList [index].ModelMaterialName;
						if (markerlList [index].ImageTargetObject.transform.Find ("Quad"))
							Destroy (markerlList [index].ImageTargetObject.transform.Find ("Quad").gameObject);
						if (markerlList [index].ImageTargetObject.transform.Find ("Plane"))
							Destroy (markerlList [index].ImageTargetObject.transform.Find ("Plane").gameObject);
						markerlList.RemoveAt (index);

					}
				} else {
					if (quadObject != null)
						DestroyImmediate (quadObject);
					if (planeObject != null)
						DestroyImmediate (planeObject);
				}

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

	}



	private int FindDigit (string digit)
	{
		string value = digit.ToLower ();
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
			isCardCollision = status;
			//	Debug.Log ("collision with object " + name + " count " + markerlList.Count);
			MarkerInfo obj = markerlList.Find (item => item.ImageTargetObject.name == name); 
			if (status && obj != null) {
				obj.ImageTargetObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material = redMaterial;
				collideObjectList.Add (name);
			} else {
				collideObjectList.Remove (name);

			}
		}
	}

	void onCollisionWithQuad (string name, bool status)
	{
		if (isLevelActivated) {
			isQuadCollision = status;
			MarkerInfo obj = markerlList.Find (item => item.ImageTargetObject.name == name); 
			int index = markerlList.FindIndex (item => item.ImageTargetObject.name == name); 

			if (status && index == 1 && quadObject != null) {

				quadObject.GetComponent<MeshRenderer> ().enabled = false;
				obj.ImageTargetObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material = blueMaterial;
			} else if (obj != null) {

				obj.ImageTargetObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material = redMaterial;
			} 
		}

	}

	void onCollisionWithPlane (string name, bool status)
	{
		if (isLevelActivated) {
			isPlaneCollision = status;
			MarkerInfo obj = markerlList.Find (item => item.ImageTargetObject.name == name); 
	
			if (status && obj != null && planeObject != null) {
			
				planeObject.GetComponent<MeshRenderer> ().enabled = false;
				obj.ImageTargetObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material = greenMaterial;
			} else if (obj != null) {
				obj.ImageTargetObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<MeshRenderer> ().material = redMaterial;
			} 
			if (!status)
				isPlaneIndex = -1;
		}
	}

	private void CreatePLane (int index)
	{
		SortList ();

		if (markerlList.Count > 0) {
//			transform.localPosition = new Vector3 (.61f, 0f, 0f);
//			transform.localRotation = Quaternion.Euler (90f, 90f, 0f);
//			planeObject = Instantiate (planePrefab, markerlList [index].ImageTargetObject.transform);
			planeObject = GameObject.CreatePrimitive (PrimitiveType.Quad);
			planeObject.name = "Plane";
			planeObject.transform.parent = markerlList [index].ImageTargetObject.transform;
			planeObject.transform.localPosition = new Vector3 (0.6f, 0.05f, 0);
			planeObject.transform.localRotation = Quaternion.Euler (90f, 90f, 0f);
			planeObject.transform.localScale = new Vector3 (0.87f, 0.6f, 0.8f);
			planeObject.AddComponent<BoxCollider> ().isTrigger = true;
			planeObject.GetComponent<MeshCollider> ().convex = true;
			planeObject.AddComponent<Rigidbody> ().useGravity = false;
			planeObject.GetComponent<Rigidbody> ().isKinematic = true;
			planeObject.AddComponent<CollisionWithPlane> ().enabled = true;
			planeObject.GetComponent<MeshRenderer> ().material = greenMaterial;
			//quadObject.GetComponent<BoxCollider> ().center = new Vector3 (0, -0.5f, 0);
			planeObject.GetComponent<BoxCollider> ().size = new Vector3 (1f, 1f, 0.35f);
		}
	}

	private void CreateQuad ()
	{
		SortList ();

		if (markerlList.Count > 0) {
			quadObject = GameObject.CreatePrimitive (PrimitiveType.Quad);
			quadObject.transform.parent = markerlList [0].ImageTargetObject.transform;
			quadObject.transform.localPosition = new Vector3 (0.6f, 0.05f, 0);
			quadObject.transform.localRotation = Quaternion.Euler (90f, 90f, 0f);
			quadObject.transform.localScale = new Vector3 (0.87f, 0.6f, 0.8f);
			quadObject.AddComponent<BoxCollider> ().isTrigger = true;
			quadObject.GetComponent<MeshCollider> ().convex = true;
			quadObject.AddComponent<Rigidbody> ().useGravity = false;
			quadObject.GetComponent<Rigidbody> ().isKinematic = true;
			quadObject.AddComponent<CollisionWithQuad> ().enabled = true;
			quadObject.GetComponent<MeshRenderer> ().material = blueMaterial;
			//quadObject.GetComponent<BoxCollider> ().center = new Vector3 (0, -0.5f, 0);
			quadObject.GetComponent<BoxCollider> ().size = new Vector3 (1f, 1f, 0.35f);
		}
	}

	void OnDisable ()
	{
		Vuforia.DefaultTrackableEventHandler.onImageTargetDetected -= onImageTargetDetected;
		isLevelActivated = false;
		if (quadObject != null)
			DestroyImmediate (quadObject);
		if (planeObject != null)
			DestroyImmediate (planeObject);
	}
}
