// @file GMinValueAttributeProperty.cs
// @date 12.3.2014
// @author Atte Vuorinen

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.Reflection;
using System.Collections;
using System;
using System.Text;

[CustomPropertyDrawer(typeof(GMinValueAttribute))]
public class GMinValueAttributeProperty : PropertyDrawer
{
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		GMinValueAttribute minAtt = (GMinValueAttribute)attribute;

		if(property.type == "float")
		{
			float value = property.floatValue;

			value = EditorGUI.FloatField(position,label,value);

			if(value >= minAtt.Min)
			{
				property.floatValue = value;
			}
			else
			{
				property.floatValue = minAtt.Min;
			}
		}

	}
}