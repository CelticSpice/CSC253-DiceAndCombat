/*
    This class represents base item objects
    10/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    abstract class Item
    {
        // Fields
        private string _name,
                       _description;

        private int _durability,
                    _value;

        private ItemType _type;

        /*
            Constructor
        */

        public Item(string n, string desc, int d, int v, ItemType t)
        {
            _name = n;
            _description = desc;
            _durability = d;
            _value = v;
            _type = t;
        }

        /*
            Name property
        */

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /*
            Description property
        */

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /*
            Durability property
        */

        public int Durability
        {
            get { return _durability; }
            set { _durability = value; }
        }

        /*
            Value property
        */

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /*
            Type property
        */

        public ItemType Type
        {
            get { return _type; }
        }
    }
}
