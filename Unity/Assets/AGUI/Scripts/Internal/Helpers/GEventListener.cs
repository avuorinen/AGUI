// GEventListener.cs
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

public sealed class GEventListener : MonoBehaviour
{
	#region Static

	/// <summary>
	/// Gets the AGUIControlBase or add it if GameObject doesn't have it.
	/// </summary>
	/// <returns>The base event.</returns>
	/// <param name="go">Go.</param>
	public static AGUIControlBase GetBaseEvent(GameObject go)
	{
		return AGUIControlBase.GetAGUIBase(go);
	}

	/// <summary>
	/// Gets the GEventListerner or add it if GameObject doesn't have it.
	/// </summary>
	/// <returns>The event.</returns>
	/// <param name="go">Go.</param>
	public static GEventListener GetEventListerner(GameObject go)
	{
		return GHelper.GetComponent<GEventListener>(go);
	}

	#endregion

	#region 2D

	/// <summary>
	/// Collision2D Delegate.
	/// </summary>
	public delegate void Collision2DDelegate(GameObject go, Collision2D col);

	/// <summary>
	/// Trigger2D Delegate.
	/// </summary>
	public delegate void Trigger2DDelegate(GameObject go, Collider2D col);
	
	/// <summary>
	/// Fires when OnCollisionEnter2D is called.
	/// </summary>
	public Collision2DDelegate onCollisionEnter2D;

	/// <summary>
	/// Fires when OnCollisionStay2D is called.
	/// </summary>
	public Collision2DDelegate onCollisionStay2D;

	/// <summary>
	/// Fires when OnCollisionExit2D is called.
	/// </summary>
	public Collision2DDelegate onCollisionExit2D;

	/// <summary>
	/// Fires when OnTriggerEnter2D is called.
	/// </summary>
	public Trigger2DDelegate onTriggerEnter2D;

	/// <summary>
	/// Fires when OnTriggerStay2D is called.
	/// </summary>
	public Trigger2DDelegate onTriggerStay2D;

	/// <summary>
	/// Fires when OnTriggerExit2D is called.
	/// </summary>
	public Trigger2DDelegate onTriggerExit2D;


	//Collision//

	private void OnCollisionEnter2D(Collision2D col)
	{
		if(onCollisionEnter2D != null)
		{
			onCollisionEnter2D(gameObject,col);
		}
	}
	
	private void OnCollisionStay2D(Collision2D col)
	{
		if(onCollisionStay2D != null)
		{
			onCollisionStay2D(gameObject,col);
		}
	}
	
	private void OnCollisionExit2D(Collision2D col)
	{
		if(onCollisionExit2D != null)
		{
			onCollisionExit2D(gameObject,col);
		}
	}

	//Trigger//

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(onTriggerEnter2D != null)
		{
			onTriggerEnter2D(gameObject,col);
		}
	}
	
	private void OnTriggerStay2D(Collider2D col)
	{
		if(onTriggerStay2D != null)
		{
			onTriggerStay2D(gameObject,col);
		}
	}
	
	private void OnTriggerExit2D(Collider2D col)
	{
		if(onTriggerExit2D != null)
		{
			onTriggerExit2D(gameObject,col);
		}
	}
	
	#endregion

	#region 3D

	/// <summary>
	/// Collision Delegate.
	/// </summary>
	public delegate void CollisionDelegate(GameObject go, Collision col);

	/// <summary>
	/// Trigger Delegate.
	/// </summary>
	public delegate void TriggerDelegate(GameObject go, Collider col);

	/// <summary>
	/// Fires when OnCollisionEnter is called.
	/// </summary>
	public CollisionDelegate onCollisionEnter;

	/// <summary>
	/// Fires when OnCollisionStay is called.
	/// </summary>
	public CollisionDelegate onCollisionStay;

	/// <summary>
	/// Fires when OnCollisionExit is called.
	/// </summary>
	public CollisionDelegate onCollisionExit;

	/// <summary>
	/// Fires when OnTriggerEnter is called.
	/// </summary>
	public TriggerDelegate onTriggerEnter;

	/// <summary>
	/// Fires when OnTriggerStay is called.
	/// </summary>
	public TriggerDelegate onTriggerStay;

	/// <summary>
	/// Fires when OnTriggerExit is called.
	/// </summary>
	public TriggerDelegate onTriggerExit;
	

	//Collision//

	private void OnCollisionEnter(Collision col)
	{
		if(onCollisionEnter != null)
		{
			onCollisionEnter(gameObject,col);
		}
	}
	
	private void OnCollisionStay(Collision col)
	{
		if(onCollisionStay != null)
		{
			onCollisionStay(gameObject,col);
		}
	}
	
	private void OnCollisionExit(Collision col)
	{
		if(onCollisionExit != null)
		{
			onCollisionExit(gameObject,col);
		}
	}

	//Trigger//

	private void OnTriggerEnter(Collider col)
	{
		if(onTriggerEnter != null)
		{
			onTriggerEnter(gameObject,col);
		}
	}
	
	private void OnTriggerStay(Collider col)
	{
		if(onTriggerStay != null)
		{
			onTriggerStay(gameObject,col);
		}
	}
	
	private void OnTriggerExit(Collider col)
	{
		if(onTriggerExit != null)
		{
			onTriggerExit(gameObject,col);
		}
	}

	#endregion
}