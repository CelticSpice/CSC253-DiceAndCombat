/*
    This class represents the basic statistics that all Creature's share
*/

namespace Dice_and_Combat_Engine
{
    class BaseStats
    {
        // Fields
        private string _name;

        private int _hitPoints,
                    _maxHitPoints,
                    _attackBonus,
                    _armorClass,
                    _xpValue;

        private RandomDie _damage;

        private bool _friendly;

        /*
            Constructor
        */

        public BaseStats()
        {
            _name = null;
            _hitPoints = 0;
            _maxHitPoints = 0;
            _attackBonus = 0;
            _armorClass = 0;
            _xpValue = 0;
            _damage = null;
            _friendly = false;
        }

        /*
            Copy Constructor
        */

        public BaseStats(BaseStats stats)
        {
            _name = stats._name;
            _hitPoints = stats._hitPoints;
            _maxHitPoints = stats._maxHitPoints;
            _attackBonus = stats._attackBonus;
            _armorClass = stats._armorClass;
            _xpValue = stats._xpValue;
            _damage = new RandomDie(stats._damage.DieSize);
            _friendly = stats._friendly;
        }

        /*
            Name Property
        */

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /*
            HitPoints Property
        */

        public int HitPoints
        {
            get { return _hitPoints; }
            set { _hitPoints = value; }
        }

        /*
            MaxHitPoints Property
        */

        public int MaxHitPoints
        {
            get { return _maxHitPoints; }
            set { _maxHitPoints = value; }
        }

        /*
            AttackBonus Property
        */

        public int AttackBonus
        {
            get { return _attackBonus; }
            set { _attackBonus = value; }
        }

        /*
            ArmorClass Property
        */

        public int ArmorClass
        {
            get { return _armorClass; }
            set { _armorClass = value; }
        }

        /*
            XPValue Property
        */

        public int XPValue
        {
            get { return _xpValue; }
            set { _xpValue = value; }
        }

        /*
            Damage Property
        */

        public RandomDie Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        /*
            Friendly Property
        */

        public bool Friendly
        {
            get { return _friendly; }
            set { _friendly = value; }
        }
    }
}
