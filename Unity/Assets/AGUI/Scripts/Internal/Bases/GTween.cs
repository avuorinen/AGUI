// GTWeen.cs
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
using System.Collections;
using UnityEngine;
using System.Text;

public abstract class GTween : MonoBehaviour
{
	//TODO: Better ignoreSame system.

	#region Header

	/// <summary>
	/// Tween delay.
	/// </summary>
	public float delay = 0;

	/// <summary>
	/// Skip play if playback is same.
	/// </summary>
	public bool ignoreSame = false;

	/// <summary>
	/// Tween curve.
	/// </summary>
	public GCurve curve = new GCurve();

	/// <summary>
	/// When tween is finished onFinish delegates are invoked.
	/// </summary>
	public List<GEventDelegate> onFinish = new List<GEventDelegate>();
	
	/// <summary>
	/// Cached Transform.
	/// </summary>
	[HideInInspector]
	public new Transform transform;

	#region Group

	//Group//

	/// <summary>
	/// Use groups.
	/// </summary>
	public bool useGroup = false;

	/// <summary>
	/// Group ID.
	/// </summary>
	public string groupID = "";

	#endregion

	/// <summary>
	/// Stored AGUIObject.
	/// </summary>
	protected AGUIObject m_aguiObject;

	/// <summary>
	/// Is delay waited or it is waiting.
	/// </summary>
	private short m_delayWaited = -1;

	#endregion

	#region Core

	/// <summary>
	/// Update tweem logic.
	/// </summary>
	public void UpdateTween()
	{
		if(delay > 0)
		{
			if(m_delayWaited == -1)
			{
				StartCoroutine(WaitDelay(delay));
				return;
			}
			else if(m_delayWaited == 0)
			{
				return;
			}
		}
		
		m_delayWaited = 1;
		

		OnTween();

	}
	
	/// <summary>
	/// Play tween. 
	/// </summary>
	public void Play()
	{
		if(!curve.IsFinished)
		{
			enabled = true;
		}
		else
		{
			Reset();
		}
	}

	/// <summary>
	/// Play tween forward.
	/// </summary>
	public void PlayForward()
	{
		if(ignoreSame && curve.playback == GCurve.CurvePlayback.Forward)
		{
			return;
		}
		
		curve.playback = GCurve.CurvePlayback.Forward;
		
		if(curve.IsFinished)
		{
			Reset();
		}
		else
		{
			this.enabled = true;
		}
	}
	
	/// <summary>
	/// Play tween reversed.
	/// </summary>
	public void PlayReversed()
	{
		if(ignoreSame && curve.playback == GCurve.CurvePlayback.Reversed)
		{
			return;
		}
		
		curve.playback = GCurve.CurvePlayback.Reversed;
		
		if(curve.IsFinished)
		{
			Reset();
		}
		else
		{
			this.enabled = true;
		}
	}

	/// <summary>
	/// Toggle playback between forward and reversed.
	/// </summary>
	public void Toggle()
	{
		if(curve.playback == GCurve.CurvePlayback.Forward)
		{
			PlayReversed();
		}
		else
		{
			PlayForward();
		}
	}

	/// <summary>
	/// Reset tween.
	/// </summary>
	public void Reset()
	{
		m_delayWaited = -1;
		this.enabled = true;

		if(curve != null)
		{
			curve.Reset();
		}
	}

	#endregion

	#region Body
	
	/// <summary>
	/// Tween action.
	/// </summary>
	protected abstract void OnTween();

	/// <summary>
	/// Update tween.
	/// </summary>
	protected void Update()
	{
		if(!curve.fixedUpdate)
		{
			UpdateTween();
		}
	}
	
	/// <summary>
	/// Fixed update tween.
	/// </summary>
	protected void FixedUpdate()
	{
		if(curve.fixedUpdate)
		{
			UpdateTween();
		}
	}
	
	/// <summary>
	/// Inits tween.
	/// </summary>
	protected virtual void Awake()
	{
		transform = GetComponent<Transform>();
		
		if(GetComponent<AGUIObject>())
		{
			m_aguiObject = GetComponent<AGUIObject>();
		}
	}

	/// <summary>
	/// Inits onFinish delegate on curve.
	/// </summary>
	protected virtual void Start()
	{
		curve.OnFinish = delegate() {

			Reset();
			
			if(curve.mode == GCurve.WrapModeGUI.Once)
			{
				this.enabled = false;
			}
			
			onFinish.Invoke();
			
		};
	}

	
	/// <summary>
	/// Wait delay.
	/// </summary>
	/// <returns>The delay.</returns>
	/// <param name="delay">Delay.</param>
	private IEnumerator WaitDelay(float delay)
	{
		m_delayWaited = 0;
		yield return new WaitForSeconds(delay);
		m_delayWaited = 1;
	}

	#endregion
}