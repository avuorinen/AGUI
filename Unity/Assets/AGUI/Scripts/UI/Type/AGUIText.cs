// AGUIText.cs
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

using UnityEngine;
using System.Collections;

/// <summary>
/// AGUI text uses Text Mesh
/// </summary>
[AddComponentMenu("AGUI/UI/Text")]
[RequireComponent(typeof(TextMesh))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class AGUIText : AGUIObject 
{
	//TODO: Text effects.

	/// <summary>
	/// The TextMesh
	/// </summary>
	private TextMesh m_textMesh;

	/// <summary>
	/// Gets or sets the text of TextMesh.
	/// </summary>
	/// <value>The text.</value>
	public string text
	{
		get
		{
			return m_textMesh.text;
		}

		set
		{
			m_textMesh.text = value;
		}
	}

	/// <summary>
	/// Custom color rule for object.
	/// For example, UILabel have multiple objects that color it changes.
	/// </summary>
	/// <value>The color tint.</value>
	protected override Color ColorTint 
	{
		get 
		{
			return m_textMesh.color;
		}
		set 
		{
			if(m_textMesh != null)
			{
				m_textMesh.color = value;
			}
		}
	}

	/// <summary>
	/// Init AGUIObject.
	/// </summary>
	protected override void Awake ()
	{
		base.Awake ();
	}

	/// <summary>
	/// Init rule for AGUIObject.
	/// (This is called when you edit values in editor)
	/// </summary>
	protected override void Init ()
	{
		if(m_textMesh == null)
		{
			m_textMesh = GetComponent<TextMesh>();
		}
	}

	/// <summary>
	/// OnEnable base.
	/// </summary>
	protected override void OnEnable ()
	{
		renderer.enabled = true;
	}

	/// <summary>
	/// OnDisable base.
	/// </summary>
	protected override void OnDisable ()
	{
		renderer.enabled = false;
	}
}
