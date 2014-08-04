// AGUILabelEditor.cs
//
// Author:
//       Atte Vuorinen <AtteVuorinen@gmail.com>
//
// Copyright (c) 2014 Atte Vuorinen
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(AGUILabel))]
[CanEditMultipleObjects()]
public class AGUILabelEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		//AGUILabel label = (AGUILabel)target;

		base.OnInspectorGUI();

		//Load//
		if(GUILayout.Button("Load Font"))
		{
			foreach(AGUILabel label in targets)
			{
				if(label.FontSheets == null)
				{
					return;
				}

				List<Sprite> sprites = new List<Sprite>();
				List<string> names = new List<string>();

				foreach(Texture2D tex in label.FontSheets)
				{
					string texturePath  = AssetDatabase.GetAssetPath(tex);
					Object[] objects = AssetDatabase.LoadAllAssetsAtPath(texturePath);

					for(int i = 0; i < objects.Length; i++)
					{
						if(objects[i].GetType() == typeof(Sprite))
						{
							Sprite sp = objects[i] as Sprite;

							if(objects[i].name.Length == 1 && !sprites.Contains(sp) && !names.Contains(sp.name) )
							{
								names.Add(sp.name);
								sprites.Add(sp);
							}
						}
					}
				}

				label.chars = sprites.ToArray();
				label.LoadChars();
				label.ResetLabel();
			}
		}

		//Edit
		if(GUILayout.Button("Toggle Advanced Edit"))
		{
			foreach(AGUILabel label in targets)
			{
				FieldInfo advInfo = label.GetType().GetField("m_advEditMode", BindingFlags.NonPublic | BindingFlags.Instance);
				bool adv = (bool)advInfo.GetValue(target);
				
				adv = !adv;
				
				advInfo.SetValue(target,(object)adv);
				
				FieldInfo lettersInfo = label.GetType().GetField("m_letters", BindingFlags.NonPublic | BindingFlags.Instance);
				object objLetters = lettersInfo.GetValue(target);
				List<SpriteRenderer> letters = (List<SpriteRenderer>)objLetters;

				foreach(Transform tr in label.GetComponentsInChildren<Transform>())
				{
					if(tr.parent != null && tr.parent == label.transform)
					{
						tr.gameObject.hideFlags = HideFlags.None;
					}
				}

				foreach(SpriteRenderer sp in letters)
				{
					sp.gameObject.isStatic = label.gameObject.isStatic;
					
					if(!adv)
					{
						sp.gameObject.hideFlags = HideFlags.HideInHierarchy;
					}
					else
					{
						sp.gameObject.hideFlags = HideFlags.None;
					}
				}

				if(label.SpriteObject != null)
				{
					if(!adv)
					{
						label.SpriteObject.hideFlags = HideFlags.HideInHierarchy;
					}
					else
					{
						label.SpriteObject.hideFlags = HideFlags.None;
					}
				}
			}

			EditorApplication.RepaintHierarchyWindow();
		}
	}
}
