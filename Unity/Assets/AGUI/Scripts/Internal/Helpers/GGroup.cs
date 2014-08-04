// GGroup.cs
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
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// WIP.
/// </summary>
[System.Serializable]
public class GGroup
{
	//TODO: Group system...
	//TODO: Addes same tween to all objects, but button will update them.

	public bool useGroup;
	public GameObject groupContainer;

	protected Type type;
	protected GTween[] tweens;

	public GGroup(Type type)
	{
		this.type = type;
	}

	public void Init(GameObject go = null)
	{
		GameObject container = groupContainer;

		if(go != null)
		{
			container = go;
		}

		if(container == null)
		{
			return;
		}

		List<GTween> tweenList = new List<GTween>();

		Transform[] childs = container.GetComponentsInChildren<Transform>();

		Debug.Log(childs.Length);

		GTween baseTween = container.GetComponent(type) as GTween;

		foreach(Transform child in childs)
		{
			if(child.GetComponent<AGUIObject>() ||child.renderer != null)
			{
				GTween tween = child.GetComponent<GTween>();

				if(tween == null)
				{
					tween = child.gameObject.AddComponent(type) as GTween;
				}

				tween.enabled = false;

				if(baseTween != null)
				{
					tween = baseTween;
				}

				tweenList.Add(tween);
			}
		}

		tweens = tweenList.ToArray();


		if(tweens == null)
			return;
	}

	/*
	public void SetValues<T>(T tween) where T : GTween
	{
		if(typeof(T) != type)
		{
			Debug.Log("Not same type!");
			return;
		}

		for(int i = 0; i < tweens.Length; i++)
		{
			T tw = (T)tweens[i];

			tw = tween;

			Debug.Log(tw.name);

			tweens[i] = tw;
		}
	}
	*/
}