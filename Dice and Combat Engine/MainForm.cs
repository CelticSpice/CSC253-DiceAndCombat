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
            playerNameTextBox.Text = game.Player.Stats.name;
            playerRaceClassTextBox.Text = game.Player.PlayerStats.race + " " + game.Player.PlayerStats.playerClass;
            playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();
            playerABTextBox.Text = game.Player.Stats.attackBonus.ToString();
            playerACTextBox.Text = game.Player.Stats.armorClass.ToString();
            roomNameLbl.Text = game.Player.Location.Name;
        }

        /*
            The UpdateViews method updates all of the views
        */

        private void UpdateViews()
        {
            playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();
            playerABTextBox.Text = game.Player.Stats.attackBonus.ToString();
            playerACTextBox.Text = game.Player.Stats.armorClass.ToString();
            roomNameLbl.Text = game.Player.Location.Name;

            if (game.Player.Target != null)
            {
                creatureNameTextBox.Text = game.Player.Target.Stats.name;
                creatureHPTextBox.Text = game.Player.Target.Stats.hitPoints.ToString();
                creatureABTextBox.Text = game.Player.Target.Stats.attackBonus.ToString();
                creatureACTextBox.Text = game.Player.Target.Stats.armorClass.ToString();
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
            outputRTxtBox.SelectionStart = outputRTxtBox.Text.Length;
            outputRTxtBox.ScrollToCaret();
            commandTxtBox.Text = "";
            commandTxtBox.Focus();
            UpdateViews();

            // Check if user requested to quit
            if (output == "Quitting...")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Dice and Combat",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Exit
                    this.Close();
                }
            }

            // Check if Player is dead
            if (game.Player.Stats.hitPoints <= 0)
            {
                MessageBox.Show("You have died!");
                this.Close();
            }
        }
    }
}
