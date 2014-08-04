// @file GSortIDAttributeProperty.cs
// @date 9.2.2014
// @author Atte Vuorinen

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.Reflection;
using System.Collections;
using System;
using System.Text;

[CustomPropertyDrawer(typeof(GSortIDAttribute))]
public class GSortIDAttributeProperty : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		int index = 0;

		string[] layers = GetSortingLayerNames();

		if(property.stringValue == "")
		{
			property.stringValue = "Default";
		}

		for(int i = 0; i < layers.Length; i++)
		{
			if(layers[i] == property.stringValue)
			{
				index = i;
				break;
			}
		}

		index = EditorGUI.Popup(position,label.text,index,GetSortingLayerNames());

		string layer = layers[index];

		if(layer == "Default")
		{
			layer = "";
		}

		property.stringValue = layer;
		//base.OnGUI (position, property, label);
	}

	public string[] GetSortingLayerNames() 
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
	
	// Get the unique sorting layer IDs -- tossed this in for good measure
	public int[] GetSortingLayerUniqueIDs() 
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}
}