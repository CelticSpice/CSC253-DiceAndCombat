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
        // Static Fields
        private static int numNeighbors = 4;

        // Properties
        private bool _linked;
        private bool[] _linksUnlocked;
        private int _weight, _xLoc, _yLoc;
        private string _roomName;
        private List<Creature> _denizens;
        private List<Item> _contents;
        private Room[] _neighbors, _links;

        /*
            Constructor
            Accepts the room name and the number of creatures and items initially in the room
        */

        public Room(string name, int numCreatures, int numItems)
        {
            _roomName = name;
            _denizens = new List<Creature>(numCreatures);
            _contents = new List<Item>(numItems);
            _linked = false;
            _linksUnlocked = new bool[numNeighbors];
            _weight = -1;
            _xLoc = -1;
            _yLoc = -1;
            _neighbors = new Room[numNeighbors];
            _links = new Room[numNeighbors];
        }

        /*
            Copy Constructor
        */

        public Room(Room room)
        {
            _roomName = room._roomName;
            _denizens = new List<Creature>(room._denizens.Capacity);
            _contents = new List<Item>(room._contents.Capacity);
            _linked = room._linked;
            _weight = room._weight;
            _xLoc = room._xLoc;
            _yLoc = room._yLoc;

            _linksUnlocked = new bool[numNeighbors];
            for (int i = 0; i < numNeighbors; i++)
            {
                _linksUnlocked[i] = room._linksUnlocked[i];
            }

            _neighbors = new Room[numNeighbors];
            for (int i = 0; i < numNeighbors; i++)
            {
                _neighbors[i] = room._neighbors[i];
            }

            _links = new Room[numNeighbors];
            for (int i = 0; i < numNeighbors; i++)
            {
                _links[i] = room._links[i];
            }
        }

        /*
            The GetContentInformation method returns an array containing information
            about the number of and types of specified objects in this room
        */

        public string[] GetContentInformation(string objs)
        {
            // Get names
            List<string> names = new List<string>();

            if (objs == "creatures")
            {
                foreach (Creature creature in _denizens)
                {
                    names.Add(creature.Stats.name);
                }
            }
            else
            {
                foreach (Item item in _contents)
                {
                    names.Add(item.Name);
                }
            }

            // Get unique names
            List<string> uniqueNames = new List<string>();

            // We will check for multiple occurances of a name
            bool appears = false;
            foreach (string name in names)
            {
                foreach (string uniqueName in uniqueNames)
                {
                    if (uniqueName == name)
                    {
                        appears = true;
                    }
                }

                // Only add name to uniqueNames if it does not already appear
                if (!appears)
                {
                    uniqueNames.Add(name);
                }
                else
                {
                    // Reset flag
                    appears = false;
                }
            }
            

            // Prepare array to return
            string[] info = new string[uniqueNames.Count];

            // Build info array
            int numOfObj = 0;
            for (int i = 0; i < info.Length; i++)
            {
                foreach (string name in names)
                {
                    if (name == uniqueNames[i])
                    {
                        numOfObj++;
                    }
                }

                // Add info to array
                if (numOfObj == 1)
                {
                    info[i] = uniqueNames[i];
                }
                else
                {
                    info[i] = numOfObj + " " + uniqueNames[i] + "s";
                }

                // Reset obj counter
                numOfObj = 0;
            }

            // Return
            return info;
        }

        /*
            The Link method links this Room to another Room
        */

        public void Link(Room toLink)
        {
            // Find which direction the Room to link to is in relation
            // to this Room so we can assign to the proper Links elements
            bool linked = false;
            Direction direction = Direction.North;
            while (!linked && direction <= Direction.West)
            {
                if (_neighbors[(int)direction] == toLink)
                {
                    _links[(int)direction] = toLink;
                    
                    // We link bidirectionally
                    if (direction == Direction.North || direction == Direction.East)
                    {
                        toLink._links[(int)direction + 1] = this;
                    }
                    else
                    {
                        toLink._links[(int)direction - 1] = this;
                    }

                    // Rooms are now linked
                    linked = true;
                    _linked = true;
                    toLink._linked = true;
                }
                else
                {
                    direction++;
                }
            }
        }

        /*
            The OpenLink method opens the direction to another Room
        */

        public void OpenLink(Room link)
        {
            // Find which direction the linked Room is in relation
            // to this Room so we can assign to the proper elements
            bool found = false;
            Direction direction = Direction.North;
            while (!found && direction <= Direction.West)
            {
                if (_links[(int)direction] == link)
                {
                    found = true;

                    _linksUnlocked[(int)direction] = true;

                    // We open the links bidirectionally
                    if (direction == Direction.North || direction == Direction.East)
                    {
                        link._linksUnlocked[(int)direction + 1] = true;
                    }
                    else
                    {
                        link._linksUnlocked[(int)direction - 1] = true;
                    }

                    // Rooms links are now opened
                }
                else
                {
                    direction++;
                }
            }
        }

        /*
            LinksUnlocked property
        */

        public bool[] LinksUnlocked
        {
            get { return _linksUnlocked; }
        }

        /*
            Linked property
        */

        public bool Linked
        {
            get { return _linked; }
            set { _linked = value; }
        }

        /*
            Weight property
        */

        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        /*
            XLoc property
        */
        
        public int XLoc
        {
            get { return _xLoc; }
            set { _xLoc = value; }
        }

        /*
            YLoc property
        */

        public int YLoc
        {
            get { return _yLoc; }
            set { _yLoc = value; }
        }

        /*
            RoomName property
        */

        public string RoomName
        {
            get { return _roomName; }
            set { _roomName = value; }
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
            Neighbors property
        */

        public Room[] Neighbors
        {
            get { return _neighbors; }
        }

        /*
            Links property
        */

        public Room[] Links
        {
            get { return _links; }
        }
    }
}
