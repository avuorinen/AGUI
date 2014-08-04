// GUtilitiesBase.cs
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

/// <summary>
/// Important Information!
/// Always remember call base.
/// </summary>
public abstract class GUtilitiesBase : MonoBehaviour
{
	//TODO: Better handling for Awake and Start!

	public enum RunMode
	{
		Once,
		Update,
		Event
	}

	public RunMode runMode;
	
	/// <summary>
	/// Stores cached transform.
	/// </summary>
	[HideInInspector]
	public new Transform transform;

	protected abstract void GUAction();

	/// <summary>
	/// Inits GUtilites.
	/// Call this before your code!
	/// </summary>
	protected virtual void Awake()
	{
		transform = GetComponent<Transform>();
	}

	/// <summary>
	/// Handles GUtilities Base mechanics.
	/// Call this after your code!
	/// </summary>
	protected virtual void Start()
	{
		if(runMode == RunMode.Event && GGlobalEvents.CanUseEvents)
		{
			GGlobalEvents.OnScreenChange.Add( new GSmartDelegate.GSmartData(this,GUActionHandler) );
		}
		
		GUActionHandler();
	}

	/// <summary>
	/// No special rule for Calling.
	/// (Handles GUtilies Action)
	/// </summary>
	protected virtual void Update()
	{
		GUActionHandler();
	}

	private void GUActionHandler()
	{
		if(runMode != RunMode.Update && Application.isPlaying)
		{
			this.enabled = false;
		}
		
		GUAction();
	}
}