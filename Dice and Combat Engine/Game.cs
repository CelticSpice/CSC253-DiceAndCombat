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
        private CombatEngine combatEngine;      // Handles combat logic
        private CommandParser parser;           // The command parser
        private Creature[] _creatures;           // The creatures in the game
        private Item[] _items;                   // The items in the game
        private Room[] rooms;                   // The rooms in the game
        private RoomGrid dungeon;               // The dungeon
        private Player _player;                 // The player character

        /*
            Constructor
            Accepts an integer representing the size of the dungeon;
            the dungeon will contain that many rows and columns

            The method also accepts an optional boolean indicating whether
            each Room should be unique
        */

        public Game(int dungeonSize, bool uniqueRooms = false)
        {
            combatEngine = new CombatEngine();
            parser = new CommandParser(this);
            LoadCreatures();
            LoadItems();
            LoadRooms();
            dungeon = new RoomGrid(dungeonSize, dungeonSize, rooms, uniqueRooms);
            dungeon.GenerateRoomContents(_creatures, _items);
            GeneratePlayer();
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
            _player.Location = dungeon.Grid[0, 0];
        }

        /*
            The GetDungeonASCII method returns the dungeon's layout in ASCII
            format
        */

        public string GetDungeonASCII()
        {
            return dungeon.ToString();
        }

        /*
            The LoadCreatures method loads the creatures in the game
        */

        private void LoadCreatures()
        {
            // Consts
            const string CREATURE_RESOURCE = "Dice_and_Combat_Engine.Resources.creatures.txt";

            List<Creature> loadList = new List<Creature>();

            Assembly assembly = Assembly.GetExecutingAssembly();

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
                        // Create new creature with collected data
                        loadList.Add(new Creature(creatureStats, creatureAttribs));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            loadList.TrimExcess();

            // Sort list alphabetically
            loadList.Sort((Creature creatureA, Creature creatureB) => creatureA.Stats.name.CompareTo(creatureB.Stats.name));

            // Set array
            _creatures = loadList.ToArray();
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

            List<Item> loadList = new List<Item>();

            Assembly assembly = Assembly.GetExecutingAssembly();

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
                            loadList.Add(new Weapon(itemName, itemDurability, itemValue, damageBonus));
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
                            loadList.Add(new Potion(itemName, itemDurability, itemValue, healthRestored));
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
                            loadList.Add(new Treasure(itemName, itemDurability, itemValue));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            loadList.TrimExcess();

            // Sort list alphabetically
            loadList.Sort((Item a, Item b) => { return a.Name.CompareTo(b.Name); });

            // Set array
            _items = loadList.ToArray();
        }

        /*
            The LoadRooms method loads the dungeon rooms
        */

        private void LoadRooms()
        {
            const string ROOM_RESOURCE = "Dice_and_Combat_Engine.Resources.rooms.txt";

            List<Room> loadList = new List<Room>();

            Assembly assembly = Assembly.GetExecutingAssembly();

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
                    if (!roomStream.EndOfStream && !(line.Length == 0))
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
                        loadList.Add(new Room(roomName, numCreatures, numItems));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            loadList.TrimExcess();

            // Sort list alphabetically
            loadList.Sort((Room a, Room b) => { return a.Name.CompareTo(b.Name); });

            // Set array
            rooms = loadList.ToArray();
        }

        /*
            The ParseCommand method parses a user-input command with the game's command parser

            The method returns the lines of output as an array
        */

        public string[] ParseCommand(string commandString)
        {
            return parser.Parse(commandString);
        }

        /*
            Creatures property
        */

        public Creature[] Creatures
        {
            get { return _creatures; }
        }

        /*
            Player property
        */

        public Player Player
        {
            get { return _player; }
        }
    }
}
