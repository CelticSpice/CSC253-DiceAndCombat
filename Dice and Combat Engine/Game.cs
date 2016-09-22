/*
    This class handles the game logic
    9/13/2016
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
        private List<Room> dungeon;             // The rooms in the dungeon
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
            LoadDungeonRooms();
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
                    
                    if (!(line.Length == 0))
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
            The LoadDungeonRooms method loads the dungeon rooms
        */

        private void LoadDungeonRooms()
        {
            const string ROOM_RESOURCE = "Dice_and_Combat_Engine.Resources.rooms.txt";

            dungeon = new List<Room>();

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
                                // Read the room's number of creatures
                                numCreatures = int.Parse(splitLine[1]);
                                break;
                            case "NumItems":
                                // Read the room's number of items
                                numItems = int.Parse(splitLine[1]);
                                break;
                        }
                    }
                    else
                    {
                        // Create new room
                        dungeon.Add(new Room(roomName, numCreatures, numItems));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            dungeon.TrimExcess();
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
                    if (creatures[i].Stats.name.CompareTo(creatures[j].Stats.name) == -1)
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
                    if (items[i].Name.CompareTo(items[j].Name) == -1)
                    {
                        Item tmpItem = items[i];
                        items[i] = items[j];
                        items[j] = tmpItem;
                    }
                }
            }

            // Sort Rooms
            for (int i = 0; i < dungeon.Count - 1; i++)
            {
                for (int j = i + 1; j < dungeon.Count; j++)
                {
                    if (dungeon[i].RoomName.CompareTo(dungeon[j].RoomName) == -1)
                    {
                        Room tmpRoom = dungeon[i];
                        dungeon[i] = dungeon[j];
                        dungeon[j] = tmpRoom;
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

            // Generate layout
            Room[] layout = new Room[dungeon.Count];

            List<Room> roomsToShuffle = new List<Room>(dungeon);

            for (int i = 0; i < layout.Length; i++)
            {
                Room toPlace = roomsToShuffle[rng.Next(roomsToShuffle.Count)];
                layout[i] = toPlace;
                roomsToShuffle.Remove(toPlace);
            }

            // Set starting room
            _currentRoom = layout[0];

            // Generate room contents
            for (int i = 0; i < layout.Length; i++)
            {
                Room toGenerate = layout[i];

                // Populate room with items
                int maxItems = toGenerate.Contents.Capacity;

                // Determine whether to place the treasure in the room
                bool treasureAdded = false;

                if (i == (layout.Length - 1))
                {
                    while (!treasureAdded)
                    {
                        Item treasureToAdd = items[rng.Next(items.Count)];

                        if (treasureToAdd is Treasure)
                        {
                            toGenerate.Contents.Add(treasureToAdd);
                            treasureAdded = true;
                        }
                    }
                }

                for (int j = (treasureAdded ? 1 : 0); j < maxItems; j++)
                {
                    Item toAdd = items[rng.Next(items.Count)];

                    // Make sure item added is not the treasure
                    while (toAdd is Treasure)
                    {
                        toAdd = items[rng.Next(items.Count)];
                    }

                    if (toAdd is Weapon)
                    {
                        toGenerate.Contents.Add(new Weapon((Weapon)toAdd));
                    }
                    else if (toAdd is Potion)
                    {
                        toGenerate.Contents.Add(new Potion((Potion)toAdd));
                    }
                }

                // Populate room with creatures
                int maxCreatures = toGenerate.Denizens.Capacity;
                
                for (int j = 0; j < maxCreatures; j++)
                {
                    Creature toAdd = creatures[rng.Next(creatures.Count)];
                    toGenerate.Denizens.Add(new Creature(toAdd));
                }

                // Set room's previous and next room
                if (i == 0)
                {
                    toGenerate.SetPreviousRoom(null);   // Previous room is null because this is the first room
                }                                       // We delay setting next room until next iteration
                else if (i > 0 && i < layout.Length - 1)
                {
                    layout[i - 1].SetNextRoom(toGenerate);
                    toGenerate.SetPreviousRoom(layout[i - 1]);
                }
                else
                {
                    layout[i - 1].SetNextRoom(toGenerate);
                    toGenerate.SetPreviousRoom(layout[i - 1]);
                    toGenerate.SetNextRoom(null);               // Because this is the last room, next room is null
                }
            }
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
