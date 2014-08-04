// GTGroup.cs
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
using System.Collections.Generic;

[AddComponentMenu("AGUI/UI/Tween/Group")]
public sealed class GTGroup : MonoBehaviour
{
	#region Static

	/// <summary>
	/// Playes the group.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="container">Container.</param>
	public static void PlayGroup(string id, GameObject container)
	{
		GTween[] tweens = container.GetComponentsInChildren<GTween>();
		
		foreach(GTween tween in tweens)
		{
			if(tween.useGroup && tween.groupID == id)
			{
				tween.Play();
			}
		}
	}

	/// <summary>
	/// Playes the group.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="container">Container.</param>
	/// <param name="mode">Mode.</param>
	public static void PlayGroup(string id, GameObject container, GCurve.CurvePlayback mode)
	{
		GTween[] tweens = container.GetComponentsInChildren<GTween>();
		
		foreach(GTween tween in tweens)
		{
			if(tween.useGroup && tween.groupID.Equals(id,System.StringComparison.OrdinalIgnoreCase))
			{
				if(mode == GCurve.CurvePlayback.Forward)
				{
					tween.PlayForward();
				}
				else
				{
					tween.PlayReversed();
				}
			}
		}
	}

	#endregion

	#region Header

	/// <summary>
	/// The group ID.
	/// </summary>
	public string groupID = "";

	/// <summary>
	/// The GameObject that contains tweens.
	/// </summary>
	public GameObject groupContainer;

	/// <summary>
	/// Curve playback.
	/// </summary>
	public GCurve.CurvePlayback playBack;

	/// <summary>
	/// The event that is fired when PlayGroup is called.
	/// </summary>
	public List<GEventDelegate> onActive = new List<GEventDelegate>();

	#endregion

	#region Core

	/// <summary>
	/// Playes the group.
	/// </summary>
	public void PlayGroup()
	{
		if(groupContainer != null)
		{
			PlayGroup(groupID,groupContainer);
			onActive.Invoke();
		}
	}

	public void PlayGroupForward()
	{
		if(groupContainer != null)
		{
			PlayGroup(groupID,groupContainer, GCurve.CurvePlayback.Forward);
			onActive.Invoke();
		}
	}

	public void PlayGroupReversed()
	{
		if(groupContainer != null)
		{
			PlayGroup(groupID,groupContainer, GCurve.CurvePlayback.Reversed);
			onActive.Invoke();
		}
	}
	
	/// <summary>
	/// Playes the group using PlayBack.
	/// </summary>
	public void PlayGroupWithPlayback()
	{
		if(groupContainer != null)
		{
			PlayGroup(groupID,groupContainer,playBack);
			onActive.Invoke();
		}
	}

	/// <summary>
	/// Toggle the playback.
	/// </summary>
	public void Toggle()
	{
		if(playBack == GCurve.CurvePlayback.Forward)
		{
			playBack = GCurve.CurvePlayback.Reversed;
		}
		else
		{
			playBack = GCurve.CurvePlayback.Forward;
		}
	}

	/// <summary>
	/// Reset group,
	/// </summary>
	private void Reset()
	{
		if(groupContainer == null)
		{
			groupContainer = gameObject;
		}
	}

	#endregion
}