/*
    This class represents base item objects
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
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
                    _value,
                    _id;          // Weapon id = 1, Potion id = 2, Treasure id = 3

        /*
            Constructor
        */

        public Item(string n, string desc, int d, int v, int i)
        {
            _name = n;
            _description = desc;
            _durability = d;
            _value = v;
            _id = i;
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
            ID property
        */

        public int ID
        {
            get { return _id; }
        }
    }
}
