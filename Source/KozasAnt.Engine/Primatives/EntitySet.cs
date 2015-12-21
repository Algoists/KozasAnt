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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KozasAnt.Engine
{
    public class EntitySet<S, G, R> : AbstractList<Entity<S, G, R>>
        where S : SettingsBase
        where G : GenomeBase<S, R>
        where R : ReportBase<R>, new()
    {
        private static CSharpCompilationOptions compilationOptions =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
        optimizationLevel: OptimizationLevel.Release);

        private static MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(Node).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(R).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        internal EntitySet(S settings, GenePool<S, G, R> genePool, int cohortId,
            IEnumerable<Entity<S, G, R>> entities)
        {
            Cohort = new Cohort(cohortId);

            if (entities == null)
            {
                for (int entityId = 0; entityId <= settings.Population - 1; entityId++)
                {
                    var codeTree = new CodeTree<S, G, R>(
                        genePool, genePool.GetFunction(), settings.MaxTreeDepth);

                    var knownAs = new KnownAs(Cohort, entityId);

                    Items.Add(new Entity<S, G, R>(knownAs, codeTree));
                }
            }
            else
            {
                Items.AddRange(entities.Take(settings.NumToKeep));

                for (int entityId = Count; entityId <= settings.Population - 2; entityId += 2)
                {
                    CodeTree<S, G, R> child1, child2, parent1, parent2;
                    int index1, index2;

                    SelectAndCloneParents(genePool, this, settings.NumToKeep,
                        out index1, out parent1, out index2, out parent2);

                    Crossover(genePool, parent1, parent2, out child1, out child2);

                    Items.Add(new Entity<S, G, R>(new KnownAs(Cohort, entityId),
                        this[index1].KnownAs, this[index2].KnownAs, child1));

                    Items.Add(new Entity<S, G, R>(new KnownAs(Cohort, entityId + 1),
                        this[index1].KnownAs, this[index2].KnownAs, child2));
                }
            }
        }

        public Cohort Cohort { get; }

        public TimeSpan Elapsed { get; internal set; }

        public string ToSource()
        {
            var sb = new StringBuilder();

            sb.Append($"using {typeof(S).Namespace};");
            sb.Append($"using {typeof(Cohort).Namespace};");
            sb.Append($"namespace {Cohort}");
            sb.Append("{");
            foreach (var entity in this)
                sb.Append(Entity<S, G, R>.BuildClass(entity));
            sb.Append("}");

            return sb.ToString();
        }

        private CodeTree<S, G, R> Combine(GenePool<S, G, R> genePool, CodeTree<S, G, R> source,
            CodeTree<S, G, R> remove, CodeTree<S, G, R> replace)
        {
            var copy = source;

            if (source == remove)
                return replace.Clone();

            if (copy.Node.Kind != NodeKind.Terminal)
            {
                for (int i = 0; i <= copy.Children.GetUpperBound(0); i++)
                {
                    var child = Combine(genePool, copy.Children[i], remove, replace);

                    copy.Children[i] = child;

                    if (genePool.GetCanMutate())
                        copy.Children[i].Node = genePool.GetTerminal();
                }
            }

            return copy;
        }

        private void SelectAndCloneParents(GenePool<S, G, R> genePool,
            EntitySet<S, G, R> entitySet, int limit,
            out int index1, out CodeTree<S, G, R> parent1,
            out int index2, out CodeTree<S, G, R> parent2)
        {
            var totalFitness = 0.0;

            for (int i = 0; i < limit; i++)
                totalFitness += entitySet[i].Report.Fitness;

            index1 = -1;
            index2 = -1;

            while ((index1 == -1) | (index2 == -1))
            {
                for (int i = 0; i < limit; i++)
                {
                    var range = entitySet[i].Report.Fitness / totalFitness;

                    if (genePool.InRange(range))
                    {
                        if (index1 == -1)
                            index1 = i;
                        else
                            index2 = i;
                    }
                }
            }

            parent1 = entitySet[index1].CodeTree.DeepClone();
            parent2 = entitySet[index2].CodeTree.DeepClone();
        }

        private void Crossover(GenePool<S, G, R> genePool,
            CodeTree<S, G, R> parent1, CodeTree<S, G, R> parent2,
            out CodeTree<S, G, R> child1, out CodeTree<S, G, R> child2)
        {
            var nodes1 = parent1.ToList();
            var nodes2 = parent2.ToList();

            int crossover1 = genePool.Next(0, nodes1.Count - 1);
            int crossover2 = genePool.Next(0, nodes2.Count - 1);

            child1 = Combine(genePool, parent1, nodes1[crossover1], nodes2[crossover2]);
            child2 = Combine(genePool, parent2, nodes2[crossover2], nodes1[crossover1]);
        }

        public bool CompileAndRun(S settings, CancellationTokenSource cts)
        {
            var cohortStartedOn = DateTime.UtcNow;

            var source = ToSource();

            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var compilation = CSharpCompilation.Create(
                assemblyName: Guid.NewGuid().ToString("N"),
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: compilationOptions);

            using (var ms = new MemoryStream())
            {
                var entity = compilation.Emit(ms);

                if (!entity.Success)
                {
                    var failures = (entity.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error)).ToList();

                    throw new EmitException(Cohort, failures);
                }

                ms.Seek(0, SeekOrigin.Begin);

                var assembly = Assembly.Load(ms.ToArray());

                var generatedTypes = assembly.GetTypes();

                var tuples = new List<Tuple<Entity<S, G, R>, Type>>();

                for (int i = 0; i < Count; i++)
                {
                    if (this[i].Report == null)
                        tuples.Add(new Tuple<Entity<S, G, R>, Type>(this[i], generatedTypes[i]));
                }

                var errors = new ConcurrentQueue<Exception>();

                Parallel.ForEach(
                    tuples,
                    new ParallelOptions()
                    {
                        CancellationToken = cts.Token,
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    },
                    item =>
                    {
                        try
                        {
                            var entityStartedOn = DateTime.UtcNow;

                            var instance = (G)Activator.CreateInstance(
                                item.Item2, item.Item1.KnownAs, settings);

                            instance.DoWork();

                            instance.Report.Elapsed = DateTime.UtcNow - entityStartedOn;

                            item.Item1.Report = instance.Report;
                        }
                        catch (Exception error)
                        {
                            errors.Enqueue(error);
                        }
                    });

                if (errors.Count > 0)
                    throw new AggregateException(errors);
            }

            Sort();

            var fitness = settings.Population;

            ForEach(entity => entity.Report.Fitness = --fitness);

            Elapsed = DateTime.UtcNow - cohortStartedOn;

            if (Cohort.CohortId < settings.MinCohorts)
                return false;

            int metGoals = 0;

            var target = Math.Ceiling(Count / settings.MetGoalsTarget);

            foreach (var entity in this)
            {
                if (entity.Report.Status == WorkStatus.MetGoals)
                {
                    metGoals++;

                    if (metGoals >= target)
                        return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            var metGoals = this.Count(e => e.Report.Status == WorkStatus.MetGoals);

            return $"{Cohort}: MetGoals: {metGoals}";
        }
    }
}