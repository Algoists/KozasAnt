#region Copyright, Author Details and Related Context  
//<notice lastUpdateOn="12/20/2015">  
//  <solution>KozasAnt</solution> 
//  <assembly>KozasAnt.Generic</assembly>  
//  <description>A modern take on Koza's connonical "Ant Foraging for Food on a Grid" problem that leverages Roslyn</description>  
//  <copyright>  
//    Copyright (C) 2015 Louis S. Berman   

//    This program is free software: you can redistribute it and/or modify  
//    it under the terms of the GNU General Public License as published by  
//    the Free Software Foundation, either version 3 of the License, or  
//    (at your option) any later version.  
  
//    This program is distributed in the hope that it will be useful,  
//    but WITHOUT ANY WARRANTY; without even the implied warranty of  
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the  
//    GNU General Public License for more details.   

//    You should have received a copy of the GNU General Public License  
//    along with this program.  If not, see http://www.gnu.org/licenses/.  
//  </copyright>  
//  <author>  
//    <fullName>Louis S. Berman</fullName>  
//    <email>louis@squideyes.com</email>  
//    <website>http://squideyes.com</website>  
//  </author>  
//</notice>  
#endregion 
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace KozasAnt.Generic
{
    public static class StringExtenders
    {
        private static Dictionary<int, Regex> regexes =
            new Dictionary<int, Regex>();

        [DebuggerHidden]
        public static bool IsTrimmed(this string value)
        {
            return value.IsTrimmed(false);
        }

        [DebuggerHidden]
        public static bool IsTrimmed(this string value, bool emptyOk)
        {
            if (value == (string)null)
                return false;

            if (value == string.Empty)
                return emptyOk;

            return (!char.IsWhiteSpace(value[0]) &&
                (!char.IsWhiteSpace(value[value.Length - 1])));
        }

        [DebuggerHidden]
        public static bool IsFileName(this string value, bool mustBeRooted = true)
        {
            Contract.Requires(value.IsTrimmed(), nameof(value));

            try
            {
                new FileInfo(value);

                if (!mustBeRooted)
                    return true;
                else
                    return Path.IsPathRooted(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (PathTooLongException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }
    }
}