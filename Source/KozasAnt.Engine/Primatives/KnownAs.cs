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

namespace KozasAnt.Engine
{
    public class KnownAs : Cohort, IComparable<KnownAs>, IEquatable<KnownAs>
    {
        public new static class Limits
        {
            public static class EntityId
            {
                public const int MinValue = 0;
                public const int MaxValue = 9999;
            }
        }

        public KnownAs(Cohort cohort, int entityId)
            : base(cohort.CohortId)
        {
            EntityId = entityId;
        }

        public int EntityId { get; }

        public int CompareTo(KnownAs other)
        {
            if (CohortId == other.CohortId)
                return EntityId.CompareTo(other.EntityId);
            else
                return CohortId.CompareTo(other.CohortId);
        }

        public bool Equals(KnownAs other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object @object)
        {
            if (!(@object is KnownAs))
                return false;

            return GetHashCode().Equals(((KnownAs)@object).GetHashCode());
        }

        public override int GetHashCode()
        {
            return (CohortId * 10000) + EntityId;
        }

        public override string ToString()
        {
            return $"Cohort{CohortId:0000}_Entity{EntityId:0000}";
        }

    }
}
