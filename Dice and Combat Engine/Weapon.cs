/*
    This class represents a weapon
    9/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Weapon : Item
    {
        // Fields
        private const ItemType TYPE = ItemType.Weapon;

        private int _damageBonus;

        /*
            Constructor
            Accepts the name, description, durability, value, and attack bonus
        */

        public Weapon(string name, string desc, int durability, int value, int damage)
            : base(name, desc, durability, value, TYPE)
        {
            _damageBonus = damage;
        }

        /*
            Copy Constructor
            Accepts a Weapon object
        */

        public Weapon(Weapon w)
            : base(w.Name, w.Description, w.Durability, w.Value, TYPE)
        {
            _damageBonus = w._damageBonus;
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
