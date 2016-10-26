/*
    This class represents a room in the game
    10/3/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            The GetItemInformation method returns a string describing
            the number and kind of items in this room
        */

        private string GetItemInformation()
        {
            StringBuilder output = new StringBuilder();
            output.Append("There are " + _contents.Count + " items in this room");
            List<string> items = new List<string>();

            // Get item names
            foreach (Item item in _contents)
                if (!items.Contains(item.Name))
                    items.Add(item.Name);

            // Build output
            if (items.Count > 0)
            {
                output.Append(":");
                for (int i = 0; i < items.Count; i++)
                {
                    int count = _contents.Where(item => item.Name == items[i]).Count();
                    string name = (count > 1) ? items[i] + "s" : items[i];

                    // Format
                    if (items.Count == 1)
                        output.Append("\n-- " + count + " " + name);
                    else if (i < items.Count - 1)
                        output.Append("\n-- " + count + " " + name + ",");
                    else
                        output.Append("\n-- and " + count + " " + name);
                }
            }
            return output.ToString();
        }

        /*
            The GetItem method returns the item with the specified name;
            If no item with that name exists, null is returned
        */

        public Item GetItem(string name)
        {
            return _contents.Find(i => i.Name.ToLower() == name.ToLower());
        }

        /*
            The GetItem method returns the nth instance of the item
            with the specified name; if no item of the specified instance
            or name exists, null is returned
        */

        public Item GetItem(string name, int instance)
        {
            Item[] items = _contents.Where(i => i.Name.ToLower() == name.ToLower()).ToArray();
            if (items.Length > 0 && instance < items.Length)
                return items[instance];
            else
                return null;
        }

        /*
            The GetItems method returns every item in the room
            with the specified name
        */

        public Item[] GetItems(string name)
        {
            return _contents.Where(i => i.Name.ToLower() == name.ToLower()).ToArray();
        }

        /*
            The GetItemNames method returns an array containing the names
            of the Room's items
        */

        public string[] GetItemNames()
        {
            List<string> names = new List<string>();
            foreach (Item i in _contents)
                if (!names.Contains(i.Name))
                    names.Add(i.Name);
            return names.ToArray();
        }

        /*
            The GetDenizen method returns the denizen with the specified name;
            If no denizen with that name exists, null is returned
        */

        public Creature GetDenizen(string name)
        {
            return _denizens.Find(d => d.Stats.Name.ToLower() == name.ToLower());
        }

        /*
            The GetDenizen method returns the nth instance of the denizen
            with the specified name; if no denizen of the specified instance
            or name exists, null is returned
        */

        public Creature GetDenizen(string name, int instance)
        {
            Creature[] denizens = _denizens.Where(d => d.Stats.Name.ToLower() == name.ToLower()).ToArray();
            if (denizens.Length > 0 && instance < denizens.Length)
                return denizens[instance];
            else
                return null;
        }

        /*
            The GetDenizenInformation method returns a string describing
            the number and kind of creatures in this room
        */

        private string GetDenizenInformation()
        {
            StringBuilder output = new StringBuilder();
            output.Append("There are " + _denizens.Count + " creatures in this room");
            List<string> creatures = new List<string>();

            // Get creature names
            foreach (Creature creature in _denizens)
                if (!creatures.Contains(creature.Stats.Name))
                    creatures.Add(creature.Stats.Name);

            // Build output
            if (creatures.Count > 0)
            {
                output.Append(":");
                for (int i = 0; i < creatures.Count; i++)
                {
                    int count = _denizens.Where(creature => creature.Stats.Name == creatures[i]).Count();
                    string name = (count > 1) ? creatures[i] + "s" : creatures[i];

                    // Format
                    if (creatures.Count == 1)
                        output.Append("\n-- " + count + " " + name);
                    else if (i < creatures.Count - 1)
                        output.Append("\n-- " + count + " " + name + ",");
                    else
                        output.Append("\n-- and " + count + " " + name);
                }
            }
            return output.ToString();
        }

        /*
            The GetDenizenNames method returns an array containing the names
            of the Room's denizens
        */

        public string[] GetDenizenNames()
        {
            List<string> names = new List<string>();
            foreach (Creature c in _denizens)
                if (!names.Contains(c.Stats.Name))
                    names.Add(c.Stats.Name);
            return names.ToArray();
        }

        /*
            The GetExitInformation method returns a string describing
            the directions in which exits exist
        */

        private string GetExitInfomation()
        {
            StringBuilder output = new StringBuilder();
            int numExits = _links.Where(exit => exit != null).Count();
            output.Append("There are " + numExits + " exits in this room:");
            int exitNumber = 1;
            for (Direction direction = Direction.North; direction <= Direction.West; direction++)
            {
                if (_links[(int)direction] != null)
                {
                    if (numExits == 1)
                        output.Append("\n-- " + direction.ToString());
                    else if (exitNumber < numExits)
                        output.Append("\n-- " + direction.ToString() + ",");
                    else
                        output.Append("\n-- and " + direction.ToString());
                    exitNumber++;
                }
            }
            return output.ToString();
        }

        /*
            The GetInfo method returns a string describing the number and
            kinds of creatures and items in this room, and the directions
            in which exits exist
        */

        public string GetInfo()
        {
            return GetItemInformation() +
                   "\n" + GetDenizenInformation() +
                   "\n" + GetExitInfomation();
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
            The OpenLink method opens the link to another Room
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
