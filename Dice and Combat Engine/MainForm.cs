/*
    This program is a simple rouglelike game
    9/13/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dice_and_Combat_Engine
{
    public partial class MainForm : Form
    {
        // Fields
        Game game;  // Handles game logic

        /*
            Constructor
        */

        public MainForm()
        {
            InitializeComponent();
            game = new Game();
            StartGame();
        }

        /*
            The StartGame method initializes the game's view
        */

        internal void StartGame()
        {
            // Initialize UI
            playerNameTextBox.Text = game.Player.Stats.name;
            playerRaceClassTextBox.Text = game.Player.PlayerStats.race + " " + game.Player.PlayerStats.playerClass;
            playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();
            playerABTextBox.Text = game.Player.Stats.attackBonus.ToString();
            playerACTextBox.Text = game.Player.Stats.armorClass.ToString();
        }

        /*
            Event listener for attack button
        */

        private void attackButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
