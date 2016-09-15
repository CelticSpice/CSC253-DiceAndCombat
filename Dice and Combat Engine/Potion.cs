/*
    This class represents potions in the game
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
    class Potion : Item
    {
        // Consts
        private const int ID = 2;   // All Potions have an id of 2

        // Fields
        private int _healthRestored;

        /*
            Constructor
            Accepts the value, durability, name, and amount of health restored
        */
        public Potion(int value, int durability, string name, int heal)
            : base(value, durability, ID, name)
        {
            _healthRestored = heal;
        }

        /*
            The Use method simulates using the potion for its
            benefits. The value gained is returned and the potion's
            durability is decreased
        */

        public int Use()
        {
            Durability -= 1;
            return _healthRestored;
        }
    }
}
