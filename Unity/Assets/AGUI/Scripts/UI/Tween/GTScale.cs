// GTScale.cs
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

[AddComponentMenu("AGUI/UI/Tween/Scale")]
public class GTScale : GTween
{
	/// <summary>
	/// GTScale using GameObject.
	/// </summary>
	/// <param name="trans">Trans.</param>
	/// <param name="curve">Curve.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	public static void Tween(Transform trans, GCurve curve, Vector3 from, Vector3 to)
	{
		trans.localScale = Vector3.Lerp(from,to,curve.Evaluate());
	}

	/// <summary>
	/// From scale.
	/// </summary>
	public Vector3 from;

	/// <summary>
	/// To scale.
	/// </summary>
	public Vector3 to;

	/// <summary>
	/// Stored GUScale.
	/// </summary>
	private GUScale m_scale = null;

	/// <summary>
	/// Stored AGUIBox.
	/// </summary>
	private AGUIBox m_box = null;

	/// <summary>
	/// Inits GTScale.
	/// </summary>
	protected override void Awake ()
	{
		base.Awake();
		
		m_scale = GetComponent<GUScale>();

		if(m_scale == null)
		{
			m_box = GetComponent<AGUIBox>();
		}
	}

	/// <summary>
	/// Tween action.
	/// </summary>
	protected override void OnTween ()
	{
		if(m_scale != null)
		{
			m_scale.scale = Vector3.Lerp(from,to,curve.Evaluate());
			m_scale.Scale();
		}
		else if(m_box)
		{
			m_box.Scale = Vector2.Lerp(from,to,curve.Evaluate());
		}
		else
		{
			Tween(transform,curve,from,to);
		}
	}
}