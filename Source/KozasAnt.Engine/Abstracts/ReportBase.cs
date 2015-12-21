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
    public abstract class ReportBase<R> : IComparable<R>
        where R : ReportBase<R>, new()
    {
        public long StepCount { get; private set; }
        public WorkStatus Status { get; private set; }

        public KnownAs KnownAs { get; internal set; }
        public TimeSpan Elapsed { get; internal set; }
        public double Fitness { get; internal set; }

        public void IncrementStepCount()
        {
            StepCount++;
        }

        public void SetFinalStatus(WorkStatus status)
        {
            switch (status)
            {
                case WorkStatus.MetGoals:
                case WorkStatus.ReachedMaxSteps:
                    Status = status;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status));
            }
        }

        public int CompareTo(R other)
        {
            Contract.Requires(other != null, nameof(other));

            return GetCompareTo(other);
        }

        protected abstract int GetCompareTo(R Other);
    }
}
