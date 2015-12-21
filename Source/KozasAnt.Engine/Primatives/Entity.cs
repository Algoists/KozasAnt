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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Text;

namespace KozasAnt.Engine
{
    public class Entity<S, G, R> : IComparable<Entity<S, G, R>>
        where S : SettingsBase
        where G : GenomeBase<S, R>
        where R : ReportBase<R>, new()
    {
        public Entity(KnownAs knownAs, CodeTree<S, G, R> codeTree)
        {
            Contract.Requires(knownAs != null, nameof(knownAs));
            Contract.Requires(codeTree != null, nameof(codeTree));

            KnownAs = knownAs;
            CodeTree = codeTree;
        }

        public Entity(KnownAs knownAs,
            KnownAs parent1, KnownAs parent2, CodeTree<S, G, R> codeTree)
            : this(knownAs, codeTree)
        {
            Contract.Requires(parent1 != null, nameof(parent1));
            Contract.Requires(parent2 != null, nameof(parent2));

            Parent1 = parent1;
            Parent2 = parent2;
        }

        public KnownAs KnownAs { get; }
        public KnownAs Parent1 { get; }
        public KnownAs Parent2 { get; }
        public CodeTree<S, G, R> CodeTree { get; }

        public R Report { get; set; }

        public int CompareTo(Entity<S, G, R> other)
        {
            return Report.CompareTo(other.Report);
        }

        public string ToCode()
        {
            var sb = new StringBuilder();

            sb.Append($"using {typeof(S).Namespace};");
            sb.Append($"using {typeof(Cohort).Namespace};");
            sb.Append($"namespace {(Cohort)KnownAs}");
            sb.Append("{");
            sb.Append(BuildClass(this));
            sb.Append("}");

            var tree = CSharpSyntaxTree.ParseText(sb.ToString());

            var root = (CSharpSyntaxNode)tree.GetRoot();

            return root.NormalizeWhitespace().ToFullString();
        }

        public override string ToString()
        {
            return $"{KnownAs} {Report}";
        }

        internal static string BuildClass(Entity<S, G, R> entity)
        {
            var sb = new StringBuilder();

            sb.Append($"public class {entity.KnownAs}: {typeof(G).Name}{{");
            sb.Append($"public {entity.KnownAs}(KnownAs knownAs, Settings settings)");
            sb.Append(": base(knownAs, settings){}");
            sb.Append("public override void DoWork(){");
            sb.Append("while(Status == WorkStatus.GoalsNotMet){");
            AddStatements(sb, entity.CodeTree);
            sb.Append("}}}");

            return sb.ToString();
        }

        private static void AddStatements(StringBuilder sb, CodeTree<S, G, R> codeTree)
        {
            switch (codeTree.Node.Kind)
            {
                case NodeKind.Function:
                    sb.Append($"if({codeTree.Node.Method.Name}()){{");
                    AddStatements(sb, codeTree.Children[0]);
                    sb.Append("}else{");
                    AddStatements(sb, codeTree.Children[1]);
                    sb.Append("}");
                    break;

                case NodeKind.ChildCount:
                    foreach (var child in codeTree.Children)
                        AddStatements(sb, child);
                    break;

                case NodeKind.Terminal:
                    sb.Append("if (Status == WorkStatus.GoalsNotMet){");
                    sb.Append(codeTree.Node.Method.Name);
                    sb.Append("();Report.IncrementStepCount();}");
                    break;
            }
        }
    }
}