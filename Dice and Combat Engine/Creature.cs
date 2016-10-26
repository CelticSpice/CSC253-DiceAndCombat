/*
    This class represents base creature objects in the game
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
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
        private string _description;

        /*
            Constructor
        */

        public Creature()
        {
            _stats = new BaseStats();
            _attributes = new Attributes();
            _location = null;
            _target = null;
            _description = "";
        }

        /*
            Constructor
            Defines stats and attributes
        */

        public Creature(BaseStats stats, Attributes attribs, string desc)
        {
            _stats = new BaseStats(stats);
            _attributes = attribs;
            _location = null;
            _target = null;
            _description = desc;
        }

        /*
            Constructor
            Creates a clone of an existing Creature object
        */

        public Creature(Creature c)
        {
            _stats = new BaseStats(c._stats);
            _attributes = c._attributes;
            _location = c._location;
            _target = c._target;
            _description = c._description;
        }

        /*
            The Attack method has the Creature execute an attack against another Creature,
            reducing the attacked Creature's hitpoints and returning the damage dealt
        */

        public virtual int Attack(Creature defender)
        {
            _stats.Damage.Roll();
            int damage = _stats.Damage.DieResult;
            defender._stats.HitPoints -= damage;
            if (defender._stats.HitPoints <= 0)
            {
                defender = null;
                Target = null;
                if (!(defender is Player))
                    _location.Denizens.Remove(defender);
            }
            return damage;
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

        /*
            Description Property
        */

        public string Description
        {
            get { return _description; }
        }
    }
}
