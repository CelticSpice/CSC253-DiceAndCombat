/*
    This class represents base creature objects in the game
    9/5/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice_and_Combat_Engine
{
    // Structs
    struct BaseStats
    {
        public string name;

        public int hitPoints,
                   attackBonus,
                   armorClass;

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
        private int _expWorth;
        private Image _portrait;

        /*
            Constructor
        */

        public Creature()
        {
            _stats = new BaseStats();
            _attributes = new Attributes();
            _expWorth = 0;
            _portrait = null;
        }

        /*
            Constructor
            Defines starting stats and attributes, experience worth, and portrait
        */

        public Creature(BaseStats creatureStats, Attributes attribs, int exp, Image img)
        {
            _stats = creatureStats;
            _attributes = attribs;
            _expWorth = exp;
            _portrait = img;
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
            ExpWorth property
        */

        public int ExpWorth
        {
            get { return _expWorth; }
            set { _expWorth = value; }
        }

        /*
            Portrait property
        */

        public Image Portrait
        {
            get { return _portrait; }
            set { _portrait = value; }
        }
    }
}
