// GUScale.cs
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

[RequireComponent(typeof(Renderer))]
[AddComponentMenu("AGUI/UI/Utilities/Scale")]
[ExecuteInEditMode]
public sealed class GUScale : GUtilitiesBase 
{
	//TODO: Run in editor (review), Run once

	#region Header

	public enum ScaleType
	{
		/// <summary>
		/// Fill screen.
		/// </summary>
		Fill,

		/// <summary>
		/// Aspect scale.
		/// </summary>
		Aspect,

		/// <summary>
		/// Scale width.
		/// </summary>
		Width,

		/// <summary>
		/// scale height.
		/// </summary>
		Height
	}

	public enum Axis
	{
		/// <summary>
		/// Use x axis.
		/// </summary>
		X = (1 << 0),

		/// <summary>
		/// Use y axis.
		/// </summary>
		Y = (1 << 1),

		/// <summary>
		/// Use z axis.
		/// </summary>
		Z = (1 << 2)
	}

	public enum SwapAxis
	{
		/// <summary>
		/// Normal.
		/// </summary>
		XYZ,

		/// <summary>
		/// Swap X and Y.
		/// </summary>
		YXZ,

		/// <summary>
		/// Swap X and Z.
		/// </summary>
		ZYX,

		/// <summary>
		/// Swap Y and Z.
		/// </summary>
		XZY,

		/// <summary>
		/// Swap X and Y and Z.
		/// </summary>
		ZXY
	}

	/// <summary>
	/// The camera.
	/// </summary>
	public new Camera camera;

	/// <summary>
	/// The mode.
	/// </summary>
	public ScaleType mode;

	/// <summary>
	/// The axis.
	/// </summary>
	[GBitFlagEnumAttribute]
	public Axis axis = Axis.X | Axis.Y | Axis.Z;

	/// <summary>
	/// The swap axis.
	/// </summary>
	public SwapAxis swapAxis;

	/// <summary>
	/// The scale.
	/// </summary>
	public Vector3 scale = Vector3.one;

	/// <summary>
	/// The orginal scale.
	/// </summary>
	private Vector3 m_orginalScale = Vector3.zero;

	/// <summary>
	/// The orginal bounds.
	/// </summary>
	private Vector3 m_orginalBounds = Vector3.zero;

	/// <summary>
	/// Store AGUIBox.
	/// </summary>
	private AGUIBox m_box = null;

	#endregion

	#region Body

	/// <summary>
	/// Scale this instance.
	/// </summary>
	public void Scale()
	{
		GUAction();
	}

	protected override void GUAction ()
	{
		if(runMode != RunMode.Update && Application.isPlaying)
		{
			this.enabled = false;
		}
		
		if(camera == null)
		{
			return;
		}
		
		if(m_orginalScale == Vector3.zero || m_orginalBounds == Vector3.zero)
		{
			return;
		}
		
		Vector3 size = Vector3.zero;
		
		float w = camera.aspect / m_orginalBounds.x * camera.orthographicSize * 2 * scale.x;
		float h = 1 / m_orginalBounds.y * camera.orthographicSize * 2 * scale.y;
		
		switch(mode)
		{
			
		case ScaleType.Fill:
			size = new Vector3(w, h, scale.z);
			break;
			
		case ScaleType.Width:
			size = new Vector3(w, scale.y, scale.z);
			break;
			
		case ScaleType.Height:
			size = new Vector3(scale.x, h ,scale.z);
			break;
			
		case ScaleType.Aspect:
			size = new Vector3( scale.x, scale.y, scale.z) * camera.aspect;
			break;
			
		}
		
		if(!axis.HasFlag(Axis.X | Axis.Y | Axis.Z))
		{
			size = new Vector3( (axis.HasFlag(Axis.X) ? size.x : m_orginalScale.x), (axis.HasFlag(Axis.Y) ? size.y : m_orginalScale.y), (axis.HasFlag(Axis.Z) ? size.z : m_orginalScale.z) );
		}
		
		//Swap//
		if(swapAxis == SwapAxis.ZYX)
		{
			size = new Vector3(size.z,size.y,size.x);
		}
		else if(swapAxis == SwapAxis.XZY)
		{
			size = new Vector3(size.x,size.z,size.y);
		}
		else if(swapAxis == SwapAxis.YXZ)
		{
			size = new Vector3(size.y,size.x,size.z);
		}
		else if(swapAxis == SwapAxis.ZXY)
		{
			size = new Vector3(size.z,size.x,size.y);
		}
		
		if(m_box)
		{
			if(mode == ScaleType.Fill || mode == ScaleType.Width)
			{
				size.x -= m_box.Left.renderer.bounds.size.x + m_box.Right.renderer.bounds.size.x;
			}
			
			if(mode == ScaleType.Fill || mode == ScaleType.Height)
			{
				size.y -= m_box.Top.renderer.bounds.size.y + m_box.Bottom.renderer.bounds.size.y;
			}
			
			m_box.Scale = size;
		}
		else
		{
			transform.localScale = size;
		}
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected override void Awake()
	{
		base.Awake();

		Renderer cachedRenderer = transform.renderer;
		m_box = GetComponent<AGUIBox>();
		
		m_orginalScale = transform.localScale = Vector3.one;
		
		if(m_box)
		{
			m_orginalBounds.x = m_box.Top.renderer.bounds.size.x;
			m_orginalBounds.y = m_box.Left.renderer.bounds.size.y;
		}
		else if(cachedRenderer != null && cachedRenderer.bounds.size != Vector3.zero)
		{
			m_orginalBounds = cachedRenderer.bounds.size;
		}
		else
		{
			m_orginalBounds = transform.localScale;
		}
	}
	
	/// <summary>
	/// Reset this instance.
	/// </summary>
	private void Reset()
	{
		camera = Camera.main;
	}

	#endregion
}
