// GSmartDelegate.cs
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


using System.Collections.Generic;
using UnityEngine;

public sealed class GSmartDelegate
{
	public struct GSmartData
	{
		public object target;
		public VoidDelegate method;
		public bool isStatic;
		
		public GSmartData(object target, VoidDelegate method)
		{
			this.target = target;
			this.method = method;
			this.isStatic = false;
		}

		public GSmartData(object target, bool isStatic, VoidDelegate method)
		{
			this.target = target;
			this.isStatic = isStatic;
			this.method = method;
		}
	}

	#region Static
	
	public static GSmartDelegate operator + (GSmartDelegate smart, GSmartData data)
	{
		smart.Add(data);
		
		return smart;
	}
	
	public static GSmartDelegate operator - (GSmartDelegate smart, GSmartData data)
	{
		smart.Remove(data);
		
		return smart;
	}

	#endregion

	#region Header

	public delegate void VoidDelegate();
	
	private List<GSmartData> m_delegates = new List<GSmartData>();

	#endregion

	#region Core

	//TODO: Allow just add delegate. (It wont be static and it will removed when scene is changed!)
	
	/// <summary>
	/// Add the specified data.
	/// Also += Works!
	/// </summary>
	/// <param name="data">Data.</param>
	public void Add(GSmartData data)
	{
		if( (!data.isStatic && data.target == null) || data.method == null)
		{
			return;
		}

		m_delegates.Add(data);
	}

	/// <summary>
	/// Remove the specified data.
	/// Also -= Works!
	/// </summary>
	/// <param name="data">Data.</param>
	public void Remove(GSmartData data)
	{
		m_delegates.Remove(data);
	}

	public void Invoke()
	{
		m_delegates.RemoveAll( data => (!data.isStatic && data.target.Equals(null) ));
		
		foreach(GSmartData d in m_delegates)
		{
			d.method();
		}
	}

	#endregion

	#region Body

	public void Clear()
	{
		m_delegates.Clear();
	}

	public bool Contains(GSmartData data)
	{
		return m_delegates.Contains(data);
	}

	public bool Contains(VoidDelegate method)
	{
		foreach(GSmartData d in m_delegates)
		{
			if(d.method == method)
			{
				return true;
			}
		}

		return false;
	}

	#endregion
}