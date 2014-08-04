// @file GEventDelegateProperty.cs
// @date 18.1.2014
// @author Atte Vuorinen
// @info Creates list for available methods from target object.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomPropertyDrawer(typeof(GEventDelegate))]
public class GEventDelegateProperty : PropertyDrawer 
{
	//TODO: Visualize!

	private int m_current;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		if(property.FindPropertyRelative("m_hasCallback").boolValue && property.FindPropertyRelative("target").objectReferenceValue == null)
		{
			return 24f;
		}

		return 62f; //48
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		bool valueChanged = false;

		Rect boxPosition = position;
		boxPosition.x += 24;
		boxPosition.width -= 24;

		GUI.Box(boxPosition,"");

		int oldInner = EditorGUI.indentLevel++;

		position.height = 16f;
		position.y += 4;

		bool hasDelegate = property.FindPropertyRelative("m_hasCallback").boolValue;

		if(hasDelegate && property.FindPropertyRelative("target").objectReferenceValue == null)
		{
			EditorGUI.LabelField(position,"Delegate");
			return;
		}

		EditorGUI.LabelField(position, (property.FindPropertyRelative("target").objectReferenceValue == null ? "" : property.FindPropertyRelative("target").objectReferenceValue.name + " | " + property.FindPropertyRelative("method").stringValue));
		position.y += 16f;

		EditorGUI.BeginChangeCheck();
		EditorGUI.PropertyField(position,property.FindPropertyRelative("target"));

		position.y += 16;

		Component target = property.FindPropertyRelative("target").objectReferenceValue as Component;
		string method = property.FindPropertyRelative("method").stringValue;

		if(EditorGUI.EndChangeCheck())
		{
			valueChanged = true;
		}


		if(target == null)
		{
			return;
		}

		Component[] componentsFromTarget = target.gameObject.GetComponents<Component>();


		List<string> names = new List<string>();
		List<string> fullName = new List<string>();
		
		List<Component> components = new List<Component>();
		
		List<MethodInfo> methodInfos = new List<MethodInfo>();
		
		MethodInfo[] infos;
		int index = 0;
		
		int componentIndex = 0;
		
		foreach(Component m in componentsFromTarget)
		{
			bool found = false;
			
			infos = m.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
			
			for(int i = 0; i < infos.Length; i++)
			{
				if (infos[i].GetParameters().Length == 0 && infos[i].ReturnType == typeof(void) 
				    && infos[i].Name != "StopAllCoroutines" &&  infos[i].Name != "CancelInvoke"
				    && !System.Attribute.IsDefined(infos[i],typeof(GIgnoreAttribute)) )
				{
					found = true;
					
					methodInfos.Add(infos[i]);
					components.Add(m);
					names.Add(infos[i].Name);
					fullName.Add(componentIndex + " | " + m.GetType().Name + "." + infos[i].Name);

					if(target == m && method == infos[i].Name)
					{
						m_current = index;
					}
					else if(target == m && method == "")
					{
						//First time init.
						m_current = index;
						method = infos[i].Name;

						valueChanged = true;
					}
					
					index++;
				}
			}
			
			if(found)
			{
				componentIndex++;
			}
		}


		if(m_current < fullName.Count)
		{
			EditorGUI.BeginChangeCheck();
			m_current = EditorGUI.Popup(position,"Method",m_current,fullName.ToArray());


			if(EditorGUI.EndChangeCheck())
			{
				valueChanged = true;
			}
			 
		}
		else
		{
			m_current = 0;
			valueChanged = true;
		}

		if(valueChanged && names.Count > m_current)
		{
			target = components[m_current];
			method = names[m_current];
			
			property.FindPropertyRelative("target").objectReferenceValue = target;
			property.FindPropertyRelative("method").stringValue = method;
		}


		EditorGUI.indentLevel = oldInner;
	}
}