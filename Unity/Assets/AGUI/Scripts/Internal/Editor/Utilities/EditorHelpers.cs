using UnityEngine;
using UnityEditor;
using System.Collections;

using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

public static class EditorHelpers
{
	
	#region Helper
	
	public static bool GetImageSize(this Texture2D asset, out int width, out int height) {
		if (asset != null) {
			string assetPath = AssetDatabase.GetAssetPath(asset);
			TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			
			if (importer != null) {
				object[] args = new object[2] { 0, 0 };
				MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
				mi.Invoke(importer, args);

				width = (int)args[0];
				height = (int)args[1];
				
				return true;
			}
		}
		
		height = width = 0;
		return false;
	}
	
	public static T CreateAsset<T> (string name = "") where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T>();
		
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "")
		{
			path = "Assets";
		}
		else if (Path.GetExtension (path) != "")
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}

		if(name.Equals(""))
		{
			name = "New " + typeof(T).ToString();
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + name + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;

		return AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T) ) as T;
	}

	
	#endregion
}
