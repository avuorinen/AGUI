// AGUIPanelEditor.cs
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

using UnityEditor;
using UnityEngine;
using System.Collections;

[CanEditMultipleObjects]
[CustomEditor(typeof(AGUIPanel))]
public class AGUIPanelEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		AGUIPanel panel = (AGUIPanel)target;

		EditorGUI.BeginChangeCheck();

		panel.gameObject.isStatic = EditorGUILayout.Toggle("Static",panel.gameObject.isStatic);

		if(EditorGUI.EndChangeCheck())
		{
			foreach(Transform trans in panel.GetComponentInChildren<Transform>())
			{
				trans.gameObject.isStatic = panel.gameObject.isStatic;
			}
		}

		if(GUILayout.Button("Override sorting layer"))
		{
			foreach(Renderer ren in panel.GetComponentsInChildren<Renderer>())
			{
				ren.sortingLayerName = panel.sortingLayer;
			}

			foreach(AGUIObject obj in panel.GetComponentsInChildren<AGUIObject>())
			{
				obj.SortingLayer = panel.sortingLayer;
			}
		}
	}

	[DrawGizmo(GizmoType.SelectedOrChild | GizmoType.NotSelected)]
	static void RenderCustomGizmo(Transform objectTransform, GizmoType gizmoType)
	{
		if(objectTransform.GetComponent<AGUIPanel>())
		{
			Camera camera = objectTransform.GetComponent<AGUIPanel>().camera;

			if(camera == null)
			{
				camera = Camera.main;
			}

			if(camera != null)
			{
				//TODO: User can change color value!
				//TODO: Better camera system.

				Color cyan = Color.cyan;
				cyan.a = 0.75f;

				Handles.color = cyan;

				Vector3 offset = Vector3.zero;
				Vector3 orginalPos = camera.transform.position;

				if(objectTransform.camera == null || objectTransform.camera != camera)
				{
					offset = objectTransform.position;
					camera.transform.position = Vector3.zero;
				}

				Rect cameraRect = camera.rect;
				
				Handles.DrawLine(offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.y)),offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.y)));
				Handles.DrawLine(offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.y)),offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.height + cameraRect.y)));
				Handles.DrawLine(offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.width + cameraRect.x, cameraRect.height + cameraRect.y)),offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.height + cameraRect.y)));
				Handles.DrawLine(offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.height + cameraRect.y)),offset + camera.ViewportToWorldPoint(new Vector3(cameraRect.x, cameraRect.y)));

				camera.transform.position = orginalPos;
			}

			Handles.color = Color.white;
		}
	}
}