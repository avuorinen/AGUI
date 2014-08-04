// AGUIPanel.cs
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

//TODO: Make panel work with any object. (Create component that works like AGUI object.)
//TODO: Make UIPanel support nested Panels!
//TODO: Different kind Panels.

[ExecuteInEditMode]
[AddComponentMenu("AGUI/Managers/Controls/Panel")]
public class AGUIPanel : MonoBehaviour
{

	/// <summary>
	/// The camera.
	/// </summary>
	public new Camera camera;

	/// <summary>
	/// The panel's alpha.
	/// </summary>
	[Range(0,1)] [SerializeField] 
	protected float m_alpha = 1;
	
	[GSortIDAttribute]
	public string sortingLayer = "Default";
	
	public int sortingOrder = 0;


	/// <summary>
	/// Gets or sets the alpha.
	/// </summary>
	/// <value>The alpha.</value>
	public float alpha
	{
		get
		{
			return m_alpha;
		}

		set
		{
			m_alpha = value;

			AGUIObject[] objects = GetComponentsInChildren<AGUIObject>();
			
			for(int i = 0; i < objects.Length; i++)
			{
				objects[i].alpha = alpha;
			}
		}
	}

	/// <summary>
	/// Gets a value indicating whether this <see cref="AGUIPanel"/> is static.
	/// </summary>
	/// <value><c>true</c> if static; otherwise, <c>false</c>.</value>
	public bool Static
	{
		get
		{
			return gameObject.isStatic;
		}
	}

	/// <summary>
	/// Raises the validate event.
	/// </summary>
	protected virtual void OnValidate()
	{
		alpha = m_alpha;
	}

	#if UNITY_EDITOR
	protected virtual void Update()
	{
		if(Application.isPlaying)
		{
			return;
		}

		//Update AGUIObjects.
		foreach(AGUIObject obj in GetComponentsInChildren<AGUIObject>())
		{
			obj.SetPanel(this);
		}
	}

	protected void Reset()
	{
		if(camera == null)
		{
			camera = AGUIController.SharedControl != null ? AGUIController.SharedControl.camera : Camera.main;
		}
	}
	#endif
}