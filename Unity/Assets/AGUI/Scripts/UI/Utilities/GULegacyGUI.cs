// GULegacyGUI.cs
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

[AddComponentMenu("AGUI/UI/Utilities/Legacy GUI")]
[ExecuteInEditMode]
public class GULegacyGUI : GUtilitiesBase 
{
	public new Camera camera;
	private Rect m_guiPoistion;
	
	/// <summary>
	/// Gets screen position.
	/// </summary>
	/// <returns>The screen position.</returns>
	public Vector2 GetScreenPosition()
	{
		return new Vector2(m_guiPoistion.x,m_guiPoistion.y);
	}
	
	/// <summary>
	/// Gets the position.
	/// </summary>
	/// <returns>The position.</returns>
	public Rect GetPosition()
	{
		return m_guiPoistion;
	}
	
	/// <summary>
	/// Gets position that is scaled.
	/// </summary>
	/// <returns>The position.</returns>
	/// <param name="w">The width.</param>
	/// <param name="h">The height.</param>
	public Rect GetPosition(float w, float h)
	{
		Rect rect = m_guiPoistion;
		rect.width *= w;
		rect.height *= h;
		
		return rect;
	}
	
	/// <summary>
	/// Gets position that is offseted and scaled.
	/// </summary>
	/// <returns>The position.</returns>
	/// <param name="offsetX">Offset x.</param>
	/// <param name="offsetY">Offset y.</param>
	/// <param name="w">The width.</param>
	/// <param name="h">The height.</param>
	public Rect GetPosition(float offsetX, float offsetY, float w, float h)
	{
		Rect rect = m_guiPoistion;
		
		rect.x += offsetX;
		rect.y += offsetY;
		
		rect.width *= w;
		rect.height *= h;
		
		return rect;
	}
	
	/// <summary>
	/// Gets non scaled position.
	/// </summary>
	/// <returns>The position non scaled.</returns>
	/// <param name="w">The width.</param>
	/// <param name="h">The height.</param>
	public Rect GetPositionNonScaled(float w, float h)
	{
		Rect rect = m_guiPoistion;
		rect.width = w;
		rect.height = h;
		
		return rect;
	}
	
	/// <summary>
	/// Gets centered position.
	/// </summary>
	/// <returns>The position centered.</returns>
	/// <param name="w">The width.</param>
	/// <param name="h">The height.</param>
	public Rect GetPositionCentered(float w, float h)
	{
		Rect rect = m_guiPoistion;
		rect.width *= w;
		rect.height *= h;
		
		rect.x -= rect.width / 2;
		rect.y -= rect.height / 2;
		
		return rect;
	}
	
	protected override void GUAction()
	{
		if (camera == null) 
		{
			return;
		}
		
		Vector3 screenPosition = camera.WorldToScreenPoint(transform.position);
		
		m_guiPoistion = new Rect(screenPosition.x, screenPosition.y,transform.localScale.x, transform.localScale.y); 
		m_guiPoistion.y = Screen.height - m_guiPoistion.y;
	}	
	
	private void Reset()
	{
		if(camera == null)
		{
			if(AGUIController.SharedControl != null)
			{
				camera = AGUIController.SharedControl.camera;
			}
			else
			{
				camera = Camera.main;
			}
		}
	}
}
