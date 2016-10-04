/*
    This class handles the game logic
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Dice_and_Combat_Engine
{
    class Game
    {
        // Fields
        private Assembly assembly;
        private CombatEngine _combatEngine;     // Handles combat logic
        private List<Creature> creatures;       // The creatures in the game
        private List<Item> items;               // The items in the game
        private List<Room> rooms;               // The rooms in the dungeon
        private Player _player;                 // The player character
        private Room _currentRoom;              // The current room

        /*
            Constructor
        */

        public Game()
        {
            assembly = Assembly.GetExecutingAssembly();
            _combatEngine = new CombatEngine();
            LoadCreatures();
            LoadItems();
            LoadRooms();
            SortLists();
            GeneratePlayer();
            GenerateDungeon();
        }

        /*
            The LoadCreatures method loads the creatures in the game
        */

        private void LoadCreatures()
        {
            // Consts
            const string CREATURE_RESOURCE = "Dice_and_Combat_Engine.Resources.creatures.txt";
            const string CREATURE_IMG_DIRECTORY = "Dice_and_Combat_Engine.Resources.";

            creatures = new List<Creature>();

            try
            {
                // Initialize stream
                StreamReader creatureStream = new StreamReader(assembly.GetManifestResourceStream(CREATURE_RESOURCE));

                // Delimiter
                char[] delim = { ':' };

                // Prepare new creature
                BaseStats creatureStats = new BaseStats();
                Attributes creatureAttribs = new Attributes();

                while (!creatureStream.EndOfStream)
                {
                    string line = creatureStream.ReadLine();

                    if (!(line.Length == 0 || creatureStream.EndOfStream))
                    {
                        string[] splitLine = line.Split(delim);

                        // Switch on the data to read
                        switch (splitLine[0])
                        {
                            // Stats
                            case "Name":
                                // Read creature's name
                                creatureStats.name = splitLine[1].Trim();
                                break;
                            case "Friendly":
                                // Read creature's friendly status
                                creatureStats.friendly = bool.Parse(splitLine[1]);
                                break;
                            case "HP":
                                // Read creature's HP
                                creatureStats.hitPoints = int.Parse(splitLine[1]);

                                // Set max HP as well
                                creatureStats.maxHitPoints = int.Parse(splitLine[1]);
                                break;
                            case "AB":
                                // Read creature's AB
                                creatureStats.attackBonus = int.Parse(splitLine[1]);
                                break;
                            case "AC":
                                // Read creature's AC
                                creatureStats.armorClass = int.Parse(splitLine[1]);
                                break;
                            case "Damage":
                                // Read creature's damage
                                creatureStats.damage = new RandomDie(int.Parse(splitLine[1]));
                                break;
                            case "XP":
                                // Read creature's XP value
                                creatureStats.xpValue = int.Parse(splitLine[1]);
                                break;

                            // Attributes
                            case "Strength":
                                // Read creature's strength
                                creatureAttribs.strength = int.Parse(splitLine[1]);
                                break;
                            case "Constitution":
                                // Read creature's constitution
                                creatureAttribs.constitution = int.Parse(splitLine[1]);
                                break;
                            case "Dexterity":
                                // Read creature's dexterity
                                creatureAttribs.dexterity = int.Parse(splitLine[1]);
                                break;
                            case "Intelligence":
                                // Read creature's intelligence
                                creatureAttribs.intelligence = int.Parse(splitLine[1]);
                                break;
                            case "Wisdom":
                                // Read creature's wisdom
                                creatureAttribs.wisdom = int.Parse(splitLine[1]);
                                break;
                            case "Charisma":
                                // Read creature's charisma
                                creatureAttribs.charisma = int.Parse(splitLine[1]);
                                break;
                        }
                    }
                    else
                    {
                        // Get new creature's image
                        Stream imageStream = assembly.GetManifestResourceStream(CREATURE_IMG_DIRECTORY +
                                                      creatureStats.name.ToLower() + ".jpg");
                        Image creatureImage = Image.FromStream(imageStream);

                        // Create new creature with collected data
                        creatures.Add(new Creature(creatureStats, creatureAttribs, creatureImage));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            creatures.TrimExcess();
        }

        /*
            The LoadItems method loads the items in the game
        */

        private void LoadItems()
        {
            const string ITEM_RESOURCE = "Dice_and_Combat_Engine.Resources.items.txt";
            const int WEAPON = 1;
            const int POTION = 2;
            const int TREASURE = 3;

            items = new List<Item>();

            try
            {
                // Initialize stream
                StreamReader itemStream = new StreamReader(assembly.GetManifestResourceStream(ITEM_RESOURCE));

                // Delimiter
                char[] delim = { ':' };

                // Prepare new item
                string itemName = "";

                int itemValue = 0,
                    itemDurability = 0;

                while (!itemStream.EndOfStream)
                {
                    string line = itemStream.ReadLine();

                    // Check for empty string delimiter
                    if (line.Length == 0)
                    {
                        // Consume empty string and get next line
                        line = itemStream.ReadLine();
                    }

                    string[] splitLine = line.Split(delim);

                    // Get the item's id
                    int id = int.Parse(splitLine[1]);

                    // Switch on the item's id
                    switch (id)
                    {
                        case WEAPON:
                            // Item is weapon
                            int damageBonus = 0;

                            while (!splitLine[0].Equals("DamageBonus"))
                            {
                                line = itemStream.ReadLine();
                                splitLine = line.Split(delim);

                                switch (splitLine[0])
                                {
                                    case "Name":
                                        itemName = splitLine[1].Trim();
                                        break;
                                    case "Durability":
                                        itemDurability = int.Parse(splitLine[1]);
                                        break;
                                    case "Value":
                                        itemValue = int.Parse(splitLine[1]);
                                        break;
                                    case "DamageBonus":
                                        damageBonus = int.Parse(splitLine[1]);
                                        break;
                                }
                            }

                            // Create weapon
                            items.Add(new Weapon(itemName, itemDurability, itemValue, damageBonus));
                            break;
                        case POTION:
                            // Item is potion
                            int healthRestored = 0;

                            while (!splitLine[0].Equals("HealthRestored"))
                            {
                                line = itemStream.ReadLine();
                                splitLine = line.Split(delim);

                                switch (splitLine[0])
                                {
                                    case "Name":
                                        itemName = splitLine[1].Trim();
                                        break;
                                    case "Durability":
                                        itemDurability = int.Parse(splitLine[1]);
                                        break;
                                    case "Value":
                                        itemValue = int.Parse(splitLine[1]);
                                        break;
                                    case "HealthRestored":
                                        healthRestored = int.Parse(splitLine[1]);
                                        break;
                                }
                            }

                            // Create potion
                            items.Add(new Potion(itemName, itemDurability, itemValue, healthRestored));
                            break;
                        case TREASURE:
                            // Item is treasure
                            while (!splitLine[0].Equals("Value"))
                            {
                                line = itemStream.ReadLine();
                                splitLine = line.Split(delim);

                                switch (splitLine[0])
                                {
                                    case "Name":
                                        itemName = splitLine[1].Trim();
                                        break;
                                    case "Durability":
                                        itemDurability = int.Parse(splitLine[1]);
                                        break;
                                    case "Value":
                                        itemValue = int.Parse(splitLine[1]);
                                        break;
                                }
                            }

                            // Create treasure
                            items.Add(new Treasure(itemName, itemDurability, itemValue));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            items.TrimExcess();
        }

        /*
            The LoadRooms method loads the dungeon rooms
        */

        private void LoadRooms()
        {
            const string ROOM_RESOURCE = "Dice_and_Combat_Engine.Resources.rooms.txt";

            rooms = new List<Room>();

            try
            {
                // Initialize stream
                StreamReader roomStream = new StreamReader(assembly.GetManifestResourceStream(ROOM_RESOURCE));

                // Delimiter
                char[] delim = { ':' };

                // Prepare new room
                string roomName = "";

                int numCreatures = 0,
                    numItems = 0;

                while (!roomStream.EndOfStream)
                {
                    string line = roomStream.ReadLine();

                    // Check for empty string delimiter
                    if (!(line.Length == 0))
                    {
                        string[] splitLine = line.Split(delim);

                        switch (splitLine[0])
                        {
                            case "Name":
                                // Read the room's name
                                roomName = splitLine[1];
                                break;
                            case "NumCreatures":
                                // Read the room's initial number of creatures
                                numCreatures = int.Parse(splitLine[1]);
                                break;
                            case "NumItems":
                                // Read the room's initial number of items
                                numItems = int.Parse(splitLine[1]);
                                break;
                        }
                    }
                    else
                    {
                        // Create new room
                        rooms.Add(new Room(roomName, numCreatures, numItems));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            rooms.TrimExcess();
        }

        /*
            The SortLists method sorts the List fields alphabetically
        */

        private void SortLists()
        {
            // Sort Creatures
            for (int i = 0; i < creatures.Count - 1; i++)
            {
                for (int j = i + 1; j < creatures.Count; j++)
                {
                    if (creatures[i].Stats.name.CompareTo(creatures[j].Stats.name) == 1)
                    {
                        Creature tmpCreature = creatures[i];
                        creatures[i] = creatures[j];
                        creatures[j] = tmpCreature;
                    }
                }
            }

            // Sort Items
            for (int i = 0; i < items.Count - 1; i++)
            {
                for (int j = i + 1; j < items.Count; j++)
                {
                    if (items[i].Name.CompareTo(items[j].Name) == 1)
                    {
                        Item tmpItem = items[i];
                        items[i] = items[j];
                        items[j] = tmpItem;
                    }
                }
            }

            // Sort Rooms
            for (int i = 0; i < rooms.Count - 1; i++)
            {
                for (int j = i + 1; j < rooms.Count; j++)
                {
                    if (rooms[i].RoomName.CompareTo(rooms[j].RoomName) == 1)
                    {
                        Room tmpRoom = rooms[i];
                        rooms[i] = rooms[j];
                        rooms[j] = tmpRoom;
                    }
                }
            }
        }

        /*
            The GeneratePlayer method generates the player character
        */

        private void GeneratePlayer()
        {
            BaseStats playerBaseStats = new BaseStats();
            Attributes playerAttributes = new Attributes();
            PlayerStats playerStats = new PlayerStats();

            /*
                Temp values to work with!
            */

            playerBaseStats.name = "Bobert";
            playerBaseStats.hitPoints = 30;
            playerBaseStats.maxHitPoints = 30;
            playerBaseStats.attackBonus = 5;
            playerBaseStats.armorClass = 12;
            playerBaseStats.damage = new RandomDie();
            playerBaseStats.xpValue = 100;
            playerBaseStats.friendly = true;

            playerAttributes.strength = 5;
            playerAttributes.constitution = 5;
            playerAttributes.dexterity = 5;
            playerAttributes.intelligence = 5;
            playerAttributes.wisdom = 5;
            playerAttributes.charisma = 5;

            playerStats.playerClass = "Fighter";
            playerStats.race = "Human";
            playerStats.experience = 0;
            playerStats.level = 1;

            _player = new Player(playerStats, playerBaseStats, playerAttributes);
        }

        /*
            The GenerateDungeon method generates the layout and contents of the dungeon
        */

        private void GenerateDungeon()
        {
            Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

            // Get 2D field
            Room[,] field = Generate2DField();

            // Link Rooms using Prim's algorithm
            LinkNodes(field);

            // Set starting room
            const int ROWS = 5;
            const int COLS = 5;
            _currentRoom = field[rng.Next(ROWS), rng.Next(COLS)];

            // Generate room contents
            Item itemToAdd;
            Creature creatureToAdd;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    // Populate room with items
                    int maxItems = field[row, col].Contents.Capacity;

                    for (int i = 0; i < maxItems; i++)
                    {
                        itemToAdd = items[rng.Next(items.Count)];

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
                        creatureToAdd = creatures[rng.Next(creatures.Count)];
                        field[row, col].Denizens.Add(new Creature(creatureToAdd));
                    }
                }
            }
        }

        /*
            The Generate2DField method randomly allocates nodes to a 5x5 two-dimensional array
            The method returns a 2D array of the field
        */

        private Room[,] Generate2DField()
        {
            Random rng = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            List<Room> toAllocate = new List<Room>(rooms);

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

        private void LinkNodes(Room[,] nodes)
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
            nodesToLink.RemoveAt(i);

            // Prepare variables
            List<Room> adjacentNodes = new List<Room>();

            // Begin
            while (nodesToLink.Count != 0)
            {
                // Get adjacent nodes not yet linked
                foreach(Room node in tree)
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

                // Add toLink to the tree, and remove from the list of nodes yet to be linked
                tree.Add(toLink);
                nodesToLink.Remove(toLink);
            }
        }

        /*
            The GetAdjacentNodes method returns an array containing the nodes in a 5x5 2D array that are
            adjacent to the node passed as an argument
            The array will be ordered such that element 0 is North, element 1 is South, element 2 is East,
            and element 3 is West
        */

        private Room[] GetAdjacentNodes(Room node, Room[,] nodes)
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
            CombatEngine property
        */

        public CombatEngine CombatEngine
        {
           get { return _combatEngine; }
        }

        /*
            Player property
        */

        public Player Player
        {
            get { return _player; }
        }

        /*
            CurrentRoom property
        */

        public Room CurrentRoom
        {
            get { return _currentRoom; }
            set { _currentRoom = value; }
        }
    }
}
