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
 
namespace KozasAnt.Client
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cohortsBox = new System.Windows.Forms.ListBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.actionButton = new System.Windows.Forms.Button();
            this.entitiesBox = new System.Windows.Forms.ListBox();
            this.antGrid = new KozasAnt.Client.AntGrid();
            this.SuspendLayout();
            // 
            // cohortsBox
            // 
            this.cohortsBox.Location = new System.Drawing.Point(7, 12);
            this.cohortsBox.Name = "cohortsBox";
            this.cohortsBox.ScrollAlwaysVisible = true;
            this.cohortsBox.Size = new System.Drawing.Size(416, 56);
            this.cohortsBox.TabIndex = 31;
            this.cohortsBox.SelectedIndexChanged += new System.EventHandler(this.cohortsBox_SelectedIndexChanged);
            // 
            // richTextBox
            // 
            this.richTextBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.richTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox.Location = new System.Drawing.Point(7, 136);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(416, 279);
            this.richTextBox.TabIndex = 29;
            this.richTextBox.Text = "";
            this.richTextBox.WordWrap = false;
            // 
            // actionButton
            // 
            this.actionButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionButton.Location = new System.Drawing.Point(428, 12);
            this.actionButton.Name = "actionButton";
            this.actionButton.Size = new System.Drawing.Size(365, 32);
            this.actionButton.TabIndex = 27;
            this.actionButton.Text = "Generate";
            this.actionButton.Click += new System.EventHandler(this.actionButton_Click);
            // 
            // entitiesBox
            // 
            this.entitiesBox.HorizontalScrollbar = true;
            this.entitiesBox.Location = new System.Drawing.Point(7, 74);
            this.entitiesBox.Name = "entitiesBox";
            this.entitiesBox.Size = new System.Drawing.Size(416, 56);
            this.entitiesBox.TabIndex = 28;
            this.entitiesBox.SelectedIndexChanged += new System.EventHandler(this.entitiesBox_SelectedIndexChanged);
            // 
            // antGrid
            // 
            this.antGrid.Location = new System.Drawing.Point(428, 50);
            this.antGrid.Name = "antGrid";
            this.antGrid.Size = new System.Drawing.Size(365, 365);
            this.antGrid.TabIndex = 32;
            this.antGrid.Text = "antGrid1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 421);
            this.Controls.Add(this.antGrid);
            this.Controls.Add(this.cohortsBox);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.actionButton);
            this.Controls.Add(this.entitiesBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Koza\'s Ant Evolver";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox cohortsBox;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Button actionButton;
        private System.Windows.Forms.ListBox entitiesBox;
        private AntGrid antGrid;
    }
}