// @file GReadOnlyAttributeProperty.cs
// @date 12.4.2014
// @author Atte Vuorinen

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(GReadOnlyAttribute))]
public class GReadOnlyAttributeProperty : PropertyDrawer
{
	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		GReadOnlyAttribute att = (GReadOnlyAttribute)attribute;

		return base.GetPropertyHeight (property, label) + att.height;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		GUI.enabled = false;
		EditorGUI.PropertyField(position,property,label);
		GUI.enabled = true;

		//base.OnGUI (position, property, label);
	}
}