﻿/*
    This class represents base creature objects in the game
    9/5/2016
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
        private Image _portrait;

        /*
            Constructor
        */

        public Creature()
        {
            _stats = new BaseStats();
            _attributes = new Attributes();
            _portrait = null;
        }

        /*
            Constructor
            Defines starting stats and attributes, and portrait
        */

        public Creature(BaseStats stats, Attributes attribs, Image portrait)
        {
            _stats = stats;
            _attributes = attribs;
            _portrait = portrait;
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
            Portrait property
        */

        public Image Portrait
        {
            get { return _portrait; }
            set { _portrait = value; }
        }
    }
}
