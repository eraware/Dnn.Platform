﻿#region Copyright

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

#region usings
using System;
using System.Text;
using System.Text.RegularExpressions;

#endregion



namespace DotNetNuke.Entities.Portals
{
    /// <summary>
    /// Represents a CSS color and it's components
    /// </summary>
    public class StyleColorBase
    {
        private enum Component
        {
            red,
            green,
            blue
        }

        private string _hex;

        /// <summary>
        /// Instantiates a new StyleColor with the default white color
        /// </summary>
        public StyleColorBase()
        {
            _hex = "FFFFFF";
        }

        /// <summary>
        /// Instantiates a new StyleColor with the provided color but falls back to white if not provided or invalid
        /// </summary>
        /// <param name="hexValue"></param>
        public StyleColorBase(string hexValue)
        {
            if (IsValidCssColor(hexValue))
            {
                this.HexValue = ExpandColor(hexValue);
            }
            else
            {
                this.HexValue = ExpandColor("FFFFFF");
            }
        }

        /// <summary>
        /// Gets or sets the red component as a byte between 0 and 255
        /// </summary>
        public byte Red
        {
            get { return GetComponent(Component.red); }
            set { SetComponent(Component.red, value); }
        }

        /// <summary>
        /// Gets or sets the green component as a byte between 0 and 255
        /// </summary>
        public byte Green
        {
            get { return GetComponent(Component.green); }
            set { SetComponent(Component.green, value); }
        }

        /// <summary>
        /// Gets or sets the blue component as a byte between 0 and 255
        /// </summary>
        public byte Blue
        {
            get { return GetComponent(Component.blue); }
            set { SetComponent(Component.blue, value); }
        }

        /// <summary>
        /// Gets or sets the color using a 3 or 6 characters hex string such as 0088FF
        /// </summary>
        public string HexValue
        {
            get { return _hex; }
            set
            {
                if (IsValidCssColor(value))
                {
                    _hex = ExpandColor(value);
                }
            }
        }

        /// <summary>
        /// If possible, returns a shorter sting for the hex color value
        /// ex: AABBCC would return ABC
        /// </summary>
        public string MinifiedHex
        {
            get
            {
                if (
                    _hex[0] == _hex[1] && 
                    _hex[2] == _hex[3] &&
                    _hex[4] == _hex[5]
                    )
                {
                    return _hex[0].ToString() + _hex[2].ToString() + _hex[4].ToString();
                }
                return _hex;
            }
        }

        private bool IsValidCssColor(string hexValue)
        {
            if (string.IsNullOrWhiteSpace(hexValue))
            {
                throw new ArgumentNullException("You need to provide a CSS color value in the constructor");
            }
            var regex = new Regex(@"([\da-f]{3}){1,2}", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(hexValue))
            {
                throw new ArgumentOutOfRangeException($"The value {hexValue} that was provided is not valid, it needs to be 3 or 6 character long hexadecimal string without the # sing");
            }
            return true;
        }

        private static string ExpandColor(string hexValue)
        {
            if (hexValue.Length == 6)
            {
                return hexValue.ToUpperInvariant();
            }
            string value;
            var r = hexValue[0];
            var g = hexValue[1];
            var b = hexValue[2];
            value = string.Concat(r, r, g, g, b, b);
            return value.ToUpperInvariant();
        }

        private byte GetComponent(Component comp)
        {
            switch (comp)
            {
                case Component.red:
                    return byte.Parse(_hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                case Component.green:
                    return byte.Parse(_hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                case Component.blue:
                    return byte.Parse(_hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                default:
                    return 0;
            }
        }

        private void SetComponent(Component comp, byte value)
        {         
            switch (comp)
            {
                case Component.red:
                    _hex = _hex.Remove(0, 2).Insert(0, $"{value:x2}");
                    break;
                case Component.green:
                    _hex = _hex.Remove(2, 2).Insert(2, $"{value:x2}");
                    break;
                case Component.blue:
                    _hex = _hex.Remove(4, 2).Insert(4, $"{value:x2}");
                    break;
                default:
                    break;
            }
        }
    }
}
