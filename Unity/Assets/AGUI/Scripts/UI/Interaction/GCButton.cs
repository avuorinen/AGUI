// GCButton.cs
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
using System.Collections.Generic;

//TODO: Make more GControls! (Slider, Toggle, etc.)

[AddComponentMenu("AGUI/UI/Interaction/Button")]
public class GCButton : AGUIControlBase 
{
	//TODO: ReWork group system use tweens!!
	//TODO: Take colors from components (active color = normal * 0.75f)
	
	#region Header

	/// <summary>
	/// When button is pressed onFinish delegate are invoked.
	/// </summary>
	public List<GEventDelegate> onFinish = new List<GEventDelegate>();

	/// <summary>
	/// The normal color.
	/// </summary>
	public Color normal = Color.white;

	/// <summary>
	/// The actived color.
	/// </summary>
	public Color actived = Color.gray;

	/// <summary>
	/// The disabled color.
	/// </summary>
	public Color disabled = Color.gray * 0.5f;

	/// <summary>
	/// The button curve.
	/// </summary>
	public GCurve curve = new GCurve(GCurve.CurvePlayback.Forward,0.1f);

	/// <summary>
	/// The is button begin holded.
	/// </summary>
	private bool m_hold;

	/// <summary>
	/// Where button press started.
	/// </summary>
	private Vector3 m_pressPosition;
	
	#endregion

	#region Body
	

	protected override void Awake ()
	{
		onFinish.Add(new GEventDelegate(delegate {

		}));

		base.Awake ();
		curve.CurveTime = 0;
	}

	/// <summary>
	/// OnPress base.
	/// Is called when you hold begins or ends.
	/// </summary>
	/// <param name="isPressed">If set to <c>true</c> is pressed.</param>
	protected override void OnPress (bool isPressed)
	{
		if(m_hold && curve.IsFinished && curve.playback == GCurve.CurvePlayback.Forward)
		{
			onFinish.Invoke();
		}
		
		m_hold = isPressed;
		m_pressPosition = Position;
	}

	/// <summary>
	/// OnDrag base.
	/// Is called when you drag game object.
	/// </summary>
	/// <param name="delta">Delta.</param>
	protected override void OnDrag (Vector2 delta)
	{
		if( (m_pressPosition - Position).sqrMagnitude > 2)
		{
			m_hold = false;
		}
	}

	protected override void OnEnable ()
	{
		GColorHelper.SetColor(gameObject,normal,false);
		base.OnEnable ();
	}

	protected override void OnDisable ()
	{
		GColorHelper.SetColor(gameObject,disabled,false);
		base.OnDisable ();
	}
	

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	private void Update()
	{
		if(m_hold)
		{
			curve.playback = GCurve.CurvePlayback.Forward;
		}
		else
		{
			curve.playback = GCurve.CurvePlayback.Reversed;
		}

		if( !curve.IsFinished )
		{
			GColorHelper.SetColor(gameObject,Color.Lerp(normal,actived,curve.Evaluate()),false);
		}
	}

	#endregion
}