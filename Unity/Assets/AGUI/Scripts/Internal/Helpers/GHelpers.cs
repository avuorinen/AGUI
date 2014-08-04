// GHelper.cs
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

#if UNITY_EDITOR || (!UNITY_FLASH && !NETFX_CORE)
#define USE_FORMATTERS
#endif

using UnityEngine;
using System.Collections;

using System.IO;
using System.Runtime.Serialization;

#if USE_FORMATTERS
using System.Runtime.Serialization.Formatters.Binary;
#endif

using System.Xml.Serialization;

public static class GHelper
{
	/// <summary>
	/// Gets the component or add it if GameObject doesn't have it.
	/// </summary>
	/// <returns>The component.</returns>
	/// <param name="go">Go.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T GetComponent<T>(GameObject go) where T : Component
	{
		T comp = go.GetComponent<T>();
		
		if(comp == null)
		{
			comp = go.AddComponent<T>();
		}
		
		return comp;
	}

	/// <summary>
	/// Gets the default material.
	/// </summary>
	/// <returns>The default material.</returns>
	public static Material GetDefaultMaterial()
	{
		return Resources.Load<Material>("Materials/AGUI-Default");
	}

	/// <summary>
	/// Determines if has flag the specified enumValue enumLookingValue.
	/// </summary>
	/// <returns><c>true</c> if has flag the specified enumValue enumLookingValue; otherwise, <c>false</c>.</returns>
	/// <param name="enumValue">Enum value.</param>
	/// <param name="enumLookingValue">Enum looking value.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static bool HasFlag<T>(this T enumValue, T enumLookingValue) where T : System.IComparable
	{
		int value = (int) (object) enumValue;
		int looking = (int) (object) enumLookingValue;
		
		if( (value & looking) == looking)
		{
			return true;
		}
		
		return false;
	}

	public static T CreateInstance<T>(string name) where T : Component
	{
		GameObject go = new GameObject(name);
		return go.AddComponent<T>();
	}

	public static T CreateStaticInstance<T>(string name) where T : Component
	{
		T instance = CreateInstance<T>(name);
		GameObject.DontDestroyOnLoad(instance.gameObject);

		return instance;
	}
}

public static class GColorHelper
{
	/// <summary>
	/// Gets the color using GameObject to get GData.
	/// </summary>
	/// <returns>The color.</returns>
	/// <param name="go">Go.</param>
	/// <param name="shared">If set to <c>true</c> shared.</param>
	public static Color GetColor(GameObject go, bool shared = false)
	{
		return GetColor(GData.GetData(go),shared);
	}

	/// <summary>
	/// Gets the color using GData.
	/// </summary>
	/// <returns>The color.</returns>
	/// <param name="data">Data.</param>
	/// <param name="shared">If set to <c>true</c> shared.</param>
	public static Color GetColor(GData data, bool shared = false)
	{
		if(data.aguiObject && !data.aguiObject.simulateNonAGUI)
		{
			return data.aguiObject.color;
		}
		else if(data.renderer)
		{
			if(shared)
			{
				return data.renderer.sharedMaterial.color;
			}
			else if(data.renderer.GetType() == typeof(SpriteRenderer) && data.aguiObject == null)
			{
				return (data.renderer as SpriteRenderer).color;
			}
			else
			{
				return data.renderer.material.color;
			}
		}

		else if(data.light)
		{
			return data.light.color;
		}
		
		return Color.black;
	}

	/// <summary>
	/// Sets the color using GameObject to get GData.
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="color">Color.</param>
	/// <param name="shared">If set to <c>true</c> shared.</param>
	public static void SetColor(GameObject go, Color color, bool shared = false)
	{
		SetColor(GData.GetData(go),color,shared);
	}

	/// <summary>
	/// Sets the color using GData
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="color">Color.</param>
	/// <param name="shared">If set to <c>true</c> shared.</param>
	public static void SetColor(GData data, Color color, bool shared = false)
	{
		if(data.aguiPanel)
		{
			data.aguiPanel.alpha = color.a;
			return;
		}
		else if(data.aguiObject && !data.aguiObject.simulateNonAGUI)
		{
			data.aguiObject.color = color;
		}
		else if(data.renderer)
		{
			if(shared)
			{
				data.renderer.sharedMaterial.color = color;
			}
			else if(data.renderer.GetType() == typeof(SpriteRenderer) && data.aguiObject == null)
			{
				(data.renderer as SpriteRenderer).color = color;
			}
			else
			{
				data.renderer.material.color = color;
			}
		}

		if(data.light)
		{
			data.light.color = color;
		}
	}
}

public static class GSerialize
{
	/*
	 * TODO
	 * - Create customize able saving and loading system.
	 */ 


	static GSerialize()
	{
		#if UNITY_IPHONE
		System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif
	}

	public static void Save<T>(string playerPrefID, T data)
	{
		#if USE_FORMATTERS

		BinaryFormatter binary = new BinaryFormatter();
		MemoryStream memory = new MemoryStream();

		binary.Serialize(memory,data);

		PlayerPrefs.SetString(playerPrefID, System.Convert.ToBase64String(memory.GetBuffer()) );

		#else

		SaveXml<T>(playerPrefID, data);

		#endif
	}

	public static void SaveXml<T>(string playerPrefID, T data)
	{
		XmlSerializer xml = new XmlSerializer(typeof(T));
		MemoryStream memory = new MemoryStream();

		xml.Serialize(memory,data);

		PlayerPrefs.SetString(playerPrefID, System.Convert.ToBase64String(memory.ToArray()) );
	}

	public static bool Load<T>(string playerPrefID, ref T objectData)
	{
		if(PlayerPrefs.HasKey(playerPrefID))
		{
			#if USE_FORMATTERS

			byte[] data = System.Convert.FromBase64String(PlayerPrefs.GetString(playerPrefID));
			return Load<T>(data,ref objectData);

			#else

			return LoadXml<T>(playerPrefID, ref objectData);

			#endif
		}

		return false;
	}

	public static bool LoadXml<T>(string playerPrefID, ref T objectData)
	{
		byte[] data = System.Convert.FromBase64String(PlayerPrefs.GetString(playerPrefID));
		MemoryStream memory = new MemoryStream(data);

		XmlSerializer xml = new XmlSerializer(typeof(T));
		StreamReader reader = new StreamReader(memory);
		objectData = (T)xml.Deserialize(reader);
		
		return true;
	}

	public static bool Load<T>(byte[] rawData, ref T objectData)
	{
		BinaryFormatter binary = new BinaryFormatter();
		MemoryStream memory = new MemoryStream(rawData);
		
		objectData = (T)binary.Deserialize(memory);

		return true;
	}
}