// AGUISprite.cs
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

//TODO: Tilling, etc.

[AddComponentMenu("AGUI/UI/Sprite")]
[RequireComponent(typeof(SpriteRenderer))]
public class AGUISprite : AGUIObject
{
	#region Header

	/// <summary>
	/// The cachedRenderer.
	/// </summary>
	[HideInInspector]
	public SpriteRenderer cachedRenderer;

	/// <summary>
	/// The pixel to unit.
	/// </summary>
	[SerializeField][GReadOnlyAttribute]
	private float m_pixelToUnit;

	/// <summary>
	/// The sprite.
	/// </summary>
	[SerializeField]
	private Sprite m_sprite;

	/// <summary>
	/// The material.
	/// </summary>
	[SerializeField]
	private Material m_material;

	/// <summary>
	/// The pivot.
	/// </summary>
	public Vector2 pivot = new Vector2(0.5f,0.5f);

	#region Advanced

	/// <summary>
	/// The type of the sprite.
	/// </summary>
	[SerializeField]
	private SpriteMeshType m_spriteType = SpriteMeshType.FullRect;

	/// <summary>
	/// The extrude.
	/// </summary>
	[SerializeField][Range(0,32)]
	private int m_extrude = 0;

	//[GMinValueAttribute(1)]
	//public float scale = 100;

	#endregion

	/// <summary>
	/// The rect.
	/// </summary>
	[SerializeField]
	private Rect m_rect;

	/// <summary>
	/// The orginal rect.
	/// </summary>
	[SerializeField][GReadOnlyAttribute(32)]
	private Rect m_orginalRect;

	/// <summary>
	/// The create sprite.
	/// </summary>
	private Sprite m_createSprite;
	
	#endregion

	#region Properties

	/// <summary>
	/// Gets the pixel to unit.
	/// </summary>
	/// <value>The pixel to unit.</value>
	public float PixelToUnit
	{
		get
		{
			return m_pixelToUnit;
		}
		
		private set
		{
			m_pixelToUnit = value;
		}
	}

	/// <summary>
	/// Gets or sets the current sprite.
	/// </summary>
	/// <value>The current sprite.</value>
	public Sprite CurrentSprite
	{
		get
		{
			return m_sprite;
		}
		
		set
		{
			m_sprite = value;
			Init();
		}
	}

	/// <summary>
	/// Gets or sets the current material.
	/// </summary>
	/// <value>The current material.</value>
	public Material CurrentMaterial
	{
		get
		{
			return m_material;
		}
		
		set
		{
			m_material = value;
			
			if(cachedRenderer != null)
			{
				cachedRenderer.material = m_material;
			}
		}
	}

	/// <summary>
	/// Gets or sets the current rect.
	/// </summary>
	/// <value>The current rect.</value>
	public Rect CurrentRect
	{
		get
		{
			return m_rect;
		}
		
		set
		{
			m_rect = value;
			Init();
		}
	}

	/// <summary>
	/// Gets the orginal rect.
	/// </summary>
	/// <value>The orginal rect.</value>
	public Rect OrginalRect 
	{
		get
		{
			return m_orginalRect;
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
			if(cachedRenderer == null)
			{
				return Color.black;
			}

			return cachedRenderer.color;
		}
		set 
		{
			if(cachedRenderer == null)
			{
				return;
			}

			cachedRenderer.color = value;
		}
	}

	#endregion

	#region Body

	/// <summary>
	/// Updates the sprite.
	/// </summary>
	public void UpdateSprite()
	{
		if(m_sprite == null || cachedRenderer == null)
		{
			if(!cachedRenderer)
			{
				cachedRenderer = GetComponent<SpriteRenderer>();
			}

			/*
			if(cachedRenderer)
			{
				cachedRenderer.sprite = null;
			}
			*/

			return;
		}

		m_pixelToUnit = Mathf.Min(m_sprite.textureRect.width / m_sprite.bounds.size.x,m_sprite.textureRect.height / m_sprite.bounds.size.y);

		if(cachedRenderer.sprite != null && m_createSprite != null && (m_createSprite.texture != m_sprite.texture || m_orginalRect != m_sprite.rect))
		{
			m_orginalRect = m_sprite.rect;
			m_rect = m_sprite.rect;
		}

		//Remove old sprite!
		GameObject.DestroyImmediate(m_createSprite);

		//Update Sprite//
		cachedRenderer.sprite = m_createSprite = Sprite.Create(m_sprite.texture,m_rect,pivot,m_pixelToUnit,(uint)m_extrude,m_spriteType);
		cachedRenderer.sharedMaterial = m_material;
		
		m_createSprite.name = m_sprite.name;
	}

	/// <summary>
	/// Init rule for AGUIObject.
	/// (This is called when you edit values in editor)
	/// </summary>
	protected override void Init()
	{
		if(!Application.isPlaying)
		{
			UpdateSprite();
		}
	}

	/// <summary>
	/// Used for first time init.
	/// Always remember call base.Reset(); !!
	/// </summary>
	protected override void Reset ()
	{
		if(!cachedRenderer)
		{
			cachedRenderer = GetComponent<SpriteRenderer>();
		}

		if(cachedRenderer)
		{
			m_material = GHelper.GetDefaultMaterial();

			if(m_sprite == null)
			{
				m_sprite = cachedRenderer.sprite;
				color = cachedRenderer.color;

				if(m_sprite != null)
				{
					m_rect = m_sprite.rect;
					m_orginalRect = m_sprite.rect;
				}

				cachedRenderer.enabled = true;
				cachedRenderer.hideFlags = HideFlags.HideInInspector;
			}

			Init();
		}

		base.Reset ();
	}

	/// <summary>
	/// OnEnable base.
	/// </summary>
	protected override void OnEnable()
	{
		if(cachedRenderer)
		{
			cachedRenderer.enabled = true;
			cachedRenderer.hideFlags = HideFlags.HideInInspector;
		}

		base.OnEnable();
	}

	/// <summary>
	/// OnDisable base.
	/// </summary>
	protected override void OnDisable()
	{
		if(cachedRenderer)
		{
			cachedRenderer.enabled = false;
			cachedRenderer.hideFlags = HideFlags.None;
		}

		base.OnDisable();
	}

	/// <summary>
	/// OnDestroy base.
	/// </summary>
	protected override void OnDestroy ()
	{
		if(cachedRenderer != null)
		{
			cachedRenderer.enabled = true;
			cachedRenderer.sprite = m_sprite;
		}

		if(m_createSprite != null)
		{
			//Remove old sprite!
			GameObject.DestroyImmediate(m_createSprite);
		}
	}

	#if UNITY_EDITOR
	
	/// <summary>
	///Creates sprite if game isn't running.
	/// </summary>
	private void Update()
	{
		if(!Application.isPlaying)
		{
			if(m_sprite != m_createSprite)
			{
				Init();
			}
		}
	}
	
	#endif

	#endregion
}