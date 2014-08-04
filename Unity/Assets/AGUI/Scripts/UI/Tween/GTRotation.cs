// GTRotation.cs
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

[AddComponentMenu("AGUI/UI/Tween/Rotation")]
public class GTRotation : GTween
{
	/// <summary>
	/// GTRotation using GameObject.
	/// </summary>
	/// <param name="trans">Trans.</param>
	/// <param name="curve">Curve.</param>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	public static void Tween(Transform trans, GCurve curve, Vector3 from, Vector3 to)
	{
		trans.localRotation = Quaternion.Euler(Vector3.Lerp(from,to,curve.Evaluate()));
	}

	/// <summary>
	/// From rotation.
	/// </summary>
	public Vector3 from;

	/// <summary>
	/// To rotation.
	/// </summary>
	public Vector3 to;

	/// <summary>
	/// Tween action.
	/// </summary>
	protected override void OnTween ()
	{
		Tween(transform,curve,from,to);
	}
}