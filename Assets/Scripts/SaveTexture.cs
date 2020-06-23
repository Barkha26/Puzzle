using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveTexture : MonoBehaviour
{


	private byte[] bytes;
	private string path;
	private Camera renderTexture;
	public static SaveTexture instance;
	public GameObject cube;
	// Use this for initialization
	void Start ()
	{
		instance = this;
		path = Application.persistentDataPath + "/Images/";
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void SaveTexturePng ()
	{
		
		//	RenderTextureCamera.instance.MakeScreen ();
		//if (renderTexture != null) {
			
		//RenderTexture.active=RenderTextureCamera.instance.GetRenderTexture();
//		        cube.GetComponent<Renderer> ().material.SetTexture ("_MainTex",Region_Capture.VideoBackgroundTexure);
		Texture2D tex = new Texture2D (256, 256);
		Texture texture = GetComponent<Renderer> ().material.mainTexture;

		tex = (Texture2D)texture;
//		tex.ReadPixels (new Rect (0, 0, Region_Capture.VideoBackgroundTexure.width,Region_Capture.VideoBackgroundTexure.height), 0, 0);
		tex.Apply ();
		Debug.Log (tex);
		bytes = tex.EncodeToPNG ();
		//cube.GetComponent<Renderer> ().material.SetTexture ("_MainTex", tex);
		//tex = null;
		Directory.CreateDirectory (path);
		Debug.Log (Directory.Exists (path));
		Debug.Log (path);
		if (Directory.Exists (path)) {
			File.WriteAllBytes (path + "texture1.png", bytes);
			Debug.Log ("saved image");
		}
//		Debug.Log ("fetch image");
//		bytes = File.ReadAllBytes (path + "texture1.png");
//		tex.LoadImage (bytes);
//		Debug.Log (tex.width);
		//cube.GetComponent<Renderer> ().material.SetTexture ("_MainTex", tex);
		//}
	}

	public void CreateSprite (Camera texture)
	{
		Debug.Log ("create sprite called");
		//	cube.GetComponent<Renderer> ().material.SetTexture ("_MainTex", texture.targetTexture);
		renderTexture = texture;
		//	RenderTexture.active=renderTexture;
		//	cube.GetComponent<Renderer> ().material.SetTexture ("_MainTex", renderTexture);
//		Texture2D tex = new Texture2D (renderTexture.width,renderTexture.height,TextureFormat.RGB24,false);
//		tex.ReadPixels (new Rect (0, 0, renderTexture.width,renderTexture.height), 0, 0);
//		tex.Apply ();
//		bytes = tex.EncodeToPNG ();
//		Directory.CreateDirectory (path);
//		Debug.Log (Directory.Exists (path));
//		Debug.Log (path);
//		if (Directory.Exists (path))
//			File.WriteAllBytes (path + "texture.png", bytes);
	}
}
