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

namespace Dice_and_Combat_Engine
{
    class CommandParser
    {
        // Fields
        private static string[] commands = { "attack", "drop", "equip", "examine", "get",
                                             "go", "inventory", "look", "open", "quit",
                                             "score", "take", "use" };

        private Game game;
        private string output;

        /*
            Constructor
            Accepts the Game class to parse commands for
        */

        public CommandParser(Game g)
        {
            game = g;
            output = "";
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

        public string GetOrdinalSuffix(int num)
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
            string command = ExtractCommand(commandString);
            string[] delims = { command, " " };
            string[] parameters = commandString.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            parameters = ResolveCompoundParameters(parameters);
            return parameters;
        }

        /*
            The Parse method parses a user-input command for some action to perform

            The method returns the output
        */

        public string Parse(string commandString)
        {
            output = "";
            string command = ExtractCommand(commandString);
            if (IsGoodCommand(command))
            {
                string[] commandParams = Parameterize(commandString);
                switch (command)
                {
                    case "attack":
                        ParseAttackCommand(commandParams);
                        break;
                    case "drop":
                        ParseDropCommand(commandParams);
                        break;
                    case "equip":
                        //ParseEquipCommand(commandParams);
                        break;
                    case "get":
                    case "use":
                        ParseGetCommand(commandParams);
                        break;
                    case "go":
                        ParseGoCommand(commandParams);
                        break;
                    case "inventory":
                        //ParseInventoryCommand();
                        break;
                    case "look":
                    case "examine":
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
                output += "Bad command: " + command + "\n";
            }
            return output;
        }

        /*
            The ParseAttackCommand method parses an "attack" command
        */

        private void ParseAttackCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 1 || commandParams.Length == 2)
            {
                List<Creature> roomDenizens = game.Player.Location.Denizens;
                Player player = game.Player;

                // Determine number of parameters
                if (commandParams.Length == 1)
                {
                    // Player attacks first instance of named creature
                    Creature creature = roomDenizens.Find(c => c.Stats.name.ToLower() == commandParams[0]);
                    if (creature != null)
                    {
                        player.Target = creature;
                        creature.Target = player;
                        output += game.CombatEngine.DoCombat(player, player.Target);
                        output += game.CombatEngine.DoCombat(creature, creature.Target);
                    }
                    else
                    {
                        output += "No such creature, " + commandParams[0] + ", exists in this room\n";
                    }
                }
                else
                {
                    // Player attacks nth instance of named creature
                    int instance;
                    if (int.TryParse(commandParams[1], out instance))
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance -= 1;       // We don't expect instance to be 0-based, so make it so
                        Creature[] creatures = roomDenizens.Where(c => c.Stats.name.ToLower() ==
                                                                  commandParams[0]).ToArray();
                        Creature creature = (instance >= 0 && instance < creatures.Length) ? creatures[instance] : null;
                        if (creature != null)
                        {
                            player.Target = creature;
                            creature.Target = player;
                            output += game.CombatEngine.DoCombat(player, player.Target);
                            output += game.CombatEngine.DoCombat(creature, creature.Target);
                        }
                        else
                        {
                            output += "No " + (instance + 1) + suffix + " " +
                                      commandParams[0] + " exists in this room\n";
                        }
                    }
                    else
                    {
                        output += "Second parameter must be an integer specifying which " +
                                  "of " + commandParams[0] + " to attack\n";
                    }
                }
            }
            else
            {
                output += "Command \"attack\" syntax: [ creatureToAttack: Creature ] { instanceOf: int }\n";
            }
        }

        /*
            The ParseDropCommand method parses a "drop" command for some action to perform
        */

        private void ParseDropCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parmeters
            if (commandParams.Length == 0 || commandParams.Length > 2)
            {
                output += "Command \"drop\" syntax: [ itemName: string ] { instanceOf: int }\n";
            }
            else
            {
                List<Item> playerInventory = game.Player.Inventory;
                Player player = game.Player;

                // Determine the number of parameters
                if (commandParams.Length == 1)
                {
                    // Drop first instance of named item from PC's inventory in the current room
                    Item item = playerInventory.Find(i => i.Name.ToLower() == commandParams[0]);
                    if (item != null)
                    {
                        player.Drop(item);
                        output += "You drop " + item.Name + "\n";
                    }
                    else
                    {
                        output += "No such item exists in your inventory\n";
                    }
                }
                else
                {
                    // Drop nth instance of named item from PC's inventory in the current room
                    int instance;
                    if (int.TryParse(commandParams[1], out instance))
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance -= 1;      // We don't expect instance to be 0-based, so make it so
                        Item[] items = playerInventory.Where(i => i.Name.ToLower() == commandParams[0]).ToArray();
                        Item item = (instance >= 0 && instance < items.Length) ? items[instance] : null;
                        if (item != null)
                        {
                            player.Drop(item);
                            output += "You drop the " + (instance + 1) + suffix + " " +
                                      item.Name + " from your inventory\n";
                        }
                        else
                        {
                            output += "You don't have a " + (instance + 1) + suffix + " " +
                                      commandParams[0] + " in your inventory to drop\n";
                        }
                    }
                    else
                    {
                        output += "Second parameter must be an integer specifying which " +
                                   "of " + commandParams[0] + " in your inventory to drop\n";
                    }
                }
            }
        }

        /*
            The ParseEquipCommand method parses a "get" command for equiping a weapon
        */
        
        private void ParseEquipCommand(string[] commandParams)
        {
            //We expect one or two parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
            {
                output += "Command \"Equip\" syntax: [ itemName: string ] { instanceOf: int }\n";
            }
            else
            {
                List<Item> playerInventory = game.Player.Inventory;
                Player player = game.Player;

                //Determine number of params
                if (commandParams.Length == 1)
                {
                    //Get first instance of named item from PC's inventory to equip
                    Item item = playerInventory.Find(i => i.Name.ToLower() == commandParams[0]);
                    if (item != null)
                    {
                        if (item is Weapon)
                        {
                            player.Get(item);
                            int bonus = ((Weapon)item).DamageBonus;
                            output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                        }
                        else
                        {
                            output += "You cannot equip the item, " + item + "\n";
                        }
                    }
                    else
                    {
                        output += "No such item exists in your inventory\n";
                    }
                }
                else
                {
                    //Get nth item of PC's inventory to equip.
                    int instance;
                    if (int.TryParse(commandParams[1], out instance))
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance--;     // We don't expect instance to be 0-based, so make it so
                        Item[] items = playerInventory.Where(i => i.Name.ToLower() == commandParams[0]).ToArray();
                        Item item = (instance >= 0 && instance < items.Length) ? items[instance] : null;
                        if (item != null)
                        {
                            if (item is Weapon)
                            {
                                player.Get(item);
                                int bonus = ((Weapon)item).DamageBonus;
                                output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                            }
                            else
                            {
                                output += "You cannot equip the item, " + item + "\n";
                            }
                        }
                        else
                        {
                            output += "You don't have a " + (instance + 1) + suffix + " " +
                                      commandParams[0] + " in your inventory to get\n";
                        }
                    }
                    else
                    {
                        output += "Second parameter must be an integer specifying which " +
                                   "of " + commandParams[0] + " in your inventory to get\n";
                    }
                }
            }
        }

        /*
            The ParseGetCommand method parses a "get" command for some action to perform
        */

        private void ParseGetCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
            {
                output += "Command \"get\" syntax: [ itemName: string ] { instanceOf: int }\n";
            }
            else
            {
                List<Item> playerInventory = game.Player.Inventory;
                Player player = game.Player;

                // Determine number of parameters
                if (commandParams.Length == 1)
                {
                    // Get first instance of named item from PC's inventory to use
                    Item item = playerInventory.Find(i => i.Name.ToLower() == commandParams[0]);
                    if (item != null)
                    {
                        if (item is Weapon)
                        {
                            player.Get(item);
                            int bonus = ((Weapon)item).DamageBonus;
                            output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                        }
                        else if (item is Potion)
                        {
                            player.Get(item);
                            int restored = ((Potion)item).HealthRestored;
                            output += "You use " + item.Name + ", restoring your health by " + restored + "\n";
                        }
                        else
                        {
                            output += "You cannot use the item, " + item.Name + "\n";
                        }
                    }
                    else
                    {
                        output += "No such item exists in your inventory\n";
                    }
                }
                else
                {
                    // Get nth instance of named item from PC's inventory to use
                    int instance;
                    if (int.TryParse(commandParams[1], out instance))
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance--;     // We don't expect instance to be 0-based, so make it so
                        Item[] items = playerInventory.Where(i => i.Name.ToLower() == commandParams[0]).ToArray();
                        Item item = (instance >= 0 && instance < items.Length) ? items[instance] : null;
                        if (item != null)
                        {
                            if (item is Weapon)
                            {
                                player.Get(item);
                                int bonus = ((Weapon)item).DamageBonus;
                                output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                            }
                            else if (item is Potion)
                            {
                                player.Get(item);
                                int restored = ((Potion)item).HealthRestored;
                                output += "You use " + item.Name + ", restoring your health by " + restored + "\n";
                            }
                            else
                            {
                                output += "You cannot use the item, " + item.Name + "\n";
                            }
                        }
                        else
                        {
                            output += "You don't have a " + (instance + 1) + suffix + " " +
                                      commandParams[0] + " in your inventory to get\n";
                        }
                    }
                    else
                    {
                        output += "Second parameter must be an integer specifying which " +
                                   "of " + commandParams[0] + " in your inventory to get\n";
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
                    if (commandParams[0] == direction.ToString().ToLower())
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
                        output += "There is no exit leading " + direction.ToString();
                    }
                    // If the link to the room in the specified direction is not open
                    else if (!currentRoom.LinksUnlocked[(int)direction])
                    {
                        output += "The " + direction.ToString() + " exit is not open";
                    }
                    // If the room in the specified direction both exists and is open
                    else
                    {
                        game.Player.Location = currentRoom.Links[(int)direction];
                        output += "You went " + direction.ToString();
                    }
                }
                else
                {
                    output += "Parameter must be one of North, South, East, or West";
                }
            }
            else
            {
                output += "Command \"go\" syntax: [ directionToTravel: Direction ]";
            }
        }

        /*
            The ParseLookCommand method parses a "look" command for some action to perform
        */

        private void ParseLookCommand(string[] commandParams)
        {
            // We expect up to 2 parameters
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
                            output += info[i][j];
                        }

                        // If we haven't yet output the last set of information, add newlines
                        if (i != info.GetLength(0) - 1)
                        {
                            output += "\n\n";
                        }
                    }
                }
                else
                {
                    List<Creature> roomDenizens = currentRoom.Denizens;
                    Player player = game.Player;

                    if (commandParams.Length == 1)
                    {
                        // Get first instance of named creature in the current room to examine
                        Creature creature = roomDenizens.Find(denizen => denizen.Stats.name.ToLower() == commandParams[0]);
                        if (creature != null)
                        {
                            player.Target = creature;
                            output += "You observe " + creature.Stats.name;
                        }
                        else
                        {
                            output += "No such creature, " + commandParams[0] + " exists in this room";
                        }
                    }
                    else
                    {
                        // Get nth instance of named creature in the current room to examine
                        int instance;
                        if (int.TryParse(commandParams[1], out instance) && instance > 0)
                        {
                            string suffix = GetOrdinalSuffix(instance);
                            instance -= 1;      // We do not expect instance to be 0-based, so make it so
                            Creature[] creatures = roomDenizens.Where(denizen => denizen.
                                                                                 Stats.
                                                                                 name.
                                                                                 ToLower() == commandParams[0]).
                                                                                 ToArray();

                            Creature creature = (instance < creatures.Length) ? creatures[instance] : null;
                            if (creature != null)
                            {
                                player.Target = creature;
                                output += "You observe " + creature.Stats.name;
                            }
                            else
                            {
                                output += "There is no " + (instance + 1) + suffix + " " +
                                          commandParams[0] + " in this room";
                            }
                        }
                        else
                        {
                            output += "Second parameter must be an integer specifying which " +
                                      "of " + commandParams[0] + " in this room to examine";
                        }
                    }
                }
            }
            else
            {
                output += "Command \"look\" syntax: [ creatureName: string ] { instanceOf: int }";
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
                        output+= "There is no exit leading " + direction.ToString() + " to open";
                    }
                    else if (currentRoom.LinksUnlocked[(int)direction])
                    {
                        output += "The " + direction.ToString() + " exit is already open";
                    }
                    else
                    {
                        // Open link
                        currentRoom.OpenLink(currentRoom.Links[(int)direction]);
                        output += "You opened the " + direction.ToString() + " exit";
                    }
                }
                else
                {
                    output += "Invalid parameter: Must be one of North, South, East, or West";
                }
            }
            else
            {
                output += "Command \"open\" takes 1 parameter";
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
                output += "Quitting...";
            }
            else
            {
                // Output error
                output += "Error: Command \"quit\" takes no parameters";
            }
        }

        /*
            The ParseTakeCommand method parses a "take" command for some action to perform
        */

        private void ParseTakeCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
            {
                output += "Command \"take\" syntax: [ itemToTake: Item ] { instanceOf: int }\n";
            }
            else
            {
                List<Item> roomContents = game.Player.Location.Contents;
                Player player = game.Player;

                // Check for "all" keyword
                if (commandParams[0] == "all")
                {
                    // Determine number of parameters
                    if (commandParams.Length == 2)
                    {
                        if (roomContents.Exists(item => item.Name.ToLower() == commandParams[1]))
                        {
                            player.TakeAll(commandParams[1]);
                            output += "You take every " + commandParams[1] + " from the room\n";
                        }
                        else
                        {
                            output += "There are no " + commandParams[1] + "s in this room to take\n";
                        }
                    }
                    else
                    {
                        if (roomContents.Count != 0)
                        {
                            player.TakeAll();
                            output += "You take every item in the room\n";
                        }
                        else
                        {
                            output += "There are no items in this room to take\n";
                        }
                    }
                }
                else
                {
                    // Determine number of parameters
                    if (commandParams.Length == 2)
                    {
                        // Take nth instance of named item from room
                        int instance;
                        if (int.TryParse(commandParams[1], out instance))
                        {
                            string suffix = GetOrdinalSuffix(instance);
                            instance -= 1;      // We don't expect instance to be 0-based, so make it so
                            Item[] items = roomContents.Where(i => i.Name.ToLower() == commandParams[0]).ToArray();
                            Item item = (instance >= 0 && instance < items.Length) ? items[instance] : null;
                            if (item != null)
                            {
                                player.Take(item);
                                output += "You take the " + (instance + 1) + suffix + " " +
                                          item.Name + " from the room\n";
                            }
                            else
                            {
                                output += "No " + (instance + 1) + suffix + " " +
                                          commandParams[0] + " exists in this room\n";
                            }
                        }
                        else
                        {
                            output += "Second parameter must be an integer specifying which " +
                                       "of " + commandParams[0] + " to take\n";
                        }
                    }
                    else
                    {
                        // Take first instance of named item from room
                        Item item = roomContents.Find(i => i.Name.ToLower() == commandParams[0]);
                        if (item != null)
                        {
                            player.Take(item);
                            output += "You take " + item.Name + "\n";
                        }
                        else
                        {
                            output += "There is no " + commandParams[0] + " in this room to take\n";
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
