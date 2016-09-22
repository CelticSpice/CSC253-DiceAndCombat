/*
    This class represents a room in the game
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;

namespace Dice_and_Combat_Engine
{
    class Room
    {
        // Fields
        /*
        private int _id;                    Do we need these?
        private string _desc;
        */
        private string _roomName;
        private List<Creature> _denizens;   // Creatures in the room
        private List<Item> _contents;       // Items in the room
        private Room nextRoom, previousRoom;

        /*
            Constructor
            Accepts the room name and the number of creatures and items in the room
        */

        public Room(string name, int numCreatures, int numItems)
        {
            _roomName = name;
            _denizens = new List<Creature>(numCreatures);
            _contents = new List<Item>(numItems);
            nextRoom = null;
            previousRoom = null;
        }

        /*
            The SetNextRoom method sets the next room that this room leads to
        */

        public void SetNextRoom(Room next)
        {
            nextRoom = next;
        }

        /*
            The GetNextRoom method returns the next room that this room leads to
        */

        public Room GetNextRoom()
        {
            return nextRoom;
        }

        /*
            The SetPreviousRoom method sets the previous room that this room leads to
        */

        public void SetPreviousRoom(Room previous)
        {
            previousRoom = previous;
        }

        /*
            The GetPreviousRoom method returns the previous room that this room leads to
        */

        public Room GetPreviousRoom()
        {
            return previousRoom;
        }

        /*
            The ToString method returns a string representation of the object
        */

        public override string ToString()
        {
            string str = "Room: " + _roomName;
                      // "\nDescription: " + _desc;
            return str;
        }

        /***************************************
        *  Should the properties have setters? *
        ****************************************/

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
    }
}
