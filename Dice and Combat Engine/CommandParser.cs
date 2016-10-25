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
                i++;
            return commandString.Substring(0, i);
        }

        /*
            The GetOrdinalSuffix method returns the appropriate suffix for an
            integer number
        */

        public string GetOrdinalSuffix(int num)
        {
            string suffix;

            // Numbers in the range of 10-19 always have the suffix "th"
            if (num >= 10 && num <= 19)
                suffix = "th";
            else
            {
                // Get the number in the 1st place
                while (num / 10 != 0)
                    num /= 10;

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
            The IsDirection method determines whether a string represents a direction
        */

        private bool IsDirection(string str)
        {
            bool isDirection;
            if (str == "n" || str == "north")
                isDirection = true;
            else if (str == "s" || str == "south")
                isDirection = true;
            else if (str == "e" || str == "east")
                isDirection = true;
            else if (str == "w" || str == "west")
                isDirection = true;
            else
                isDirection = false;
            return isDirection;
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
                    good = true;
                else
                    i++;
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
                        ParseEquipCommand(commandParams);
                        break;
                    case "get":
                    case "use":
                        ParseGetCommand(commandParams);
                        break;
                    case "go":
                        ParseGoCommand(commandParams);
                        break;
                    case "inventory":
                        ParseInventoryCommand();
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
                output += "Bad command: " + command + "\n";
            return output;
        }

        /*
            The ParseAttackCommand method parses an "attack" command
        */

        private void ParseAttackCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"attack\" syntax: [ creatureToAttack: Creature ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                // Player attacks first instance of named creature
                Creature creature = game.Player.Location.GetDenizen(commandParams[0]);
                if (creature != null)
                {
                    game.Player.Target = creature;
                    creature.Target = game.Player;
                    output += game.CombatEngine.DoCombat(game.Player, creature);
                    if (creature != null)
                        output += game.CombatEngine.DoCombat(creature, creature.Target);
                }
                else
                    output += "No such creature exists in this room\n";
            }
            else
            {
                // Player attacks nth instance of named creature
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;       // We don't expect instance to be 0-based, so make it so
                    Creature creature = game.Player.Location.GetDenizen(commandParams[0], instance);
                    if (creature != null)
                    {
                        game.Player.Target = creature;
                        creature.Target = game.Player;
                        output += game.CombatEngine.DoCombat(game.Player, creature);
                        if (creature != null)
                            output += game.CombatEngine.DoCombat(creature, game.Player);
                    }
                    else
                        output += "No " + (instance - 1) + suffix +
                                  " creature of that name exists\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to drop\n";
            }
        }

        /*
            The ParseDirection method parses a string representing a Direction
            into its appropriate Direction type
        */

        private Direction ParseDirection(string str)
        {
            Direction direction;
            if (str == "n" || str == "north")
                direction = Direction.North;
            else if (str == "s" || str == "south")
                direction = Direction.South;
            else if (str == "e" || str == "east")
                direction = Direction.East;
            else
                direction = Direction.West;
            return direction;
        }

        /*
            The ParseDropCommand method parses a "drop" command for some action to perform
        */

        private void ParseDropCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parmeters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"drop\" syntax: [ itemName: string ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                Player player = game.Player;

                // Drop first instance of named item from PC's inventory in the current room
                Item item = player.GetItem(commandParams[0]);
                if (item != null)
                {
                    player.Drop(item);
                    output += "You drop " + item.Name + "\n";
                }
                else
                    output += "No such item exists in your inventory\n";
            }
            else
            {
                Player player = game.Player;

                // Drop nth instance of named item from PC's inventory in the current room
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;      // We don't expect instance to be 0-based, so make it so
                    Item item = player.GetItem(commandParams[0], instance);
                    if (item != null)
                    {
                        player.Drop(item);
                        output += "You drop the " + (instance + 1) + suffix + " " +
                                  item.Name + " from your inventory\n";
                    }
                    else
                        output += "No " + (instance - 1) + suffix +
                                  " item of that name is in your inventory\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to drop\n";
            }
        }

        /*
            The ParseEquipCommand method parses an "equip" command for some action to perform
        */

        private void ParseEquipCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
            {
                output += "Command \"Equip\" syntax: [ itemName: string ] { instanceOf: int }\n";
            }
            else if (commandParams.Length == 1)
            {
                // Get first instance of named item from PC's inventory to equip
                Item item = game.Player.GetItem(commandParams[0]);
                if (item != null)
                {
                    if (item is Weapon)
                    {
                        game.Player.EquipWeapon((Weapon)item);
                        int bonus = ((Weapon)item).DamageBonus;
                        output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                    }
                    else
                        output += "You cannot equip that item\n";
                }
                else
                    output += "No such item exists in your inventory\n";
            }
            else
            {
                //Get nth instance of named item from PC's inventory to equip.
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;     // We don't expect instance to be 0-based, so make it so
                    Item item = game.Player.GetItem(commandParams[0], instance);
                    if (item != null)
                    {
                        if (item is Weapon)
                        {
                            game.Player.EquipWeapon((Weapon)item);
                            int bonus = ((Weapon)item).DamageBonus;
                            output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                        }
                        else
                            output += "You cannot equip that item\n";
                    }
                    else
                        output += "No " + (instance - 1) + suffix +
                                  " item of that name is in your inventory\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to drop\n";
            }
        }

        /*
            The ParseGetCommand method parses a "get" command for some action to perform
        */

        private void ParseGetCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"get\" syntax: [ itemName: string ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                Player player = game.Player;
                Room currentRoom = player.Location;

                // Get first instance of named item from PC's inventory to use
                Item item = currentRoom.GetItem(commandParams[0]);
                if (item != null)
                {
                    if (item is Weapon)
                    {
                        player.Use(item);
                        int bonus = ((Weapon)item).DamageBonus;
                        output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                    }
                    else if (item is Potion)
                    {
                        player.Use(item);
                        int restored = ((Potion)item).HealthRestored;
                        output += "You use " + item.Name + ", restoring your health by " + restored + "\n";
                    }
                    else
                        output += "You cannot use the item, " + item.Name + "\n";
                }
                else
                    output += "No such item exists in your inventory\n";
            }
            else
            {
                Player player = game.Player;
                Room currentRoom = player.Location;

                // Get nth instance of named item from PC's inventory to use
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;     // We don't expect instance to be 0-based, so make it so
                    Item item = currentRoom.GetItem(commandParams[0], instance);
                    if (item != null)
                    {
                        if (item is Weapon)
                        {
                            player.Use(item);
                            int bonus = ((Weapon)item).DamageBonus;
                            output += "You equip " + item.Name + ", increasing your damage by " + bonus + "\n";
                        }
                        else if (item is Potion)
                        {
                            player.Use(item);
                            int restored = ((Potion)item).HealthRestored;
                            output += "You use " + item.Name + ", restoring your health by " + restored + "\n";
                        }
                        else
                            output += "You cannot use the item, " + item.Name + "\n";
                    }
                    else
                        output += "No " + (instance - 1) + suffix +
                                  " item of that name is in your inventory\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to get\n";
            }
        }

        /*
            The ParseGoCommand method parses a "go" command for some action to perform
        */

        private void ParseGoCommand(string[] commandParams)
        {
            // We expect only 1 parameter: The direction to travel in
            if (commandParams.Length == 1)
                if (IsDirection(commandParams[0]))
                {
                    Direction direction = ParseDirection(commandParams[0]);
                    Player player = game.Player;
                    Room currentRoom = player.Location;

                    // If a room in the specified direction does not exist
                    if (currentRoom.Links[(int)direction] == null)
                        output += "There is no exit leading " + direction.ToString() + "\n";

                    // If the link to the room in the specified direction is not open
                    else if (!currentRoom.LinksUnlocked[(int)direction])
                        output += "The " + direction.ToString() + " exit is not open\n";

                    // If the room in the specified direction both exists and is open
                    else
                    {
                        player.Go(direction);
                        output += "You go " + direction.ToString() + "\n";
                    }
                }
                else
                    output += "Parameter must be a cardinal direction\n";
            else
                output += "Command \"go\" syntax: [ directionToTravel: Direction ]\n";
        }

        /*
            The ParseInventoryCommand method parses an "inventory" command for some action to perform
        */

        private void ParseInventoryCommand()
        {
            output += "Inventory:";

            Player player = game.Player;

            // Check if there are items in the inventory.
            if (player.Inventory.Count > 0)
            {
                int i = 0;      // Accumulator, the number for 
                                // an item in the inventory

                foreach (Item item in player.Inventory)
                {
                    // Print on new line.
                    output += "\n";

                    i++;

                    // Check if the item is equipped.
                    if (item == player.EquippedWeapon)
                    {
                        // Display item with asterisk.
                        output += i + ". " + "*" + item.Name;
                    }
                    else
                    {
                        // Display item as usual, w/o asterisk.
                        output += i + ". " + item.Name;
                    }
                }
                // New line for spacing/readability.
                output += "\n";
            }
            else
                output += "Your inventory is lacking of items\n";
        }

        /*
            The ParseLookCommand method parses a "look" command for some action to perform
        */

        private void ParseLookCommand(string[] commandParams)
        {
            // We expect up to 2 parameters
            if (commandParams.Length > 3)
                output += "Command \"look\" syntax: { creatureName: string } { instanceOf: int }\n";
            else if (commandParams.Length == 0)
            {
                // Provide general information about the Player's current location
                Room currentRoom = game.Player.Location;
                output += currentRoom.GetInfo() + "\n";
            }
            else if (commandParams.Length == 1)
            {
                // Output description of object and, if creature, set Player's target
                Player player = game.Player;
                Room currentRoom = player.Location;
                if (currentRoom.GetDenizenNames().Contains(commandParams[0], StringComparer.OrdinalIgnoreCase))
                {
                    player.Target = currentRoom.GetDenizen(commandParams[0]);
                    output += player.Target.Description + "\n";
                }
                else if (currentRoom.GetItemNames().Contains(commandParams[0], StringComparer.OrdinalIgnoreCase))
                    output += currentRoom.GetItem(commandParams[0]).Description + "\n";
                else
                    output += "No object with that name exists\n";
            }
            else
            {
                // Output description of nth instance of object and, if creature, set Player's target
                Player player = game.Player;
                Room currentRoom = player.Location;
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;      // We do not expect instance to be 0-based, so make it so
                    if (game.IsCreature(commandParams[0]))
                    {
                        Creature creature = currentRoom.GetDenizen(commandParams[0], instance);
                        if (creature != null)
                        {
                            player.Target = creature;
                            output += player.Target.Description + "\n";
                        }
                        else
                            output += "No " + (instance + 1) + suffix + " instance of that creature exists\n";
                    }
                    else if (game.IsItem(commandParams[0]))
                    {
                        Item item = currentRoom.GetItem(commandParams[0], instance);
                        if (item != null)
                            output += item.Description + "\n";
                        else
                            output += "No " + (instance + 1) + suffix + " instance of that item exists\n";
                    }
                    else
                        output += "No object with that name exists\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to examine\n";
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
                if (IsDirection(commandParams[0]))
                {
                    Direction direction = ParseDirection(commandParams[0]);
                    Room currentRoom = game.Player.Location;

                    // If a room in the specified direction does not exist
                    if (currentRoom.Links[(int)direction] == null)
                        output += "There is no exit leading " + direction.ToString() + " to open\n";

                    // If the link to the room in the specified direction is already open
                    else if (currentRoom.LinksUnlocked[(int)direction])
                        output += "The " + direction.ToString() + " exit is already open\n";

                    // Open link
                    else
                    {
                        currentRoom.OpenLink(currentRoom.Links[(int)direction]);
                        output += "You opened the " + direction.ToString() + " exit\n";
                    }
                }
                else
                    output += "Parameter must be a cardinal direction\n";
            }
            else
                output += "Command \"open\" syntax: [ directionToOpen: Direction ]\n";
        }

        /*
            The ParseQuitCommand method parses a "quit" command for some action to perform
        */

        private void ParseQuitCommand(string[] commandParams)
        {
            // We expect no parameters
            if (commandParams.Length > 0)
                output += "Command \"quit\" takes no parameters\n";
            else
                output += "Quitting...\n";
        }

        /*
            The ParseTakeCommand method parses a "take" command for some action to perform
        */

        private void ParseTakeCommand(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"take\" syntax: [ itemToTake: Item ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                // Check for "all" keyword
                if (commandParams[0] == "all")
                {
                    if (game.Player.Location.Contents.Count != 0)
                    {
                        // Take every item from Player's current location
                        game.Player.TakeAll();
                        output += "You take every item in the room\n";
                    }
                    else
                        output += "There are no items in this room to take\n";
                }
                else
                {
                    // Take first instance of item with specified name
                    Item item = game.Player.Location.GetItem(commandParams[0]);
                    if (item != null)
                    {
                        game.Player.Take(item);
                        output += "You take " + item.Name + "\n";
                    }
                    else
                        output += "No item of that name exists\n";
                }
            }
            else
            {
                // Check for "all" keyword
                if (commandParams[0] == "all")
                {
                    // Take every item with specified name from the room
                    Item[] items = game.Player.Location.GetItems(commandParams[0]);
                    if (items.Length > 0)
                    {
                        game.Player.TakeAll(commandParams[0]);
                        output += "You take every " + items[0].Name + "\n";
                    }
                    else
                        output += "No items with that name exists\n";
                }
                else
                {
                    // Take nth instance of named item from room
                    int instance;
                    if (int.TryParse(commandParams[1], out instance) && instance > 0)
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance -= 1;      // We don't expect instance to be 0-based, so make it so
                        Item item = game.Player.Location.GetItem(commandParams[0], instance);
                        if (item != null)
                        {
                            game.Player.Take(item);
                            output += "You take the " + (instance + 1) + suffix + " " +
                                      item.Name + " from the room\n";
                        }
                        else
                            output += "No " + (instance + 1) + suffix + " instance of that item exists\n";
                    }
                    else
                        output += "Second parameter must be an integer for the instance of the object to examine\n";
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
            string[] toReturn;

            if (parameters.Length >= 2)
            {
                // Prepare variables
                List<string> paramList = new List<string>(parameters);
                List<string> names = new List<string>();
                StringBuilder resolvedParam = new StringBuilder();
                int start = 0;
                int current = 0;

                // While there are more parameters
                while (start < paramList.Count)
                {
                    resolvedParam.Append(paramList[start]);
                    names.AddRange(game.GetObjectNames().Where(name => name.StartsWith(paramList[start])));

                    // While matching names list is not empty and there are more parameters
                    while (names.Count != 0 && current != paramList.Count - 1)
                    {
                        // If name list contains elements that start with the resolved parameter and the next parameter joined
                        if (names.Find(name => name.StartsWith(resolvedParam.ToString() + " " + paramList[current + 1])) != null)
                        {
                            // Join the resolved parameter and the next parameter together into a single parameter
                            resolvedParam.Append(" " + paramList[current + 1]);

                            // Increment current to point to the next parameter which we have expanded to
                            current++;

                            // Check if the currently resolved parameter matches exactly with an object name
                            // In which case, a compound parameter has definitely been resolved
                            if (names.Contains(resolvedParam.ToString()))
                            {
                                // Set start parameter to the resolved parameter
                                paramList[start] = resolvedParam.ToString();

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
                            names.RemoveAll(name => !name.StartsWith(resolvedParam.ToString()));
                        }
                        else
                            names.Clear();
                    }

                    // Increment start to point to the next parameter in the parameter list
                    start++;
                    resolvedParam.Clear();
                }

                // Prepare to return possibly modifed array
                toReturn = paramList.ToArray();
            }
            else
                // Parameters are unchanged
                toReturn = parameters;

            // Return
            return toReturn;
        }
    }
}
