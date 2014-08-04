// AGUIControllerEditor.cs
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
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(AGUIController))]
public class AGUIControllerEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		AGUIController cont = (AGUIController)target;

		if(!cont.camera.isOrthoGraphic)
		{
			EditorGUILayout.TextArea("Unfortunately Controller doesn't support perspective camera!");
		}

		base.OnInspectorGUI ();
	}

	[DrawGizmo(GizmoType.SelectedOrChild | GizmoType.NotSelected)]
	static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
	{
		if(gizmoType == GizmoType.Selected)
		{
			return;
		}

		if(objectTransform.GetComponent<AGUIController>())
		{
			AGUIController cont = objectTransform.GetComponent<AGUIController>();

			if(cont.controllerType == AGUIController.ControllerType.Shared)
			{
				if(cont == AGUIController.SharedControl)
				{
					Handles.color = Color.magenta;
				}	
				else
				{
					Handles.color = Color.magenta * 0.75f;
				}
			}
			else
			{
				Handles.color = Color.white;
			}

			Rect cameraRect = objectTransform.camera.rect;

			Handles.DrawLine(objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.y)),objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.y)));
			Handles.DrawLine(objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.y)),objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.height + cameraRect.y)));
			Handles.DrawLine(objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.height + cameraRect.y)),objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.height + cameraRect.y)));
			Handles.DrawLine(objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.height + cameraRect.y)),objectTransform.camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.y)));

			Handles.color = Color.white;
		}
	}
}