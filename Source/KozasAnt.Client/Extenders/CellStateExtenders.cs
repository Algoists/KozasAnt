#region Copyright, Author Details and Related Context  
//<notice lastUpdateOn="12/20/2015">  
//  <solution>KozasAnt</solution> 
//  <assembly>KozasAnt.Client</assembly>  
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
 
using KozasAnt.Smarts;
using System;
using System.Drawing;

namespace KozasAnt.Client
{
    public static class CellStateExtenders
    {
        public static CellState ToCellState(this CellKind kind)
        {
            switch (kind)
            {
                case CellKind.Food:
                    return CellState.Food;
                case CellKind.Gap:
                    return CellState.Gap;
                case CellKind.Empty:
                    return CellState.Empty;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Color ToColor(this CellState state)
        {
            switch (state)
            {
                case CellState.Food:
                    return WellKnown.Colors.Food;
                case CellState.Gap:
                    return WellKnown.Colors.Gap;
                case CellState.Empty:
                    return WellKnown.Colors.Empty;
                case CellState.AntIs:
                    return WellKnown.Colors.AntIs;
                case CellState.AntWas:
                    return WellKnown.Colors.AntWas;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
        }
    }
}
