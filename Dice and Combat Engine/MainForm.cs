/*
    This program is a simple rouglelike game
    9/13/2016
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
            game = new Game();
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

            roomNameLbl.Text = game.CurrentRoom.RoomName;

            string[] creatureNames = new string[game.CurrentRoom.Denizens.Count];

            int i = 0;
            foreach (Creature mob in game.CurrentRoom.Denizens)
            {
                creatureNames[i] = mob.Stats.name;
                i++;
            }

            creatureList.Items.AddRange(creatureNames);
            creatureList.SetSelected(0, true);

            creatureImgBox.Image = game.CurrentRoom.Denizens[0].Portrait;

            creatureNameTextBox.Text = game.CurrentRoom.Denizens[0].Stats.name;
            creatureHPTextBox.Text = game.CurrentRoom.Denizens[0].Stats.hitPoints.ToString();
            creatureABTextBox.Text = game.CurrentRoom.Denizens[0].Stats.attackBonus.ToString();
            creatureACTextBox.Text = game.CurrentRoom.Denizens[0].Stats.armorClass.ToString();
        }

        /*
            Event listener for attack button
        */

        private void attackButton_Click(object sender, EventArgs e)
        {
            // Get selected creature index
            int selectedCreatureIndex = creatureList.SelectedIndex;

            // Make sure that a creature is selected
            if (selectedCreatureIndex >= 0)
            {
                // Get selected creature
                Creature selectedCreature = game.CurrentRoom.Denizens[selectedCreatureIndex];

                // Initialize combat if not already active. If already active, check if the creature combat was
                // initialized with is the same creature selected

                if (!game.CombatEngine.CombatActive || !game.CombatEngine.IsCombatant(selectedCreature))
                {
                    Creature[] combatants = { game.Player, selectedCreature };
                    game.CombatEngine.InitCombat(combatants);
                }

                // Get weapon used, if any
                Weapon weapon = game.Player.EquippedWeapon;
                bool weaponUsed = (weapon != null) ? true : false;
                int weaponIndex = -1;

                if (weaponUsed)
                {
                    weaponIndex = game.Player.Inventory.IndexOf(weapon);
                }

                // Take combat round
                game.CombatEngine.TakeCombatRound();

                // Display combat feedback
                feedbackList.Items.AddRange(game.CombatEngine.CombatFeedback.ToArray());

                // Check if either combatant is dead
                if (game.Player.Stats.hitPoints == 0 || selectedCreature.Stats.hitPoints == 0)
                {
                    if (selectedCreature.Stats.hitPoints == 0)
                    {
                        // Remove creature from room and from list
                        game.CurrentRoom.Denizens.Remove(selectedCreature);
                        creatureList.Items.RemoveAt(creatureList.SelectedIndex);

                        creatureList.SelectedIndex = -1;

                        // Player gains experience for killing creature
                        game.Player.GainExperience(selectedCreature.Stats.xpValue);

                        // Check for level up
                        if (game.Player.LeveledUp)
                        {
                            // Level up
                            game.Player.LevelUp();

                            // Update feedback to inform of level up
                            feedbackList.Items.Add("You leveled up! You are now level " +
                                                   game.Player.PlayerStats.level.ToString());
                        }

                        // Check if the room has been cleared
                        if (game.CurrentRoom.Denizens.Count == 0)
                        {
                            // Player claims the loot
                            game.Player.Inventory.AddRange(game.CurrentRoom.Contents.ToArray());

                            string[] itemNames = new string[game.CurrentRoom.Contents.Count];

                            int i = 0;
                            foreach (Item item in game.CurrentRoom.Contents)
                            {
                                itemNames[i] = item.Name;
                                i++;
                            }

                            // Update player's inventory list
                            playerInventoryList.Items.AddRange(itemNames);

                            // Remove items from room
                            game.CurrentRoom.Contents.Clear();

                            // Update feedback to inform that the room has been looted of items
                            feedbackList.Items.Add("You have claimed the loot in this room!");

                            // Make next room available
                            nextBtn.Enabled = true;
                        }
                    }
                    else
                    {
                        // Player has died. Game over
                        playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();
                        creatureHPTextBox.Text = selectedCreature.Stats.hitPoints.ToString();
                        MessageBox.Show("You have died! Game Over!");
                        this.Close();
                    }
                }
                else
                {
                    // Update displayed statistics appropriately
                    playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();
                    creatureHPTextBox.Text = selectedCreature.Stats.hitPoints.ToString();
                }

                // If weapon was used, check if weapon is broken
                if (weaponUsed)
                {
                    if (weapon.Durability == 0)
                    {
                        // Remove weapon from inventory list
                        playerInventoryList.Items.RemoveAt(weaponIndex);

                        // Display feedback that weapon was broken
                        feedbackList.Items.Add("Your weapon broke!");
                    }
                }
            }
        }

        /*
            Event listener for use button
        */

        private void useItemBtn_Click(object sender, EventArgs e)
        {
            // Get selected item index
            int selectedItemIndex = playerInventoryList.SelectedIndex;

            // Make sure that an item is actually selected
            if (selectedItemIndex >= 0)
            {
                // Retrieve item as proper type
                if (game.Player.Inventory[selectedItemIndex] is Weapon)
                {
                    // Get weapon
                    Weapon weapon = (Weapon)game.Player.Inventory[selectedItemIndex];

                    // Player equips weapon
                    if (game.Player.EquippedWeapon != null)
                    {
                        game.Player.UnequipWeapon();
                    }

                    game.Player.EquipWeapon(weapon);

                    // Display feedback that player equips the selected weapon
                    feedbackList.Items.Add("You equip " + weapon.Name + ", increasing your damage by " +
                                           weapon.DamageBonus);
                }
                else if (game.Player.Inventory[selectedItemIndex] is Potion)
                {
                    // Get potion
                    Potion potion = (Potion)game.Player.Inventory[selectedItemIndex];

                    // Player uses potion
                    game.Player.UsePotion(potion);

                    // Update player information display to reflect potion use
                    playerHPTextBox.Text = game.Player.Stats.hitPoints.ToString();

                    // Display feedback about potion use
                    feedbackList.Items.Add("You used " + potion.Name + " and restored " +
                                           potion.HealthRestored + " hit points");

                    // Update inventory list
                    if (!game.Player.Inventory.Contains(potion))
                    {
                        playerInventoryList.Items.RemoveAt(selectedItemIndex);
                    }
                }
                else
                {
                    // Get treasure
                    Treasure treasure = (Treasure)game.Player.Inventory[selectedItemIndex];

                    // Player uses treasure
                    treasure.Use();
                }
            }
            else
            {
                // Inform user that no item is selected
                MessageBox.Show("No item is selected.");
            }
        }

        /*
            Event listener for the creature list selection changed
        */

        private void creatureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected creature index
            int selectedCreatureIndex = creatureList.SelectedIndex;

            if (selectedCreatureIndex >= 0)
            {
                // Change creature information display to that of selected creature
                Creature selectedCreature = game.CurrentRoom.Denizens[selectedCreatureIndex];

                creatureImgBox.Image = selectedCreature.Portrait;
                creatureHPTextBox.Text = selectedCreature.Stats.hitPoints.ToString();
                creatureABTextBox.Text = selectedCreature.Stats.attackBonus.ToString();
                creatureACTextBox.Text = selectedCreature.Stats.armorClass.ToString();
            }
            else
            {
                // Remove all displayed information
                creatureImgBox.Image = null;
                creatureHPTextBox.Text = "";
                creatureABTextBox.Text = "";
                creatureACTextBox.Text = "";
            }
        }

        /*
            Event listener for previous button
        */

        private void previousBtn_Click(object sender, EventArgs e)
        {
            // Get previous room
            Room previousRoom = game.CurrentRoom.GetPreviousRoom();

            // Change current room to previous room, if possible
            if (previousRoom != null)
            {
                game.CurrentRoom = previousRoom;

                // Update the information displays
                creatureList.Items.Clear();

                foreach (Creature creature in game.CurrentRoom.Denizens)
                {
                    creatureList.Items.Add(creature.Stats.name);
                }

                roomNameLbl.Text = game.CurrentRoom.RoomName;

                // Check if the current room has a previous room
                if (game.CurrentRoom.GetPreviousRoom() != null)
                {
                    previousBtn.Enabled = true;
                }
                else
                {
                    previousBtn.Enabled = false;
                }

                // Enable the next button
                nextBtn.Enabled = true;
            }
        }

        /*
            Event listener for the next button
        */

        private void nextBtn_Click(object sender, EventArgs e)
        {
            // Get next room
            Room nextRoom = game.CurrentRoom.GetNextRoom();

            // Change current room to next room, if possible
            if (nextRoom != null)
            {
                game.CurrentRoom = nextRoom;

                // Update the information displays
                foreach (Creature creature in game.CurrentRoom.Denizens)
                {
                    creatureList.Items.Add(creature.Stats.name);
                }

                roomNameLbl.Text = game.CurrentRoom.RoomName;

                // Check if this room's creatures have been slain
                bool roomCleared = false;

                if (game.CurrentRoom.Denizens.Count == 0)
                {
                    roomCleared = true;
                }

                // Check if the current room has a next room
                if (roomCleared && game.CurrentRoom.GetNextRoom() != null)
                {
                    nextBtn.Enabled = true;
                }
                else
                {
                    nextBtn.Enabled = false;
                }

                // Enable the previous button
                previousBtn.Enabled = true;
            }
        }
    }
}
