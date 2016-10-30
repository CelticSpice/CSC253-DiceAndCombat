/*
    This class represents a room
    10/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_and_Combat_Engine
{
    class Room
    {
        // Fields
        private static int numNeighbors = 4;

        private bool _linked;
        private bool[] _linksUnlocked;
        private int _weight, _xLoc, _yLoc;
        private string _name;
        private List<Creature> _denizens;
        private List<Item> _contents;
        private Room[] _neighbors, _links;

        /*
            Constructor
            Accepts the room name
        */

        public Room(string name)
        {
            _name = name;
            _denizens = new List<Creature>();
            _contents = new List<Item>();
            _linked = false;
            _linksUnlocked = new bool[numNeighbors];
            _weight = 0;
            _xLoc = 0;
            _yLoc = 0;
            _neighbors = new Room[numNeighbors];
            _links = new Room[numNeighbors];
        }

        /*
            Copy Constructor
        */

        public Room(Room room)
        {
            _name = room._name;
            _denizens = new List<Creature>(room._denizens);
            _contents = new List<Item>(room._contents);
            _linked = room._linked;
            _weight = room._weight;
            _xLoc = room._xLoc;
            _yLoc = room._yLoc;

            _linksUnlocked = new bool[numNeighbors];
            for (int i = 0; i < numNeighbors; i++)
                _linksUnlocked[i] = room._linksUnlocked[i];

            _neighbors = new Room[numNeighbors];
            for (int i = 0; i < numNeighbors; i++)
                _neighbors[i] = room._neighbors[i];

            _links = new Room[numNeighbors];
            for (int i = 0; i < numNeighbors; i++)
                _links[i] = room._links[i];
        }

        /*
            The ContainsDenizen method returns whether the Room
            contains the named denizen
        */

        public bool ContainsDenizen(string name)
        {
            return _denizens.Exists(c => c.Stats.Name.ToLower() == name);
        }

        /*
            The ContainsDenizen method returns whether the Room
            contains a nth instance of the named denizen
        */

        public bool ContainsDenizen(string name, int instance)
        {
            return _denizens.Where(c => c.Stats.Name.ToLower() == name).Count() > instance;
        }

        /*
            The ContainsItem method returns whether the Room
            contains the named item
        */

        public bool ContainsItem(string name)
        {
            return _contents.Exists(i => i.Name.ToLower() == name);
        }

        /*
            The ContainsItem method returns whether the Room
            contains a nth instance of the named item
        */

        public bool ContainsItem(string name, int instance)
        {
            return _contents.Where(i => i.Name.ToLower() == name).Count() > instance;
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
            The GetItem method returns the item with the specified name
        */

        public Item GetItem(string name)
        {
            return _contents.Find(i => i.Name.ToLower() == name.ToLower());
        }

        /*
            The GetItem method returns the nth instance of the item
            with the specified name
        */

        public Item GetItem(string name, int instance)
        {
            Item[] items = _contents.Where(i => i.Name.ToLower() == name.ToLower()).ToArray();
            return items[instance];
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
            The GetDenizen method returns the denizen with the specified name;
        */

        public Creature GetDenizen(string name)
        {
            return _denizens.Find(d => d.Stats.Name.ToLower() == name.ToLower());
        }

        /*
            The GetDenizen method returns the nth instance of the denizen
            with the specified name
        */

        public Creature GetDenizen(string name, int instance)
        {
            Creature[] denizens = _denizens.Where(d => d.Stats.Name.ToLower() == name.ToLower()).ToArray();
            return denizens[instance];
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
            The IsCleared method returns whether the Room
            has been or is clear of enemies
        */

        public bool IsCleared()
        {
            return _denizens.Where(c => c.Stats.Friendly == false).Count() == 0;
        }

        /*
            The IsLinked method returns whether this Room is linked
            with another Room in the specified direction
        */

        public bool IsLinked(Direction direction)
        {
            return _links[(int)direction] != null;
        }

        /*
            The IsLinkOpen method returns whether the link in the
            specified direction is open
        */

        public bool IsLinkOpen(Direction direction)
        {
            return _linksUnlocked[(int)direction];
        }

        /*
            The Link method links this Room to another Room
        */

        public void Link(Room toLink)
        {
            // Get direction room is in
            bool set = false;
            Direction direction = Direction.North;
            for (Direction i = Direction.North; i <= Direction.West && !set; i++)
                if (_neighbors[(int)i] == toLink)
                {
                    direction = i;
                    set = true;
                }

            // We link bidirectionally
            _links[(int)direction] = toLink;
            if (direction == Direction.North || direction == Direction.East)
                toLink._links[(int)direction + 1] = this;
            else
                toLink._links[(int)direction - 1] = this;
            _linked = true;
            toLink._linked = true;
        }

        /*
            The OpenLink method opens the link to another Room
            in the specified direction
        */

        public void OpenLink(Direction direction)
        {
            // We open the links bidirectionally
            _linksUnlocked[(int)direction] = true;
            if (direction == Direction.North || direction == Direction.East)
                _links[(int)direction]._linksUnlocked[(int)direction + 1] = true;
            else
                _links[(int)direction]._linksUnlocked[(int)direction - 1] = true;
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
