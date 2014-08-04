// GEventDelegate.cs
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

#if UNITY_EDITOR || (!UNITY_FLASH && !NETFX_CORE)
#define USE_REFLECTION
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public static class GEventDelegateHelper
{
	#region Static

	/// <summary>
	/// Invokes the list of GEventDelegates.
	/// </summary>
	/// <param name="list">List.</param>
	public static void Invoke(this List<GEventDelegate> list)
	{
		foreach(GEventDelegate d in list)
		{
			d.Invoke();
		}
	}

	/// <summary>
	/// Add the GEventDelegate to list.
	/// </summary>
	/// <param name="list">List.</param>
	/// <param name="gDelegate">GEventDelegate.</param>
	public static void Add(this List<GEventDelegate> list, GEventDelegate.CallbackDelegate gDelegate)
	{
		list.Add(new GEventDelegate(gDelegate));
	}

	/// <summary>
	/// Invokes the array of GEventDelegates.
	/// </summary>
	/// <param name="array">Array.</param>
	public static void Invoke(this GEventDelegate[] array)
	{
		foreach(GEventDelegate d in array)
		{
			d.Invoke();
		}
	}

	#endregion
}

[System.Serializable]
public sealed class GEventDelegate
{
	/// <summary>
	/// CallBack Delegate.
	/// </summary>
	public delegate void CallbackDelegate();

	/// <summary>
	/// The CallBack delegate that will be invoked.
	/// </summary>
	public CallbackDelegate callback;

	/// <summary>
	/// Target component that contains delegate.
	/// </summary>
	public Component target;

	/// <summary>
	/// Name of Method that will be invoked.
	/// </summary>
	public string method;

	#if UNITY_EDITOR

	/* These values are used only in Editor,
	 * so compiler will throw warning because
	 * it doesn't notice values are actualy begin used!
	 * 
	 * 0414 - private field is assigned but not used
	 */

	#pragma warning disable 0414
	/// <summary>
	/// Is set to be true if callback is set.
	/// This value is used only in EDITOR!
	/// </summary>
	[SerializeField]
	private bool m_hasCallback;

	#pragma warning restore 0414

	#endif

	/// <summary>
	/// Initializes a new instance of the <see cref="GEventDelegate"/> class.
	/// </summary>
	/// <param name="del">Del.</param>
	public GEventDelegate(CallbackDelegate del)
	{
		#if UNITY_EDITOR
		m_hasCallback = true;
		#endif

		callback = del;
	}

	/// <summary>
	/// Invokes GEventDelegate.
	/// </summary>
	public void Invoke()
	{
		//Uses delegate//
		if(callback != null)
		{
			callback();
			return;
		}

		//Uses reflection//
		if(target == null || method == null)
		{
			return;
		}

		#if USE_REFLECTION

		//target.GetType().GetMethod(method,BindingFlags.Public | BindingFlags.Instance).Invoke(target,null);

		//Creates callback delegate from target and method, so it won't need use reflection any more!
		callback = System.Delegate.CreateDelegate(typeof(CallbackDelegate),target,method) as CallbackDelegate;
		callback();

		#else

		//TODO: Better system for handing non MonoBehaviour objects!

		MonoBehaviour mono = target as MonoBehaviour;

		if(mono)
		{
			mono.Invoke(method,0);
		}
		else
		{
			target.SendMessage(method,SendMessageOptions.DontRequireReceiver);
		}

		#endif
	}	
}