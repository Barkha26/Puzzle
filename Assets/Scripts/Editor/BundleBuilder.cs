using UnityEngine;
using System.Collections;
using UnityEditor;

public class BundleBuilder : Editor {

	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAssetBundle(){
		#if UNITY_ANDROID
		BuildPipeline.BuildAssetBundles ("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
		#endif

		#if UNITY_IPHONE
		BuildPipeline.BuildAssetBundles ("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.iOS);
		#endif

		#if UNITY_STANDALONE
		BuildPipeline.BuildAssetBundles ("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows );
		#endif
	}

	[MenuItem("Assets/DeleteSaved")]
	static void DeleteSavedBundle(){
		#if UNITY_ANDROID
		PlayerPrefs.DeleteAll();
		Debug.Log("Player Preferences Cleared");
		#endif

		#if UNITY_IPHONE
		PlayerPrefs.DeleteAll();
		Debug.Log("Player Preferences Cleared");
		#endif

		#if UNITY_STANDALONE
		PlayerPrefs.DeleteAll();
		Debug.Log("Player Preferences Cleared");
		#endif
	}

}
