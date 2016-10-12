/*
    This class represents base creature objects in the game
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Drawing;

namespace Dice_and_Combat_Engine
{
    // Structs
    struct BaseStats
    {
        public string name;

        public int hitPoints,
                   maxHitPoints,
                   attackBonus,
                   armorClass,
                   xpValue;

        public RandomDie damage;

        public bool friendly;
    }

    struct Attributes
    {
        public int strength,
                   constitution,
                   dexterity,
                   intelligence,
                   wisdom,
                   charisma;
    }

    class Creature
    {
        // Fields
        private BaseStats _stats;
        private Attributes _attributes;
        private Room _location;
        private Creature _target;

        /*
            Constructor
        */

        public Creature()
        {
            _stats = new BaseStats();
            _attributes = new Attributes();
            _location = null;
            _target = null;
        }

        /*
            Constructor
            Defines starting stats and attributes
        */

        public Creature(BaseStats stats, Attributes attribs)
        {
            _stats = stats;
            _attributes = attribs;
            _location = null;
            _target = null;
        }

        /*
            Constructor
            Creates a clone of an existing Creature object
        */

        public Creature(Creature c)
        {
            _stats = c._stats;
            _attributes = c._attributes;
            _location = c._location;
            _target = c._target;
        }

        /*
            Stats property
        */

        public BaseStats Stats
        {
            get { return _stats; }
            set { _stats = value; }
        }

        /*
            Attributes property
        */

        public Attributes Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        /*
            Location property
        */

        public Room Location
        {
            get { return _location; }
            set { _location = value; }
        }

        /*
            Target property
        */

        public Creature Target
        {
            get { return _target; }
            set { _target = value; }
        }
    }
}
