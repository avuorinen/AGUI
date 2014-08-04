// GAttributes.cs
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

using System.Collections;
using UnityEngine;

/// <summary>
/// Gets Unity's sorting layer as popup list.
/// Only works with string values!!
/// </summary>
public sealed class GSortIDAttribute : PropertyAttribute
{
	// @file GSortIDAttribute.cs
	// @date 9.2.2014
	// @author Atte Vuorinen
}

/// <summary>
/// Set min value to float value!
/// </summary>
public sealed class GMinValueAttribute : PropertyAttribute
{
	// @file GMinValueAttribute.cs
	// @date 9.2.2014
	// @author Atte Vuorinen

	public float Min
	{
		get;
		private set;
	}

	public GMinValueAttribute(float min)
	{
		Min = min;
	}
}

/// <summary>
/// Allow bit flag enums.
/// </summary>
public sealed class GBitFlagEnumAttribute : PropertyAttribute
{
	// @file GBitFlagEnumAttribute.cs
	// @date 30.3.2014
	// @author Atte Vuorinen
}

/// <summary>
/// Make variable in editor to be read only.
/// </summary>
public sealed class GReadOnlyAttribute : PropertyAttribute
{
	// @file GReadOnlyAttribute.cs
	// @date 12.4.2014
	// @author Atte Vuorinen

	public float height;

	public GReadOnlyAttribute(float height = 0)
	{
		this.height = height;
	}
}

/// <summary>
/// GIgnore attribute.
/// Hides / Ignores Method or Field on editor based functionality.
/// </summary>
public sealed class GIgnoreAttribute : System.Attribute
{
	// @file GIgnoreAttribute.cs
	// @date 26.5.2014
	// @author Atte Vuorinen
}