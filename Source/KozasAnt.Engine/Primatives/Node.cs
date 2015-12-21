#region Copyright, Author Details and Related Context  
//<notice lastUpdateOn="12/20/2015">  
//  <solution>KozasAnt</solution> 
//  <assembly>KozasAnt.Engine</assembly>  
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
using System.Reflection;

namespace KozasAnt.Engine
{
    public class Node
    {
        public Node(NodeKind kind, MethodInfo method)
        {
            Contract.Requires((kind != NodeKind.ChildCount), nameof(kind));
            Contract.Requires(method != null, nameof(method));

            if (kind == NodeKind.Function) 
            {
                Kind = kind;
                ChildCount = 2;
            }
            else
            {
                Kind = NodeKind.Terminal;
            }

            Method = method;
        }

        public Node(int childCount)
        {
            ChildCount = childCount;
            Kind = NodeKind.ChildCount;
        }

        public NodeKind Kind { get; }
        public int ChildCount { get;}
        public MethodInfo Method { get; }
    }
}
