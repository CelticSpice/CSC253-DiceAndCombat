/*
    This class handles the game logic
    10/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Dice_and_Combat_Engine
{
    class Game
    {
        // Fields
        private bool _requestToClear;           // Whether a request was made to clear
        private bool _requestToQuit;            // Whether a request was made to quit
        private CombatEngine _combatEngine;     // Handles combat logic
        private CommandParser parser;           // The command parser
        private Creature[] creatures;           // The creatures in the game
        private Item[] items;                   // The items in the game
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
            _requestToClear = false;
            _requestToQuit = false;
            _combatEngine = new CombatEngine();
            parser = new CommandParser(this);
            LoadCreatures();
            LoadItems();
            LoadRooms();
            dungeon = new RoomGrid(dungeonSize, dungeonSize, rooms, uniqueRooms);
            dungeon.GenerateRoomContents(creatures, items);
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

            playerBaseStats.Name = "Bobert";
            playerBaseStats.HitPoints = 30;
            playerBaseStats.MaxHitPoints = 30;
            playerBaseStats.AttackBonus = 5;
            playerBaseStats.ArmorClass = 12;
            playerBaseStats.Damage = new RandomDie();
            playerBaseStats.XPValue = 100;
            playerBaseStats.Friendly = true;

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

            _player = new Player(playerStats, playerBaseStats, playerAttributes, "You");
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
            The GetObjectNames method returns an array containing the names of every
            object in the game, sorted in alphabetical order
        */

        public string[] GetObjectNames()
        {
            List<string> names = new List<string>();
            foreach (Creature creature in creatures)
            {
                names.Add(creature.Stats.Name.ToLower());
            }
            foreach (Item item in items)
            {
                names.Add(item.Name.ToLower());
            }
            names.Sort((string a, string b) => { return a.CompareTo(b); });
            return names.ToArray();
        }

        /*
            The IsCreature method returns whether a creature with the passed name
            exists
        */

        public bool IsCreature(string name)
        {
            return creatures.First(c => c.Stats.Name.ToLower() == name.ToLower()) != null;
        }

        /*
            The IsItem method returns whether an item with the passed name
            exists
        */

        public bool IsItem(string name)
        {
            return items.First(i => i.Name.ToLower() == name.ToLower()) != null;
        }

        /*
            The LoadCreatures method loads the creatures in the game
        */

        private void LoadCreatures()
        {
            // Const
            const string CREATURE_RESOURCE = "Dice_and_Combat_Engine.Resources.creatures.txt";

            List<Creature> loadList = new List<Creature>();
            Assembly assembly = Assembly.GetExecutingAssembly();

            try
            {
                // Initialize stream
                StreamReader creatureStream = new StreamReader(assembly.GetManifestResourceStream(CREATURE_RESOURCE));

                // Delimiter
                char[] delim = { ':' };
                string[] responseDelim = { " , " };

                // Prepare to read
                Attributes attribs = new Attributes();
                BaseStats stats = new BaseStats();
                bool isNPC = false;
                List<string> responses = new List<string>();
                string description = "";

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
                                stats.Name = splitLine[1].Trim();
                                break;
                            case "Description":
                                // Read creature's description
                                description = splitLine[1].Trim();
                                break;
                            case "Responses":
                                // Creature is NPC : Read responses
                                isNPC = true;
                                string[] responseTokens = splitLine[1].Split(responseDelim, StringSplitOptions.None);
                                foreach (string response in responseTokens)
                                    responses.Add(response.Trim());
                                break;
                            case "Friendly":
                                // Read creature's friendly status
                                stats.Friendly = bool.Parse(splitLine[1]);
                                break;
                            case "HP":
                                // Read creature's HP
                                stats.HitPoints = int.Parse(splitLine[1]);

                                // Set max HP as well
                                stats.MaxHitPoints = int.Parse(splitLine[1]);
                                break;
                            case "AB":
                                // Read creature's attack bonus
                                stats.AttackBonus = int.Parse(splitLine[1]);
                                break;
                            case "AC":
                                // Read creature's armor class
                                stats.ArmorClass = int.Parse(splitLine[1]);
                                break;
                            case "Damage":
                                // Read creature's damage
                                stats.Damage = new RandomDie(int.Parse(splitLine[1]));
                                break;
                            case "XP":
                                // Read creature's XP value
                                stats.XPValue = int.Parse(splitLine[1]);
                                break;

                            // Attributes
                            case "Strength":
                                // Read creature's strength
                                attribs.strength = int.Parse(splitLine[1]);
                                break;
                            case "Constitution":
                                // Read creature's constitution
                                attribs.constitution = int.Parse(splitLine[1]);
                                break;
                            case "Dexterity":
                                // Read creature's dexterity
                                attribs.dexterity = int.Parse(splitLine[1]);
                                break;
                            case "Intelligence":
                                // Read creature's intelligence
                                attribs.intelligence = int.Parse(splitLine[1]);
                                break;
                            case "Wisdom":
                                // Read creature's wisdom
                                attribs.wisdom = int.Parse(splitLine[1]);
                                break;
                            case "Charisma":
                                // Read creature's charisma
                                attribs.charisma = int.Parse(splitLine[1]);
                                break;
                        }
                    }

                    if (line.Length == 0 || creatureStream.EndOfStream)
                    {
                        // Check if creature is NPC
                        if (isNPC)
                        {
                            loadList.Add(new NPC(stats, attribs, description, responses.ToArray()));
                            responses.Clear();
                            isNPC = false;
                        }
                        else
                            loadList.Add(new Creature(stats, attribs, description));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Sort list alphabetically
            loadList.Sort((Creature a, Creature b) => a.Stats.Name.CompareTo(b.Stats.Name));

            // Set array
            creatures = loadList.ToArray();
        }

        /*
            The LoadItems method loads the items in the game
        */

        private void LoadItems()
        {
            const string ITEM_RESOURCE = "Dice_and_Combat_Engine.Resources.items.txt";

            List<Item> loadList = new List<Item>();
            Assembly assembly = Assembly.GetExecutingAssembly();

            try
            {
                // Initialize stream
                StreamReader itemStream = new StreamReader(assembly.GetManifestResourceStream(ITEM_RESOURCE));

                // Delimiter
                char[] delim = { ':' };

                // Prepare to read
                ItemType type = ItemType.Weapon;

                string itemName = "",
                       itemDesc = "";

                int itemValue = 0,
                    itemDurability = 0,
                    damageBonus = 0,
                    healthRestored = 0;

                while (!itemStream.EndOfStream)
                {
                    string line = itemStream.ReadLine();

                    if (!(line.Length == 0))
                    {
                        string[] splitLine = line.Split(delim);

                        // Switch on the data to read
                        switch (splitLine[0])
                        {
                            case "ID":
                                type = (ItemType) int.Parse(splitLine[1]);
                                break;
                            case "Name":
                                itemName = splitLine[1].Trim();
                                break;
                            case "Description":
                                itemDesc = splitLine[1].Trim();
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
                            case "HealthRestored":
                                healthRestored = int.Parse(splitLine[1]);
                                break;
                        }
                    }

                    if (line.Length == 0 || itemStream.EndOfStream)
                    {
                        // Determine the type of item to create
                        if (type == ItemType.Weapon)
                            loadList.Add(new Weapon(itemName, itemDesc, itemDurability, itemValue, damageBonus));
                        else if (type == ItemType.Potion)
                            loadList.Add(new Potion(itemName, itemDesc, itemDurability, itemValue, healthRestored));
                        else
                            loadList.Add(new Treasure(itemName, itemDesc, itemDurability, itemValue));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Sort list alphabetically
            loadList.Sort((Item a, Item b) => { return a.Name.CompareTo(b.Name); });

            // Set array
            items = loadList.ToArray();
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

                // Prepare to read
                string roomName = "";

                int numCreatures = 0,
                    numItems = 0;

                while (!roomStream.EndOfStream)
                {
                    string line = roomStream.ReadLine();

                    if (!(line.Length == 0))
                    {
                        string[] splitLine = line.Split(delim);

                        switch (splitLine[0])
                        {
                            case "Name":
                                roomName = splitLine[1];
                                break;
                            case "NumCreatures":
                                numCreatures = int.Parse(splitLine[1]);
                                break;
                            case "NumItems":
                                numItems = int.Parse(splitLine[1]);
                                break;
                        }
                    }

                    if (line.Length == 0 || roomStream.EndOfStream)
                        loadList.Add(new Room(roomName, numCreatures, numItems));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Sort list alphabetically
            loadList.Sort((Room a, Room b) => { return a.Name.CompareTo(b.Name); });

            // Set array
            rooms = loadList.ToArray();
        }

        /*
            The ParseCommand method parses a user-input command with the game's command parser

            The method returns the output
        */

        public string ParseCommand(string commandString)
        {
            return parser.Parse(commandString.Trim().ToLower());
        }

        /*
            RequestToClear Property
        */

        public bool RequestToClear
        {
            get { return _requestToClear; }
            set { _requestToClear = value; }
        }

        /*
            RequestToQuit Property
        */

        public bool RequestToQuit
        {
            get { return _requestToQuit; }
            set { _requestToQuit = value; }
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
    }
}
