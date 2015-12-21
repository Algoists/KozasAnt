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
 
using System.Collections.Generic;
using System.Linq;

namespace KozasAnt.Engine
{
    public class CodeTree<S, G, R>
        where S : SettingsBase
        where G : GenomeBase<S, R>
        where R : ReportBase<R>, new()
    {
        private CodeTree()
        {
        }

        public CodeTree(GenePool<S, G, R> genePool, Node node, int maxDepth)
        {
            Node = node;

            if (Node.Kind != NodeKind.Terminal)
            {
                Children = Enumerable.Range(1, Node.ChildCount).Select(
                    x => new CodeTree<S, G, R>(genePool, GetNode(genePool, maxDepth), maxDepth - 1)).ToArray();
            }
        }

        public CodeTree<S, G, R>[] Children { get; private set; }

        public Node Node { get; set; }

        private Node GetNode(GenePool<S, G, R> genePool, int maxDepth)
        {
            if (maxDepth > 0)
                return genePool.GetFunctionOrTerminal();
            else
                return genePool.GetTerminal();
        }

        public CodeTree<S, G, R> Clone()
        {
            return new CodeTree<S, G, R>()
            {
                Node = this.Node,
                Children = this.Children
            };
        }

        public CodeTree<S, G, R> DeepClone()
        {
            var codeTree = new CodeTree<S, G, R>();

            codeTree.Node = Node;

            if (Children != null)
            {
                codeTree.Children = Children.Select(
                    c => c.DeepClone()).ToArray();
            }

            return codeTree;
        }

        public List<CodeTree<S, G, R>> ToList()
        {
            var codeTrees = new List<CodeTree<S, G, R>>();

            AddNodes(codeTrees, this);

            return codeTrees;
        }

        private void AddNodes(List<CodeTree<S, G, R>> codeTrees,
            CodeTree<S, G, R> codeTree)
        {
            codeTrees.Add(codeTree);

            if (codeTree.Node.Kind != NodeKind.Terminal)
            {
                foreach (CodeTree<S, G, R> child in codeTree.Children)
                    AddNodes(codeTrees, child);
            }
        }
    }
}