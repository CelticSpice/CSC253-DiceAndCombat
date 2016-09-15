/*
    This class represents playable characters
    9/5/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /*
            Constructor
            
            Accepts Player-specific stats, base stats, attributes, and a portrait of the player
        */

        public Player(PlayerStats playerStats, BaseStats baseStats, Attributes attribs, Image portrait = null)
            : base(baseStats, attribs, 0, portrait)
        {
            _playerStats = playerStats;
            _inventory = new List<Item>();
            _equippedWeapon = null;
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
    }
}
