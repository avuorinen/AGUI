// GUPlatform.cs
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

[AddComponentMenu("AGUI/UI/Utilities/Platform")]
[ExecuteInEditMode]
/// <summary>
/// GUPlatform.
/// EDITOR ONLY!
/// </summary>
public sealed class GUPlatform : MonoBehaviour
{
	//TODO: Design a bit better system. (Easier edit mode

	#if UNITY_EDITOR

	[UnityEditor.Callbacks.PostProcessBuild]
	private static void InitPlatforms(UnityEditor.BuildTarget target, string message)
	{
		InitPlatforms();
	}
	
	[UnityEditor.Callbacks.PostProcessScene]
	private static void InitPlatforms()
	{
		foreach(GUPlatform plat in GameObject.FindObjectsOfType<GUPlatform>())
		{
			plat.HandlePlatform();
		}
	}

	[SerializeField]
	private bool m_useGroup = true;

	[SerializeField]
	private GameObject[] m_containers;

	[SerializeField]
	private UnityEditor.BuildTargetGroup[] m_buildTargetGroups;

	[SerializeField]
	private UnityEditor.BuildTarget[] m_buildTargets;

	public bool UseGroup
	{
		get
		{
			return m_useGroup;
		}
	}
	
	private void HandlePlatform()
	{
		if(m_containers == null || m_containers.Length == 0)
		{
			return;
		}

		bool foundTarget = false;

		if(m_useGroup)
		{
			foreach(UnityEditor.BuildTargetGroup target in m_buildTargetGroups)
			{
				if(target == UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup)
				{
					foundTarget = true;
				}
			}
		}
		else
		{
			foreach(UnityEditor.BuildTarget target in m_buildTargets)
			{
				if(target == UnityEditor.EditorUserBuildSettings.activeBuildTarget)
				{
					foundTarget = true;
				}
			}
		}

		foreach(GameObject container in m_containers)
		{
			if(container == null)
			{
				continue;
			}

			if(foundTarget)
			{
				container.SetActive(true);
			}
			else
			{
				container.SetActive(false);
			}
		}
	}

	private void Awake()
	{
		HandlePlatform();
	}

	private void Update()
	{
		if(!Application.isPlaying)
		{
			HandlePlatform();
		}
	}

	#endif
}