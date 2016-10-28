/*
    This class represents a potion
    9/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Potion : Item
    {
        // Fields
        private const ItemType TYPE = ItemType.Potion;

        private int _healthRestored;

        /*
            Constructor
            Accepts the name, description, durability, value, and amount of health restored
        */
        public Potion(string name, string desc, int durability, int value, int heal)
            : base(name, desc, durability, value, TYPE)
        {
            _healthRestored = heal;
        }

        /*
            Copy Constructor
        */

        public Potion(Potion p)
            : base(p.Name, p.Description, p.Durability, p.Value, TYPE)
        {
            _healthRestored = p._healthRestored;
        }

        /*
            HealthRestored property
        */

        public int HealthRestored
        {
            get { return _healthRestored; }
        }
    }
}
