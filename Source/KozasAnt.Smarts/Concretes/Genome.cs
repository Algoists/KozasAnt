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
    public abstract class Genome : GenomeBase<Settings, Report>
    {
        private Point current = new Point(0, 0);
        private Facing facing = Facing.Right;
        private HashSet<Point> gathered = new HashSet<Point>();

        public Genome(KnownAs knownAs, Settings settings)
            : base(knownAs, settings)
        {
            if (Settings.Grid[current] == CellKind.Food)
            {
                Report.FoodCount++;

                gathered.Add(current);
            }

            Report.Route.Add(current);
        }

        public override WorkStatus Status
        {
            get
            {
                if ((Report.StepCount <= Settings.MaxSteps) &&
                    (Report.FoodCount == Settings.FoodGoal))
                {
                    Report.SetFinalStatus(WorkStatus.MetGoals);

                    return WorkStatus.MetGoals;
                }
                else if (Report.StepCount == Settings.MaxSteps)
                {
                    Report.SetFinalStatus(WorkStatus.ReachedMaxSteps);

                    return WorkStatus.ReachedMaxSteps;
                }
                else
                {
                    return WorkStatus.GoalsNotMet;
                }
            }
        }

        private Point GetFacingPoint()
        {
            var x = current.X;
            var y = current.Y;

            switch (facing)
            {
                case Facing.Left:
                    x = current.X.Retreat(WellKnown.GridWidth - 1);
                    break;
                case Facing.Right:
                    x = current.X.Advance(WellKnown.GridWidth - 1);
                    break;
                case Facing.Up:
                    y = current.Y.Retreat(WellKnown.GridHeight - 1);
                    break;
                case Facing.Down:
                    y = current.Y.Advance(WellKnown.GridHeight - 1);
                    break;
            }

            return new Point(x, y);
        }

        private bool IsFacing(CellKind cellKind)
        {
            var point = GetFacingPoint();

            if (gathered.Contains(point))
                return false;

            return Settings.Grid[GetFacingPoint()] == cellKind;
        }

        [Gene]
        public bool IsFacingFood()
        {
            return IsFacing(CellKind.Food);
        }

        [Gene]
        public bool IsFacingGap()
        {
            return IsFacing(CellKind.Gap);
        }

        [Gene]
        public void Move()
        {
            current = GetFacingPoint();

            Report.Route.Add(current);

            if ((Settings.Grid[current] == CellKind.Food) &&
                (!gathered.Contains(current)))
            {
                Report.FoodCount++;

                gathered.Add(current);
            }
        }

        [Gene]
        public void TurnLeft()
        {
            switch (facing)
            {
                case Facing.Left:
                    facing = Facing.Down;
                    break;
                case Facing.Right:
                    facing = Facing.Up;
                    break;
                case Facing.Up:
                    facing = Facing.Left;
                    break;
                case Facing.Down:
                    facing = Facing.Right;
                    break;
            }
        }

        [Gene]
        public void TurnRight()
        {
            switch (facing)
            {
                case Facing.Left:
                    facing = Facing.Up;
                    break;
                case Facing.Right:
                    facing = Facing.Down;
                    break;
                case Facing.Up:
                    facing = Facing.Right;
                    break;
                case Facing.Down:
                    facing = Facing.Left;
                    break;
            }
        }
    }
}
