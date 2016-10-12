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
            The GetOrdinalSuffix method returns the appropriate suffix for an
            integer number
        */

        private string GetOrdinalSuffix(int num)
        {
            string suffix = "";
            
            // Numbers in the range of 10-19 always have the suffix "th"
            if (num >= 10 && num <= 19)
            {
                suffix = "th";
            }
            else
            {
                // Get the number in the 1st place
                while (num / 10 != 0)
                {
                    num /= 10;
                }

                // Determine suffix
                switch (num)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }

            return suffix;
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

                // If the parameter is a proper direction, continue
                if (isDirection)
                {
                    Room currentRoom = game.Player.Location;

                    // If a room in the specified direction does not exist
                    if (currentRoom.Links[(int)direction] == null)
                    {
                        output.Add("There is no exit leading " + direction.ToString());
                    }
                    // If the link to the room in the specified direction is not open
                    else if (!currentRoom.LinksUnlocked[(int)direction])
                    {
                        output.Add("The " + direction.ToString() + " exit is not open");
                    }
                    // If the room in the specified direction both exists and is open
                    else
                    {
                        game.Player.Location = currentRoom.Links[(int)direction];
                        output.Add("You went " + direction.ToString());
                    }
                }
                else
                {
                    // Output error because the parameter is not a proper direction
                    output.Add("Invalid parameter: Must be one of North, South, East, or West");
                }
            }
            else
            {
                // Output error because there are an invalid number of parameters
                output.Add("Invalid number of parameters for command: go");
            }
        }

        /*
            The ParseLookCommand method parses a "look" command for some action to perform
        */

        private void ParseLookCommand(string[] commandParams)
        {
            // We expect up to 2 parameters: The object to look at, and
            // an integer specifying the instance of the object
            if (commandParams.Length <= 2)
            {
                Room currentRoom = game.Player.Location;

                // Determine the number of parameters
                if (commandParams.Length == 0)
                {
                    // We will provide general information about the current room
                    string[][] info = currentRoom.GetInfo();
                    for (int i = 0; i < info.GetLength(0); i++)
                    {
                        for (int j = 0; j < info[i].Length; j++)
                        {
                            output.Add(info[i][j]);
                        }

                        // If we haven't yet output the last set of information
                        if (i != info.GetLength(0) - 1)
                        {
                            // Add newline
                            output.Add("");
                        }
                    }
                }
                else
                {
                    List<Creature> roomDenizens = currentRoom.Denizens;
                    Player player = game.Player;

                    if (commandParams.Length == 1)
                    {
                        // We expect the name of a creature in the current room to examine

                        // We get the first instance of the named creature in the room
                        Creature creature = roomDenizens.Find(denizen => denizen.Stats.name.ToLower() == commandParams[0]);

                        // If we found the creature, set PC's target and inform so; else, output error
                        if (creature != null)
                        {
                            player.Target = creature;
                            output.Add("You observe " + creature.Stats.name);
                        }
                        else
                        {
                            output.Add("No such creature, " + commandParams[0] + ", exists in this room");
                        }
                    }
                    else
                    {
                        // We expect the name of a creature in the current room and an integer specifying
                        // the instance of the named creature to examine

                        // First check that second parameter is an integer number specifying the nth
                        // instance to get
                        int instance;
                        if (int.TryParse(commandParams[1], out instance) && instance > 0)
                        {
                            // Get instance suffix
                            string suffix = GetOrdinalSuffix(instance);

                            // We do not expect instance to be 0-based, so make it so
                            instance -= 1;

                            // Get creatures with passed name
                            Creature[] creatures = roomDenizens.Where(denizen => denizen.
                                                                                 Stats.
                                                                                 name.
                                                                                 ToLower() == commandParams[0]).
                                                                                 ToArray();

                            // Assign the nth instance of creature, if it exists
                            Creature creature = (instance < creatures.Length) ? creatures[instance] : null;

                            // If we found the creature, set PC's target and inform so; else, output error
                            if (creature != null)
                            {
                                player.Target = creature;
                                output.Add("You observe " + creature.Stats.name);
                            }
                            else
                            {
                                output.Add("No such creature, " + commandParams[0] + ", exists in this room");
                            }
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
                // Output error because there is an invalid number of parameters
                output.Add("Error: Command \"take\" takes at least 1 parameter: name of item to take");
            }
            else
            {
                List<Item> playerInventory = game.Player.Inventory;
                List<Item> roomContents = game.Player.Location.Contents;

                // Check if first parameter is item name or "all" keyword
                if (commandParams[0] == "all")
                {
                    // Check if second parameter of names of items to take is supplied
                    if (commandParams.Length == 2)
                    {
                        // Get all items with the specified name from the room
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
                            // Output error because no items with the specifed name were found
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
                            // Output error because there are no items in this room to take
                            output.Add("There are no items in this room to take");
                        }
                    }
                }
                else
                {
                    // Check if user is specifying which instance of named item to take
                    if (commandParams.Length == 2)
                    {
                        // First check that second parameter is an integer
                        int instance;
                        if (int.TryParse(commandParams[1], out instance))
                        {
                            // Get suffix of instance
                            string suffix = GetOrdinalSuffix(instance);

                            // We don't expect instance to be 0-based, so make it so
                            instance -= 1;

                            // Get listing of items with passed name
                            Item[] items = roomContents.Where(item => item.Name.ToLower() == commandParams[1]).ToArray();

                            // Get nth instance of item
                            Item itemToTake = (instance < items.Length) ? items[instance] : null;
                            if (itemToTake != null)
                            {
                                playerInventory.Add(itemToTake);
                                output.Add("You take the " + instance + suffix + " " + itemToTake.Name);
                                roomContents.Remove(itemToTake);
                            }
                            else
                            {
                                // Output error because nth instance of named item does not exist in this room
                                output.Add("There is no " + instance + suffix + " " + itemToTake.Name +
                                           " in this room");
                            }
                        }
                        else
                        {
                            // Output error because second parameter is not an integer specifying the instance
                            // of named item to take
                            output.Add("Error: Second parameter must be an integer specifying the instance " +
                                       "of the item to take");
                        }
                    }
                    else
                    {
                        // Take first instance of named item from room
                        Item itemToTake = roomContents.Find(item => item.Name == commandParams[0]);
                        if (itemToTake != null)
                        {
                            playerInventory.Add(itemToTake);
                            output.Add("You take " + itemToTake.Name);
                            roomContents.Remove(itemToTake);
                        }
                        else
                        {
                            // Output error because named item was not found in this room
                            output.Add("Item of name " + commandParams[0] + " does not exist in this room");
                        }
                    }
                }
            }
        }
    }
}
