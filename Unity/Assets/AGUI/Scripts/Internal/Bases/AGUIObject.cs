// AGUIObject.cs
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

//TODO: Better system for group Tween objects.
//TODO: Support AGUIPanel. (Use transform.root.GetComponent<AGUIPanel>(); )

[ExecuteInEditMode]
public abstract class AGUIObject : MonoBehaviour
{
	#region Header
	/// <summary>
	/// The sorting layer.
	/// </summary>
	[GSortIDAttribute] [SerializeField]
	private string m_sortingLayer = "Default";

	/// <summary>
	/// The sorting order.
	/// </summary>
	[SerializeField]
	private int m_sortingOrder;

	//TODO: Tell more about this!
	/// <summary>
	/// The simulate nonAGUI object.
	/// </summary>
	public bool simulateNonAGUI = false;

	private Renderer m_cachedRenderer;

	/// <summary>
	/// The color.
	/// </summary>
	[SerializeField]
	private Color m_color = Color.white;

	/// <summary>
	/// The alpha.
	/// </summary>
	[SerializeField][Range(0,1)]
	private float m_alpha = 1;

	#endregion

	#region Panel

	[SerializeField][HideInInspector]
	private AGUIPanel m_panel;

	[SerializeField][HideInInspector]
	private Transform m_root;

	public Transform Root
	{
		get
		{
			return m_root;
		}

		private set
		{
			m_root = value;
		}
	}

	public void SetPanel(AGUIPanel panel)
	{
		Root = panel.transform.root;
		m_panel = panel;
		HandlePanel();
	}

	private void HandlePanel()
	{
		if(Root != transform.root)
		{
			m_panel = null;
		}

		SortingOrder = m_sortingOrder;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets the sorting layer.
	/// </summary>
	/// <value>The sorting layer.</value>
	public string SortingLayer
	{
		get
		{
			if(!m_cachedRenderer)
			{
				return m_sortingLayer;
			}
			
			return m_cachedRenderer.sortingLayerName;
		}
		
		set
		{
			m_sortingLayer = value;
			
			if(m_cachedRenderer)
			{
				m_cachedRenderer.sortingLayerName = m_sortingLayer;
			}
		}
	}

	/// <summary>
	/// Gets or sets the sorting order.
	/// </summary>
	/// <value>The sorting order.</value>
	public int SortingOrder
	{
		get
		{
			if(!m_cachedRenderer)
			{
				return m_sortingOrder;
			}
			
			return m_cachedRenderer.sortingOrder;
		}
		
		set
		{
			m_sortingOrder = value;
			
			if(m_cachedRenderer)
			{
				m_cachedRenderer.sortingOrder = m_sortingOrder + (m_panel ? m_panel.sortingOrder : 0);
			}
		}
	}

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
			FinalColor = m_color;
		}
	}

	/// <summary>
	/// Gets color variable that uses alpha variable.
	/// Sets raw color.
	/// (DONT SET this value on ColorTint.)
	/// </summary>
	/// <value>The color.</value>
	public Color color
	{
		get
		{
			return FinalColor;
		}
		
		set
		{
			m_color = value;
			FinalColor = m_color;
		}
	}

	/// <summary>
	/// Custom color rule for object.
	/// For example, UILabel have multiple objects that color it changes.
	/// Default: Get and Set sharedMaterial's color. (Prevent leaks)
	/// </summary>
	protected virtual Color ColorTint
	{
		get
		{
			if(m_cachedRenderer)
			{
				return m_cachedRenderer.sharedMaterial.color;
			}

			return m_color;
		}
		
		set
		{
			if(m_cachedRenderer)
			{
				m_cachedRenderer.sharedMaterial.color = value;
			}

			//m_color = value;
		}
	}

	/// <summary>
	/// Gets color value that doesn't use about alpha variable.
	/// Sets raw color
	/// </summary>
	/// <value>The color of the raw.</value>
	public Color RawColor
	{
		get
		{
			return m_color;
		}
		
		set
		{
			color = value;
		}
	}


	/// <summary>
	/// Set & Get color from ColorTint,
	/// but it sets alpha from panel.
	/// </summary>
	private Color FinalColor
	{
		get
		{
			//ColorTint returns color from component. (Maybe should use RawColor to get final color.) (Now can return totaly different color if color is edited from sprite m_renderer.)
			//Color final = ColorTint; // or color
			Color final = RawColor;
			
			final.a *= alpha;
			
			return final;
		}
		
		set
		{
			Color final = value;
			
			//set master alpha
			final.a = alpha * value.a;
			
			ColorTint = final;
		}
	}

	#endregion

	#region Body
	
	/// <summary>
	/// Init rule for AGUIObject.
	/// (This is called when you edit values in editor)
	/// </summary>
	protected abstract void Init();
	
	/// <summary>
	/// Init AGUIObject.
	/// </summary>
	protected virtual void Awake()
	{
		m_cachedRenderer = GetComponent<Renderer>();
		
		HandlePanel();
		Init();
	}

	/// <summary>
	/// OnEnable base.
	/// </summary>
	protected virtual void OnEnable()
	{
		
	}

	/// <summary>
	/// OnDisable base.
	/// </summary>
	protected virtual void OnDisable()
	{
		
	}

	/// <summary>
	/// OnDestroy base.
	/// </summary>
	protected virtual void OnDestroy()
	{
		
	}

	/// <summary>
	/// Used for first time init.
	/// Always remember call base.Reset(); !!
	/// </summary>
	protected virtual void Reset()
	{
		if(m_cachedRenderer != null)
		{
			if(m_cachedRenderer.sortingLayerName != "")
			{
				m_sortingLayer = m_cachedRenderer.sortingLayerName;
			}
			
			m_sortingOrder = m_cachedRenderer.sortingOrder;
		}
	}

	/// <summary>
	/// Raises the validate event.
	/// </summary>
	protected void OnValidate()
	{
		if(m_cachedRenderer != null)
		{
			SortingLayer = m_sortingLayer;
			SortingOrder = m_sortingOrder;
		}

		FinalColor = RawColor;
		
		Init();
	}

	#endregion
}