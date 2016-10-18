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
                if (command == commands[i])
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
            The Parameterize method converts a command string into an array of parameters
        */

        private string[] Parameterize(string commandString)
        {
            // Get command
            string command = ExtractCommand(commandString);

            // Specify delimits as the command and space characters
            string[] delims = { command, " " };

            // Split the command string into parameters
            string[] parameters = commandString.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            // Resolve any compound parameters
            parameters = ResolveCompoundParameters(parameters);

            // Return
            return parameters;
        }

        /*
            The Parse method parses a user-input command for some action to perform

            The method returns the lines of output as an array
        */

        public string[] Parse(string commandString)
        {
            // Clear output for fresh feedback
            output.Clear();

            string command = ExtractCommand(commandString);

            // Make sure that command is acceptable
            if (IsGoodCommand(command))
            {
                string[] commandParams = Parameterize(commandString);

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
                        ParseGetCommand(commandParams);
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
                        ParseQuitCommand(commandParams);
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
            The ParseGetCommand method parses a "get" command for some action to perform
        */

        private void ParseGetCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length >= 1 && commandParams.Length <= 2)
            {
                // Determine number of parameters
                if (commandParams.Length == 1)
                {
                    // Get first instance of named item from PC's inventory to use
                    Item item = game.Player.Inventory.Find(i => i.Name.ToLower() == commandParams[0]);
                    
                    if (item != null)
                    {
                        string feedback = game.Player.UseItem(item);
                        output.Add(feedback);
                    }
                    else
                    {
                        output.Add("No such item exists in your inventory");
                    }
                }
                else
                {
                    // Get nth instance of named item from PC's inventory to use
                    int instance;
                    if (int.TryParse(commandParams[1], out instance))
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance--;
                        Item[] items = game.Player.Inventory.Where(i => i.Name.ToLower() ==
                                                                   commandParams[0]).ToArray();

                        Item item = (instance >= 0 && instance < items.Length) ? items[instance] : null;

                        if (item != null)
                        {
                            string feedback = game.Player.UseItem(item);
                            output.Add(feedback);
                        }
                        else
                        {
                            output.Add("No " + (instance + 1) + suffix + " instance of such " +
                                       "an item exists in your inventory");
                        }
                    }
                    else
                    {
                        output.Add("Second parameter must be an integer specifying the instace " +
                                   "of the item in your inventory to get");
                    }
                }
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
                output.Add("Command \"go\" takes 1 parameter");
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
                            output.Add("No such creature exists in this room");
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
                                output.Add("No " + (instance + 1) + suffix + " instance of such a creature " +
                                           "exists in this room");
                            }
                        }
                        else
                        {
                            // Output error
                            output.Add("Second parameter must be an integer specifying the instance " +
                                       "of the creature to look at");
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
                output.Add("Command \"open\" takes 1 parameter");
            }
        }

        /*
            The ParseQuitCommand method parses a "quit" command for some action to perform
        */

        private void ParseQuitCommand(string[] commandParams)
        {
            // We expect no parameters
            if (commandParams.Length == 0)
            {
                // Confirm quit
                output.Add("Quitting...");
            }
            else
            {
                // Output error
                output.Add("Error: Command \"quit\" takes no parameters");
            }
        }

        /*
            The ParseTakeCommand method parses a "take" command for some action to perform
        */

        private void ParseTakeCommand(string[] commandParams)
        {
            // We expect up to 2 parameters, and at least 1
            if (commandParams.Length == 0 || commandParams.Length > 2)
            {
                // Output error because there is an invalid number of parameters
                output.Add("Command \"take\" takes at least 1 and up to 2 parameters");
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
                            output.Add("No such items exist in this room");
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
                            Item[] items = roomContents.Where(item => item.Name.ToLower() == commandParams[0]).ToArray();

                            // Get nth instance of item
                            Item itemToTake = (instance >= 0 && instance < items.Length) ? items[instance] : null;
                            if (itemToTake != null)
                            {
                                playerInventory.Add(itemToTake);
                                output.Add("You take the " + (instance + 1) + suffix + " " + itemToTake.Name);
                                roomContents.Remove(itemToTake);
                            }
                            else
                            {
                                // Output error because nth instance of named item does not exist in this room
                                output.Add("No " + (instance + 1) + suffix + " instance of such an item " +
                                           "exists in this room");
                            }
                        }
                        else
                        {
                            // Output error because second parameter is not an integer specifying the instance
                            // of named item to take
                            output.Add("Second parameter must be an integer specifying the instance " +
                                       "of the item to take");
                        }
                    }
                    else
                    {
                        // Take first instance of named item from room
                        Item itemToTake = roomContents.Find(item => item.Name.ToLower() == commandParams[0]);
                        if (itemToTake != null)
                        {
                            playerInventory.Add(itemToTake);
                            output.Add("You take " + itemToTake.Name);
                            roomContents.Remove(itemToTake);
                        }
                        else
                        {
                            // Output error because named item was not found in this room
                            output.Add("No such items exist in this room");
                        }
                    }
                }
            }
        }

        /*
            The ResolveCompoundParameters method handles merging two or more elements
            of an array that represent a single parameter into a single parameter

            If there are 1 or 0 parameters, the array is returned unchanged
        */

        private string[] ResolveCompoundParameters(string[] parameters)
        {
            // The array to return
            string[] toReturn;

            if (parameters.Length >= 2)
            {
                // Prepare list for the purposes of modifying the array
                List<string> paramList = new List<string>(parameters);

                // Prepare list of object names
                List<string> names = new List<string>();

                // Candidate for a compound parameter name, built as name matches are found
                StringBuilder builtParameter = new StringBuilder();               

                // Index of the parameter we begin checking names against
                int start = 0;

                // The current index of the element in the parameter list we have expanded to
                int current = 0;

                // While there are more parameters
                while (start < paramList.Count)
                {
                    // Start off built parameter with the start parameter
                    builtParameter.Append(paramList[start]);

                    // Get list of object names that start with the start parameter
                    names.AddRange(game.GetObjectNames().Where(name => name.StartsWith(paramList[start])));

                    // While matching names list is not empty and there are more parameters
                    while (names.Count != 0 && current != paramList.Count - 1)
                    {
                        // If name list contains elements that start with the built parameter and the next parameter joined together
                        if (names.Find(name => name.StartsWith(builtParameter.ToString() + " " + paramList[current + 1])) != null)
                        {
                            // Join the built parameter and the next parameter together into a single parameter
                            builtParameter.Append(" " + paramList[current + 1]);

                            // Increment current to point to the next parameter which we have expanded to
                            current++;

                            // Check if the currently built parameter matches exactly with an object name
                            // In which case, a compound parameter has definitely been resolved
                            if (names.Contains(builtParameter.ToString()))
                            {
                                // Set start parameter to the resolved parameter
                                paramList[start] = builtParameter.ToString();

                                // Remove all parameters from the parameter list starting at
                                // the current index and down to but not including the start
                                // index
                                while (current != start)
                                {
                                    paramList.RemoveAt(current);
                                    current--;
                                }
                            }

                            // Refresh list of names that begin with the built parameter
                            names.RemoveAll(name => !name.StartsWith(builtParameter.ToString()));
                        }
                        else
                        {
                            // Clear names
                            names.Clear();
                        }
                    }

                    // Increment start to point to the next parameter in the parameter list
                    start++;
                    builtParameter.Clear();
                }

                // Prepare to return possibly modifed array
                toReturn = paramList.ToArray();
            }
            else
            {
                // Parameters are unchanged
                toReturn = parameters;
            }

            // Return
            return toReturn;
        }
    }
}
