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
        private static string[] commands = { "attack", "drop", "equip", "get", "go",
                                             "inventory", "look", "open", "quit",
                                             "score", "take" };

        private CombatEngine _combatEngine;     // Handles combat logic
        private Creature[] creatures;           // The creatures in the game
        private Item[] items;                   // The items in the game
        private Room[] rooms;                   // The rooms in the game
        private RoomGrid dungeon;               // The dungeon
        private Player _player;                 // The player character
        private List<string> _feedback;         // Feedback

        /*
            Constructor
            Accepts an integer representing the size of the dungeon;
            the dungeon will contain that many rows and columns

            The method also accepts an optional boolean indicating whether
            each Room should be unique
        */

        public Game(int dungeonSize, bool uniqueRooms = false)
        {
            _combatEngine = new CombatEngine();
            LoadCreatures();
            LoadItems();
            LoadRooms();
            SortResources();
            _feedback = new List<string>();
            dungeon = new RoomGrid(dungeonSize, dungeonSize, rooms, uniqueRooms);
            dungeon.GenerateRoomContents(creatures, items);
            GeneratePlayer();
        }

        /*
            The ExtractCommand method extracts the command issued from a user-input string
        */

        private string ExtractCommand(string commandString)
        {
            // Separate command from parameters
            int i = 0;
            while (i < commandString.Length && !char.IsWhiteSpace(commandString[i]))
            {
                i++;
            }
            return commandString.Substring(0, i);
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
            The IsGoodCommand method determines if a user-input command is acceptable
        */

        private bool IsGoodCommand(string command)
        {
            // Find command being issued
            bool good = false;
            int i = 0;
            while (!good && i < commands.Length)
            {
                if (command.Equals(commands[i]))
                {
                    good = true;
                }
                else
                {
                    i++;
                }
            }
            return good;
        }

        /*
            The LoadCreatures method loads the creatures in the game
        */

        private void LoadCreatures()
        {
            // Consts
            const string CREATURE_RESOURCE = "Dice_and_Combat_Engine.Resources.creatures.txt";
            const string CREATURE_IMG_DIRECTORY = "Dice_and_Combat_Engine.Resources.";

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
                        // Get new creature's image
                        Stream imageStream = assembly.GetManifestResourceStream(CREATURE_IMG_DIRECTORY +
                                                      creatureStats.name.ToLower() + ".jpg");
                        Image creatureImage = Image.FromStream(imageStream);

                        // Create new creature with collected data
                        loadList.Add(new Creature(creatureStats, creatureAttribs, creatureImage));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim list
            loadList.TrimExcess();

            // Set array
            creatures = loadList.ToArray();
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

            // Set array
            rooms = loadList.ToArray();
        }

        /*
            The ParseCommand method parses a user-input command for some action to perform
        */

        public void ParseCommmand(string commandString)
        {
            string command = ExtractCommand(commandString);
            
            // Make sure that command is acceptable
            if (IsGoodCommand(command))
            {
                string[] commandParams = commandString.Split(new string[] { command, " " },
                                                             StringSplitOptions.RemoveEmptyEntries);
                switch (command)
                {
                    case "attack":
                        //ParseAttackCommand(commandParams);
                        break;
                    case "drop":
                        //ParseDropCommand(commandParams);
                        break;
                    case "equip":
                        //ParseEquipCommand(commandParams);
                        break;
                    case "get":
                        //ParseGetCommand(commandParams);
                        break;
                    case "go":
                        ParseGoCommand(commandParams);
                        break;
                    case "inventory":
                        //ParseInventoryCommand();
                        break;
                    case "look":
                        ParseLookCommand(commandParams);
                        break;
                    case "open":
                        ParseOpenCommand(commandParams);
                        break;
                    case "quit":
                        //ParseQuitCommand();
                        break;
                    case "score":
                        //ParseScoreCommand();
                        break;
                    case "take":
                        //ParseTakeCommand(commandParams);
                        break;
                }
            }
            else
            {
                // Set feedback to notify of bad command
                _feedback.Add("Bad command: " + command);
            }
        }

        /*
            The ParseGoCommand method parses a "go" command for some action to perform
        */

        private void ParseGoCommand(string[] commandParams)
        {
            // We expect only 1 parameter: The direction to travel in
            if (commandParams.Length == 1)
            {
                // Make sure that parameter is a direction
                Direction direction = Direction.North;
                bool isDirection = false;
                while (!isDirection && direction <= Direction.West)
                {
                    if (commandParams[0].Equals(direction.ToString().ToLower()))
                    {
                        isDirection = true;
                    }
                    else
                    {
                        direction++;
                    }
                }

                if (isDirection)
                {
                    Room currentRoom = _player.Location;
                    if (currentRoom.Links[(int)direction] == null)
                    {
                        _feedback.Add("There is no exit leading " + direction.ToString());
                    }
                    else if (!currentRoom.LinksUnlocked[(int)direction])
                    {
                        _feedback.Add("The " + direction.ToString() + " exit is not open");
                    }
                    else
                    {
                        _player.Location = currentRoom.Links[(int)direction];
                        _feedback.Add("You went " + direction.ToString());
                    }
                }
                else
                {
                    _feedback.Add("Invalid parameter: Must be one of North, South, East, or West");
                }                
            }
            else
            {
                _feedback.Add("Invalid number of parameters for command: go");
            }
        }

        /*
            The ParseLookCommand method parses a "look" command for some action to perform
        */

        private void ParseLookCommand(string[] commandParams)
        {
            // We expect up to 2 parameters: The object to look at, and a number specifying the object
            // out of many
            if (commandParams.Length <= 2)
            {
                // Determine the number of parameters
                if (commandParams.Length == 0)
                {
                    // We will provide general information about the current room
                    string[] creatureInfo = _player.Location.GetContentInformation("creatures");
                    string[] itemInfo = _player.Location.GetContentInformation("items");

                    int numExits = 0;
                    for (Direction direction = Direction.North; direction <= Direction.West; direction++)
                    {
                        if (_player.Location.Links[(int)direction] != null)
                        {
                            numExits++;
                        }
                    }

                    // Build feedback
                    // Creatures
                    _feedback.Add("There are " + _player.Location.Denizens.Count + " creatures in this room:");
                    for (int i = 0; i < creatureInfo.Length; i++)
                    {
                        if (creatureInfo.Length == 1)
                        {
                            _feedback.Add("  " + creatureInfo[i]);
                        }
                        else if (i != creatureInfo.Length - 1)
                        {
                            _feedback.Add("  " + creatureInfo[i] + ",");
                        }
                        else
                        {
                            _feedback.Add("  and " + creatureInfo[i]);
                        }
                    }

                    // Items
                    _feedback.Add("");
                    _feedback.Add("There are " + _player.Location.Contents.Count + " items in this room:");
                    for (int i = 0; i < itemInfo.Length; i++)
                    {
                        if (itemInfo.Length == 1)
                        {
                            _feedback.Add("  " + itemInfo[i]);
                        }
                        else if (i != itemInfo.Length - 1)
                        {
                            _feedback.Add("  " + itemInfo[i] + ",");
                        }
                        else
                        {
                            _feedback.Add("  and " + itemInfo[i]);
                        }
                    }

                    // Exits
                    _feedback.Add("");
                    _feedback.Add("There are " + numExits + " exits in this room:");
                    for (Direction direction = Direction.North; direction <= Direction.West; direction++)
                    {
                        if (_player.Location.Links[(int)direction] != null)
                        {
                            _feedback.Add("  " + direction.ToString());
                        }
                    }
                }
            }
        }

        /*
            The ParseOpenCommand method parses an "open" command for some action to perform
        */

        private void ParseOpenCommand(string[] commandParams)
        {
            // We expect only 1 parameter: The direction of the exit to open
            if (commandParams.Length == 1)
            {
                // Make sure that parameter is a direction
                Direction direction = Direction.North;
                bool isDirection = false;
                while (!isDirection && direction <= Direction.West)
                {
                    if (commandParams[0].Equals(direction.ToString().ToLower()))
                    {
                        isDirection = true;
                    }
                    else
                    {
                        direction++;
                    }
                }

                if (isDirection)
                {
                    Room currentRoom = _player.Location;
                    if (currentRoom.Links[(int)direction] == null)
                    {
                        _feedback.Add("There is no exit leading " + direction.ToString() + " to open");
                    }
                    else if (currentRoom.LinksUnlocked[(int)direction])
                    {
                        _feedback.Add("The " + direction.ToString() + " exit is already open");
                    }
                    else
                    {
                        // Open link
                        currentRoom.OpenLink(currentRoom.Links[(int)direction]);
                        _feedback.Add("You opened the " + direction.ToString() + " exit");
                    }
                }
                else
                {
                    _feedback.Add("Invalid parameter: Must be one of North, South, East, or West");
                }
            }
            else
            {
                _feedback.Add("Invalid number of parameters for command: open");
            }
        }

        /*
            The SortResources method sorts the creatures, items, and rooms resource fields alphabetically
        */

        private void SortResources()
        {
            // Sort Creatures
            for (int i = 0; i < creatures.Length - 1; i++)
            {
                for (int j = i + 1; j < creatures.Length; j++)
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
            for (int i = 0; i < items.Length - 1; i++)
            {
                for (int j = i + 1; j < items.Length; j++)
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
            for (int i = 0; i < rooms.Length - 1; i++)
            {
                for (int j = i + 1; j < rooms.Length; j++)
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
            Feedback property
        */

        public List<string> Feedback
        {
            // When the feedback is retrieved, we will clear all entries
            get
            {
                List<string> toReturn = new List<string>(_feedback);
                _feedback.Clear();
                return toReturn;
            }
        }
    }
}
