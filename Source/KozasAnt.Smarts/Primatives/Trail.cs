#region Copyright, Author Details and Related Context  
//<notice lastUpdateOn="12/20/2015">  
//  <solution>KozasAnt</solution> 
//  <assembly>KozasAnt.Smarts</assembly>  
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
 
using KozasAnt.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KozasAnt.Smarts
{
    public class Trail : AbstractList<Cell>, IComparable<Trail>
    {
        private Trail(string name, List<Cell> cells)
        {
            Name = name;
            Items.AddRange(cells);
        }

        public string Name { get; }

        public int CompareTo(Trail other)
        {
            Contract.Requires(other != null, nameof(other));

            return string.Compare(Name, other.Name, true);
        }

        public override string ToString()
        {
            return Name;
        }

        public static Trail Load(string fileName)
        {
            Contract.Requires(fileName.IsFileName(false), nameof(fileName));

            var doc = XDocument.Load(fileName);

            var root = doc.Element("antTrail");

            var name = root.Attribute("name").Value;

            var cells = new List<Cell>();

            var q = from c in root.Elements("cell")
                    select new
                    {
                        X = (int)c.Attribute("x"),
                        Y = (int)c.Attribute("y"),
                        Kind = c.Attribute("kind").Value.ToEnum<CellKind>()
                    };

            foreach (var cell in q)
                cells.Add(new Cell(cell.X, cell.Y, cell.Kind));

            return new Trail(name, cells);
        }
    }
}
