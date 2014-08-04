// GCurve.cs
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

[System.Serializable]
public sealed class GCurve
{	
	#region Header
	public enum WrapModeGUI
	{
		Once = 1,
		Loop = 2,
		PingPong = 4
	}

	public enum CurvePlayback
	{
		Forward,
		Reversed
	}

	public delegate void VoidDelegate();

	/// <summary>
	/// On Curve finishes.
	/// Set this variable at START NOT at AWAKE, otherwise it won't be setted.
	/// </summary>
	public VoidDelegate OnFinish;

	/// <summary>
	/// USe fixed update.
	/// </summary>
	public bool fixedUpdate = false;

	/// <summary>
	/// Ignore time scale.
	/// </summary>
	public bool ignoreTimeScale = true;

	/// <summary>
	/// Value curve.
	/// </summary>
	public AnimationCurve curve = AnimationCurve.Linear(0,0,1,1);

	/// <summary>
	/// Curve mode.
	/// </summary>
	public WrapModeGUI mode = WrapModeGUI.Once;

	/// <summary>
	/// Curve duration.
	/// </summary>
	public float duration = 1;

	/// <summary>
	/// Playback mode.
	/// </summary>
	public CurvePlayback playback
	{
		get
		{
			return m_playback;
		}

		set
		{
			if(value != m_playback)
			{
				IsFinished = false;
			}

			m_playback = value;
		}
	}

	/// <summary>
	/// Playback mode.
	/// </summary>
	[SerializeField]
	private CurvePlayback m_playback;

	/// <summary>
	/// The ignore finish rule.
	/// (Only "Once" mode will active finish if this is false.)
	/// </summary>
	public bool ignoreFinishRule = false;

	/// <summary>
	/// Curve time.
	/// </summary>
	private float m_time;

	/// <summary>
	/// First call.
	/// </summary>
	private bool m_init = false;

	#endregion

	#region Properties

	/// <summary>
	/// Gets a value indicating whether this <see cref="GCurve"/> is finished.
	/// </summary>
	/// <value><c>true</c> if finished; otherwise, <c>false</c>.</value>
	public bool IsFinished
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the curve time.
	/// </summary>
	/// <value>The curve time.</value>
	public float CurveTime
	{
		get
		{
			return m_time;
		}

		set
		{
			if(!m_init)
			{
				m_init = true;
			}

			m_time = value;
		}
	}

	/// <summary>
	/// Gets or sets the mode.
	/// </summary>
	/// <value>The mode.</value>
	public WrapMode Mode
	{
		get
		{
			return (WrapMode)mode;
		}
		
		set
		{
			mode = (WrapModeGUI)value;
		}
	}

	#endregion

	#region Core

	/// <summary>
	/// Initializes a new instance of the <see cref="GCurve"/> class.
	/// </summary>
	public GCurve()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GCurve"/> class.
	/// </summary>
	/// <param name="mode">Mode.</param>
	public GCurve(CurvePlayback mode)
	{
		playback = mode;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="GCurve"/> class.
	/// </summary>
	/// <param name="mode">Mode.</param>
	/// <param name="duration">Duration.</param>
	public GCurve(CurvePlayback mode, float duration)
	{
		this.duration = duration;
		this.playback = mode;
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void Reset()
	{
		m_init = false;
		//Init();
		IsFinished = false;
	}

	/// <summary>
	/// Evaluate the specified time.
	/// </summary>
	/// <param name="time">Time.</param>
	/// <param name="useDuration">If set to <c>true</c> use duration.</param>
	public float Evaluate(ref float time, bool useDuration = true)
	{
		if(!m_init)
		{
			Init();
		}

		curve.postWrapMode = Mode;
		curve.preWrapMode = Mode;


		float curveValue; 

		if(!useDuration)
		{
			curveValue = curve.Evaluate(time);
		}
		else
		{
			curveValue = curve.Evaluate(time / duration);
		}

		if(float.IsNaN(curveValue))
		{
			curveValue = (playback == CurvePlayback.Forward ? 1 : 0);
		}

		if(Mode == WrapMode.Once || ignoreFinishRule)
		{
			if(playback == CurvePlayback.Forward)
			{
				if(time >= 1)
				{
					IsFinished = true;

					if(OnFinish != null)
					{

						OnFinish();
					}
				}
			}
			else
			{
				if(time <= 0)
				{
					IsFinished = true;

					if(OnFinish != null)
					{
						OnFinish();
					}
				}
			}
		}
		
		if(Mode == WrapMode.Once)
		{
			if(time >= 1)
			{
				time = 1;
			}
			else if(time <= 0)
			{
				time = 0;
			}
		}
		else if(Mode == WrapMode.Loop)
		{
			if(time > 1)
			{
				time = 0;
			}
			else if(time < 0)
			{
				time = 1;
			}
		}
		else if(Mode == WrapMode.PingPong)
		{
			if(time > 1)
			{
				playback = CurvePlayback.Reversed;
			}
			else if(time < 0)
			{
				playback = CurvePlayback.Forward;
			}
		}

		return curveValue;
	}

	/// <summary>
	/// Evaluate this instance.
	/// </summary>
	public float Evaluate()
	{
		if(!m_init)
		{
			Init();
		}

		float speed = 0;

		if(!fixedUpdate)
		{
			if(ignoreTimeScale)
			{
				speed = GTime.DeltaTime;
			}
			else
			{
				speed = GTime.TimeScaledDelta;
			}
		}
		else
		{
			//Fixed Update doesn't have ignore time scale.
			speed = GTime.FixedDeltaTime;
		}

		speed /= duration;

		if(float.IsInfinity(speed))
		{
			m_time = (playback == CurvePlayback.Forward ? 1 : 0);
		}
		else
		{
			if(playback == CurvePlayback.Forward)
			{
				if(m_time >= 1 && Mode == WrapMode.Once)
				{
					m_time = 1;
				}
				else
				{
					m_time += speed;
				}
			}
			else
			{
				if(m_time <= 0 && Mode == WrapMode.Once)
				{
					m_time = 0;
				}
				else
				{
					m_time -= speed;
				}
			}
		}

		return Evaluate(ref m_time,false);
	}

	/// <summary>
	/// Init this instance.
	/// </summary>
	private void Init()
	{
		if(playback == CurvePlayback.Forward)
		{
			m_time = 0;
		}
		else
		{
			m_time = 1;
		}
		
		m_init = true;
	}

	#endregion
}