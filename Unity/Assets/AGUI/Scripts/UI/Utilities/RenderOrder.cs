// RenderOrder.cs
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
// THE SOFTWARE.// GEventDelegate.cs
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

[ExecuteInEditMode]
[AddComponentMenu("AGUI/UI/Utilities/RenderOrder")]
public sealed class RenderOrder : MonoBehaviour
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
			if(!renderer)
			{
				return m_sortingLayer;
			}
			
			return renderer.sortingLayerName;
		}
		
		set
		{
			m_sortingLayer = value;
			
			if(renderer)
			{
				renderer.sortingLayerName = m_sortingLayer;
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
			if(!renderer)
			{
				return m_sortingOrder;
			}
			
			return renderer.sortingOrder;
		}
		
		set
		{
			m_sortingOrder = value;
			
			if(renderer)
			{
				renderer.sortingOrder = m_sortingOrder;
			}
		}
	}

	#endregion

	#region Body

	/// <summary>
	/// Raises the validate event.
	/// </summary>
	private void OnValidate()
	{
		SortingLayer = m_sortingLayer;
		SortingOrder = m_sortingOrder;
	}

	/// <summary>
	/// Raises the destroy event.
	/// Reset values.
	/// </summary>
	private void OnDestroy()
	{
		if(renderer)
		{
			renderer.sortingLayerID = 0;
			renderer.sortingOrder = 0;
		}
	}

	#endregion
}

