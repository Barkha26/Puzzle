using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarkerInfo
{
	private string name = null;
	private GameObject obj = null;
	private GameObject modelObj = null;
	private Material material = null;
	private bool meshShown = false;

	public string Name{ set { name = value; } get { return name; } }


	public GameObject ImageTargetObject{ set { obj = value; } get { return obj; } }


	public GameObject ModelObject{ set { modelObj = value; } get { return modelObj; } }

	public Material ModelMaterialName{ set { material = value; } get { return material; } }

	public bool ObjectMeshVisible{ set { meshShown = value; } get { return meshShown; } }
}