/*
    This class is a wrapper for a 2D grid of Rooms
    10/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dice_and_Combat_Engine
{
    class RoomGrid
    {
        private Random rng;
        private Room[,] _grid;
        private int rows, columns;

        /*
            Constructor
            Accepts the number of rows and columns that the grid should have,
            and an array containing the Rooms that the Grid should contain
        */

        public RoomGrid(int size, Room[] rooms)
        {
            if (rooms.Length == (size * size))
            {
                rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                _grid = new Room[size, size];
                rows = size;
                columns = size;

                PrepareGrid(rooms);
                SetNeighbors();
                LinkRooms();
            }
            else
                throw new Exception("Error: Grid size inappropriate for number of rooms");
        }

        /*
            The PrepareGrid method initializes the Grid with Rooms
        */

        private void PrepareGrid(Room[] rooms)
        {
            List<Room> toCreate = new List<Room>(rooms);
            Room room;

            for (int row = 0; row < rows; row++)
                for (int col = 0; col < columns; col++)
                {
                    room = toCreate[rng.Next(toCreate.Count)];
                    _grid[row, col] = room;
                    room.XLoc = col;
                    room.YLoc = row;
                    toCreate.Remove(room);
                }
        }

        /*
            The SetNeighbors method sets the neighboring Rooms for each Room in the Grid
        */

        private void SetNeighbors()
        {
            foreach (Room room in _grid)
            {
                // Get Room's x,y coordinates
                int xLoc = room.XLoc;
                int yLoc = room.YLoc;

                // Set neighbors
                for (Direction direction = Direction.North; direction <= Direction.West; direction++)
                    // Direction represents one of 4 directions: North, South, East, West, respectively
                    // If no neighbor exists in a direction, neighbor in that direction will be left null
                    switch (direction)
                    {
                        case Direction.North:
                            if (yLoc > 0)
                                room.Neighbors[(int)Direction.North] = _grid[yLoc - 1, xLoc];
                            break;
                        case Direction.South:
                            if (yLoc < rows - 1)
                                room.Neighbors[(int)Direction.South] = _grid[yLoc + 1, xLoc];
                            break;
                        case Direction.East:
                            if (xLoc < columns - 1)
                                room.Neighbors[(int)Direction.East] = _grid[yLoc, xLoc + 1];
                            break;
                        case Direction.West:
                            if (xLoc > 0)
                                room.Neighbors[(int)Direction.West] = _grid[yLoc, xLoc - 1];
                            break;
                    }
            }
        }

        /*
            The LinkRooms method links the rooms of the Grid using Prim's algorithm

            Reference: https://en.wikipedia.org/wiki/Prim%27s_algorithm
        */

        private void LinkRooms()
        {
            // Initialize rooms with random weights
            foreach (Room room in _grid)
                room.Weight = rng.Next(100);

            // Initialize list of active rooms to keep track of those rooms which may contain valid neighbors
            List<Room> active = new List<Room>();
            active.Add(_grid[rng.Next(rows), rng.Next(columns)]);
            active[0].Linked = true;

            // Prepare variables
            Room activeRoom, neighbor;
            List<Room> availableNeighbors = new List<Room>();

            // Begin
            while (active.Count != 0)
            {
                // Get active room with lowest weight
                activeRoom = active[0];
                foreach (Room room in active)
                    if (room.Weight < activeRoom.Weight)
                        activeRoom = room;

                // Get neighbors available to link to
                foreach (Room room in activeRoom.Neighbors)
                    if (room != null && !room.Linked)
                        availableNeighbors.Add(room);

                // If there are neighbors available, get the one with the lowest weight to link to
                if (availableNeighbors.Count != 0)
                {
                    neighbor = availableNeighbors[0];
                    foreach (Room room in availableNeighbors)
                        if (room.Weight < neighbor.Weight)
                            neighbor = room;

                    // Link the active room and its neighbor together
                    activeRoom.Link(neighbor);

                    // Add neighbor to active list
                    active.Add(neighbor);
                }
                // If there are no neighbors to link to from the active room, remove it from the active list
                else
                    active.Remove(activeRoom);

                // Clear list of available neighbors
                availableNeighbors.Clear();                
            }
        }

        /*
            The GenerateRoomContents method randomly populates the Rooms in the Grid with objects
            made avaiable in passed arrays of creatures and items
        */

        public void GenerateRoomContents(Creature[] creatures, Item[] items)
        {
            int maxItems = 5;
            int maxCreatures = 5;

            foreach (Room room in _grid)
            {
                // Populate room with items
                int numItems = rng.Next(maxItems + 1);

                for (int i = 0; i < numItems; i++)
                {
                    Item itemToAdd = items[rng.Next(items.Length)];

                    if (itemToAdd is Weapon)
                        room.Contents.Add(new Weapon((Weapon)itemToAdd));
                    else if (itemToAdd is Potion)
                        room.Contents.Add(new Potion((Potion)itemToAdd));
                    else if (itemToAdd is Treasure)
                        room.Contents.Add(new Treasure((Treasure)itemToAdd));
                    else
                    {
                        Container container = new Container((Container)itemToAdd);
                        Item[] loot = items.Where(item => !(item is Container)).ToArray();
                        for (int j = 0; j < container.Items.Length; j++)
                        {
                            Item lootToAdd = loot[rng.Next(loot.Length)];

                            if (lootToAdd is Weapon)
                                container.AddItem(new Weapon((Weapon)lootToAdd));
                            else if (lootToAdd is Potion)
                                container.AddItem(new Potion((Potion)lootToAdd));
                            else
                                container.AddItem(new Treasure((Treasure)lootToAdd));
                        }
                        room.Contents.Add(container);
                    }
                }

                // Populate room with creatures
                int numCreatures = rng.Next(maxCreatures + 1);

                for (int i = 0; i < numCreatures; i++)
                {
                    Creature creatureToAdd = creatures[rng.Next(creatures.Length)];
                    if (creatureToAdd is NPC)
                        room.Denizens.Add(new NPC((NPC)creatureToAdd));
                    else
                        room.Denizens.Add(new Creature(creatureToAdd));
                    creatureToAdd.Location = room;
                }
                    
                // Sort room contents alphabetically
                room.Denizens.Sort((Creature a, Creature b) => { return a.Stats.Name.CompareTo(b.Stats.Name); });
                room.Contents.Sort((Item a, Item b) => { return a.Name.CompareTo(b.Name); });
            }
        }

        /*
            The ToString method returns a string representation of the Grid.
            More specifically, it will return an ASCII map of the dungeon
        */

        public override string ToString()
        {
            string body   = "   ";
            string corner = "+";

            string output = "+" + String.Concat(Enumerable.Repeat("---+", columns)) + "\n";

            for (int row = 0; row < rows; row++)
            {
                string top    = "|";
                string bottom = "+";

                for (int col = 0; col < columns; col++)
                {
                    string east  = (_grid[row, col].Links[(int)Direction.East] != null) ? " " : "|";
                    string south = (_grid[row, col].Links[(int)Direction.South] != null) ? "   " : "---";

                    top    += body + east;
                    bottom += south + corner;
                }

                output += top + "\n";
                output += bottom + "\n";
            }

            return output;
        }

        /*
            Grid property
        */

        public Room[,] Grid
        {
            get { return _grid; }
        }
    }
}
