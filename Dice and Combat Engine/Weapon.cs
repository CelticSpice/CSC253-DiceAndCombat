/*
    This class represents a weapon in the game
    9/14/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice_and_Combat_Engine
{
    class Weapon : Item
    {
        // Consts
        private const int ID = 1;    // All weapons have an id of 1

        //Field
        private int _damageBonus;

        /*
            Constructor
            Accepts the value, durability, name, and attack bonus of the weapon
        */

        public Weapon(int value, int durability, string name, int attack)
            : base(value, durability, ID, name)
        {
            _damageBonus = attack;
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
