using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;

namespace torchlightIIstathack
{
    public partial class Form1 : Form
    {
        Torchlight game;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRefreshStats_Click(object sender, EventArgs e)
        {
            this.txtDexterity.Text = game.Dexterity.ToString();
            this.txtStrength.Text = game.Strength.ToString();
            this.txtFocus.Text = game.Focus.ToString();
            this.txtVitality.Text = game.Vitality.ToString();
        }

        private void btnApplyStats_Click(object sender, EventArgs e)
        {
            game.Dexterity = int.Parse(this.txtDexterity.Text);
            game.Strength = int.Parse(this.txtStrength.Text);
            game.Focus = int.Parse(this.txtFocus.Text);
            game.Vitality = int.Parse(this.txtVitality.Text);
            game.PlayerLevel = int.Parse(this.txtPlayerLevel.Text);
            game.ApplyStats();
            MessageBox.Show("Successfully Applied New Stat Levels :)");
        }

        private void btnRefreshSkills_Click(object sender, EventArgs e)
        {
            this.txtSkillPoints.Text = game.SkillPoints.ToString();
        }

        private void btnApplySkills_Click(object sender, EventArgs e)
        {
            game.SkillPoints = int.Parse(this.txtSkillPoints.Text);
            game.ApplySkills();
            MessageBox.Show("Successfully Applied New Skill Levels :)");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Torchlight();
            this.txtDexterity.Text = game.Dexterity.ToString();
            this.txtStrength.Text = game.Strength.ToString();
            this.txtFocus.Text = game.Focus.ToString();
            this.txtVitality.Text = game.Vitality.ToString();
            this.txtPlayerLevel.Text = game.PlayerLevel.ToString();

            this.txtSkillPoints.Text = game.SkillPoints.ToString();
        }


    }
    
}
