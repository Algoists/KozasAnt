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
using KozasAnt.Smarts;
using System;
using System.Threading;
using System.Windows.Forms;

namespace KozasAnt.Client
{
    public partial class MainForm : Form
    {
        private Mode mode = Mode.Evolve;

        private Settings settings;

        public MainForm()
        {
            InitializeComponent();

            var trail = Trail.Load(@"AntTrails\Default.xml");

            settings = new Settings(trail)
            {
                MinCohorts = Properties.Settings.Default.MinCohorts,
                MaxCohorts = Properties.Settings.Default.MaxCohorts,
                MaxSteps = Properties.Settings.Default.MaxSteps,
                Population = Properties.Settings.Default.Population,
                MaxTreeDepth = Properties.Settings.Default.MaxTreeDepth,
                MutationRate = Properties.Settings.Default.MutationRate,
                PercentToKeep = Properties.Settings.Default.PercentToKeep,
                MetGoalsTarget = Properties.Settings.Default.MetGoalsTarget
            };

            antGrid.Load(trail);
        }

        private Entity<Settings, Genome, Report> SelectedEntity
        {
            get
            {
                return (Entity<Settings, Genome, Report>)entitiesBox.SelectedItem;
            }
        }

        private void cohortsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            entitiesBox.Items.Clear();
            richTextBox.Clear();
            entitiesBox.Refresh();

            var entitySet = (EntitySet<Settings, Genome, Report>)cohortsBox.SelectedItem;

            foreach (var entity in entitySet)
                entitiesBox.Items.Add(entity);

            entitiesBox.Refresh();

            entitiesBox.SelectedIndex = 0;
        }

        private void entitiesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entitiesBox.SelectedItem is Entity<Settings, Genome, Report>)
                richTextBox.Text = SelectedEntity.ToCode();
            else
                richTextBox.Text = "";
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            if (mode == Mode.Evolve)
            {
                SetGUI(false);

                var cts = new CancellationTokenSource();

                // TODO: Handle Cancellation
                Evolver<Settings, Genome, Report>.Evolve(
                    settings, cts,
                    resultSet =>
                    {
                        cohortsBox.Items.Insert(0, resultSet);

                        cohortsBox.Refresh();
                    },
                    errors =>
                    {
                        // TODO: Handle Errors Gracefully
                        throw errors;
                    },
                    () =>
                    {
                        SetGUI(true);

                        cohortsBox.SelectedIndex = 0;

                        mode = Mode.WalkTrail;

                        actionButton.Text = "Walk Route";
                    });
            }
            else
            {
                antGrid.WalkTrail(SelectedEntity,
                    Properties.Settings.Default.TrailDelay);
            }
        }

        private void SetGUI(bool state)
        {
            if (!state)
                richTextBox.Text = "";

            actionButton.Enabled = state;
            cohortsBox.Enabled = state;
            entitiesBox.Enabled = state;
            richTextBox.Enabled = state;
        }
    }
}