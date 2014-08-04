// AGUITexture.cs
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

[AddComponentMenu("AGUI/UI/Texture")]
[RequireComponent(typeof(SpriteRenderer))]
public class AGUITexture : AGUIObject
{
	#region Header

	/// <summary>
	/// The renderer.
	/// </summary>
	[HideInInspector]
	public new SpriteRenderer renderer;

	/// <summary>
	/// The extrude.
	/// </summary>
	[SerializeField] [Range(0,32)]
	private int m_extrude = 0;

	/// <summary>
	/// The type of the sprite.
	/// </summary>
	[SerializeField]
	private SpriteMeshType m_textureType = SpriteMeshType.FullRect;

	/// <summary>
	/// The texture.
	/// </summary>
	public Texture2D texture;

	/// <summary>
	/// The sprite.
	/// </summary>
	private Sprite m_sprite = null;

	/// <summary>
	/// The scale.
	/// </summary>
	[GMinValueAttribute(1)]
	public float scale = 100;

	/// <summary>
	/// The rect.
	/// </summary>
	public Rect rect;
	
	#endregion

	#region Properties

	/// <summary>
	/// Gets the current sprite.
	/// </summary>
	/// <value>The current sprite.</value>
	public Sprite CurrentSprite
	{
		get
		{
			return m_sprite;
		}
		
		private set
		{
			m_sprite = value;
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
			return color;
		}
		set 
		{
			renderer.color = value;
		}
	}

	#endregion
	
	#region Body

	/// <summary>
	/// Init AGUIObject.
	/// </summary>
	protected override void Awake ()
	{
		renderer = GetComponent<SpriteRenderer>();
		base.Awake ();
	}

	/// <summary>
	/// Init rule for AGUIObject.
	/// (This is called when you edit values in editor)
	/// </summary>
	protected override void Init ()
	{
		if(texture == null || renderer == null)
		{
			return;
		}

		if(m_sprite != null)
		{
			//Remove old sprite!
			GameObject.DestroyImmediate(m_sprite);
		}

		if(renderer.sprite == null || (texture != null && texture != renderer.sprite.texture))
		{
			rect = new Rect(0,0,texture.width,texture.height);
		}
		
		//renderer.color = color;
		renderer.sprite = m_sprite = Sprite.Create(texture,rect,Vector2.one / 2,scale,(uint)m_extrude,m_textureType);
		renderer.sprite.name = texture.name;
	}

	/// <summary>
	/// OnEnable base.
	/// </summary>
	protected override void OnEnable()
	{
		if(!renderer)
		{
			renderer = GetComponent<SpriteRenderer>();
		}
		
		if(renderer)
		{
			renderer.enabled = true;
			renderer.hideFlags = HideFlags.HideInInspector;
		}
	}

	/// <summary>
	/// OnDisable base.
	/// </summary>
	protected override void OnDisable()
	{
		if(renderer)
		{
			renderer.enabled = false;
			renderer.hideFlags = HideFlags.None;
		}
	}

	/// <summary>
	/// OnDestroy base.
	/// </summary>
	protected override void OnDestroy ()
	{
		if(m_sprite != null)
		{
			//Remove old sprite!
			GameObject.DestroyImmediate(m_sprite);
		}
	}

	#if UNITY_EDITOR

	/// <summary>
	/// Update the AGUITexture.
	/// </summary>
	private void Update()
	{
		if(!Application.isPlaying)
		{
			if(m_sprite != renderer.sprite)
			{
				Init();
			}
		}
	}
	#endif

	#endregion
}