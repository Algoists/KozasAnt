#region Copyright, Author Details and Related Context  
//<notice lastUpdateOn="12/20/2015">  
//  <solution>KozasAnt</solution> 
//  <assembly>KozasAnt.Client</assembly>  
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
using KozasAnt.Generic;
using KozasAnt.Smarts;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace KozasAnt.Client
{
    public partial class AntGrid : Control
    {
        private const int GRIDSIZE = ((WellKnown.CellSize + 1) * Smarts.WellKnown.GridWidth) + 1;

        private const int PENWIDTH = 1;

        private const int MARGIN = 4;

        private readonly int heightWidth = GRIDSIZE + (MARGIN * 2) + (SystemInformation.Border3DSize.Width * 2);

        private Dictionary<System.Drawing.Point, CellState> cells = new Dictionary<System.Drawing.Point, CellState>();
        private Dictionary<System.Drawing.Point, CellState> snapshot = null;

        public AntGrid()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void SetState(
            Smarts.Point gridXY, CellState state, bool refresh = true)
        {
            SetState(gridXY.X, gridXY.Y, state, refresh);
        }

        public void SetState(
            int x, int y, CellState state, bool refresh = true)
        {
            Contract.Requires((x >= 0) && (x < Smarts.WellKnown.GridWidth), nameof(x));
            Contract.Requires((y >= 0) && (y < Smarts.WellKnown.GridHeight), nameof(y));
            Contract.Requires(!state.IsDefault(), nameof(state));

            var point = new System.Drawing.Point(x, y);

            if (state == CellState.Empty)
                cells.Remove(point);
            else if (cells.ContainsKey(point))
                cells[point] = state;
            else
                cells.Add(point, state);

            if (refresh)
                Refresh();
        }

        public void Reset()
        {
            cells.Clear();

            foreach (var point in snapshot.Keys)
                cells.Add(point, snapshot[point]);

            Refresh();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.FillRectangle(
                new SolidBrush(WellKnown.Colors.Empty), pe.ClipRectangle);

            DrawGrid(pe.Graphics);

            for (int x = 0; x < Smarts.WellKnown.GridWidth; x++)
            {
                for (int y = 0; y < Smarts.WellKnown.GridHeight; y++)
                {
                    CellState state;

                    if (cells.TryGetValue(new System.Drawing.Point(x, y), out state))
                    {
                        var left = MARGIN + ((WellKnown.CellSize + 1) * x) + 1;
                        var top = MARGIN + ((WellKnown.CellSize + 1) * y) + 1;

                        var brush = new SolidBrush(state.ToColor());

                        pe.Graphics.FillRectangle(brush,
                            left, top, WellKnown.CellSize, WellKnown.CellSize);
                    }
                }
            }
        }

        private void DrawGrid(Graphics graphics)
        {
            var pen = new Pen(WellKnown.Colors.Border, PENWIDTH);

            var x = 4;
            var y = 4;

            for (int i = 0; i <= Smarts.WellKnown.GridWidth; i++)
            {
                graphics.DrawLine(pen, 4, y, GRIDSIZE + 3, y);
                graphics.DrawLine(pen, x, 4, y, GRIDSIZE + 3);

                x += WellKnown.CellSize + 1;
                y += WellKnown.CellSize + 1;
            }
        }

        protected override void SetBoundsCore(
            int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(
                x, y, heightWidth, heightWidth, BoundsSpecified.All);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_CLIENTEDGE = unchecked((int)0x00000200);
                const int WS_BORDER = unchecked((int)0x00800000);

                var cp = base.CreateParams;

                cp.Style &= (~WS_BORDER);
                cp.ExStyle |= (WS_EX_CLIENTEDGE);

                return cp;
            }
        }

        public void WalkTrail(Entity<Settings, Genome, Report> entity, int delay)
        {
            Contract.Requires(entity != null, nameof(entity));
            Contract.Requires(delay >= 0, nameof(delay));

            Reset();

            Smarts.Point? selected = null;

            foreach (var current in entity.Report.Route)
            {
                if (selected.HasValue)
                    SetState(selected.Value, CellState.AntWas);

                selected = current;

                SetState(selected.Value, CellState.AntIs);

                Refresh();

                Thread.Sleep(delay);
            }
        }

        public void Load(Trail trail)
        {
            Contract.Requires(trail != null, nameof(trail));

            cells.Clear();

            trail.ForEach(cell => SetState(cell.X, cell.Y, cell.Kind.ToCellState()));

            snapshot = new Dictionary<System.Drawing.Point, CellState>(cells);
        }
    }
}
