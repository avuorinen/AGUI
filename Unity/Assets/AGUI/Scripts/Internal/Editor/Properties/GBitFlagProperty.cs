// @file GBitFlagProperty.cs
// @date 30.3.2014
// @author Atte Vuorinen

using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GBitFlagEnumAttribute))]
public class GBitFlagProperty : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		property.intValue = EditorGUI.MaskField(position,label,property.intValue,property.enumNames);
	}
}