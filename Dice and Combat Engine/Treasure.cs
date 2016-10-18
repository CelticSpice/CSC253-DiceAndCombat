/*
    This class represents treasure in the game
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Windows.Forms;

namespace Dice_and_Combat_Engine
{
    class Treasure : Item
    {
        // Consts
        private const int TREASURE_ID = 3;   // All treasure has an ID of 3

        /*
             Constructor
             Accepts the name, durability, and value
        */

        public Treasure(string name, string desc, int durability, int value)
            : base(name, desc, durability, value, TREASURE_ID)
        {
            // ???  XD
        }

        /*
            Copy Constructor
        */

        public Treasure(Treasure t)
        {
            this.Name = t.Name;
            this.Description = t.Description;
            this.Durability = t.Durability;
            this.Value = t.Value;
        }
    }
}
