// AGUIControlBase.cs
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

public class AGUIControlBase : MonoBehaviour 
{
	#region Static

	/// <summary>
	/// Gets the AGUI base or add it if GameObject doesn't have it.
	/// </summary>
	/// <returns>The AGUI base.</returns>
	/// <param name="go">GameObject.</param>
	public static AGUIControlBase GetAGUIBase(GameObject go)
	{
		return GHelper.GetComponent<AGUIControlBase>(go);
	}
	
	#endregion

	#region Header

	public class LinkedControl
	{
		public enum CallerType
		{
			Null,
			Busy,
			Normal,
			Hover
		}
		
		public Vector3 position = Vector3.zero;
		public Vector3 screenPosition = Vector3.zero;
		public AGUIController caller = null;
		public List<AGUIControlBase> bases = new List<AGUIControlBase>();
		
		public List<Touch> touches = new List<Touch>();
		public CallerType callerType = CallerType.Null;

		//Delegates//
		public VoidBaseDelegate onHover;
		public BoolBaseDelegate onHovered;
		
		public VoidBaseDelegate onClick;
		public VoidBaseDelegate onDoubleClick;
		public VoidBaseDelegate onHold;
		
		public BoolBaseDelegate onPress;
		public Vector2BaseDelegate onDrag;
	}

	public delegate void VoidBaseDelegate(AGUIControlBase cont);
	public delegate void BoolBaseDelegate(AGUIControlBase cont, bool boolean);
	public delegate void Vector2BaseDelegate(AGUIControlBase cont, Vector2 vector);


	private GLinker<LinkedControl> m_linker = new GLinker<LinkedControl>();
	private LinkedControl m_link;

	#endregion

	#region Properties

	/// <summary>
	/// Gets the bases.
	/// </summary>
	/// <value>The bases.</value>
	public List<AGUIControlBase> Bases
	{
		get
		{
			//TODO: Better system for this!
			CheckLink();
			return new List<AGUIControlBase>(m_link.bases);
		}
	}

	/// <summary>
	/// Gets or sets the touches.
	/// </summary>
	/// <value>The touches.</value>
	public List<Touch> Touches
	{
		get
		{
			CheckLink();
			return m_link.touches;
		}
		
		set
		{
			CheckLink();
			m_link.touches = value;
		}
	}

	/// <summary>
	/// Gets or sets the position.
	/// Position is set by AGUIController using input positions.
	/// </summary>
	/// <value>The position.</value>
	public Vector3 Position
	{
		get
		{
			CheckLink();
			return m_link.position;
		}
		
		set
		{
			CheckLink();
			m_link.position = value;
		}
	}

	/// <summary>
	/// Gets or sets the screen space Position.
	/// </summary>
	/// <value>The screen position.</value>
	public Vector3 ScreenPosition
	{
		get
		{
			CheckLink();
			return m_link.screenPosition;
		}
		
		set
		{
			CheckLink();
			m_link.screenPosition = value;
		}
	}

	/// <summary>
	/// Gets the caller.
	/// </summary>
	/// <value>The caller.</value>
	public AGUIController Caller
	{
		get
		{
			CheckLink();
			return m_link.caller;
			
		}
		
		private set
		{
			CheckLink();
			m_link.caller = value;
		}
	}

	/// <summary>
	/// Gets the type of the caller.
	/// </summary>
	/// <value>The type of the caller.</value>
	public LinkedControl.CallerType CallerType
	{
		get
		{
			CheckLink();
			return m_link.callerType;
		}
		
		private set
		{
			CheckLink();
			m_link.callerType = value;
		}
	}

	#endregion

	#region Delegates

	/// <summary>
	/// Gets or sets the onHover delegate.
	/// </summary>
	/// <value>The on hover.</value>
	public VoidBaseDelegate onHover
	{
		get
		{
			CheckLink();
			return m_link.onHover;
		}

		set
		{
			CheckLink();
			m_link.onHover = value;
		}
	}

	/// <summary>
	/// Gets or sets the onHovered delegate.
	/// </summary>
	/// <value>The on hovered.</value>
	public BoolBaseDelegate onHovered
	{
		get
		{
			CheckLink();
			return m_link.onHovered;
		}
		
		set
		{
			CheckLink();
			m_link.onHovered = value;
		}
	}

	/// <summary>
	/// Gets or sets the onClick delegate.
	/// </summary>
	/// <value>The on click.</value>
	public VoidBaseDelegate onClick
	{
		get
		{
			CheckLink();
			return m_link.onClick;
		}
		
		set
		{
			CheckLink();
			m_link.onClick = value;
		}
	}

	/// <summary>
	/// Gets or sets the onDoubleClick delegate.
	/// </summary>
	/// <value>The on double click.</value>
	public VoidBaseDelegate onDoubleClick
	{
		get
		{
			CheckLink();
			return m_link.onDoubleClick;
		}
		
		set
		{
			CheckLink();
			m_link.onDoubleClick = value;
		}
	}

	/// <summary>
	/// Gets or sets the onHold delegate.
	/// </summary>
	/// <value>The on hold.</value>
	public VoidBaseDelegate onHold
	{
		get
		{
			CheckLink();
			return m_link.onHold;
		}
		
		set
		{
			CheckLink();
			m_link.onHold = value;
		}
	}

	/// <summary>
	/// Gets or sets the onPress delegate.
	/// </summary>
	/// <value>The on press.</value>
	public BoolBaseDelegate onPress
	{
		get
		{
			CheckLink();
			return m_link.onPress;
		}
		
		set
		{
			CheckLink();
			m_link.onPress = value;
		}
	}

	/// <summary>
	/// Gets or sets the onDrag delegate.
	/// </summary>
	/// <value>The on drag.</value>
	public Vector2BaseDelegate onDrag
	{
		get
		{
			CheckLink();
			return m_link.onDrag;
		}
		
		set
		{
			CheckLink();
			m_link.onDrag = value;
		}
	}
	#endregion

	#region Body

	/// <summary>
	/// Determines whether this instance is called.
	/// </summary>
	/// <returns><c>true</c> if this instance is called; otherwise, <c>false</c>.</returns>
	public bool IsCalled()
	{
		return m_link != null && Caller != null;
	}

	/// <summary>
	/// Sets the caller to be normal.
	/// </summary>
	/// <param name="caller">Caller.</param>
	public void SetCaller(AGUIController caller)
	{
		SetCaller(caller,LinkedControl.CallerType.Normal);
	}

	/// <summary>
	/// Sets the caller.
	/// </summary>
	/// <param name="manager">Manager.</param>
	/// <param name="hover">If set to <c>true</c> hover.</param>
	public void SetCaller(AGUIController caller, LinkedControl.CallerType type)
	{
		CheckLink();
		Caller = caller;
		
		if(Caller == null)
		{
			CallerType = LinkedControl.CallerType.Null;
		}
		else
		{
			CallerType = type;
		}
	}



	/// <summary>
	/// Awake this instance.
	/// </summary>
	protected virtual void Awake()
	{
		if(!m_linker.IsInited)
		{
			m_linker.checkLinks = delegate(ref GLinker<LinkedControl> gLinker) {

				foreach(AGUIControlBase cBase in GetComponents<AGUIControlBase>())
				{
					if(gLinker.CheckLink(cBase.m_linker))
					{
						gLinker = cBase.m_linker;
						return true;
					}
				}

				return false;

			};

			m_linker.setLinks = delegate(ref GLinker<LinkedControl> gLinker) {

				AGUIControlBase[] bases = GetComponents<AGUIControlBase>();

				for(int i = 0; i < bases.Length; i++)
				{
					bases[i].m_linker = gLinker;
				}

				return true;

			};

			m_linker.InitLink();
			m_linker.LinkedInstance = new LinkedControl();
		}

		m_link = m_linker.LinkedInstance;
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	protected virtual void OnEnable()
	{
		CheckLink();
		m_link.bases.Add(this);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	protected virtual void OnDisable()
	{
		CheckLink();
		m_link.bases.Remove(this);
	}

	/// <summary>
	/// Checks the active rules.
	/// </summary>
	/// <returns><c>true</c>, if active rules was checked, <c>false</c> otherwise.</returns>
	private bool CheckActiveRules()
	{
		CheckLink();
		return enabled && gameObject.activeInHierarchy;
	}

	/// <summary>
	/// Checks the link.
	/// </summary>
	private void CheckLink()
	{
		if(m_link == null || !m_linker.IsInited)
		{
			this.Awake();
		}
	}

	#endregion
	
	#region Invoke

	//All invoke methods need caller, so it won't be called using GDelegates!

	//Hover//

	/// <summary>
	/// Invokes the hover.
	/// </summary>
	public void InvokeHover(AGUIController caller)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnHover();

			if(bases.onHover != null)
			{
				bases.onHover(this);
			}
		}
	}

	/// <summary>
	/// Invokes the hovered.
	/// </summary>
	/// <param name="isHovered">If set to <c>true</c> is hovered.</param>
	public void InvokeHovered(AGUIController caller,bool isHovered)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnHovered(isHovered);
			
			if(bases.onHovered != null)
			{
				bases.onHovered(this,isHovered);
			}
		}
	}
	
	//Mouse & Touch//

	/// <summary>
	/// Invokes the click.
	/// </summary>
	public void InvokeClick(AGUIController caller)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnClick();
			
			if(bases.onClick != null)
			{
				bases.onClick(this);
			}
		}
	}

	/// <summary>
	/// Invokes the double click.
	/// </summary>
	public void InvokeDoubleClick(AGUIController caller)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnDoubleClick();
			
			if(bases.onDoubleClick != null)
			{
				bases.onDoubleClick(this);
			}
		}
	}

	/// <summary>
	/// Invokes the hold.
	/// </summary>
	public void InvokeHold(AGUIController caller)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnHold();
			
			if(bases.onHold != null)
			{
				bases.onHold(this);
			}
		}
	}

	/// <summary>
	/// Invokes the press.
	/// </summary>
	/// <param name="isPressed">If set to <c>true</c> is pressed.</param>
	public void InvokePress(AGUIController caller,bool isPressed)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnPress(isPressed);
			
			if(bases.onPress != null)
			{
				bases.onPress(this,isPressed);
			}
		}
	}

	/// <summary>
	/// Invokes the drag.
	/// </summary>
	/// <param name="delta">Delta.</param>
	public void InvokeDrag(AGUIController caller,Vector2 delta)
	{
		if(!CheckActiveRules())
		{
			return;
		}

		foreach(AGUIControlBase bases in Bases)
		{
			bases.OnDrag(delta);
			
			if(bases.onDrag != null)
			{
				bases.onDrag(this, delta);
			}
		}
	}
	
	#endregion
	
	#region Virtuals
	
	//Hover//
	
	/// <summary>
	/// OnHover base.
	/// Is called when mouse or "touch" is over the game object.
	/// </summary>
	protected virtual void OnHover()
	{
		
	}
	
	/// <summary>
	/// OnHovered base.
	/// Is called when hover begins or ends.
	/// </summary>
	/// <param name="isHovered">If set to <c>true</c> is hovered.</param>
	protected virtual void OnHovered(bool isHovered)
	{
		
	}
	
	//Mouse & Touch//

	/// <summary>
	/// OnClick base.
	/// Is called when game object is clicked or tapped.
	/// </summary>
	protected virtual void OnClick()
	{
		
	}

	/// <summary>
	/// OnDoubleClick base.
	/// Is called when game object is double clicked or tapped.
	/// </summary>
	protected virtual void OnDoubleClick()
	{
		
	}

	/// <summary>
	/// OnHold base.
	/// Is called when game object is begin holded.
	/// </summary>
	protected virtual void OnHold()
	{
		
	}

	/// <summary>
	/// OnPress base.
	/// Is called when you hold begins or ends.
	/// </summary>
	/// <param name="isPressed">If set to <c>true</c> is pressed.</param>
	protected virtual void OnPress(bool isPressed)
	{
		
	}

	/// <summary>
	/// OnDrag base.
	/// Is called  when you drag game object.
	/// </summary>
	/// <param name="delta">Delta.</param>
	protected virtual void OnDrag(Vector2 delta)
	{
		
	}
	
	#endregion
}
