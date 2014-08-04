// AGUIControllerDataBase.cs
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

public class AGUIControllerDataBase
{
	//TODO: Design, AGUIControllerDataBase to use as base class.
	//TODO: Hover support!

	#region Header

	public enum ControlType
	{
		Mouse,
		Touch,
	}
	
	public enum ControlStatus
	{
		Idle,
		Begin,
		Running,
		End
	}

	private const int MOUSE_INDEX = 0;

	private AGUIController m_controller;
	private int m_touchIndex;
	
	#endregion

	#region Data

	private List<ushort> m_fingerIDs;

	private List<AGUIController> m_controllers;

	private List<AGUIControlBase> m_linkedBases;
	private List<ControlStatus> m_statues;
	private List<ControlType> m_types;
	
	private List<Vector2> m_positions;
	private List<Vector2> m_lastPositions;

	private List<ushort> m_clicks;
	
	#endregion

	#region Properties
	
	public int Count
	{
		get
		{
			return m_linkedBases.Count;
		}
	}
	
	public int TouchesCount
	{
		get;
		private set;
	}

	#endregion

	#region Core

	public AGUIControllerDataBase(AGUIController controller, bool useCoroutine)
	{
		TouchesCount = -1;
		
		m_controller = controller;

		m_fingerIDs = new List<ushort>();

		m_controllers = new List<AGUIController>();

		m_linkedBases = new List<AGUIControlBase>();
		m_statues = new List<ControlStatus>();
		m_types = new List<ControlType>();
		
		m_positions = new List<Vector2>();
		m_lastPositions = new List<Vector2>();

		m_clicks = new List<ushort>();
		

		if(useCoroutine)
		{
			m_controller.StartCoroutine_Auto(InstanceUpdater());
		}

		
		//Adds mouse.
		Add(ControlType.Mouse);
	}

	public void Add(ControlType type)
	{
		if(type == ControlType.Touch)
		{
			m_fingerIDs.Add( (ushort)Count );
		}

		m_controllers.Add(null);

		m_linkedBases.Add(null);
		m_statues.Add(ControlStatus.Idle);
		m_types.Add(type);
		
		m_positions.Add(Vector2.zero);
		m_lastPositions.Add(Vector2.zero);
		
		m_clicks.Add(0);
	}
	
	public void Update()
	{
		m_touchIndex = 0;
		CheckTouches();
		ResetTouches();

		for(int index = 0; index < Count; index++)
		{
			HandleUpdate(index);
			UpdateData(index);
		}
	}
	
	public void SetController(AGUIController controller)
	{
		m_controller = controller;
	}

	#endregion
	
	#region Body

	private void UpdateData(int index)
	{
		if(m_statues[index] == ControlStatus.Idle || m_linkedBases[index] == null)
		{
			m_linkedBases[index] = null;
			//m_statues[index] = ControlStatus.Idle;
			//return;
		}
		else
		{
			//TODO: Set caller.
			m_linkedBases[index].SetCaller(m_controllers[index]);

			AGUIControlBase[] bases = new AGUIControlBase[]{m_linkedBases[index]};

			if(m_controller.invokeChildren)
			{
				bases = GetLinkedBases(index);
			}

			if(bases == null)
			{
				return;
			}

			if(m_statues[index] == ControlStatus.Begin)
			{
				m_statues[index] = ControlStatus.Running;

				m_clicks[index]++;

				if(m_clicks[index] == 1)
				{
					m_controller.StartCoroutine(WaitForClicks(index));
				}

				foreach(AGUIControlBase b in bases)
				{
					b.InvokeClick(m_controller);

					if(m_clicks[index] % 2 == 0)
					{
						b.InvokeDoubleClick(m_controller);
					}

					b.InvokePress(m_controller,true);
				}
			}
			
			//Force Update// 
			HandleUpdate(index);

			if(m_statues[index] == ControlStatus.Running)
			{
				foreach(AGUIControlBase b in bases)
				{
					//Hold
					b.InvokeHold(m_controller);
					
					if(m_lastPositions[index] != m_positions[index])
					{
						//Drag
						b.InvokeDrag(m_controller, (m_lastPositions[index] - m_positions[index]) );
					}
				}
				
				m_lastPositions[index] = m_positions[index];
			}
			
			if(m_statues[index] == ControlStatus.End)
			{
				foreach(AGUIControlBase b in bases)
				{
					b.InvokePress(m_controller,false);
				}

				m_linkedBases[index].SetCaller(null);
				m_linkedBases[index] = null;
			}
		}

		if(m_statues[index] == ControlStatus.End)
		{
			m_statues[index] = ControlStatus.Idle;
		}
	}
	
	private void HandleUpdate(int index)
	{
		if(m_types[index] == ControlType.Mouse)
		{
			if(m_controller.controls.HasFlag(AGUIController.ControllerControls.Mouse))
			{
				UpdateMouse(index);
			}
		}
		else if(m_types[index] == ControlType.Touch)
		{
			if(m_controller.controls.HasFlag(AGUIController.ControllerControls.Touch))
			{
				UpdateTouch();
			}
		}
	}

	private IEnumerator WaitForClicks(int index)
	{
		int oldClickCount = m_clicks[index];

		while(true)
		{
			yield return new WaitForSeconds(m_controller.clicksWaitTime);

			if(oldClickCount != m_clicks[index])
			{
				oldClickCount = m_clicks[index];
				continue;
			}

			break;
		}

		m_clicks[index] = 0;
	}

	private AGUIControlBase[] GetLinkedBases(int index)
	{
		if(m_linkedBases[index])
		{
			GameObject currentObj = null;
			List<AGUIControlBase> bases = new List<AGUIControlBase>();

			foreach(AGUIControlBase link in m_linkedBases[index].GetComponentsInChildren<AGUIControlBase>())
			{
				if(currentObj != link.gameObject)
				{
					bases.Add(link);
					currentObj = link.gameObject;
				}
			}

			return bases.ToArray();
		}

		return null;
	}

	#endregion

	#region Mouse

	private void UpdateMouse(int index)
	{
		//Ignores mouse if there is active touches.
		if(Input.touchCount != 0)
		{
			return;
		}
		
		if(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			if(m_statues[index] == ControlStatus.Idle)
			{
				m_statues[index] = ControlStatus.Begin;
				
				//Get Base//
				m_linkedBases[index] = m_controller.GetBases( Input.mousePosition );

				if(m_linkedBases[index])
				{
					m_controllers[index] = m_linkedBases[index].Caller;
				}
			}
		}
		else if(m_statues[index] != ControlStatus.Idle)
		{
			m_statues[index] = ControlStatus.End;
		}
		
		if(m_linkedBases[index] != null && m_statues[index] != ControlStatus.Idle)
		{
			m_linkedBases[index].Position = m_positions[index] = m_controller.Screen2World( Input.mousePosition, m_controllers[index] );
		}
	}
	
	#endregion

	#region Touch

	private void CheckTouches()
	{
		if(m_controller.maxTouches > TouchesCount)
		{
			foreach(Touch touch in Input.touches)
			{
				if(m_controller.maxTouches > TouchesCount && touch.fingerId > TouchesCount)
				{
					Add(ControlType.Touch);
					TouchesCount++;
				}
			}
		}
	}

	private void ResetTouches()
	{
		foreach(int fingerID in m_fingerIDs)
		{
			if(m_statues[fingerID] != ControlStatus.Idle)
			{
				m_statues[fingerID] = ControlStatus.End;

				if(m_linkedBases[fingerID])
				{
					m_linkedBases[fingerID].Position = Vector3.zero;
					m_linkedBases[fingerID].Touches.Clear();
				}
			}
		}
	}
	
	private void UpdateTouch()
	{
		if(m_touchIndex < Input.touchCount)
		{
			Touch touch = Input.GetTouch(m_touchIndex);
			int fingerID = m_fingerIDs[touch.fingerId];

			if(m_statues[fingerID] == ControlStatus.Idle)
			{
				m_statues[fingerID] = ControlStatus.Begin;

				//Get base//
				m_linkedBases[fingerID] = m_controller.GetBases( touch.position );

				if(m_linkedBases[fingerID])
				{
					m_controllers[fingerID] = m_linkedBases[fingerID].Caller;
				}
			}
			else if(m_statues[fingerID] == ControlStatus.End)
			{
				m_statues[fingerID] = ControlStatus.Running;
			}

			//Update position//

			if(m_linkedBases[fingerID] != null)
			{
				m_positions[fingerID] = m_controller.Screen2World( touch.position, m_controllers[fingerID] );

				if(m_linkedBases[fingerID].Position == Vector3.zero)
				{
					m_linkedBases[fingerID].Position = m_positions[fingerID];
				}
				else
				{
					m_linkedBases[fingerID].Position = Vector3.Lerp(m_linkedBases[fingerID].Position,m_positions[fingerID],0.5f);
				}

				m_linkedBases[fingerID].Touches.Add(touch);
			}

			m_touchIndex++;

		}
	}
	
	#endregion

	#region Debug

	#if UNITY_EDITOR

	public void DebugGizmos()
	{
		for(int index = 0; index < Count; index++)
		{
			if(m_statues[index] != ControlStatus.Idle)
			{
				if(m_linkedBases[index])
				{
					Gizmos.color = Color.cyan * 0.75f;
					Gizmos.DrawLine(m_positions[index],m_linkedBases[index].Position);
				}

				if(m_controllers[index] && m_controllers[index].controllerType == AGUIController.ControllerType.Shared)
				{
					Gizmos.color = Color.magenta * 0.75f;
				}
				else
				{
					Gizmos.color = Color.white * 0.75f;
				}

				Gizmos.DrawCube(m_positions[index],Vector3.one / 2);

				if(m_linkedBases[index])
				{
					Gizmos.color = Color.cyan * 0.75f;
					Gizmos.DrawCube(m_linkedBases[index].Position,Vector3.one / 2);
				}

			}
		}

		Gizmos.color = Color.white;
	}

	public void DebugGUI()
	{
		GUILayout.Label("Touches Count: " + m_fingerIDs.Count);

		if(Input.touchCount == 0)
		{
			GUILayout.Label("Mouse: Status: " + m_statues[MOUSE_INDEX] + ", Target: " + m_linkedBases[MOUSE_INDEX] + ", Clicks: " + m_clicks[MOUSE_INDEX]);
		}

		foreach(ushort touch in m_fingerIDs)
		{
			if(m_statues[touch] != ControlStatus.Idle)
			{
				GUILayout.Label("Touch " + touch + ": Status: " + m_statues[touch] + ", Target: " + m_linkedBases[touch] + ", Clicks: " + m_clicks[touch]);
			}
		}
	}

	#endif

	#endregion

	private IEnumerator InstanceUpdater()
	{
		while(true)
		{
			if(m_controller.enabled == true && m_controller.gameObject.activeInHierarchy == true)
			{
				for(int index = 0; index < Count; index++)
				{
					HandleUpdate(index);
				}
			}

			if(m_controller.fixedUpdate)
			{
				yield return new WaitForFixedUpdate();
			}
			else
			{
				yield return new WaitForEndOfFrame();
			}
		}
	}
}