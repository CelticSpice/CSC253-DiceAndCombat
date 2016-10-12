/*
    This class represents a command-line parser
    10/10/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice_and_Combat_Engine
{
    class CommandParser
    {
        // Fields
        private static string[] commands = { "attack", "drop", "equip", "get", "go",
                                             "inventory", "look", "open", "quit",
                                             "score", "take" };

        private Game game;
        private List<string> output;

        /*
            Constructor
            Accepts the Game class to parse commands for
        */

        public CommandParser(Game g)
        {
            game = g;
            output = new List<string>();
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
            The Parse method parses a user-input command for some action to perform

            The method returns the lines of output as an array
        */

        public string[] Parse(string commandString)
        {
            output.Clear();

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
                        ParseTakeCommand(commandParams);
                        break;
                }
            }
            else
            {
                // Set output to notify of bad command
                output.Add("Bad command: " + command);
            }

            return output.ToArray();
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
                    Room currentRoom = game.Player.Location;
                    if (currentRoom.Links[(int)direction] == null)
                    {
                        output.Add("There is no exit leading " + direction.ToString());
                    }
                    else if (!currentRoom.LinksUnlocked[(int)direction])
                    {
                        output.Add("The " + direction.ToString() + " exit is not open");
                    }
                    else
                    {
                        game.Player.Location = currentRoom.Links[(int)direction];
                        output.Add("You went " + direction.ToString());
                    }
                }
                else
                {
                    output.Add("Invalid parameter: Must be one of North, South, East, or West");
                }
            }
            else
            {
                output.Add("Invalid number of parameters for command: go");
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
                Room currentRoom = game.Player.Location;

                // Determine the number of parameters
                if (commandParams.Length == 0)
                {
                    // We will provide general information about the current room
                    string[] creatureInfo = currentRoom.GetContentInformation("creatures");
                    string[] itemInfo = currentRoom.GetContentInformation("items");
                    int numExits = currentRoom.Links.Where(room => room != null).Count();

                    // Build output
                    string indent = "--  ";

                    // Creatures
                    output.Add("There are " + currentRoom.Denizens.Count + " creatures in this room:");
                    for (int i = 0; i < creatureInfo.Length; i++)
                    {
                        if (creatureInfo.Length == 1)
                        {
                            output.Add(indent + creatureInfo[i]);
                        }
                        else if (i != creatureInfo.Length - 1)
                        {
                            output.Add(indent + creatureInfo[i] + ",");
                        }
                        else
                        {
                            output.Add(indent + "and " + creatureInfo[i]);
                        }
                    }

                    // Items
                    output.Add("");     // New line
                    output.Add("There are " + currentRoom.Contents.Count + " items in this room:");
                    for (int i = 0; i < itemInfo.Length; i++)
                    {
                        if (itemInfo.Length == 1)
                        {
                            output.Add(indent + itemInfo[i]);
                        }
                        else if (i != itemInfo.Length - 1)
                        {
                            output.Add(indent + itemInfo[i] + ",");
                        }
                        else
                        {
                            output.Add(indent + "and " + itemInfo[i]);
                        }
                    }

                    // Exits
                    output.Add("");     // New line
                    output.Add("There are " + numExits + " exits in this room:");
                    for (Direction direction = Direction.North; direction <= Direction.West; direction++)
                    {
                        if (currentRoom.Links[(int)direction] != null)
                        {
                            output.Add(indent + direction.ToString());
                        }
                    }
                }
                else
                {
                    string name = commandParams[0];
                    Creature creature = null;

                    if (commandParams.Length == 1)
                    {
                        // We expect the name of a creature in the current room to examine

                        // We assign the first instance of the named creature in the room, if it exists
                        creature = currentRoom.Denizens.Find(denizen => denizen.Stats.name.ToLower() == name);

                        // If we did not find the creature, inform so
                        if (creature == null)
                        {
                            output.Add("No such creature, " + name + ", exists in this room");
                        }

                    }
                    else
                    {
                        // We expect the name of a creature in the current room and a number specifying
                        // the specific creature out of more than one of its kind to examine

                        // First check that second parameter is an integer number specifying the nth
                        // instance to get
                        int instance;
                        if (int.TryParse(commandParams[1], out instance) && instance > 0)
                        {
                            // Get creatures with passed name
                            Creature[] creatures = currentRoom.Denizens.Where(denizen => denizen.
                                                                                         Stats.
                                                                                         name.
                                                                                         ToLower() == name).
                                                                                         ToArray();

                            // Assign the nth instance of creature, if it exists
                            creature = (instance - 1 < creatures.Length) ? creatures[instance - 1] : null;

                            // If we could not find the specified instance of the creature, inform so
                            if (creature == null)
                            {
                                if (instance == 1)
                                {
                                    output.Add("A " + instance + "st " + name + " does not exist in this room");
                                }
                                else if (instance == 2)
                                {
                                    output.Add("A " + instance + "nd " + name + " does not exist in this room");
                                }
                                else if (instance == 3)
                                {
                                    output.Add("A " + instance + "rd " + name + " does not exist in this room");
                                }
                                else
                                {
                                    output.Add("A " + instance + "th " + name + " does not exist in this room");
                                }
                            }
                        }
                    }

                    // If we found the creature requested, set the Player's target to this creature
                    if (creature != null)
                    {
                        game.Player.Target = creature;
                        output.Add("You observe " + creature.Stats.name);
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
                    Room currentRoom = game.Player.Location;
                    if (currentRoom.Links[(int)direction] == null)
                    {
                        output.Add("There is no exit leading " + direction.ToString() + " to open");
                    }
                    else if (currentRoom.LinksUnlocked[(int)direction])
                    {
                        output.Add("The " + direction.ToString() + " exit is already open");
                    }
                    else
                    {
                        // Open link
                        currentRoom.OpenLink(currentRoom.Links[(int)direction]);
                        output.Add("You opened the " + direction.ToString() + " exit");
                    }
                }
                else
                {
                    output.Add("Invalid parameter: Must be one of North, South, East, or West");
                }
            }
            else
            {
                output.Add("Invalid number of parameters for command: open");
            }
        }

        /*
            The ParseTakeCommand method parses a "take" command for some action to perform
        */

        private void ParseTakeCommand(string[] commandParams)
        {
            // We expect up to 2 parameters, and at least 1
            if (commandParams.Length == 0)
            {
                // Output error
                output.Add("Error: Command \"take\" takes at least 1 parameter - name of item to take");
            }
            else
            {
                List<Item> playerInventory = game.Player.Inventory;
                List<Item> roomContents = game.Player.Location.Contents;

                // Check if item name or "all" keyword
                if (commandParams[0] == "all")
                {
                    // Check if name of item is supplied
                    if (commandParams.Length == 2)
                    {
                        // Add every item with the specified name from the room
                        // into the PC's inventory
                        Item[] items = roomContents.Where(item => item.Name.ToLower() == commandParams[1]).ToArray();

                        // If we found items with the specified name, continue; else, output error
                        if (items.Length > 0)
                        {
                            playerInventory.AddRange(items);
                            output.Add("You take every " + items[0].Name + " from the room");
                            foreach (Item item in items)
                            {
                                roomContents.Remove(item);
                            }
                        }
                        else
                        {
                            // Output error
                            output.Add("No such items of " + commandParams[1] + " exist in this room");
                        }
                    }
                    else
                    {
                        // Add every item in the current room to the PC's inventory
                        if (roomContents.Count != 0)
                        {
                            playerInventory.AddRange(roomContents);
                            output.Add("You take every item in the room");
                            roomContents.Clear();
                        }
                        else
                        {
                            // Output error
                            output.Add("There are no items in this room to take");
                        }
                    }
                }
                else
                {
                    // Check if user is specifying which item of named item to take
                    if (commandParams.Length == 2)
                    {
                        // First check that second parameter is an integer
                        int instance;
                        if (int.TryParse(commandParams[1], out instance))
                        {
                            // Get listing of items with passed name
                            Item[] items = roomContents.Where(item => item.Name.ToLower() == commandParams[1]).ToArray();

                            // Get item
                        }
                    }
                }
            }
        }
    }
}
