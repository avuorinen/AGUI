// AGUIBoxEditor.cs
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
using System.Collections;
using System.Reflection;

[CanEditMultipleObjects()]
[CustomEditor(typeof(AGUIBox))]
public class AGUIBoxEditor : Editor
{
	//TODO: Support Pixel2Unit (Change box offset ratio to match to box size. ( Mathf.Min(c.sprite.textureRect.width / c.sprite.bounds.size.x, m_sprite.textureRect.height / c.sprite.bounds.size.y); )

	private static Rect NormalizeRect(Rect rect)
	{
		rect.x = Mathf.Abs(rect.x);
		rect.y = Mathf.Abs(rect.y);
		rect.width = Mathf.Abs(rect.width);
		rect.height = Mathf.Abs(rect.height);
		
		return rect;
	}

	private static Rect GetRect(Vector3 pos1,Vector3 pos2)
	{
		return new Rect(Mathf.Abs(pos1.x),Mathf.Abs(pos1.y),Mathf.Abs( (pos2.x - pos1.x) ),Mathf.Abs( (pos2.y - pos1.y) ));
	}

	private static bool m_showLines = false;
	private bool m_editMode = false;
	private bool m_show;

	private void OnDisable()
	{
		m_showLines = false;
		m_editMode = false;

		HandleSprites();
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();

		AGUIBox box = (AGUIBox)target;

		if(box.sprite == null)
		{
			return;
		}


		if(GUILayout.Button("Toggle Edit"))
		{
			m_showLines = m_editMode = !m_editMode;
			HandleSprites();
		}

		if(m_editMode)
		{	

			m_show = EditorGUILayout.Foldout(m_show,"Parts");
			EditorGUI.indentLevel++;

			if(m_show)
			{
				box.Center = (AGUISprite)EditorGUILayout.ObjectField("Center",box.Center, typeof(AGUISprite),true );
				box.Top = (AGUISprite)EditorGUILayout.ObjectField("Top",box.Top, typeof(AGUISprite),true );
				box.Bottom = (AGUISprite)EditorGUILayout.ObjectField("Bottom",box.Bottom, typeof(AGUISprite),true );
				box.Left = (AGUISprite)EditorGUILayout.ObjectField("Left",box.Left, typeof(AGUISprite),true );
				box.Right = (AGUISprite)EditorGUILayout.ObjectField("Right",box.Right, typeof(AGUISprite),true );
				box.TopLeft = (AGUISprite)EditorGUILayout.ObjectField("TopLeft",box.TopLeft, typeof(AGUISprite),true );
				box.TopRight = (AGUISprite)EditorGUILayout.ObjectField("TopRight",box.TopRight, typeof(AGUISprite),true );
				box.BottomLeft = (AGUISprite)EditorGUILayout.ObjectField("BottomLeft",box.BottomLeft, typeof(AGUISprite),true );
				box.BottomRight = (AGUISprite)EditorGUILayout.ObjectField("BottomRight",box.BottomRight, typeof(AGUISprite),true );
			}

			EditorGUI.indentLevel--;

			if(GUILayout.Button("Show / Hide parts"))
			{
				foreach(AGUISprite sp in box.sprites)
				{
					if(sp.gameObject.hideFlags != HideFlags.None)
					{
						sp.gameObject.hideFlags = HideFlags.None;
					}
					else
					{
						sp.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable;
					}
				}
			}

			if(GUILayout.Button("Create"))
			{
				bool forceCreate = false;

				if(box.sprites.Length != 9)
				{
					box.sprites = new AGUISprite[9];
				}

				for(int i = 0; i < box.sprites.Length; i++)
				{
					AGUISprite current = box.sprites[i];

					for(int j = 0; j < box.sprites.Length; j++)
					{
						if(j == i)
						{
							continue;
						}

						if(current == box.sprites[j])
						{
							box.sprites[j] = null;
						}
					}
				}

				for(int i = 0; i < box.sprites.Length; i++)
				{
					if(box.sprites[i] == null)
					{
						GameObject go = new GameObject("_Part");
						box.sprites[i] = go.AddComponent<AGUISprite>();
						go.transform.parent = box.transform;
						go.SetActive(false);

						forceCreate = true;
					}
				}

			Create:

				foreach(AGUISprite sp in box.sprites)
				{
					sp.CurrentSprite = box.sprite;
				}

				Bounds bounds = new Bounds(new Vector3(box.sprite.rect.x, box.sprite.rect.y),new Vector3(box.sprite.rect.width,box.sprite.rect.height));

				float size = ((box.sprite.bounds.size.x * 100) / bounds.size.x / 2) + ((box.sprite.bounds.size.y * 100) / bounds.size.y / 2);

				//TODO: (10) logic. (Better way get that "magical" number!)
				float scale = (bounds.size.x / bounds.size.y + 10) * (10 / size);
				Vector2 scaled = box.Offset * scale;

				Rect topLeft = GetRect(new Vector3(-scaled.x ,scaled.y),new Vector3(-bounds.extents.x,bounds.extents.y));
				topLeft.y = bounds.size.y - topLeft.height;
				topLeft.x = 0;

				Rect topRight = GetRect(new Vector3(scaled.x ,bounds.extents.y),new Vector3(bounds.extents.x,scaled.y));
				topRight.y = bounds.size.y - topRight.height;
				topRight.x = -bounds.size.x + topRight.width;

				//Top
				Rect top = GetRect(new Vector3(-scaled.x ,bounds.extents.y),new Vector3(scaled.x,scaled.y));
				top.y = bounds.size.y - top.height;
				top.x = topLeft.width;

				//Bottom
				Rect bottom = GetRect(new Vector3(-scaled.x ,-scaled.y),new Vector3(scaled.x ,-bounds.extents.y));
				bottom.y = 0;
				bottom.x = topLeft.width;
				
				Rect bottomLeft = GetRect(new Vector3(-bounds.extents.x,-scaled.y),new Vector3(-scaled.x ,-bounds.extents.y));
				bottomLeft.y = 0;
				bottomLeft.x = 0;
				
				Rect bottomRight = GetRect(new Vector3(scaled.x,-scaled.y),new Vector3(bounds.extents.x,-bounds.extents.y));
				bottomRight.y = 0;
				bottomRight.x = -bounds.size.x + bottomRight.width;

				//Left
				Rect left = GetRect(new Vector3(-bounds.extents.x,scaled.y),new Vector3(-scaled.x ,-scaled.y));
				left.x = 0;
				left.y = topLeft.height;
				
				//Right
				Rect right = GetRect(new Vector3(scaled.x,scaled.y),new Vector3(bounds.extents.x,-scaled.y));
				right.x = -bounds.size.x + right.width;
				right.y = topLeft.height;

				//Center
				Rect center = GetRect(new Vector3(-scaled.x,scaled.y),new Vector3(scaled.x ,-scaled.y));
				center.x = left.width;
				center.y = top.height;

				//Set rects//

				box.Center.name = "_Center";
				box.Center.CurrentRect = NormalizeRect(center);
				box.Center.pivot = new Vector2(0.5f,0.5f);

				box.Top.name = "_Top";
				box.Top.CurrentRect = NormalizeRect(top);
				box.Top.pivot = new Vector2(0.5f,0);

				box.Bottom.name = "_Bottom";
				box.Bottom.CurrentRect = NormalizeRect(bottom);
				box.Bottom.pivot = new Vector2(0.5f,1);

				box.Left.name = "_Left";
				box.Left.CurrentRect = NormalizeRect(left);
				box.Left.pivot = new Vector2(1,0.5f);

				box.Right.name = "_Right";
				box.Right.CurrentRect = NormalizeRect(right);
				box.Right.pivot = new Vector2(0,0.5f);

				box.TopLeft.name = "_TopLeft";
				box.TopLeft.CurrentRect = NormalizeRect(topLeft);
				box.TopLeft.pivot = new Vector2(1,0);

				box.TopRight.name = "_TopRight";
				box.TopRight.CurrentRect = NormalizeRect(topRight);
				box.TopRight.pivot = new Vector2(0,0);

				box.BottomLeft.name = "_BottomLeft";
				box.BottomLeft.CurrentRect = NormalizeRect(bottomLeft);
				box.BottomLeft.pivot = new Vector2(1,1);

				box.BottomRight.name = "_BottomRight";
				box.BottomRight.CurrentRect = NormalizeRect(bottomRight);
				box.BottomRight.pivot = new Vector2(0,1);

				foreach(AGUISprite sp in box.sprites)
				{
					sp.UpdateSprite();
				}

				box.UpdatePositions();

				if(forceCreate)
				{
					forceCreate = false;
					goto Create;
				}
			}
		}
	}

	private void HandleSprites()
	{
		AGUIBox box = (AGUIBox)target;

		foreach(AGUISprite sp in box.sprites)
		{
			if(sp == null)
			{
				continue;
			}

			sp.gameObject.SetActive(!m_editMode);
		}
		
		box.renderer.enabled = m_editMode;
		box.renderer.sprite = box.sprite;
	}

	[DrawGizmo(GizmoType.SelectedOrChild)]
	static void RenderCustomGizmo(Transform transform, GizmoType gizmoType)
	{
		if(transform.GetComponent<AGUIBox>())
		{
			if(!m_showLines)
			{
				return;
			}

			//TODO: Only positive offset values! (Need test first)
			//TODO: Visual effects for scaling.

			AGUIBox c = transform.GetComponent<AGUIBox>();

			if(c.sprite == null)
			{
				return;
			}

			Bounds bounds = c.sprite.bounds;

			Matrix4x4 old = Handles.matrix;
			Handles.matrix = Matrix4x4.TRS(transform.position,transform.rotation,transform.localScale);

			//GUI//


			//GRID//

			Handles.color = new Color(1,0.5f,0);
			//Top 2 bottom
			Handles.DrawLine(new Vector3(c.Offset.x, bounds.size.y * 0.75f), new Vector3(c.Offset.x, -bounds.size.y * 0.75f));
			//Left 2 righ
			Handles.DrawLine(new Vector3(bounds.size.x * 0.75f ,-c.Offset.y),new Vector3(-bounds.size.x * 0.75f ,-c.Offset.y));
			Handles.CubeCap(0,new Vector3(c.Offset.x,-c.Offset.y),Quaternion.identity,0.12f);

			Handles.color = Color.yellow;
			//Top 2 bottom
			Handles.DrawLine(new Vector3(-c.Offset.x, bounds.size.y * 0.75f),new Vector3(-c.Offset.x, -bounds.size.y * 0.75f));
			//Left 2 righ
			Handles.DrawLine(new Vector3(bounds.size.x * 0.75f ,c.Offset.y),new Vector3(-bounds.size.x * 0.75f ,c.Offset.y));
			Handles.CubeCap(0,new Vector3(-c.Offset.x,c.Offset.y),Quaternion.identity,0.12f);


			//DEBUG//

			/*
			Handles.color = Color.blue;

			//Top
			//Handles.CubeCap(0,new Vector3(-c.offset.x ,bounds.extents.y),Quaternion.identity,0.12f);
			//Handles.CubeCap(0,new Vector3(c.offset.x,c.offset.y),Quaternion.identity,0.12f);


			//Left
			//Handles.CubeCap(0,new Vector3(-bounds.extents.x,c.offset.y),Quaternion.identity,0.12f);
			//Handles.CubeCap(0,new Vector3(-c.offset.x ,-c.offset.y),Quaternion.identity,0.12f);

			//Right

			//Handles.CubeCap(0,new Vector3(c.offset.x,c.offset.y),Quaternion.identity,0.12f);
			//Handles.CubeCap(0,new Vector3(bounds.extents.x,-c.offset.y),Quaternion.identity,0.12f);

			//Bottom
			//Handles.CubeCap(0,new Vector3(-c.offset.x ,-c.offset.y),Quaternion.identity,0.12f);
			//Handles.CubeCap(0,new Vector3(c.offset.x ,-bounds.extents.y),Quaternion.identity,0.12f);


			//Center
			Handles.CubeCap(0,new Vector3(-c.offset.x,c.offset.y),Quaternion.identity,0.12f);
			Handles.CubeCap(0,new Vector3(c.offset.x ,-c.offset.y),Quaternion.identity,0.12f);

			Handles.color = Color.red;


			//Top Left
			Handles.CubeCap(0,new Vector3(-c.offset.x ,c.offset.y),Quaternion.identity,0.06f);
			Handles.CubeCap(0,new Vector3(-bounds.extents.x,bounds.extents.y),Quaternion.identity,0.06f);

			//Top right
			Handles.CubeCap(0,new Vector3(c.offset.x ,bounds.extents.y),Quaternion.identity,0.06f);
			Handles.CubeCap(0,new Vector3(bounds.extents.x,c.offset.y),Quaternion.identity,0.06f);


			//Bottom Left
		
			Handles.CubeCap(0,new Vector3(-bounds.extents.x,-c.offset.y),Quaternion.identity,0.06f);
			Handles.CubeCap(0,new Vector3(-c.offset.x ,-bounds.extents.y),Quaternion.identity,0.06f);

			//Bottom Right
			Handles.CubeCap(0,new Vector3(c.offset.x,-c.offset.y),Quaternion.identity,0.06f);
			Handles.CubeCap(0,new Vector3(bounds.extents.x,-bounds.extents.y),Quaternion.identity,0.06f);
			*/

			//Reset//
			Handles.matrix = old;

			/*
			Handles.color = Color.blue;

			//Top
			Rect top = GetRect(new Vector3(-c.offset.x ,bounds.extents.y),new Vector3(c.offset.x,c.offset.y));
			top.y = bounds.size.y - top.height;
			top.NormalizeRect();

			Rect topLeft = GetRect(new Vector3(-c.offset.x ,c.offset.y),new Vector3(-bounds.extents.x,bounds.extents.y));
			topLeft.y = bounds.size.y - topLeft.height;
			topLeft.x = 0;
			topLeft.NormalizeRect();
	
			Rect topRight = GetRect(new Vector3(c.offset.x ,bounds.extents.y),new Vector3(bounds.extents.x,c.offset.y));
			topRight.y = bounds.size.y - topRight.height;
			topRight.x = -bounds.size.x + topRight.width;
			topRight.NormalizeRect();

			//Bottom
			Rect bottom = GetRect(new Vector3(-c.offset.x ,-c.offset.y),new Vector3(c.offset.x ,-bounds.extents.y));
			bottom.y = 0;
			bottom.NormalizeRect();

			Rect bottomLeft = GetRect(new Vector3(-bounds.extents.x,-c.offset.y),new Vector3(-c.offset.x ,-bounds.extents.y));
			bottomLeft.y = 0;
			bottomLeft.x = 0;
			bottomLeft.NormalizeRect();

			Rect bottomRight = GetRect(new Vector3(c.offset.x,-c.offset.y),new Vector3(bounds.extents.x,-bounds.extents.y));
			bottomRight.y = 0;
			bottomRight.x = -bounds.size.x + bottomRight.width;
			bottomRight.NormalizeRect();

			//Center
			Rect center = GetRect(new Vector3(-c.offset.x,c.offset.y),new Vector3(c.offset.x ,-c.offset.y));
			center.NormalizeRect();

			//Left
			Rect left = GetRect(new Vector3(-bounds.extents.x,c.offset.y),new Vector3(-c.offset.x ,-c.offset.y));
			left.x = 0;
			left.NormalizeRect();

			//Right
			Rect right = GetRect(new Vector3(c.offset.x,c.offset.y),new Vector3(bounds.extents.x,-c.offset.y));
			right.x = -bounds.size.x + right.width;
			right.NormalizeRect();

			//Debug.Log(bottomLeft.Rect2World() );
			*/
		
			Handles.color = Color.white;
		}
	}
}

