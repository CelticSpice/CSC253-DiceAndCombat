/*
    This class represents a room in the game
    10/3/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
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
        private string _name;
        private List<Creature> _denizens;
        private List<Item> _contents;
        private Room[] _neighbors, _links;

        /*
            Constructor
            Accepts the room name and the number of creatures and items initially in the room
        */

        public Room(string name, int numCreatures, int numItems)
        {
            _name = name;
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
            _name = room._name;
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
            about the number and kind of objects in this room
        */

        public string[] GetContentInformation(string objs)
        {
            List<string> info = new List<string>();

            // Determine whether to get creature or item info
            if (objs == "creatures")
            {
                // Get creature names
                foreach (Creature creature in _denizens)
                {
                    if (!info.Contains(creature.Stats.name))
                    {
                        info.Add(creature.Stats.name);
                    }
                }

                // Build info
                int count = 0;
                for (int i = 0; i < info.Count; i++)
                {
                    count = 0;

                    foreach (Creature creature in _denizens)
                    {
                        if (creature.Stats.name == info[i])
                        {
                            count++;
                        }
                    }

                    if (count > 1)
                    {
                        info[i] = count + " " + info[i] + "s";
                    }
                }
            }
            else if (objs == "items")
            {
                // Get item names
                foreach (Item item in _contents)
                {
                    if (!info.Contains(item.Name))
                    {
                        info.Add(item.Name);
                    }
                }

                // Build info
                int count = 0;
                for (int i = 0; i < info.Count; i++)
                {
                    count = 0;

                    foreach (Item item in _contents)
                    {
                        if (item.Name == info[i])
                        {
                            count++;
                        }
                    }

                    if (count > 1)
                    {
                        info[i] = count + " " + info[i] + "s";
                    }
                }
            }

            // Return
            return info.ToArray();
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
            Name property
        */

        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
