// ShoeBoxUtilities.cs
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

using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

public class ShoeBoxUtilities 
{	
	//Create SpriteSheet from xml or json file!

	private static string[] fileExtensions = {"json","xml","txt"}; 
	
	[MenuItem("AGUI/Shoebox/Slice")]
	public static void Slice()
	{
		if(Selection.objects.Length == 0 && Selection.activeObject == null)
		{
			return;
		}

		Texture2D sprite = null;
		TextAsset parse = null;

		string type = "";
		string path = "";

		if(Selection.objects.Length == 2)
		{
			foreach(Object obj in Selection.objects)
			{
				if(sprite == null)
				{
					if(obj.GetType() == typeof(Texture2D))
					{
						sprite = obj as Texture2D;
					}
				}

				if(parse == null)
				{
					if(obj.GetType() == typeof(TextAsset))
					{
						parse = obj as TextAsset;

						path = AssetDatabase.GetAssetPath(obj);
						type = Path.GetExtension(path).Replace(".","");
					}
				}
			}
		}
		else if(Selection.activeObject != null)
		{
			path = AssetDatabase.GetAssetPath(Selection.activeObject);
			
			if(Selection.activeObject.GetType() == typeof(Texture2D))
			{
				sprite = Selection.activeObject as Texture2D;
				
				path = Path.ChangeExtension(path,"");
				
				foreach(string extension in fileExtensions)
				{
					parse = AssetDatabase.LoadAssetAtPath(path + extension, typeof(TextAsset)) as TextAsset;
					
					if(parse)
					{
						type = extension;
						break;
					}
				}
			}
		}

		if(parse && sprite)
		{
			TextureImporter importer = AssetImporter.GetAtPath( AssetDatabase.GetAssetPath(sprite) ) as TextureImporter;

			int width = 0, height = 0;

			sprite.GetImageSize(out width,out height);

			UpdateSprite(importer,parse.text,type, new Vector2(width,height) );
		}
	}

	private static void UpdateSprite(TextureImporter assetImporter, string parse, string type, Vector2 size)
	{
		assetImporter.textureType = TextureImporterType.Sprite;
		assetImporter.spriteImportMode = SpriteImportMode.Multiple;

		List<SpriteMetaData> spriteData = new List<SpriteMetaData>();

		if(type.Equals("") || type.Equals("txt"))
		{
			if(parse.Contains("{"))
			{
				type = "json";
			}
			else if(parse.Contains("<"))
			{
				type = "xml";
			}
			else
			{
				Debug.LogError("unknown type!");
				return;
			}
		}

		SpriteMetaData current = new SpriteMetaData();

		if(type.Equals("json"))
		{
			foreach(string row in parse.Split(new char[]{'\n'}))
			{
				string line = row.Trim();

				if(line.Contains("."))
				{
					current = new SpriteMetaData();

					int startIndex = line.IndexOf('"') + 1;
					int lenght = line.IndexOf(".") - startIndex;

					current.name = line.Substring(startIndex,lenght);

					current.pivot = new Vector2(0.5f,0.5f);

					//Just making sure...//
					current.alignment = (int)UnityEngine.SpriteAlignment.Center;
				}

				if(line.Contains("frame") && !line.Contains("frames"))
				{
					line = line.Replace(",",":").Replace("{","").Replace("}","");
					string[] sizes = line.Split(new char[]{':'});

					string lastIndex = "";

					Rect rect = new Rect();
					float tryFloat;

					foreach(string s in sizes)
					{
						if(float.TryParse(s,out tryFloat))
						{
							if(lastIndex.Equals("x"))
							{
								rect.x = tryFloat;
							}
							else if(lastIndex.Equals("y"))
							{
								rect.y = tryFloat;
							}
							else if(lastIndex.Equals("w"))
							{
								rect.width = tryFloat;
							}
							else if(lastIndex.Equals("h"))
							{
								rect.height = tryFloat;
							}
						}

						lastIndex = s.Replace("\"","").Trim();
					}

					rect.y = size.y - (rect.height + rect.y);
					current.rect = rect;

					if(!spriteData.Contains(current))
					{

						spriteData.Add(current);
					}
				}
			}
		}
		else if(type.Equals("xml"))
		{
			using(XmlReader reader = XmlReader.Create(new StringReader(parse)))
			{
				reader.ReadToFollowing("TextureAtlas");
				
				while(reader.ReadToFollowing("SubTexture") != false)
				{
					string name = reader.GetAttribute("name");

					int lenght = name.IndexOf(".");
					
					current.name = name.Substring(0,lenght);

					current.pivot = new Vector2(0.5f,0.5f);
					
					//Just making sure...//
					current.alignment = (int)UnityEngine.SpriteAlignment.Center;

					Rect rect = new Rect();

					rect.x = float.Parse(reader.GetAttribute("x"));
					rect.y = float.Parse(reader.GetAttribute("y"));
					rect.width = float.Parse(reader.GetAttribute("width"));
					rect.height = float.Parse(reader.GetAttribute("height"));

					rect.y = size.y - (rect.height + rect.y);

					current.rect = rect;

					if(!spriteData.Contains(current))
					{
						
						spriteData.Add(current);
					}
				}
			}
		}

		if(spriteData.Count != 0)
		{
			assetImporter.spritesheet = spriteData.ToArray();
			AssetDatabase.ImportAsset(assetImporter.assetPath,ImportAssetOptions.ForceUpdate);
		}
	}
}
