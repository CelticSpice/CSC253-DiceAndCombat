/*
    This class represents potions in the game
    9/14/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Potion : Item
    {
        // Consts
        private const int POTION_ID = 2;   // All Potions have an id of 2

        // Fields
        private int _healthRestored;

        /*
            Constructor
            Accepts the name, durability, value, and amount of health restored
        */
        public Potion(string name, int durability, int value, int heal)
            : base(name, durability, value, POTION_ID)
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

        /*
            HealthRestored property
        */

        public int HealthRestored
        {
            get { return _healthRestored; }
        }
    }
}
