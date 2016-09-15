/*
    This class represents treasure in the game
    9/14/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Dice_and_Combat_Engine
{
    class Treasure : Item
    {
        // Consts
        private const int ID = 3;   // All treasure has an ID of 3

        /*
             Constructor
             Accepts value, durability, and name
        */

        public Treasure(int value, int durability, string name)
            : base(value, durability, ID, name)
        {
            // ???  XD
        }

        /*
            The Use method simulates using treasure.
            Shall we have it end the game for now?
        */

        public void Use()
        {
            MessageBox.Show("You conquered the dungeon!");
            Application.Exit();
        }
    }
}
