/*
    This program is a simple rougelike game
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Windows.Forms;

namespace Dice_and_Combat_Engine
{
    public partial class MainForm : Form
    {
        // Fields
        Game game;

        /*
            Constructor
        */

        public MainForm()
        {
            InitializeComponent();
            const int SIZE = 5;
            game = new Game(SIZE);
            Console.WriteLine(game.GetDungeonASCII());
            StartGame();
        }

        /*
            The StartGame method initializes the game's view
        */

        private void StartGame()
        {
            // Initialize UI
            playerNameTextBox.Text = game.Player.Stats.Name;
            playerRaceClassTextBox.Text = game.Player.PlayerStats.race + " " + game.Player.PlayerStats.playerClass;
            playerHPTextBox.Text = game.Player.Stats.HitPoints.ToString();
            playerABTextBox.Text = game.Player.Stats.AttackBonus.ToString();
            playerACTextBox.Text = game.Player.Stats.ArmorClass.ToString();
            roomNameLbl.Text = game.Player.Location.Name;
            outputRTxtBox.Text = ">>  ";
        }

        /*
            The UpdateViews method updates all of the views
        */

        private void UpdateViews()
        {
            playerHPTextBox.Text = game.Player.Stats.HitPoints.ToString();
            playerABTextBox.Text = game.Player.Stats.AttackBonus.ToString();
            playerACTextBox.Text = game.Player.Stats.ArmorClass.ToString();
            roomNameLbl.Text = game.Player.Location.Name;
            outputRTxtBox.Text += ">>  ";
            outputRTxtBox.SelectionStart = outputRTxtBox.Text.Length;
            outputRTxtBox.ScrollToCaret();
            commandTxtBox.Text = "";
            commandTxtBox.Focus();

            if (game.Player.Target != null)
            {
                creatureNameTextBox.Text = game.Player.Target.Stats.Name;
                creatureHPTextBox.Text = game.Player.Target.Stats.HitPoints.ToString();
                creatureABTextBox.Text = game.Player.Target.Stats.AttackBonus.ToString();
                creatureACTextBox.Text = game.Player.Target.Stats.ArmorClass.ToString();
            }
            else
            {
                creatureNameTextBox.Text = "";
                creatureHPTextBox.Text = "";
                creatureABTextBox.Text = "";
                creatureACTextBox.Text = "";
            }
        }

        /*
            Handler for goBtn
        */

        private void goBtn_Click(object sender, EventArgs e)
        {
            string output = game.ParseCommand(commandTxtBox.Text);
            outputRTxtBox.Text += output;

            // Check for utility requests
            if (game.RequestToClear)
            {
                outputRTxtBox.Clear();
                game.RequestToClear = false;
                commandTxtBox.Text = "";
                commandTxtBox.Focus();
            }
            else if (game.RequestToQuit)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Dice and Combat",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    this.Close();
                else
                    game.RequestToQuit = false;

                commandTxtBox.Text = "";
                commandTxtBox.Focus();
            }
            else
                UpdateViews();

            // Check if Player is dead
            if (game.Player.Stats.HitPoints <= 0)
            {
                MessageBox.Show("You have died!");
                this.Close();
            }
        }
    }
}
