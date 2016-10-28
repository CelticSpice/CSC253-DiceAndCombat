/*
    This class represents treasure
    10/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Treasure : Item
    {
        // Fields
        private const ItemType TYPE = ItemType.Treasure;

        /*
             Constructor
             Accepts the name, durability, and value
        */

        public Treasure(string name, string desc, int durability, int value)
            : base(name, desc, durability, value, TYPE)
        {
        }

        /*
            Copy Constructor
        */

        public Treasure(Treasure t)
            : base(t.Name, t.Description, t.Durability, t.Value, TYPE)
        {
        }
    }
}
