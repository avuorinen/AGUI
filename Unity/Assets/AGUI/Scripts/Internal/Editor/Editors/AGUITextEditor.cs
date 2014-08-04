// AGUITextEditor.cs
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


using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[CustomEditor(typeof(AGUIText))]
public class AGUITextEditor : Editor
{
	private const float MAX_SIZE = 100;

	public Color color = Color.white;
	public float fontSize = 15;

	public override void OnInspectorGUI ()
	{
		AGUIText aText = (AGUIText)target;

		base.OnInspectorGUI ();

		GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea);
		textAreaStyle.richText = true;

		GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.richText = true;

		//GUI//

		EditorGUILayout.Separator();

		string noSizeText = aText.text;
		noSizeText = Regex.Replace(noSizeText,"</size>","",RegexOptions.IgnoreCase);
		noSizeText = Regex.Replace(noSizeText,"<size=(.*?)>","",RegexOptions.IgnoreCase);

		EditorGUILayout.LabelField("Preview");
		EditorGUILayout.TextArea(noSizeText,textAreaStyle);

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Edit");
		aText.text = EditorGUILayout.TextArea(aText.text);

		//Size
		
		GUILayout.BeginHorizontal();
		
		fontSize = EditorGUILayout.FloatField("Size",fontSize);
		
		if(GUILayout.Button("Size"))
		{
			aText.text += "<size=" + fontSize + ">Size</size>";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();

		if(GUILayout.Button("<b>Bold</b>",buttonStyle))
		{
			aText.text += "<b>Bold</b>";
			Repaint();

			EditorUtility.SetDirty (target);
		}

		if(GUILayout.Button("<i>Italic</i>",buttonStyle))
		{
			aText.text += "<i>Italic</i>";
			Repaint();

			EditorUtility.SetDirty (target);
		}

		GUILayout.EndHorizontal();

		//Color

		GUILayout.BeginHorizontal();
	
		color = EditorGUILayout.ColorField(color);
		
		if(GUILayout.Button("<color=#" + ColorToHex(color) + ">Color</color>",buttonStyle))
		{
			aText.text += "<color=#" + ColorToHex(color) + ">Color</color>";
			Repaint();
		}
		
		GUILayout.EndHorizontal();

		//TAGS

		//Style

		GUILayout.BeginHorizontal();
		
		//TODO: Better design for this.
		
		if(GUILayout.Button("<b>"))
		{
			aText.text += "<b>";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		if(GUILayout.Button("</b>"))
		{
			aText.text += "</b>";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		if(GUILayout.Button("<i>"))
		{
			aText.text += "<i>";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		if(GUILayout.Button("</i>"))
		{
			aText.text += "</i>";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		GUILayout.EndHorizontal();

		//Size

		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("<size>"))
		{
			aText.text += "<size=" + fontSize + ">";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		if(GUILayout.Button("</size>"))
		{
			aText.text += "</size>";
			Repaint();
			
			EditorUtility.SetDirty (target);
		}
		
		GUILayout.EndHorizontal();

		//Color

		GUILayout.BeginHorizontal();

		if(GUILayout.Button("<color>"))
		{
			aText.text += "<color=#" + ColorToHex(color) + ">";
			Repaint();

			EditorUtility.SetDirty (target);
		}
		
		if(GUILayout.Button("</color>"))
		{
			aText.text += "</color>";
			Repaint();

			EditorUtility.SetDirty (target);
		}

		GUILayout.EndHorizontal();
	}

	string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		return hex;
	}
	
	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		byte a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b,a);
	}
}
