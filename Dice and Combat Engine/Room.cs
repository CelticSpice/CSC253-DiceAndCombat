/*
    This class represents a room in the game
    10/3/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;

namespace Dice_and_Combat_Engine
{
    class Room
    {
        // Fields
        private bool _linked;                // Whether Room has been linked (for linking with Prim's algorithm)
        private int _weight;                 // Value of Room (for linking with Prim's algorithm)

        private int _xLoc, _yLoc;

        private string _roomName;
        private List<Creature> _denizens;   // Creatures in the room
        private List<Item> _contents;       // Items in the room
        private Room[] _exits;

        /*
            Constructor
            Accepts the room name and the number of creatures and items initially in the room
        */

        public Room(string name, int numCreatures, int numItems)
        {
            _roomName = name;
            _denizens = new List<Creature>(numCreatures);
            _contents = new List<Item>(numItems);

            const int NUM_EXITS = 4;
            _exits = new Room[NUM_EXITS];

            _linked = false;
            _weight = -1;

            _xLoc = -1;
            _yLoc = -1;
        }

        /*
            Linked property
        */

        public bool Linked
        {
            get;
            set;
        }

        /*
            Weight property
        */

        public int Weight
        {
            get;
            set;
        }

        /*
            XLoc property
        */
        
        public int XLoc
        {
            get;
            set;
        }

        /*
            YLoc property
        */

        public int YLoc
        {
            get;
            set;
        }

        /*
            RoomName property
        */

        public string RoomName
        {
            get { return _roomName; }
        }

        /*
            Denizens property
        */

        public List<Creature> Denizens
        {
            get { return _denizens; }
        }

        /*
            Contents property
        */

        public List<Item> Contents
        {
            get { return _contents; }
        }

        /*
            Exits property
        */

        public Room[] Exits
        {
            get { return _exits; }
        }
    }
}
