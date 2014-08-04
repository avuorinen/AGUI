// AGUILabel.cs
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

/// <summary>
/// AGUI label uses bitmap fonts.
/// </summary>
[AddComponentMenu("AGUI/UI/Label")]
[ExecuteInEditMode]
public sealed class AGUILabel : AGUIObject 
{
	
	//TODO: Text commands for color changing.
	//TODO: Special effects. (Shadow, Outline)

	//TODO: Text limiter (Max letter per line, etc.)
	//TODO: font kerning (XML / JSON file).

	//TODO: Text that is text that is rendered. (Almost same as parsed text but it has end lines and spaces!)

	#region Special

	/// <summary>
	/// The advanced edit mode.
	/// (Editor)
	/// </summary>
	[SerializeField] [HideInInspector]
	private bool m_advEditMode = false;

	/// <summary>
	/// Create text.
	/// (Editor)
	/// </summary>
	private bool m_createText = false;
	
	#endregion
	
	#region Header

	/// <summary>
	/// Default letter name.
	/// </summary>
	private const string LETTER_NAME = "__NULL";

	/// <summary>
	/// The text.
	/// </summary>
	[SerializeField][Multiline]
	private string m_text = "";

	/// <summary>
	/// The text size.
	/// </summary>
	[SerializeField] 
	private float m_size = 1;

	/// <summary>
	/// The text scale.
	/// </summary>
	[SerializeField]
	private Vector2 m_textScale = Vector2.one;

	/// <summary>
	/// The text anchor.
	/// </summary>
	[SerializeField] 
	private TextAnchor m_anchor = TextAnchor.MiddleCenter;
	
	#region TextBuffer

	/// <summary>
	/// The parsed text.
	/// </summary>
	private string m_parsedText = "";

	/// <summary>
	/// Limit text.
	/// </summary>
	[SerializeField]
	private bool m_limitText = false;

	/// <summary>
	/// The text limit / buffer.
	/// </summary>
	[SerializeField]
	private int m_textBuffer = -1;
	
	#endregion

	/// <summary>
	/// The text spacing.
	/// </summary>
	[SerializeField]
	private Vector2 m_spacing = new Vector2(1f,1f);

	/// <summary>
	/// The text offset.
	/// </summary>
	[SerializeField]
	private float m_offset = 0;

	/// <summary>
	/// The text rotation.
	/// </summary>
	[SerializeField]
	private Vector3 m_rotation = Vector3.zero;

	/// <summary>
	/// Use capital letters to find letters from dictionary.
	/// </summary>
	[SerializeField]
	private bool m_useCaps = true;

	/// <summary>
	/// The font material.
	/// </summary>
	public Material fontMaterial;

	/// <summary>
	/// The font sheet.
	/// </summary>
	[SerializeField] 
	private Texture2D[] m_fontSheets;

	/// <summary>
	/// The font chars.
	/// </summary>
	public Sprite[] chars;

	/// <summary>
	/// The font dictionary.
	/// </summary>
	public Dictionary<char,Sprite> charsDic = new Dictionary<char, Sprite>();

	/// <summary>
	/// The list of current letters.
	/// </summary>
	[SerializeField]
	private List<SpriteRenderer> m_letters = new List<SpriteRenderer>();

	/// <summary>
	/// The letter object creater.
	/// </summary>
	[SerializeField][HideInInspector]
	private GameObject m_spriteObject;

	/// <summary>
	/// The color of the letters.
	/// </summary>
	private Color m_spriteColor = Color.white;

	/// <summary>
	/// Value that stores temporary bounds.
	/// </summary>
	private Bounds m_bounds;
	
	#endregion

	#region Properties

	/// <summary>
	/// Gets or sets the text.
	/// </summary>
	/// <value>The text.</value>
	public string text
	{
		get
		{
			return m_text;
		}
		
		set
		{
			m_text = value;
			CreateText();
		}
	}

	/// <summary>
	/// Gets or sets the size of text.
	/// </summary>
	/// <value>The size.</value>
	public float Size
	{
		get
		{
			return m_size;
		}
		
		set
		{
			m_size = value;
			CreateText(true);
		}
	}

	/// <summary>
	/// Gets or sets the text scale.
	/// </summary>
	/// <value>The text scale.</value>
	public Vector2 TextScale
	{
		get
		{
			return m_textScale;
		}

		set
		{
			m_textScale = value;
			CreateText(true);
		}
	}

	/// <summary>
	/// Gets or sets the anchor of text.
	/// </summary>
	/// <value>The anchor.</value>
	public TextAnchor Anchor
	{
		get
		{
			return m_anchor;
		}
		
		set
		{
			m_anchor = value;
			CreateText();
		}
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="AGUILabel"/> limit text.
	/// </summary>
	/// <value><c>true</c> if limit text; otherwise, <c>false</c>.</value>
	public bool LimitText
	{
		get
		{
			return m_limitText;
		}
		
		set
		{
			m_limitText = value;
			CreateText(true);
		}
	}

	/// <summary>
	/// Gets or sets the text bufffer.
	/// </summary>
	/// <value>The text bufffer.</value>
	public int TextBufffer
	{
		get
		{
			return m_textBuffer;
		}
		
		set
		{
			m_textBuffer = value;
			CreateText(true);
		}
	}

	/// <summary>
	/// Gets or sets the spacing of text.
	/// </summary>
	/// <value>The spacing.</value>
	public Vector2 Spacing
	{
		get
		{
			return m_spacing;
		}
		
		set
		{
			m_spacing = value;
			CreateText();
		}
	}

	/// <summary>
	/// Gets or sets the offset of text.
	/// </summary>
	/// <value>The offset.</value>
	public float Offset
	{
		get
		{
			return m_offset;
		}
		
		set
		{
			m_offset = value;
			CreateText();
		}
	}

	/// <summary>
	/// Gets or sets the rotation of text.
	/// </summary>
	/// <value>The rotation.</value>
	public Vector3 Rotation
	{
		get
		{
			return m_rotation;
		}
		
		set
		{
			m_rotation = value;
			CreateText();
		}
	}

	/// <summary>
	/// Gets the font sheet.
	/// </summary>
	/// <value>The font sheet.</value>
	public Texture2D[] FontSheets
	{
		get
		{
			return m_fontSheets;
		}
	}

	/// <summary>
	/// Gets the sprite object.
	/// </summary>
	/// <value>The sprite object.</value>
	public GameObject SpriteObject
	{
		get
		{
			CreateSpriteObject();
			return m_spriteObject;
		}
	}

	/// <summary>
	/// Custom color rule for object.
	/// For example, UILabel have multiple objects that color it changes.
	/// </summary>
	/// <value>The color tint.</value>
	protected override Color ColorTint 
	{
		get 
		{
			return m_spriteColor;
		}
		set 
		{
			m_spriteColor = value;
			
			for(int i = 0; i < m_letters.Count; i++)
			{
				if(m_letters[i] != null)
				{
					m_letters[i].color = value;
				}
			}
		}
	}

	#endregion

	#region Core
	
	/// <summary>
	/// Loads the chars to dictionary.
	/// </summary>
	public void LoadChars()
	{
		if(charsDic != null)
		{
			charsDic.Clear();
		}
		
		if(chars == null || chars.Length == 0)
		{
			return;
		}
		
		foreach(Sprite sp in chars)
		{
			charsDic.Add(sp.name[0],sp);
		}
		
		m_createText = true;
	}

	/// <summary>
	/// Reset this instance.
	/// </summary>
	public void ResetLabel()
	{
		UpdateLetters(true);
	}
	
	/// <summary>
	/// Clears the null objects.
	/// </summary>
	public void ClearNullObjects()
	{
		SpriteRenderer[] rens = GetComponentsInChildren<SpriteRenderer>();
		
		for(int i = 0; i < rens.Length; i++)
		{
			if(rens[i].name == "__NULL" || rens[i].name == "__Letter")
			{
				if(!m_letters.Contains(rens[i]))
				{
					GameObject.DestroyImmediate(rens[i].gameObject);
				}
			}
		}
	}
	

	/// <summary>
	/// Creates the text.
	/// </summary>
	/// <param name="force">If set to <c>true</c> force.</param>
	public void CreateText(bool force = false)
	{
		if(gameObject.isStatic || m_advEditMode)
		{
			return;
		}
		
		if(FontSheets == null || FontSheets.Length == 0)
		{
			for(int i = 0; i < m_letters.Count; i++)
			{
				if(m_letters[i] != null)
				{
					GameObject.DestroyImmediate(m_letters[i].gameObject);
					i--;
				}
			}
			
			m_letters.Clear();
			chars = new Sprite[0];
			
			return;
		}
		
		Quaternion rotation = transform.rotation;
		transform.rotation = Quaternion.Euler(0,0,0);
		
		Vector2 scaledSpacing = Spacing * Size;
		
		#if UNITY_EDITOR

		if(!Application.isPlaying)
		{
			LoadChars();
			//ClearNullObjects();
		}
		#endif

		m_parsedText = text.Replace(" ","");
		m_parsedText = m_parsedText.Replace("\n","");

		//Create Letters//
		InitLetters();

		//Update Letters//
		UpdateLetters();

		//Update Positions//
		List<Transform> gTransforms;
		int totalLines = UpdatePositions(scaledSpacing,out gTransforms);
		
		//Handle anchors//
		
		for(int i = 0; i < m_letters.Count; i++)
		{
			if(Anchor == TextAnchor.MiddleCenter || Anchor == TextAnchor.MiddleLeft || Anchor == TextAnchor.MiddleRight)
			{
				gTransforms[i].position -= new Vector3(0,-scaledSpacing.y * totalLines * transform.localScale.y) / 2;
			}
			else if(Anchor == TextAnchor.LowerCenter || Anchor == TextAnchor.LowerLeft || Anchor == TextAnchor.LowerRight)
			{
				gTransforms[i].position -= new Vector3(0,(-scaledSpacing.y * totalLines - (scaledSpacing.y / 2)) * transform.localScale.y);
			}
			else
			{
				gTransforms[i].position -= new Vector3(0,scaledSpacing.y / 2 * transform.localScale.y);
			}
		}
		
		//Reset values//
		transform.rotation = rotation;
	}

	private bool InitLetters()
	{
		int letters2Create = m_parsedText.Length;

		if(m_textBuffer >= 0)
		{
			//Buffer//
		
			//Remove Letters that are over buffer limit//
			if(m_letters.Count > m_textBuffer)
			{
				for(int i = m_textBuffer; i < m_letters.Count; i++)
				{
					if(m_letters[i] != null)
					{
						GameObject.DestroyImmediate(m_letters[i].gameObject);
						m_letters[i] = null;
					}
				}
			}

			if(m_limitText == true)
			{
				letters2Create = m_textBuffer;
			}

			//Add Objects to list.
			if(m_letters.Count < m_textBuffer)
			{
				for(int i = m_letters.Count; i < m_textBuffer; i++)
				{
					m_letters.Add(CreateSprite());
				}
			}
		}
		else if(m_letters.Count > m_parsedText.Length && m_textBuffer == -1)
		{
			//Remove objects that are over parsed text if text buffer is -1.
			for(int i = m_parsedText.Length; i < m_letters.Count; i++)
			{
				if(m_letters[i] != null)
				{
					GameObject.DestroyImmediate(m_letters[i].gameObject);
					m_letters[i] = null;
				}
			}
		}

		//Remove Null objects.
		m_letters.RemoveAll( x => x == null);

		for(int i = m_letters.Count; i < letters2Create; i++)
		{
			m_letters.Add(CreateSprite());
		}

		return true;
	}

	/// <summary>
	/// Updates the letters.
	/// </summary>
	/// <returns><c>true</c>, if letters was updated, <c>false</c> otherwise.</returns>
	private bool UpdateLetters(bool force = false)
	{
		Sprite sprite = null;

		SpriteRenderer sRenderer = null;
		//GameObject gObject = null;
		Transform gTransform = null;

		Quaternion rot = Quaternion.Euler(Rotation);
	
		//TODO:Better way handle sorting layers in update.

		if(m_letters.Count == 0)
		{
			return false;
		}

		bool updateSortLayer = true;

		//m_letters[0] != null && 
		if(m_letters[0] != null && m_letters[0].sortingLayerName == SortingLayer && m_letters[0].sortingOrder == SortingOrder)
		{
			updateSortLayer = false;
		}

		for(int i = 0; i < m_letters.Count; i++)
		{
			sRenderer = m_letters[i];
			//gObject = sRenderer.gameObject;
			gTransform  = sRenderer.transform;

			if(m_parsedText.Length <= i)
			{
				gTransform.localPosition = Vector3.zero;
				gTransform.name = LETTER_NAME;

				if(sRenderer.sprite != null)
				{
					sRenderer.sprite = null;
				}
				
				continue;
			}

			//Skip Sprite chaging if sprite already same.
			if(sRenderer.name != m_parsedText[i].ToString() || force)
			{
				if(sprite == null || sprite.name != m_parsedText[i].ToString())
				{
					sprite = GetSprite(m_parsedText[i]);
				}

				sRenderer.sprite = sprite;

				if(sprite == null)
				{
					gTransform.localPosition = Vector3.zero;
					gTransform.name = LETTER_NAME;
					continue;
				}

				gTransform.name = sRenderer.sprite.name;
			}

			if(updateSortLayer == true)
			{
				sRenderer.sortingLayerName = SortingLayer;
				sRenderer.sortingOrder = SortingOrder;
			}

			sRenderer.material = fontMaterial;
			sRenderer.color = m_spriteColor;
			
			gTransform.localRotation = rot;
			gTransform.localScale = m_textScale * Size;
		}
		
		return true;
	}

	/// <summary>
	/// Updates the positions.
	/// </summary>
	/// <returns>The positions.</returns>
	/// <param name="spacing">Spacing.</param>
	private int UpdatePositions(Vector3 spacing, out List<Transform> transforms)
	{

		Vector3 position = Vector3.zero;

		//0 == Middle, -1 == Left, 1 == Right//
		int anchor = 0;
		int totalLines = 0;

		int lastIndex = 0;
		int letterIndex = 0;


		if(Anchor == TextAnchor.LowerLeft || Anchor == TextAnchor.MiddleLeft || Anchor == TextAnchor.UpperLeft)
		{
			anchor = -1;
		}
		else if(Anchor == TextAnchor.LowerRight || Anchor == TextAnchor.MiddleRight || Anchor == TextAnchor.UpperRight)
		{
			anchor = 1;
		}

		Transform gTransform = null;
		List<Transform> gTransforms = new List<Transform>();

		SpriteRenderer sRenderer = null;
		int count = m_letters.Count;

		for(int i = 0; i < m_text.Length; i++)
		{
			if(m_text[i] == ' ')
			{
				position.x += spacing.x;
				continue;
			}
			else if(m_text[i] == '\n')
			{
				if(anchor == 1)
				{
					for(int j = lastIndex; j < letterIndex; j++)
					{
						gTransforms[j].localPosition -= new Vector3(position.x,0);
						lastIndex++;
					}
				}
				else if(anchor == 0)
				{
					for(int j = lastIndex; j < letterIndex; j++)
					{
						gTransforms[j].localPosition -= new Vector3(position.x,0) / 2;
						lastIndex++;
					}
				}
				
				totalLines++;
				
				position.x = 0;
				position.y -= spacing.y;
				continue;
			}

			if(count > letterIndex)
			{
				sRenderer = m_letters[letterIndex];

				if(sRenderer)
				{
					gTransform = sRenderer.transform;
					gTransforms.Add(gTransform);
				}
			}
			else
			{
				break;
			}

			if(sRenderer == null || sRenderer.sprite == null)
			{
				letterIndex++;



				continue;
			}

			if(letterIndex - 1 >= 0)
			{
				if(m_parsedText[letterIndex -1] != text[i])
				{
					m_bounds = sRenderer.sprite.bounds;
				}
			}
			else
			{
				m_bounds = sRenderer.sprite.bounds;
			}

			gTransform.localPosition = position + new Vector3((position.x != 0 ? Offset : 0) + m_bounds.extents.x * Size,0);
			position.x += m_bounds.size.x * Size + (position.x != 0 ? Offset : 0);

			letterIndex++;
		}
		
		if(anchor == 1)
		{
			for(int j = lastIndex; j < count; j++)
			{
				gTransforms[j].localPosition -= new Vector3(position.x,0);
			}
		}
		else if(anchor == 0)
		{
			for(int j = lastIndex; j < count; j++)
			{
				gTransforms[j].localPosition -= new Vector3(position.x,0) / 2;
			}
		}

		transforms = gTransforms;
		return totalLines;
	}

	/// <summary>
	/// Creates the sprite.
	/// </summary>
	/// <returns>The sprite.</returns>
	private SpriteRenderer CreateSprite()
	{
		SpriteRenderer sRenderer = (GameObject.Instantiate(SpriteObject,Vector3.zero,Quaternion.identity) as GameObject).GetComponent<SpriteRenderer>();
		sRenderer.transform.parent = transform;
		sRenderer.transform.localPosition = Vector3.zero;

		sRenderer.name = LETTER_NAME;
		
		sRenderer.gameObject.hideFlags = HideFlags.HideInHierarchy;
		sRenderer.gameObject.isStatic = gameObject.isStatic;
		sRenderer.gameObject.layer = gameObject.layer;

		sRenderer.sortingLayerName = SortingLayer;
		sRenderer.sortingOrder = SortingOrder;
	
		return sRenderer;
	}

	/// <summary>
	/// Gets the sprite from dictionary.
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="name">Name.</param>
	private Sprite GetSprite(char name)
	{
		Sprite sp = null;

		if(m_useCaps)
		{
			char nameUpper = char.ToUpper(name);
			charsDic.TryGetValue(nameUpper, out sp);

			if(sp != null)
			{
				return sp;
			}

			char nameLower = char.ToLower(name);
			charsDic.TryGetValue(nameLower, out sp);
		}
		else
		{
			charsDic.TryGetValue(name, out sp);
		}
		
		return sp;
	}

	/// <summary>
	/// Creates the sprite object.
	/// </summary>
	private void CreateSpriteObject()
	{
		if(m_spriteObject == null)
		{
			m_spriteObject = new GameObject("__Letter",typeof(SpriteRenderer));
			m_spriteObject.transform.parent = transform;
			m_spriteObject.transform.localPosition = Vector3.zero;
			m_spriteObject.hideFlags = HideFlags.HideInHierarchy;
		}
	}
	
	#endregion
	
	#region Body

	/// <summary>
	/// Init AGUILabel..
	/// </summary>
	protected override void Awake()
	{
		LoadChars();
	}

	/// <summary>
	/// Init rule for AGUIObject.
	/// (This is called when you edit values in editor)
	/// </summary>
	protected override void Init ()
	{
		m_createText = true;
	}

	/// <summary>
	/// Used for first time init.
	/// Always remember call base.Reset(); !!
	/// </summary>
	protected override void Reset ()
	{
		fontMaterial = GHelper.GetDefaultMaterial();
		
		base.Reset ();
	}

	/// <summary>
	/// OnEnable base.
	/// </summary>
	protected override void OnEnable()
	{
		for(int i = 0; i < m_letters.Count; i++)
		{
			if(m_letters[i] != null)
			{
				m_letters[i].gameObject.SetActive(true);
			}
		}
	}

	/// <summary>
	/// OnDisable base.
	/// </summary>
	protected override void OnDisable()
	{
		for(int i = 0; i < m_letters.Count; i++)
		{
			if(m_letters[i] != null)
			{
				m_letters[i].gameObject.SetActive(false);
			}
		}
	}

	#if UNITY_EDITOR
	/// <summary>
	/// Update AGUILabel.
	/// Create text when game isn't running.
	/// </summary>
	private void Update()
	{
		if(!Application.isPlaying && m_createText)
		{
			m_createText = false;
			CreateText(true);
		}
	}
	#endif
	
	#endregion
}