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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        internal Game()
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
            const string CREATURES_RESOURCE = "Dice_and_Combat_Engine.Resources.creatures.txt";
            const string CREATURE_IMG_DIRECTORY = "Dice_and_Combat_Engine.Resources.";

            creatures = new List<Creature>();

            try
            {
                // Initialize stream
                StreamReader creatureStream = new StreamReader(assembly.GetManifestResourceStream(CREATURES_RESOURCE));

                char[] delim = { ':' };
                char[] dataDelim = { '[', ']' };

                while (!creatureStream.EndOfStream)
                {
                    string line;
                    string[] splitLine;

                    Creature newCreature = new Creature();

                    BaseStats creatureStats = new BaseStats();
                    Attributes creatureAttribs = new Attributes();

                    // Read creature statistics and attributes
                    while (!((line = creatureStream.ReadLine()).Equals("")))
                    {
                        // Determine data being read
                        splitLine = line.Split(dataDelim);

                        switch (splitLine[1])
                        {
                            case "Stats":
                                const int NUM_STATS = 6;

                                // Get the creature's stats
                                for (int i = 0; i < NUM_STATS; i++)
                                {
                                    line = creatureStream.ReadLine();
                                    splitLine = line.Split(delim);

                                    switch (splitLine[0])
                                    {
                                        case "Name":
                                            creatureStats.name = splitLine[1].Trim();

                                            // Get creature's image as well
                                            Stream imageStream = assembly.GetManifestResourceStream(CREATURE_IMG_DIRECTORY +
                                                                 splitLine[1].Trim().ToLower() + ".jpg");
                                            newCreature.Portrait = Image.FromStream(imageStream);
                                            break;
                                        case "Friendly":
                                            creatureStats.friendly = bool.Parse(splitLine[1]);
                                            break;
                                        case "HP":
                                            creatureStats.hitPoints = int.Parse(splitLine[1]);
                                            break;
                                        case "AB":
                                            creatureStats.attackBonus = int.Parse(splitLine[1]);
                                            break;
                                        case "AC":
                                            creatureStats.armorClass = int.Parse(splitLine[1]);
                                            break;
                                        case "Damage":
                                            creatureStats.damage = new RandomDie(int.Parse(splitLine[1]));
                                            break;
                                    }
                                }

                                newCreature.Stats = creatureStats;

                                break;
                            case "Attributes":
                                const int NUM_ATTRIBUTES = 6;

                                // Get the creature's attributes
                                for (int i = 0; i < NUM_ATTRIBUTES; i++)
                                {
                                    line = creatureStream.ReadLine();
                                    splitLine = line.Split(delim);

                                    switch (splitLine[0])
                                    {
                                        case "Strength":
                                            creatureAttribs.strength = int.Parse(splitLine[1]);
                                            break;
                                        case "Constitution":
                                            creatureAttribs.constitution = int.Parse(splitLine[1]);
                                            break;
                                        case "Dexterity":
                                            creatureAttribs.dexterity = int.Parse(splitLine[1]);
                                            break;
                                        case "Intelligence":
                                            creatureAttribs.intelligence = int.Parse(splitLine[1]);
                                            break;
                                        case "Wisdom":
                                            creatureAttribs.wisdom = int.Parse(splitLine[1]);
                                            break;
                                        case "Charisma":
                                            creatureAttribs.charisma = int.Parse(splitLine[1]);
                                            break;
                                    }
                                }

                                newCreature.Attributes = creatureAttribs;

                                break;
                            case "Other":
                                const int NUM_OTHER = 1;

                                // Get the creature's other information
                                for (int i = 0; i < NUM_OTHER; i++)
                                {
                                    line = creatureStream.ReadLine();
                                    splitLine = line.Split(delim);

                                    switch (splitLine[0])
                                    {
                                        case "Exp":
                                            newCreature.ExpWorth = int.Parse(splitLine[1]);
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    creatures.Add(newCreature);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Trim lists
            creatures.TrimExcess();
        }

        /*
            The LoadItems method loads the items in the game
        */

        private void LoadItems()
        {
            items = new List<Item>();

            // Trim list
            items.TrimExcess();
        }

        /*
            The LoadDungeonRooms method loads the dungeon rooms
        */

        private void LoadDungeonRooms()
        {
            const string ROOM_RESOURCE = "Dice_and_Combat_Engine.Resources.room.txt";

            dungeon = new List<Room>();

            try
            {
                // Initialize stream
                StreamReader roomStream = new StreamReader(assembly.GetManifestResourceStream(ROOM_RESOURCE));

                char[] delim = { ':' };

                while (!roomStream.EndOfStream)
                {
                    string line;
                    string[] splitLine;

                    string roomName = "Name";
                    int numCreatures = 0,
                        numItems = 0;

                    while (!((line = roomStream.ReadLine()).Equals("")))
                    {
                        line = roomStream.ReadLine();
                        splitLine = line.Split(delim);

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

                    dungeon.Add(new Room(roomName, numCreatures, numItems));
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
            The SortLists method sorts the List fields
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
            playerBaseStats.hitPoints = 6;
            playerBaseStats.attackBonus = 2;
            playerBaseStats.armorClass = 12;
            playerBaseStats.damage = new RandomDie();
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

            List<Room> toShuffle = dungeon.ToList();

            for (int i = 0; i < layout.Length; i++)
            {
                Room toPlace = toShuffle[rng.Next(toShuffle.Count)];
                layout[i] = toPlace;
                toShuffle.Remove(toPlace);
            }

            // Set starting room
            _currentRoom = layout[0];

            // Generate dungeon room contents
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
                        foreach (Item item in items)
                        {
                            if (item is Treasure)
                            {
                                toGenerate.Contents.Add(item);
                                treasureAdded = true;
                            }
                        }
                    }
                }

                for (int j = (treasureAdded ? 1 : 0); j < maxItems; j++)
                {
                    Item toAdd = items[rng.Next(items.Count)];
                    toGenerate.Contents.Add(toAdd);
                }

                // Populate room with creatures
                int maxCreatures = toGenerate.Denizens.Capacity;
                
                for (int j = 0; j < maxCreatures; j++)
                {
                    Creature toAdd = creatures[rng.Next(creatures.Count)];
                    toGenerate.Denizens.Add(toAdd);
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
        }
    }
}
