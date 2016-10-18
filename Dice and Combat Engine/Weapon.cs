/*
    This class represents a weapon in the game
    9/22/2016
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
            Accepts the name, description, durability, value, and attack bonus of the weapon
        */

        public Weapon(string name, string desc, int durability, int value, int damage)
            : base(name, desc, durability, value, WEAPON_ID)
        {
            _damageBonus = damage;
        }

        /*
            Copy Constructor
            Accepts a Weapon object
        */

        public Weapon(Weapon w)
        {
            this.Name = w.Name;
            this.Description = w.Description;
            this.Durability = w.Durability;
            this.Value = w.Value;
            this._damageBonus = w._damageBonus;
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
