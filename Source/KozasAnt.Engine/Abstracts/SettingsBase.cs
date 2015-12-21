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

namespace KozasAnt.Engine
{
    public abstract class SettingsBase
    {
        public static class Limits
        {
            public static class MinMaxCohorts
            {
                public const int MinValue = 5;
                public const int Default = 20;
                public const int MaxValue = int.MaxValue;
            }

            public static class Population
            {
                public const int MinValue = 10;
                public const int Default = 100;
                public const int MaxValue = 200;
            }

            public static class MaxTreeDepth
            {
                public const int MinValue = 5;
                public const int Default = 5;
                public const int MaxValue = 50;
            }

            public static class MaxSteps
            {
                public const int MinValue = 1;
                public const int Default = int.MaxValue;
                public const int MaxValue = int.MaxValue;
            }

            public static class MutationRate
            {
                public const int MinValue = 1;
                public const int Default = 2;
                public const int MaxValue = 10;
            }

            public static class PercentToKeep
            {
                public const double MinValue = 0.0;
                public const double Default = 10.0;
                public const double MaxValue = 100.0;
            }

            public static class MetGoalsTarget
            {
                public const double MinValue = 1.0;
                public const double Default = 10.0;
                public const double MaxValue = 100.0;
            }
        }

        public SettingsBase()
        {
            Population = Limits.Population.Default;
            MaxTreeDepth = Limits.Population.Default;
            MinCohorts = Limits.MinMaxCohorts.Default;
            MaxCohorts = Limits.MinMaxCohorts.Default;
            MaxSteps = Limits.MaxSteps.Default;
            MutationRate = Limits.MutationRate.Default;
            PercentToKeep = Limits.PercentToKeep.Default;
        }

        public int Population { get; set; }
        public int MaxTreeDepth { get; set; }
        public int MinCohorts { get; set; }
        public int MaxCohorts { get; set; }
        public int MaxSteps { get; set; }
        public int MutationRate { get; set; }
        public double PercentToKeep { get; set; }
        public double MetGoalsTarget { get; set; }

        public int NumToKeep
        {
            get
            {
                return (int)Math.Ceiling(Population / PercentToKeep);
            }
        }

        protected abstract void DoValidate();

        public void Validate()
        {
            Contract.Requires(MinCohorts >= Limits.MinMaxCohorts.MinValue &&
                MinCohorts <= Limits.MinMaxCohorts.MaxValue, nameof(MinCohorts));

            Contract.Requires(MaxCohorts >= MinCohorts &&
                MaxCohorts <= Limits.MinMaxCohorts.MaxValue, nameof(MaxCohorts));

            Contract.Requires(Population >= Limits.Population.MinValue &&
                Population <= Limits.Population.MaxValue, nameof(Population));

            Contract.Requires(MaxTreeDepth >= Limits.MaxTreeDepth.MinValue &&
                MaxTreeDepth <= Limits.MaxTreeDepth.MaxValue, nameof(MaxTreeDepth));

            Contract.Requires(MaxSteps >= Limits.MaxSteps.MinValue &&
                MaxSteps <= Limits.MaxSteps.MaxValue, nameof(MaxSteps));

            Contract.Requires(MutationRate >= Limits.MutationRate.MinValue &&
                MutationRate <= Limits.MutationRate.MaxValue, nameof(MutationRate));

            Contract.Requires(PercentToKeep >= Limits.PercentToKeep.MinValue &&
                PercentToKeep <= Limits.PercentToKeep.MaxValue, nameof(PercentToKeep));

            Contract.Requires(MetGoalsTarget >= Limits.MetGoalsTarget.MinValue &&
                MetGoalsTarget <= Limits.MetGoalsTarget.MaxValue, nameof(MetGoalsTarget));
        }
    }
}
