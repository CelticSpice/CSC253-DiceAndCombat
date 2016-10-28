/*
    This class represents base creature objects in the game
    9/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Creature
    {
        // Fields
        private BaseStats _stats;
        private Creature _target;
        private Room _location;
        private string _description;

        /*
            Constructor
        */

        public Creature()
        {
            _stats = new BaseStats();
            _target = null;
            _location = null;
            _description = "";
        }

        /*
            Constructor
            Defines stats
        */

        public Creature(BaseStats stats, string desc)
        {
            _stats = new BaseStats(stats);
            _target = null;
            _location = null;
            _description = desc;
        }

        /*
            Constructor
            Creates a clone of an existing Creature object
        */

        public Creature(Creature c)
        {
            _stats = new BaseStats(c._stats);
            _target = c._target;
            _location = c._location;
            _description = c._description;
        }

        /*
            The Attack method has the Creature execute an attack against another Creature,
            reducing the attacked Creature's hitpoints and returning the damage dealt
        */

        public virtual int Attack(Creature defender)
        {
            _target = defender;
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
            Target property
        */

        public Creature Target
        {
            get { return _target; }
            set { _target = value; }
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
            Description Property
        */

        public string Description
        {
            get { return _description; }
        }
    }
}
