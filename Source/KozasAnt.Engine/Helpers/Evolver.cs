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
using System;
using System.Threading;

namespace KozasAnt.Engine
{
    public class Evolver<S, G, R>
        where S : SettingsBase
        where G : GenomeBase<S, R>
        where R : ReportBase<R>, new()
    {
        public static void Evolve(S settings, CancellationTokenSource cts,
            Action<EntitySet<S, G, R>> onResultSet,
            Action<AggregateException> onErrors, Action onSuccess)
        {
            try
            {
                Contract.Requires(settings != null, nameof(settings));

                settings.Validate();

                var genePool = new GenePool<S, G, R>(settings);

                EntitySet<S, G, R> entitySet = null;

                for (var cohortId = 0; cohortId < settings.MaxCohorts; cohortId++)
                {
                    entitySet = new EntitySet<S, G, R>(
                        settings, genePool, cohortId, entitySet);

                    var metGoals = entitySet.CompileAndRun(settings, cts);

                    onResultSet(entitySet);

                    if (metGoals)
                        break;
                }

                onSuccess();
            }
            catch (AggregateException errors)
            {
                onErrors(errors);
            }
            catch (Exception error)
            {
                onErrors(new AggregateException(error));
            }
        }
    }
}