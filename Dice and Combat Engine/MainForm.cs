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
        Game game;  // Handles game logic

        /*
            Constructor
        */

        public MainForm()
        {
            InitializeComponent();
            const int SIZE = 5;
            game = new Game(SIZE);

            Console.WriteLine(game.GetDungeonASCII());
        }

        ///*
        //    The StartGame method initializes the game's view
        //*/

        //private void StartGame()
        //{
        //    // Initialize UI
        //    playerNameTextBox.Text = game.Player.Stats.name;
        //    playerRaceClassTextBox.Text = game.Player.PlayerStats.race + " " + game.Player.PlayerStats.playerClass;
        //    playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();
        //    playerABTextBox.Text = game.Player.Stats.attackBonus.ToString();
        //    playerACTextBox.Text = game.Player.Stats.armorClass.ToString();

        //    roomNameLbl.Text = game.CurrentRoom.RoomName;

        //    string[] creatureNames = new string[game.CurrentRoom.Denizens.Count];

        //    int i = 0;
        //    foreach (Creature mob in game.CurrentRoom.Denizens)
        //    {
        //        creatureNames[i] = mob.Stats.name;
        //        i++;
        //    }

        //    creatureList.Items.AddRange(creatureNames);
        //    creatureList.SetSelected(0, true);

        //    creatureImgBox.Image = game.CurrentRoom.Denizens[0].Portrait;

        //    creatureNameTextBox.Text = game.CurrentRoom.Denizens[0].Stats.name;
        //    creatureHPTextBox.Text = game.CurrentRoom.Denizens[0].Stats.hitPoints.ToString();
        //    creatureABTextBox.Text = game.CurrentRoom.Denizens[0].Stats.attackBonus.ToString();
        //    creatureACTextBox.Text = game.CurrentRoom.Denizens[0].Stats.armorClass.ToString();
        //}

        ///*
        //    Event listener for attack button
        //*/

        //private void attackButton_Click(object sender, EventArgs e)
        //{

        //}

        ///*
        //    Event listener for use button
        //*/

        //private void useItemBtn_Click(object sender, EventArgs e)
        //{

        //}

        ///*
        //    Event listener for the creature list selection changed
        //*/

        //private void creatureList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Get selected creature index
        //    int selectedCreatureIndex = creatureList.SelectedIndex;

        //    if (selectedCreatureIndex >= 0)
        //    {
        //        // Change creature information display to that of selected creature
        //        Creature selectedCreature = game.CurrentRoom.Denizens[selectedCreatureIndex];

        //        creatureImgBox.Image = selectedCreature.Portrait;
        //        creatureHPTextBox.Text = selectedCreature.Stats.hitPoints.ToString();
        //        creatureABTextBox.Text = selectedCreature.Stats.attackBonus.ToString();
        //        creatureACTextBox.Text = selectedCreature.Stats.armorClass.ToString();
        //    }
        //    else
        //    {
        //        // Remove all displayed information
        //        creatureImgBox.Image = null;
        //        creatureHPTextBox.Text = "";
        //        creatureABTextBox.Text = "";
        //        creatureACTextBox.Text = "";
        //    }
        //}

        ///*
        //    Event listener for previous button
        //*/

        //private void previousBtn_Click(object sender, EventArgs e)
        //{
        //    // Get previous room
        //    Room previousRoom = game.CurrentRoom.GetPreviousRoom();

        //    // Change current room to previous room, if possible
        //    if (previousRoom != null)
        //    {
        //        game.CurrentRoom = previousRoom;

        //        // Update the information displays
        //        creatureList.Items.Clear();

        //        foreach (Creature creature in game.CurrentRoom.Denizens)
        //        {
        //            creatureList.Items.Add(creature.Stats.name);
        //        }

        //        roomNameLbl.Text = game.CurrentRoom.RoomName;

        //        // Check if the current room has a previous room
        //        if (game.CurrentRoom.GetPreviousRoom() != null)
        //        {
        //            previousBtn.Enabled = true;
        //        }
        //        else
        //        {
        //            previousBtn.Enabled = false;
        //        }

        //        // Enable the next button
        //        nextBtn.Enabled = true;
        //    }
        //}

        ///*
        //    Event listener for the next button
        //*/

        //private void nextBtn_Click(object sender, EventArgs e)
        //{

        //}
    }
}
