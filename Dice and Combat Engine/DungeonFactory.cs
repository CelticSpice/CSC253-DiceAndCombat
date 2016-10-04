/*
    This class provides static methods for generating dungeon layout and contents
    10/4/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;

namespace Dice_and_Combat_Engine
{
    class DungeonFactory
    {
        /*
            The GenerateDungeon method links variable objects together to form
            a dungeon data structure.
            The method accepts three arrays specifying Creatures, Items, and Rooms
            that might appear in the dungeon (all Room objects will appear).

            The method will return a reference to the starting Room in the dungeon
        */

        public static Room GenerateDungeon(Creature[] creatures, Item[] items, Room[] rooms)
        {
            Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            Room[,] field = Generate2DField(rooms);
            LinkNodes(field);
            GenerateContents(field, creatures, items);
            const int ROWS = 5;
            const int COLS = 5;
            return field[rng.Next(ROWS), rng.Next(COLS)];
        }

        /*
            The Generate2DField method randomly allocates an array of nodes passed
            as an argument to a 5x5 two-dimensional array.
            The method returns a 2D array of the field
        */

        private static Room[,] Generate2DField(Room[] nodes)
        {
            Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            List<Room> toAllocate = new List<Room>(nodes);

            const int ROWS = 5;
            const int COLS = 5;
            Room[,] field = new Room[ROWS, COLS];

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    Room allocated = toAllocate[rng.Next(toAllocate.Count)];
                    field[row, col] = allocated;
                    allocated.XLoc = row;
                    allocated.YLoc = col;
                    toAllocate.Remove(allocated);
                }
            }

            return field;
        }

        /*
            The LinkNodes method links the nodes of a 5x5 2D array using Prim's algorithm

            Reference: https://en.wikipedia.org/wiki/Prim%27s_algorithm
        */

        private static void LinkNodes(Room[,] nodes)
        {
            Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            // Initialize nodes with random weights
            foreach (Room node in nodes)
            {
                node.Weight = rng.Next(100) + 1;
            }

            // Initialize list of nodes not yet linked
            List<Room> nodesToLink = new List<Room>();
            const int ROWS = 5;
            const int COLS = 5;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    nodesToLink.Add(nodes[row, col]);
                }
            }

            // Initialize tree
            List<Room> tree = new List<Room>();
            int i = rng.Next(nodesToLink.Count);
            tree.Add(nodesToLink[i]);
            nodesToLink[i].Linked = true;
            nodesToLink.RemoveAt(i);

            // Prepare variables
            List<Room> adjacentNodes = new List<Room>();

            // Begin
            while (nodesToLink.Count != 0)
            {
                // Get adjacent nodes not yet linked
                foreach (Room node in tree)
                {
                    Room[] nodesAdjacentTo = GetAdjacentNodes(node, nodes);
                    foreach (Room adjacentNode in nodesAdjacentTo)
                    {
                        if (!(adjacentNode == null) && !adjacentNode.Linked)
                        {
                            adjacentNodes.Add(adjacentNode);
                        }
                    }
                }

                // Find adjacent node with lowest weight
                Room toLink = adjacentNodes[0];
                for (i = 1; i < adjacentNodes.Count; i++)
                {
                    if (adjacentNodes[i].Weight < toLink.Weight)
                    {
                        toLink = adjacentNodes[i];
                    }
                }

                // Select neighbor to link to adjacent node
                Room[] neighbors = GetAdjacentNodes(toLink, nodes);
                Room linker = null;
                do
                {
                    i = rng.Next(neighbors.Length);
                    if (neighbors[i] != null && neighbors[i].Linked)
                    {
                        linker = neighbors[i];
                    }
                } while (linker == null);

                // The index of the element in neighbors tells us whether linker is to the north,
                // south, east, or west (0, 1, 2, 3)
                toLink.Exits[i] = linker;

                // We also specify the exit for linker, which will be the opposite of toLink's direction
                if (i == (int)Direction.North)
                {
                    linker.Exits[(int)Direction.South] = toLink;
                }
                else if (i == (int)Direction.South)
                {
                    linker.Exits[(int)Direction.North] = toLink;
                }
                else if (i == (int)Direction.East)
                {
                    linker.Exits[(int)Direction.West] = toLink;
                }
                else
                {
                    linker.Exits[(int)Direction.East] = toLink;
                }

                // Add toLink to the tree, set it as linked, and remove from the list of nodes yet to be linked
                tree.Add(toLink);
                toLink.Linked = true;
                nodesToLink.Remove(toLink);
            }
        }

        /*
            The GetAdjacentNodes method returns an array containing the nodes in a 5x5 2D array that are
            adjacent to the node passed as an argument
            The array will be ordered such that element 0 is North, element 1 is South, element 2 is East,
            and element 3 is West
        */

        private static Room[] GetAdjacentNodes(Room node, Room[,] nodes)
        {
            const int NUM_NEIGHBORS = 4;
            Room[] adjacentNodes = new Room[NUM_NEIGHBORS];

            int xLoc = node.XLoc;
            int yLoc = node.YLoc;
            int i = 0;
            while (i < NUM_NEIGHBORS)
            {
                // i represents one of 4 directions: North, South, East, West, respectively
                // If no neighbor exists, element at index i will be left null
                switch (i)
                {
                    case (int)Direction.North:
                        if (yLoc > 0)
                        {
                            adjacentNodes[i] = nodes[xLoc, yLoc - 1];
                        }
                        i++;
                        break;
                    case (int)Direction.South:
                        if (yLoc < 4)
                        {
                            adjacentNodes[i] = nodes[xLoc, yLoc + 1];
                        }
                        i++;
                        break;
                    case (int)Direction.East:
                        if (xLoc < 4)
                        {
                            adjacentNodes[i] = nodes[xLoc + 1, yLoc];
                        }
                        i++;
                        break;
                    case (int)Direction.West:
                        if (xLoc > 0)
                        {
                            adjacentNodes[i] = nodes[xLoc - 1, yLoc];
                        }
                        i++;
                        break;
                }
            }

            return adjacentNodes;
        }

        /*
            The GenerateContents method generates the contents of each Room in a 2D field
        */

        private static void GenerateContents(Room[,] field, Creature[] creatures, Item[] items)
        {
            Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            // Generate room contents
            Item itemToAdd;
            Creature creatureToAdd;
            const int ROWS = 5;
            const int COLS = 5;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    // Populate room with items
                    int maxItems = field[row, col].Contents.Capacity;

                    for (int i = 0; i < maxItems; i++)
                    {
                        itemToAdd = items[rng.Next(items.Length)];

                        if (itemToAdd is Weapon)
                        {
                            field[row, col].Contents.Add(new Weapon((Weapon)itemToAdd));
                        }
                        else if (itemToAdd is Potion)
                        {
                            field[row, col].Contents.Add(new Potion((Potion)itemToAdd));
                        }
                    }

                    // Populate room with creatures
                    int maxCreatures = field[row, col].Denizens.Capacity;

                    for (int i = 0; i < maxCreatures; i++)
                    {
                        creatureToAdd = creatures[rng.Next(creatures.Length)];
                        field[row, col].Denizens.Add(new Creature(creatureToAdd));
                    }
                }
            }
        }
    }
}
