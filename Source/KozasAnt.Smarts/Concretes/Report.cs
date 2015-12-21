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
 
using KozasAnt.Engine;
using System.Collections.Generic;

namespace KozasAnt.Smarts
{
    public class Report : ReportBase<Report>
    {
        public Report()
        {
            Route = new List<Point>();
        }

        public List<Point> Route { get; }

        public long FoodCount { get; internal set; }

        protected override int GetCompareTo(Report other)
        {
            if (other.FoodCount.CompareTo(FoodCount) == 0)
            {
                if (StepCount == other.StepCount)
                    return KnownAs.CompareTo(other.KnownAs);
                else
                    return StepCount.CompareTo(other.StepCount);
            }
            else
            {
                return other.FoodCount.CompareTo(FoodCount);
            }
        }

        public override string ToString()
        {
            return $"Food: {FoodCount:N0}, Steps: {StepCount:N0}, Fitness: {Fitness:N0}";
        }
    }
}