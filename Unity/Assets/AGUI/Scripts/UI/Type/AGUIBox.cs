// AGUIBox.cs
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

[AddComponentMenu("AGUI/UI/Box")]
[RequireComponent(typeof(SpriteRenderer))]
public sealed class AGUIBox : AGUIObject
{

	//UNDONE: Support for long objects. (Some times there is problems with long objects with GUScale.)

	public Sprite sprite;

	[HideInInspector]
	public AGUISprite[] sprites = new AGUISprite[9];

	[SerializeField]
	private Vector2 m_offset;

	[SerializeField]
	private Vector2 m_scale = Vector2.one;
	
	#region Properties

	public AGUISprite Center
	{
		get
		{
			return sprites[0];
		}

		set
		{
			sprites[0] = value;
		}
	}

	public AGUISprite Top
	{
		get
		{
			return sprites[1];
		}

		set
		{
			sprites[1] = value;
		}
	}

	public AGUISprite Bottom
	{
		get
		{
			return sprites[2];
		}

		set
		{
			sprites[2] = value;
		}
	}

	public AGUISprite Left
	{
		get
		{
			return sprites[3];
		}

		set
		{
			sprites[3] = value;
		}
	}

	public AGUISprite Right
	{
		get
		{
			return sprites[4];
		}
		
		set
		{
			sprites[4] = value;
		}
	}

	public AGUISprite TopLeft
	{
		get
		{
			return sprites[5];
		}
		
		set
		{
			sprites[5] = value;
		}
	}

	public AGUISprite TopRight
	{
		get
		{
			return sprites[6];
		}
		
		set
		{
			sprites[6] = value;
		}
	}

	public AGUISprite BottomLeft
	{
		get
		{
			return sprites[7];
		}
		
		set
		{
			sprites[7] = value;
		}
	}

	public AGUISprite BottomRight
	{
		get
		{
			return sprites[8];
		}
		
		set
		{
			sprites[8] = value;
		}
	}

	public Vector2 Offset
	{
		get
		{
			return m_offset;
		}
	}

	public Vector2 Scale
	{
		get
		{
			return m_scale;
		}

		set
		{
			m_scale = value;
			UpdatePositions();
		}
	}

	public new SpriteRenderer renderer
	{
		get;
		private set;
	}

	protected override Color ColorTint 
	{
		get {

			//TODO: Better color getting here!
			return renderer.color;
		}

		set {

			#if UNITY_EDITOR

			if(renderer)
			{
				renderer.color = value;
			}

			#endif

			//color = value;

			foreach(AGUISprite sp in sprites)
			{
				if(sp == null)
				{
					continue;
				}

				sp.RawColor = color;
			}
		}
	}

	#endregion

	protected override void Init ()
	{
		renderer = GetComponent<SpriteRenderer>();

		if(renderer)
		{
			renderer.sprite = sprite;
		}

		if(sprites != null)
		{
			foreach(AGUISprite sp in sprites)
			{
				if(sp == null)
				{
					continue;
				}

				sp.simulateNonAGUI = simulateNonAGUI;
				sp.SortingLayer = SortingLayer;
				sp.SortingOrder = SortingOrder;
			}
		}

		UpdatePositions();
	}

	protected override void OnEnable ()
	{
		if(renderer)
		{
			renderer.enabled = false;
			renderer.hideFlags = HideFlags.HideInInspector;
		}

		base.OnEnable ();
	}

	protected override void OnDestroy ()
	{
		if(renderer)
		{
			renderer.enabled = true;
			renderer.hideFlags = HideFlags.None;
		}

		base.OnDestroy ();
	}

	public void UpdatePositions()
	{
		if(sprites == null || Center == null)
		{
			return;
		}
		
		Vector3 position;

		Vector3 oldScale = transform.localScale;
		Quaternion oldRotation = transform.localRotation;

		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;

		foreach(AGUISprite sp in sprites)
		{
			if(sp == null)
			{
				continue;
			}

			//sp.transform.parent = null;
			sp.transform.localScale = Vector3.one;

			//sp.transform.parent = transform;

			position = Center.transform.localPosition;
			
			//Top
			if(sp.pivot == new Vector2(0.5f,0))
			{
				sp.transform.localScale = new Vector3(m_scale.x,1,1);
				position.y += Center.renderer.bounds.extents.y;
			}
			//Bottom
			else if(sp.pivot == new Vector2(0.5f,1))
			{
				sp.transform.localScale = new Vector3(m_scale.x,1,1);
				position.y -= Center.renderer.bounds.extents.y;
			}
			//Left
			else if(sp.pivot == new Vector2(1,0.5f))
			{
				sp.transform.localScale = new Vector3(1,m_scale.y,1);
				position.x -= Center.renderer.bounds.extents.x;
			}
			//Right
			else if(sp.pivot == new Vector2(0,0.5f))
			{
				sp.transform.localScale = new Vector3(1,m_scale.y,1);
				position.x += Center.renderer.bounds.extents.x;
			}
			//TopLeft
			else if(sp.pivot == new Vector2(1,0))
			{
				position.y += Center.renderer.bounds.extents.y;
				position.x -= Center.renderer.bounds.extents.x;
			}
			//TopRight
			else if(sp.pivot == new Vector2(0,0))
			{
				position.y += Center.renderer.bounds.extents.y;
				position.x += Center.renderer.bounds.extents.x;
			}
			//BottomLeft
			else if(sp.pivot == new Vector2(1,1))
			{
				position.y -= Center.renderer.bounds.extents.y;
				position.x -= Center.renderer.bounds.extents.x;
			}
			//BottomRight
			else if(sp.pivot == new Vector2(0,1))
			{
				position.y -= Center.renderer.bounds.extents.y;
				position.x += Center.renderer.bounds.extents.x;
			}
			//Center
			else
			{
				sp.transform.localScale = new Vector3(m_scale.x,m_scale.y,1);
				position = Vector3.zero;
			}

			sp.transform.localPosition = position;
		}

		transform.localScale = oldScale;
		transform.localRotation = oldRotation;
	}
}