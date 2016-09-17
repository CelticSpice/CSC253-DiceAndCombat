﻿/*
    This class represents a weapon in the game
    9/14/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Weapon : Item
    {
        // Consts
        private const int WEAPON_ID = 1;    // All weapons have an id of 1

        //Field
        private int _damageBonus;

        /*
            Constructor
            Accepts the name, durability, value, and attack bonus of the weapon
        */

        public Weapon(string name, int durability, int value, int damage)
            : base(name, durability, value, WEAPON_ID)
        {
            _damageBonus = damage;
        }

        /*
            The Use method simulates using a weapon, reducing its durability
        */

        public void Use()
        {
            Durability -= 1;
        }

        /*
            DamageBonus property
        */

        public int DamageBonus
        {
            get { return _damageBonus; }
        }
    }
}
