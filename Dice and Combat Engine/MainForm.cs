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
            Player player = game.Player;

            playerNameTextBox.Text = player.Stats.name;
            playerRaceClassTextBox.Text = player.PlayerStats.race + " " + player.PlayerStats.playerClass;
            playerHPTextBox.Text = player.Stats.hitPoints.ToString();
            playerABTextBox.Text = player.Stats.attackBonus.ToString();
            playerACTextBox.Text = player.Stats.armorClass.ToString();

            roomNameLbl.Text = game.CurrentRoom.RoomName;

            string[] creatureNames = new string[game.CurrentRoom.Denizens.Count];
            int i = 0;
            foreach (Creature crea in game.CurrentRoom.Denizens)
            {
                creatureNames[i] = crea.Stats.name;
                i++;
            }

            creatureList.Items.AddRange(creatureNames);
            creatureList.SetSelected(0, true);

            Creature creature = game.CurrentRoom.Denizens[0];

            creatureImgBox.Image = creature.Portrait;

            creatureNameTextBox.Text = creature.Stats.name;
            creatureHPTextBox.Text = creature.Stats.hitPoints.ToString();
            creatureABTextBox.Text = creature.Stats.attackBonus.ToString();
            creatureACTextBox.Text = creature.Stats.armorClass.ToString();
        }

        /*
            Event listener for attack button
        */

        private void attackButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
