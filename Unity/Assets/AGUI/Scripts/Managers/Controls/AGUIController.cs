// AGUIController.cs
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


[RequireComponent(typeof(Camera))]
[AddComponentMenu("AGUI/Managers/Controls/Controller")]
public class AGUIController : MonoBehaviour
{
	//TODO: Special Modes! (SingleTouchMultiOperation & MultiTouchSingleOperation)
	//TODO: Hover.

	#region Header

	public enum ControllerType
	{
		/// <summary>
		/// Share control information with muiltiple cameras.
		/// </summary>
		Shared,
		
		/// <summary>
		/// Keep control information self.
		/// </summary>
		NonShared
	}

	public enum ControlDimension
	{
		/// <summary>
		/// The use only 2D.
		/// </summary>
		Use2D,
		
		/// <summary>
		/// The use only 3D.
		/// </summary>
		Use3D,
		
		/// <summary>
		/// The both dimensions.
		/// </summary>
		Both
	}

	public enum ControllerMode
	{
		/// <summary>
		/// Single operation per finger, cant have multiple same operations.
		/// </summary>
		Single,
		
		/// <summary>
		/// Allow active multiple operations.
		/// </summary>
		Multi,
		
		/// <summary>
		/// Allow multiple operations but only one touch will be actived for same operations, 
		/// other touch will be actived when current opeation is gone.
		/// </summary>
		SingleTouchMultiOperation,
		
		//TODO:MultiTouchSingleOperation
		/// <summary>
		/// Allow multiple touches for ONE operation, others operations are ignored.
		/// </summary>
		MultiTouchSingleOperation,
	}

	public enum ControllerControls
	{
		/// <summary>
		/// Use mouse.
		/// </summary>
		Mouse = (1 << 0),
		
		/// <summary>
		/// Use touch.
		/// </summary>
		Touch = (1 << 1),
		
		/// <summary>
		/// Use hover.
		/// </summary>

		//Hover = (1 << 2),
		
		/// <summary>
		/// Use touch hover.
		/// </summary>

		//TouchHover = (1 << 3)
	}

	[Range(-1,10)]
	public int maxTouches = 10;

	[GBitFlagEnumAttribute]
	public ControllerControls controls = ControllerControls.Mouse | ControllerControls.Touch;
	
	public ControllerType controllerType = ControllerType.Shared;
	public ControlDimension controllerDimension = ControlDimension.Use2D;
	public ControllerMode controllerMode = ControllerMode.Multi;

	public bool fixedUpdate = true;
	public bool invokeChildren = false;

	/// <summary>
	/// Uses coroutine to update controls.
	/// This makes controls end events faster.
	/// Otherwise it will eat more performance.
	/// </summary>
	[SerializeField]
	private bool m_useCoroutine = false;
	
	/// <summary>
	/// Max check distance.
	/// </summary>
	public float distace = 10;
	
	/// <summary>
	/// Check layer.
	/// </summary>
	public LayerMask layer = -1;
	
	public float clicksWaitTime = 0.25f;

	//Makes Press(false) faster!
	//[SerializeField]
	//private bool m_coroutineUpdate = true;

	private AGUIControllerDataBase m_controllerData;

	#endregion

	#region Shared

	/// <summary>
	/// Alls shared controllers.
	/// </summary>
	private static List<AGUIController> m_sharedAGUIControllers = new List<AGUIController>();

	public static AGUIControllerDataBase SharedControllerData
	{
		get
		{
			return SharedControl.m_controllerData;
		}

		private set
		{
			SharedControl.m_controllerData = value;
		}
	}

	public static AGUIController SharedControl
	{
		get
		{
			foreach(AGUIController controller in m_sharedAGUIControllers)
			{
				if(!controller.enabled || !controller.gameObject.activeInHierarchy || !controller.camera.enabled || controller.camera == null)
				{
					continue;
				}
				
				//m_sharedActions = cont.m_actions;
				return controller;
			}
			
			return null;
		}
	}
	
	/// <summary>
	/// Sorts the shared controllers.
	/// </summary>
	public static void SortSharedControllers()
	{
		//Saves orginal data.
		AGUIControllerDataBase controllerData = SharedControllerData;

		m_sharedAGUIControllers.Sort(delegate(AGUIController x, AGUIController y) {
			
			if (x.camera.depth < y.camera.depth)
			{
				return 1;
			}
			else if (x.camera.depth > y.camera.depth) 
			{
				return -1;
			}
			
			return 0;
		});

		//Sets orginal data to new controller!
		controllerData.SetController(SharedControl);
		SharedControllerData = controllerData;
	}
	
	/// <summary>
	/// Raises the enable event.
	/// Adds Controller to list.
	/// </summary>
	private void OnEnable()
	{
		if(controllerType == ControllerType.Shared)
		{
			m_sharedAGUIControllers.Add(this);
			SortSharedControllers();
		}
		else
		{
			m_sharedAGUIControllers.Remove(this);
		}
	}
	
	/// <summary>
	/// Raises the disable event.
	/// Remove Controller from list.
	/// </summary>
	private void OnDisable()
	{
		m_sharedAGUIControllers.Remove(this);
	}
	
	/// <summary>
	/// Raises the validate event.
	/// Updated shared controllers list.
	/// </summary>
	private void OnValidate()
	{
		if(controllerType == ControllerType.NonShared)
		{
			m_sharedAGUIControllers.Remove(this);
		}
	}

	#endregion

	#region Core

	private void Awake()
	{
		m_controllerData = new AGUIControllerDataBase(this,m_useCoroutine);
	}

	private void Update()
	{
		if(!fixedUpdate)
		{
			if(controllerType == ControllerType.Shared)
			{
				if(this == SharedControl)
				{
					SharedControllerData.Update();
				}
			}
			else
			{
				m_controllerData.Update();
			}
		}
	}

	private void FixedUpdate()
	{
		if(fixedUpdate)
		{
			if(controllerType == ControllerType.Shared)
			{
				if(this == SharedControl)
				{
					SharedControllerData.Update();
				}
			}
			else
			{
				m_controllerData.Update();
			}
		}
	}

	#endregion

	#region Body

	public Vector3 Screen2World(Vector3 position, AGUIController controller = null)
	{	
		if(controller)
		{
			return controller.camera.ScreenToWorldPoint(position);
		}

		return camera.ScreenToWorldPoint(position);
	}

	public bool ValidatePosition(Vector3 position, Camera cam)
	{
		Vector3 viewPos = cam.WorldToViewportPoint(position);
		
		if(viewPos.x > 0 && viewPos.x < 1
		   && viewPos.y > 0 && viewPos.y < 1)
		{
			return true;
		}
		
		return false;
	}
	
	public AGUIControlBase GetBases(Vector3 position)
	{
		AGUIController controller = this;
		AGUIControlBase controlBase = null;

		Vector3 checkPosition = Screen2World(position);

		if(controllerType == ControllerType.NonShared)
		{
			if(!ValidatePosition(checkPosition,camera))
			{
				return null;
			}

			//2D
			if(controllerDimension == ControlDimension.Use2D || controllerDimension == ControlDimension.Both)
			{
				RaycastHit2D target = Physics2D.Raycast(checkPosition,transform.forward,distace,layer);
				
				if(target.collider != null)
				{
					controlBase = target.collider.GetComponent<AGUIControlBase>();
				}
			}

			//3D
			if(controllerDimension == ControlDimension.Use3D || controllerDimension == ControlDimension.Both)
			{
				RaycastHit target;
				Physics.Raycast(checkPosition,controller.transform.forward,out target,distace,layer);
				
				if(target.collider != null)
				{
					controlBase = target.collider.GetComponent<AGUIControlBase>();
				}
			}
		}
		else
		{
			controlBase = GetSharedBase(position,ref controller);
		}

		if(controlBase != null)
		{
			//TODO: Special rules!
			if(!controlBase.IsCalled() || controllerMode == ControllerMode.Multi)
			{
				controlBase.SetCaller(controller);
			}
			else
			{
				controlBase = null;
			}
		}

		return controlBase;
	}

	private AGUIControlBase GetSharedBase(Vector3 position, ref AGUIController activeController)
	{
		Vector3 checkPosition = Vector3.zero;
		
		RaycastHit2D hit2D;
		RaycastHit hit3D;
		
		foreach(AGUIController controller in m_sharedAGUIControllers)
		{
			checkPosition = controller.Screen2World(position);
			
			if(!ValidatePosition(checkPosition,controller.camera))
			{
				continue;
			}
			
			//2D
			if(controller.controllerDimension == ControlDimension.Use2D || controller.controllerDimension == ControlDimension.Both)
			{
				//Physics2D.raycastsHitTriggers = controller.raycastHitTrigger;
				
				hit2D = Physics2D.Raycast(checkPosition,controller.transform.forward,controller.distace,controller.layer);
				
				if(hit2D.collider != null)
				{
					activeController = controller;
					return hit2D.collider.GetComponent<AGUIControlBase>();
				}
				
				//Physics2D.raycastsHitTriggers = controller.m_defaultRaycastHitTrigger;
			}
			
			//3D
			if(controller.controllerDimension == ControlDimension.Use3D || controller.controllerDimension == ControlDimension.Both)
			{
				Physics.Raycast(checkPosition,controller.transform.forward,out hit3D,controller.distace,controller.layer);
				
				if(hit3D.collider != null)
				{
					activeController = controller;
					return hit3D.collider.GetComponent<AGUIControlBase>();
				}
			}
		}

		return null;
	}
	
	#endregion

	#region Debug

	#if UNITY_EDITOR

	[SerializeField]
	private bool m_debugGizmos;

	[SerializeField]
	private bool m_debugGUI;

	private void OnDrawGizmos()
	{
		if(!Application.isPlaying)
		{
			return;
		}

		if(m_debugGizmos)
		{
			m_controllerData.DebugGizmos();
		}
	}

	private void OnGUI()
	{
		if(m_debugGUI)
		{
			m_controllerData.DebugGUI();
		}
	}

	#endif

	#endregion
}