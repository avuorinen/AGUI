// GTColor.cs
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
using UnityEngine;

[AddComponentMenu("AGUI/UI/Tween/Color")]
public class GTColor : GTween
{
	/// <summary>
	/// GTColor using GameObject.
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="curve">Curve.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="shared">If set to <c>true</c> shared.</param>
	public static void Tween(GameObject go, GCurve curve, Color from, Color to, bool shared)
	{
		Tween(GData.GetData(go),curve,from,to,shared);
	}

	/// <summary>
	/// GTColor using GData.
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="curve">Curve.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="shared">If set to <c>true</c> shared.</param>
	public static void Tween(GData data, GCurve curve, Color from, Color to, bool shared)
	{
		GColorHelper.SetColor(data,Color.Lerp(from,to,curve.Evaluate()),shared);
	}


	/// <summary>
	/// From color.
	/// </summary>
	public Color from = Color.white;

	/// <summary>
	/// To color.
	/// </summary>
	public Color to = Color.gray;

	/// <summary>
	/// Use shared material.
	/// </summary>
	[SerializeField]
	protected bool m_sharedMaterial = false;

	/// <summary>
	/// Store GData.
	/// </summary>
	protected GData m_data;

	/// <summary>
	/// Store default color.
	/// </summary>
	private Color m_defaultColor;

	/// <summary>
	/// Init.
	/// </summary>
	protected override void Awake ()
	{
		if(m_sharedMaterial)
		{
			if(renderer)
			{
				m_defaultColor = renderer.sharedMaterial.color;
			}
		}

		m_data = GData.GetData(gameObject);

		base.Awake ();
	}

	/// <summary>
	/// Tween action.
	/// </summary>
	protected override void OnTween ()
	{
		if(m_data != null)
		{
			Tween(m_data,curve,from,to,m_sharedMaterial);
		}
		else
		{
			Tween(gameObject,curve,from,to,m_sharedMaterial);
		}
	}

	/// <summary>
	/// Raises the application quit event.
	/// Sets shared material color back to default.
	/// </summary>
	private void OnApplicationQuit()
	{
		if(m_sharedMaterial)
		{
			renderer.sharedMaterial.color = m_defaultColor;
		}
	}
}