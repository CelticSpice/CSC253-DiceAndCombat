/*
    This class handles item objects
    9/12/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice_and_Combat_Engine
{
    abstract class Item
    {
        // Fields
        private int _value,
                    _durability,
                    _id;          // Weapon id = 1, Potion id = 2, Treasure id = 3

        private string _name;

        /*
            Constructor
        */

        public Item(int v, int d, int i, string n)
        {
            _value = v;
            _durability = d;
            _id = i;
            _name = n;
        }

        /*
            No-arg Constructor
        */

        public Item()
        {
            _value = 0;
            _durability = -1;
            _id = -1;
            _name = "spork";
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
            Durability property
        */

        public int Durability
        {
            get { return _durability; }
            set { _durability = value; }
        }

        /*
            ID property
        */

        public int ID
        {
            get { return _id; }
        }

        /*
            Name property
        */

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
