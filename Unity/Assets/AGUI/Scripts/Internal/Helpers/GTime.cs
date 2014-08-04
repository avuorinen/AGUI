// GTime.cs
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

public sealed class GTime : MonoBehaviour
{
	#region Static

	/// <summary>
	/// Gets the real time.
	/// </summary>
	/// <value>The real time.</value>
	public static float RealTime
	{
		get
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return Time.realtimeSinceStartup;
			}
			#endif

			if(m_current == null)
			{
				CreateGTime();
			}
			
			return m_current.m_realtime;
		}
	}

	/// <summary>
	/// Gets the delta time.
	/// </summary>
	/// <value>The delta time.</value>
	public static float DeltaTime
	{
		get
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) 
			{
				return 0f;
			}
			#endif

			if(m_current == null)
			{
				CreateGTime();
			}
			
			//Uses Unity's build in delta time when it's time scale is 1.
			//Returns better values at start up.
			if(Time.timeScale == 1)
			{
				return Time.deltaTime;
			}

			return m_current.m_delta;
		}
	}

	/// <summary>
	/// Gets the time scaled delta.
	/// </summary>
	/// <value>The time scaled delta.</value>
	public static float TimeScaledDelta
	{
		get
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) 
			{
				return 0f;
			}
			#endif

			if(m_current == null)
			{
				CreateGTime();
			}
			
			return m_current.m_timeScaledDelta;
		}
	}

	/// <summary>
	/// Gets the fixed delta time.
	/// </summary>
	/// <value>The fixed delta time.</value>
	public static float FixedDeltaTime
	{
		get
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying) 
			{
				return 0f;
			}
			#endif

			if(m_current == null)
			{
				CreateGTime();
			}
			
			return Time.fixedDeltaTime;
		}
	}

	/// <summary>
	/// Current GTime object.
	/// </summary>
	private static GTime m_current = null;

	/// <summary>
	/// Creates the GTime object.
	/// </summary>
	private static void CreateGTime()
	{
		GameObject go = new GameObject("_gTime");

		m_current = go.AddComponent<GTime>();
		m_current.m_realtime = Time.realtimeSinceStartup;
		
		GameObject.DontDestroyOnLoad(go);
	}

	#endregion

	#region Core

	/// <summary>
	/// The real time.
	/// </summary>
	private float m_realtime;

	/// <summary>
	/// The delta time.
	/// </summary>
	private float m_delta;

	/// <summary>
	/// The time that is caled using delta.
	/// </summary>
	private float m_timeScaledDelta;


	/// <summary>
	/// Update GTime.
	/// </summary>
	private void Update()
	{
		float realtime = Time.realtimeSinceStartup;
		m_delta = realtime - m_realtime;
		m_realtime = realtime;

		m_timeScaledDelta = m_delta * Time.timeScale;
	}

	#endregion
}