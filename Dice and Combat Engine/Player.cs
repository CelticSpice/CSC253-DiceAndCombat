/*
    This class represents playable characters
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;
using System.Drawing;

namespace Dice_and_Combat_Engine
{
    // Structs
    struct PlayerStats
    {
        public string playerClass,
                      race;

        public int experience, level;
    }

    class Player : Creature
    {
        // Fields
        private PlayerStats _playerStats;
        private List<Item> _inventory;
        private Weapon _equippedWeapon;
        private bool _leveledUp;

        /*
            Constructor
            
            Accepts Player-specific stats, base stats, attributes, and a portrait of the player
        */

        public Player(PlayerStats playerStats, BaseStats baseStats, Attributes attribs)
            : base(baseStats, attribs)
        {
            _playerStats = playerStats;
            _inventory = new List<Item>();
            _equippedWeapon = null;
            _leveledUp = false;
        }

        /*
            The GainExperience method simulates the player gaining experience points
            The method accepts the number of experience points earned as an argument
        */

        public void GainExperience(int xp)
        {
            // Experience needed to level up modifier
            const int MODIFIER = 25;

            // Add experience to player
            _playerStats.experience += xp;

            // Check for level up
            if (_playerStats.experience >= _playerStats.level * MODIFIER)
            {
                // Set level up notifier to true
                _leveledUp = true;
            }
        }

        /*
            The LevelUp method levels up the player
        */

        public void LevelUp()
        {
            // Points to spend on upgrades each level up
            // const int POINTS = 3;
            
            // Raise player's level
            _playerStats.level += 1;

            // Upgrade player's max hitpoints
            BaseStats newStats = Stats;
            newStats.maxHitPoints += _playerStats.level;
            Stats = newStats;

            /*

            // Add 1 to each of the player's stats
            BaseStats newStats = Stats;
            newStats.hitPoints += 1;
            newStats.maxHitPoints += 1;
            newStats.attackBonus += 1;
            newStats.armorClass += 1;

            // Add 1 to each of the player's attributes
            Attributes attribs = Attributes;
            attribs.strength += 1;
            attribs.constitution += 1;
            attribs.dexterity += 1;
            attribs.intelligence += 1;
            attribs.wisdom += 1;
            attribs.charisma += 1;

            // Set new stats and attributes
            /Stats = newStats;
            Attributes = attribs;

            */

            // Return leveled up notifier to false
            _leveledUp = false;
        }

        /*
            The EquipWeapon method simulates the player equipping a weapon
        */

        public void EquipWeapon(Weapon weapon)
        {
            _equippedWeapon = weapon;
            BaseStats newStats = Stats;
            newStats.damage.DieSize += _equippedWeapon.DamageBonus;
            Stats = newStats;
        }

        /*
            The UnequipWeapon method simulates the player unequipping a weapon
        */

        public void UnequipWeapon()
        {
            BaseStats newStats = Stats;
            newStats.damage.DieSize -= _equippedWeapon.DamageBonus;
            Stats = newStats;
            _equippedWeapon = null;
        }

        /*
            The UsePotion method simulates the player using a potion
        */

        public void UsePotion(Potion potion)
        {
            // Update player's hp appropriately
            BaseStats newStats = Stats;
            newStats.hitPoints += potion.Use();

            // Make sure hp does not exceed max
            if (newStats.hitPoints > Stats.maxHitPoints)
            {
                newStats.hitPoints = Stats.maxHitPoints;
            }

            // Set new stats
            Stats = newStats;

            // Check if potion is destroyed
            if (potion.Durability == 0)
            {
                // Remove potion from inventory
                _inventory.Remove(potion);
            }
        }

        /*
            PlayerStats property
        */

        public PlayerStats PlayerStats
        {
            get { return _playerStats; }
            set { _playerStats = value; }
        }

        /*
            Inventory property
        */

        public List<Item> Inventory
        {
            get { return _inventory; }
        }

        /*
            EquippedWeapon property
        */

        public Weapon EquippedWeapon
        {
            get { return _equippedWeapon; }
        }

        /*
            LeveledUp property
        */

        public bool LeveledUp
        {
            get { return _leveledUp; }
        }
    }
}
