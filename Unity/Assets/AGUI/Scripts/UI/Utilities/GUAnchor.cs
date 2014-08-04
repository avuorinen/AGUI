// GUAnchor.cs
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

[ExecuteInEditMode]
[AddComponentMenu("AGUI/UI/Utilities/Anchor")]
public sealed class GUAnchor : GUtilitiesBase
{
	#region Header

	public enum Anchor
	{
		TopLeft, Top, TopRight,
		MiddleLeft, Middle, MiddleRight,
		BottomLeft, Bottom, BottomRight
	}

	/// <summary>
	/// The camera.
	/// </summary>
	public new Camera camera;

	/// <summary>
	/// The space.
	/// </summary>
	public Space space = Space.World;

	/// <summary>
	/// Is relative.
	/// </summary>
	public bool aspect = false;

	/// <summary>
	/// The anchor.
	/// </summary>
	public Anchor anchor = Anchor.Middle;

	/// <summary>
	/// The offset.
	/// </summary>
	public Vector2 offset = Vector2.zero;

	#endregion

	#region Body
	
	/// <summary>
	/// Updates the anchor.
	/// </summary>
	public void UpdateAnchor()
	{
		GUAction();
	}

	protected override void GUAction ()
	{
		if(camera == null)
		{
			return;
		}
		
		Vector3 position = Vector3.zero;
		
		switch(anchor)
		{
			
		case Anchor.Bottom:
			position = new Vector3(0.5f,0);
			break;
			
		case Anchor.BottomLeft:
			position = new Vector3(0,0);
			break;
			
		case Anchor.BottomRight:
			position = new Vector3(1,0);
			break;
			
		case Anchor.Middle:
			position = new Vector3(0.5f,0.5f);
			break;
			
		case Anchor.MiddleLeft:
			position = new Vector3(0,0.5f);
			break;
			
		case Anchor.MiddleRight:
			position = new Vector3(1,0.5f);
			break;
			
		case Anchor.Top:
			position = new Vector3(0.5f,1);
			break;
			
		case Anchor.TopLeft:
			position = new Vector3(0,1);
			break;
			
		case Anchor.TopRight:
			position = new Vector3(1,1);
			break;
			
		default:
			break;
		}
		
		Vector3 orginalPos = camera.transform.position;
		//Quaternion orginalRot = camera.transform.rotation;
		
		if(space == Space.Self)
		{
			camera.transform.position = Vector3.zero;
			//camera.transform.rotation = Quaternion.Euler(0,0,0);
		}
		
		position = camera.ViewportToWorldPoint(position + (aspect ? (Vector3)offset : Vector3.zero));
		
		camera.transform.position = orginalPos;
		//camera.transform.rotation = orginalRot;
		
		if(!aspect)
		{
			position += (Vector3)offset;
		}
		
		if(space == Space.Self)
		{
			position.z = transform.localPosition.z;
			transform.localPosition = position;
		}
		else
		{
			position.z = transform.position.z;
			transform.position = position;
		}
	}

	#endregion
}