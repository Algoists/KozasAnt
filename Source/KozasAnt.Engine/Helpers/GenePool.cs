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
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KozasAnt.Engine
{
    public class GenePool<S, G, R>
        where S : SettingsBase
        where G : GenomeBase<S, R>
        where R : ReportBase<R>, new()
    {
        private Random random = new Random(5000);
        private List<Node> terminals = new List<Node>();
        private List<Node> functions = new List<Node>();

        private S settings;

        public GenePool(S settings)
        {
            this.settings = settings;

            var flags = BindingFlags.Public | BindingFlags.Instance;

            foreach (var method in typeof(G).GetMethods(flags))
            {
                if (method.GetParameters().Length != 0)
                    continue;

                if (!method.GetCustomAttributes<GeneAttribute>().Any())
                    continue;

                if (method.ReturnType == typeof(void))
                    terminals.Add(new Node(NodeKind.Terminal, method));
                else if (method.ReturnType == typeof(bool))
                    functions.Add(new Node(NodeKind.Function, method));
            }

            for (int i = 2; i <= 3; i++)
                functions.Add(new Node(i));
        }

        public Node GetTerminal()
        {
            return terminals[random.Next(0, terminals.Count - 1)];
        }

        public Node GetFunctionOrTerminal()
        {
            int index = random.Next(0, functions.Count + terminals.Count);

            if (index >= functions.Count)
                return terminals[index - functions.Count];
            else
                return functions[index];
        }

        public Node GetFunction()
        {
            return functions[random.Next(0, functions.Count - 1)];
        }

        public bool GetCanMutate()
        {
            return random.Next(0, 100) < settings.MutationRate;
        }

        public bool InRange(double range)
        {
            var number = random.NextDouble();

            return (number < range);
        }

        public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}
