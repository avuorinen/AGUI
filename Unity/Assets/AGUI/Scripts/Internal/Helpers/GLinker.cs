// GLinker.cs
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

//

using UnityEngine;
using System.Collections;

/// <summary>
/// Glinker.
/// Allow easier object linking.
/// </summary>
public class GLinker<T>
{
	public delegate bool VoidLinkerDelegate(ref GLinker<T> gLinker);

	/// <summary>
	/// Try find another link that is inited if find any return true and set link override this link.
	/// Return false if need set links.
	/// </summary>
	public VoidLinkerDelegate checkLinks = delegate {return false;};

	/// <summary>
	/// Set links.
	/// </summary>
	public VoidLinkerDelegate setLinks = delegate { throw new System.Exception("Override default setLinks delegate!"); };

	/// <summary>
	/// The linker.
	/// Allow this class be sended as reference.
	/// </summary>
	private GLinker<T> m_linker;
	
	private T m_linkedInstance;
	public T LinkedInstance
	{
		get
		{
			return m_linker.m_linkedInstance;
		}

		set
		{
			if(LinkedInstance == null || !IsInited)
			{
				m_linker.m_linkedInstance = value;
			}
		}
	}
	
	public bool IsInited
	{
		get;
		private set;
	}

	public GLinker()
	{
		m_linker = this;
	}

	public GLinker(T instance)
	{
		m_linker = this;
		LinkedInstance = instance;
	}

	public void SetNewInstance(T instance)
	{
		m_linker.m_linkedInstance = instance;
	}

	/// <summary>
	/// Inits the link.
	/// </summary>
	public void InitLink()
	{
		if(!checkLinks(ref m_linker))
		{
			if(!setLinks(ref m_linker))
			{
				return;
			}
		}

		IsInited = true;

	}

	/// <summary>
	/// Checks the link.
	/// </summary>
	/// <returns><c>true</c>, if link was checked, <c>false</c> otherwise.</returns>
	/// <param name="instance">Instance.</param>
	public bool CheckLink(GLinker<T> instance)
	{
		if(instance.IsInited && this != instance)
		{
			return true;
		}

		return false;
	}
}