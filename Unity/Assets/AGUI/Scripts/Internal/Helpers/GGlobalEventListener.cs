// GGlobalEvents.cs
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

public static class GGlobalEvents
{
	/// <summary>
	/// Gets a value indicating can use events.
	/// Use this variable when you want to check if can use GGlobal events!
	/// Mostly for [ExecuteInEditMode] stuff.
	/// </summary>
	/// <value><c>true</c> if can use events; otherwise, <c>false</c>.</value>
	public static bool CanUseEvents
	{
		get
		{
			return GGlobalEventListener.CanUseEvents;
		}
	}

	public static GSmartDelegate OnScreenChange
	{
		get
		{
			return GGlobalEventListener.Instance.OnScreenChange;
		}

		set
		{
			GGlobalEventListener.Instance.OnScreenChange = value;
		}
	}

	public static GSmartDelegate OnSceneChange
	{
		get
		{
			return GGlobalEventListener.Instance.OnSceneChange;
		}

		set
		{
			GGlobalEventListener.Instance.OnSceneChange = value;
		}
	}

	public static GSmartDelegate OnTimeScaleChange
	{
		get
		{
			return GGlobalEventListener.Instance.OnTimeScaleChange;
		}

		set
		{
			GGlobalEventListener.Instance.OnTimeScaleChange = value;
		}
	}
}

public sealed class GGlobalEventListener : MonoBehaviour
{
	#region Static
	
	private static GGlobalEventListener m_instance;

	/// <summary>
	/// Gets the instance.
	/// Returns null variable if application isn't running.
	/// </summary>
	/// <value>The instance.</value>
	public static GGlobalEventListener Instance
	{
		get
		{
			//TODO: Better handler here!
			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				return null;
				//throw new System.Exception("Don't call this property when application isn't playing! (WIP => Will have better handler!)");
			}
			#endif
		
			if(m_instance == null)
			{
				m_instance = GHelper.CreateStaticInstance<GGlobalEventListener>("_globalEvents");
			}

			return m_instance;
		}
	}

	/// <summary>
	/// Gets a value indicating can use events.
	/// Use this variable when you want to check if can use GGlobal events!
	/// Mostly for [ExecuteInEditMode] stuff.
	/// </summary>
	/// <value><c>true</c> if can use events; otherwise, <c>false</c>.</value>
	public static bool CanUseEvents
	{
		get
		{
			#if UNITY_EDITOR
			if(!Application.isPlaying)
			{
				return false;
			}
			#endif
			
			return true;
		}
	}

	#endregion

	#region Header
	
	private float m_currentRatio;
	private int m_currentLevel;
	private float m_currentTimeScale;

	private GSmartDelegate m_onScreenChange = new GSmartDelegate();
	private GSmartDelegate m_onSceneChange = new GSmartDelegate();
	private GSmartDelegate m_onTimeScaleChange = new GSmartDelegate();

	#endregion

	#region Properties

	public GSmartDelegate OnScreenChange
	{
		get
		{
			return m_onScreenChange;
		}

		set
		{
			if(value == null)
			{
				return;
			}

			m_onScreenChange = value;
		}
	}
	
	public GSmartDelegate OnSceneChange
	{
		get
		{
			return m_onSceneChange;
		}

		set
		{
			if(value == null)
			{
				return;
			}

			m_onSceneChange = value;
		}
	}

	public GSmartDelegate OnTimeScaleChange
	{
		get
		{
			return m_onTimeScaleChange;
		}

		set
		{
			if(value == null)
			{
				return;
			}

			m_onTimeScaleChange = value;
		}
	}

	#endregion

	#region Body

	public void HandleEvents()
	{
		if(m_currentLevel != Application.loadedLevel)
		{
			m_currentLevel = Application.loadedLevel;
			OnSceneChange.Invoke();
		}

		if(m_currentTimeScale != Time.timeScale)
		{
			m_currentTimeScale = Time.timeScale;
			OnTimeScaleChange.Invoke();
		}
		
		if(Camera.main && m_currentRatio != Camera.main.aspect)
		{
			m_currentRatio = Camera.main.aspect;
			OnScreenChange.Invoke();
		}
	}

	private void Start()
	{
		m_currentLevel = Application.loadedLevel;
		m_currentTimeScale = Time.timeScale;

		if(Camera.main)
		{
			m_currentRatio = Camera.main.aspect;
		}
	}

	private void Update()
	{
		HandleEvents();
	}

	#endregion
}

