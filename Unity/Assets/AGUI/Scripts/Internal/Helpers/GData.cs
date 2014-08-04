// GData.cs
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

public sealed class GData : MonoBehaviour
{
	//TODO: More usage for GData!

	/// <summary>
	/// Gets the data or add it if GameObject doesn't have it.
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="go">Go.</param>
	public static GData GetData(GameObject go)
	{
		GData data = GHelper.GetComponent<GData>(go);

		if(!data.m_inited)
		{
			data.Init();
		}

		return data;
	}

	/// <summary>
	/// The stored AGUIObject.
	/// </summary>
	public AGUIObject aguiObject;

	/// <summary>
	/// The store AGUIPanel.
	/// </summary>
	public AGUIPanel aguiPanel;

	/// <summary>
	/// Is the data inited.
	/// </summary>
	private bool m_inited = false;

	/// <summary>
	/// Init the data.
	/// </summary>
	[GIgnoreAttribute]
	public void Init()
	{
		m_inited = true;
		aguiObject = GetComponent<AGUIObject>();
		aguiPanel = GetComponent<AGUIPanel>();
	}
}