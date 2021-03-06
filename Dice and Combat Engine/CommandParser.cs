﻿/*
    This class represents a command-line parser
    10/30/2016
    CSC 253 0001 - CH8P1
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
        private static string[] commands = { "attack", "clear", "drop", "equip", "examine", "get",
                                             "go", "inventory", "look", "open", "quit",
                                             "score", "take", "talk", "use" };

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
                        ParseAttack(commandParams);
                        break;
                    case "clear":
                        ParseClear(commandParams);
                        break;
                    case "drop":
                        ParseDrop(commandParams);
                        break;
                    case "equip":
                        ParseEquip(commandParams);
                        break;
                    case "use":
                        ParseUse(commandParams);
                        break;
                    case "go":
                        ParseGo(commandParams);
                        break;
                    case "inventory":
                        ParseInventory(commandParams);
                        break;
                    case "look":
                    case "examine":
                        ParseLook(commandParams);
                        break;
                    case "open":
                        ParseOpen(commandParams);
                        break;
                    case "quit":
                        ParseQuit(commandParams);
                        break;
                    case "score":
                        ParseScore(commandParams);
                        break;
                    case "talk":
                        ParseTalk(commandParams);
                        break;
                    case "take":
                    case "get":
                        ParseTake(commandParams);
                        break;
                }
            }
            else
                output += "Bad command: " + command + "\n";
            return output;
        }

        /*
            The ParseAttack method parses an "attack" command for some action to perform
        */

        private void ParseAttack(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"attack\" syntax: [ creatureToAttack: Creature ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                if (game.Player.Location.ContainsDenizen(commandParams[0]))
                {
                    // Player attacks first instance of named creature
                    Creature c = game.Player.Location.GetDenizen(commandParams[0]);
                    if (!(c is NPC))
                    {
                        output += game.CombatEngine.DoCombat(game.Player, c);
                        if (c.Stats.HitPoints > 0)
                            output += game.CombatEngine.DoCombat(c, game.Player);
                    }
                    else
                        output += "You cannot attack that NPC\n";
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
                    if (game.Player.Location.ContainsDenizen(commandParams[0], instance))
                    {
                        Creature c = game.Player.Location.GetDenizen(commandParams[0], instance);
                        if (!(c is NPC))
                        {
                            output += game.CombatEngine.DoCombat(game.Player, c);
                            if (c.Stats.HitPoints > 0)
                                output += game.CombatEngine.DoCombat(c, game.Player);
                        }
                        else
                            output += "You cannot attack that NPC\n";
                    }
                    else
                        output += "No " + (instance + 1) + suffix +
                                  " creature of that name exists\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the creature to attack\n";
            }
        }

        /*
            The ParseClear method parses a "clear" command for some action to perform
        */

        private void ParseClear(string[] commandParams)
        {
            // We expect no parameters
            if (commandParams.Length == 0)
                game.RequestToClear = true;
            else
                output += "Command \"clear\" takes no parameters\n";
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
            The ParseDrop method parses a "drop" command for some action to perform
        */

        private void ParseDrop(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parmeters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"drop\" syntax: [ itemName: string ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                if (game.Player.HasItem(commandParams[0]))
                {
                    // Drop first instance of named item from PC's inventory in the current room
                    Item item = game.Player.GetItem(commandParams[0]);
                    game.Player.Drop(item);
                    output += "You drop " + item.Name + "\n";
                }
                else
                    output += "No such item exists in your inventory\n";
            }
            else
            {
                // Drop nth instance of named item from PC's inventory in the current room
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;      // We don't expect instance to be 0-based, so make it so
                    if (game.Player.HasItem(commandParams[0], instance))
                    {
                        Item i = game.Player.GetItem(commandParams[0], instance);
                        game.Player.Drop(i);
                        output += "You drop the " + (instance + 1) + suffix + " " +
                                  i.Name + " from your inventory\n";
                    }
                    else
                        output += "No " + (instance + 1) + suffix +
                                  " item of that name is in your inventory\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to drop\n";
            }
        }

        /*
            The ParseEquip method parses an "equip" command for some action to perform
        */

        private void ParseEquip(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"Equip\" syntax: [ itemName: string ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                if (game.Player.HasItem(commandParams[0]))
                {
                    // Get first instance of named item from PC's inventory to equip
                    Item i = game.Player.GetItem(commandParams[0]);
                    if (i is Weapon)
                    {
                        game.Player.EquipWeapon((Weapon)i);
                        int bonus = ((Weapon)i).DamageBonus;
                        output += "You equip " + i.Name + ", increasing your damage by " + bonus + "\n";
                    }
                    else
                        output += "You cannot equip that item\n";
                }
                else
                    output += "No such item exists in your inventory\n";
            }
            else
            {
                // Get nth instance of named item from PC's inventory to equip.
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;     // We don't expect instance to be 0-based, so make it so
                    if (game.Player.HasItem(commandParams[0], instance))
                    {
                        Item i = game.Player.GetItem(commandParams[0], instance);
                        if (i is Weapon)
                        {
                            game.Player.EquipWeapon((Weapon)i);
                            int bonus = ((Weapon)i).DamageBonus;
                            output += "You equip " + i.Name + ", increasing your damage by " + bonus + "\n";
                        }
                        else
                            output += "You cannot equip that item\n";
                    }
                    else
                        output += "No " + (instance + 1) + suffix +
                                  " item of that name is in your inventory\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the item to equip\n";
            }
        }

        /*
            The ParseGo method parses a "go" command for some action to perform
        */

        private void ParseGo(string[] commandParams)
        {
            // We expect only 1 parameter: The direction to travel in
            if (commandParams.Length != 1)
                output += "Command \"go\" syntax: [ directionToTravel: Direction ]\n";
            else if (IsDirection(commandParams[0]))
            {
                Direction direction = ParseDirection(commandParams[0]);

                if (!game.Player.Location.IsLinked(direction))
                    output += "There is no exit leading " + direction.ToString() + "\n";
                else if (!game.Player.Location.IsLinkOpen(direction))
                    output += "The " + direction.ToString() + " exit is not open\n";
                else
                {
                    game.Player.Go(direction);
                    output += "You go " + direction.ToString() + "\n";
                }
            }
            else
                output += "Parameter must be a cardinal direction\n";
        }

        /*
            The ParseInventory method parses an "inventory" command for some action to perform
        */

        private void ParseInventory(string[] commandParams)
        {
            // We expect no parameters
            if (commandParams.Length == 0)
                // Display the contents of the Player's inventory
                output += game.Player.GetInventoryInformation() + "\n";
            else
                output += "Command \"inventory\" takes no parameters\n";
        }

        /*
            The ParseLook method parses a "look" command for some action to perform
        */

        private void ParseLook(string[] commandParams)
        {
            // We expect up to 2 parameters
            if (commandParams.Length > 2)
                output += "Command \"look\" syntax: { objectName: string } { instanceOf: int }\n";
            else if (commandParams.Length == 0)
                // Display general information about the Player's current location
                output += game.Player.Location.GetInfo() + "\n";
            else if (commandParams.Length == 1)
            {
                // Output description of first instance of object and, if creature, set Player's target
                if (game.Player.Location.ContainsDenizen(commandParams[0]))
                {
                    game.Player.Target = game.Player.Location.GetDenizen(commandParams[0]);
                    output += game.Player.Target.Description + "\n";
                }
                else if (game.Player.Location.IsCleared() && game.Player.Location.ContainsItem(commandParams[0]))
                    output += game.Player.Location.GetItem(commandParams[0]).Description + "\n";
                else if (game.Player.HasItem(commandParams[0]))
                    output += game.Player.GetItem(commandParams[0]).Description + "\n";
                else
                    output += "No object with that name exists\n";
            }
            else
            {
                // Output description of nth instance of object and, if creature, set Player's target
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;      // We don't expect instance to be 0-based, so make it so
                    if (game.Player.Location.ContainsDenizen(commandParams[0], instance))
                    {
                        game.Player.Target = game.Player.Location.GetDenizen(commandParams[0], instance);
                        output += game.Player.Target.Description + "\n";
                    }
                    else if (game.Player.Location.IsCleared() && game.Player.Location.ContainsItem(commandParams[0], instance))
                        output += game.Player.Location.GetItem(commandParams[0], instance).Description + "\n";
                    else if (game.Player.HasItem(commandParams[0], instance))
                        output += game.Player.GetItem(commandParams[0], instance).Description + "\n";
                    else
                        output += "No " + (instance + 1) + suffix +
                                  " object of that name exists\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to examine\n";
            }
        }

        /*
            The ParseOpen method parses an "open" command for some action to perform
        */

        private void ParseOpen(string[] commandParams)
        {
            // We expect only 1 parameter: The object or direction of the exit to open
            if (commandParams.Length != 1)
                output += "Command \"open\" syntax: [ objectToOpen: Object ]\n";
            else if (IsDirection(commandParams[0]))
            {
                Direction direction = ParseDirection(commandParams[0]);

                if (!game.Player.Location.IsLinked(direction))
                    output += "There is no exit leading " + direction.ToString() + " to open\n";
                else if (game.Player.Location.IsLinkOpen(direction))
                    output += "The " + direction.ToString() + " exit is already open\n";
                else if (!game.Player.Location.IsCleared())
                    output += "There are enemies remaining in this room\n";
                else
                {
                    game.Player.Location.OpenLink(direction);
                    output += "You opened the " + direction.ToString() + " exit\n";
                }
            }
            else if (game.Player.Location.IsCleared() && game.Player.Location.ContainsItem(commandParams[0]))
            {
                Item item = game.Player.Location.GetItem(commandParams[0]);
                if (item is Container)
                {
                    game.Player.Open((Container)item);
                    output += "You open the " + item.Name + " and take its contents\n";
                }
                else
                    output += "You cannot open " + item.Name + "\n";
            }
            else
                output += "Parameter must be an object or exit to open\n";
        }

        /*
            The ParseQuit method parses a "quit" command for some action to perform
        */

        private void ParseQuit(string[] commandParams)
        {
            // We expect no parameters
            if (commandParams.Length == 0)
                game.RequestToQuit = true;
            else
                output += "Command \"quit\" takes no parameters\n";
        }

        /*
            The ParseScore method parses a "score" command for some action to perform 
        */

        private void ParseScore(string[] commandParams)
        {
            // We expect no parameters
            if (commandParams.Length == 0)
                output += "Your score is " + game.Player.GetScore() + "\n";
            else
                output += "Command \"score\" takes no parameters\n";
        }

        /*
            The ParseTake method parses a "take" command for some action to perform
        */

        private void ParseTake(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"take\" syntax: [ itemToTake: Item ] { instanceOf: int }\n";
            else if (commandParams.Length == 1 && game.Player.Location.IsCleared())
            {
                // Check for "all" keyword
                if (commandParams[0] == "all")
                {
                    if (game.Player.Location.Contents.Count != 0)
                    {
                        // Player takes every item from its current location
                        game.Player.TakeAll();
                        output += "You take every item in the room\n";
                    }
                    else
                        output += "There are no items in this room to take\n";
                }
                else if (game.Player.Location.ContainsItem(commandParams[0]))
                {
                    // Take first instance of named item from room
                    Item i = game.Player.Location.GetItem(commandParams[0]);
                    if (!(i is Container))
                    {
                        game.Player.Take(i);
                        output += "You take " + i.Name + "\n";
                    }
                    else
                        output += "You cannot take that item\n";
                }
                else
                    output += "No item of that name exists\n";
            }
            else if (commandParams.Length == 2 && game.Player.Location.IsCleared())
            {
                // Check for "all" keyword
                if (commandParams[0] == "all")
                {
                    if (game.Player.Location.ContainsItem(commandParams[1]))
                    {
                        // Take every item with specified name from the room
                        Item i = game.Player.Location.GetItem(commandParams[1]);
                        if (!(i is Container))
                        {
                            game.Player.TakeAll(commandParams[1]);
                            output += "You take every " + i.Name + " from the room\n";
                        }
                        else
                            output += "You cannot take that item\n";
                    }
                    else
                        output += "No items with that name exist\n";
                }
                else
                {
                    // Take nth instance of named item from room
                    int instance;
                    if (int.TryParse(commandParams[1], out instance) && instance > 0)
                    {
                        string suffix = GetOrdinalSuffix(instance);
                        instance--;      // We don't expect instance to be 0-based, so make it so
                        if (game.Player.Location.ContainsItem(commandParams[0], instance))
                        {
                            Item item = game.Player.Location.GetItem(commandParams[0], instance);
                            if (!(item is Container))
                            {
                                game.Player.Take(item);
                                output += "You take the " + (instance + 1) + suffix + " " +
                                          item.Name + " from the room\n";
                            }
                            else
                                output += "You cannot take that item\n";
                        }
                        else
                            output += "No " + (instance + 1) + suffix + " item of that name exists\n";
                    }
                    else
                        output += "Second parameter must be an integer for the instance of the object to take\n";
                }
            }
            else
                output += "There are no items in this room to take\n";
        }

        /*
            The ParseTalk method parses a "talk" command for some action to perform
        */

        private void ParseTalk(string[] commandParams)
        {
            // We expect only 1 parameter: The name of the NPC to talk to
            if (commandParams.Length != 1)
                output += "Command \"talk\" syntax: [ toTalkTo: NPC ] { instanceOf: int }\n";
            else if (game.Player.Location.ContainsDenizen(commandParams[0]))
            {
                // Talk to first instance of named NPC
                Creature c = game.Player.Location.GetDenizen(commandParams[0]);
                if (c is NPC)
                    output += ((NPC)c).GetResponse() + "\n";
                else
                    output += "You cannot talk to that creature\n";
            }
            else
                output += "No NPC with that name exists\n";
        }

        /*
            The ParseUse method parses a "use" command for some action to perform
        */

        private void ParseUse(string[] commandParams)
        {
            // We expect at least 1 and up to 2 parameters
            if (commandParams.Length == 0 || commandParams.Length > 2)
                output += "Command \"use\" syntax: [ itemToUse: Item ] { instanceOf: int }\n";
            else if (commandParams.Length == 1)
            {
                if (game.Player.HasItem(commandParams[0]))
                {
                    // Get first instance of named item from PC's inventory to use
                    Item i = game.Player.GetItem(commandParams[0]);

                    if (i is Weapon)
                    {
                        game.Player.EquipWeapon((Weapon)i);
                        int bonus = ((Weapon)i).DamageBonus;
                        output += "You equip " + i.Name + ", increasing your damage by " + bonus + "\n";
                    }
                    else if (i is Potion)
                    {
                        game.Player.UsePotion((Potion)i);
                        int restored = ((Potion)i).HealthRestored;
                        output += "You drink " + i.Name + ", restoring your health by " + restored + "\n";
                    }
                    else
                        output += "You cannot use the item, " + i.Name + "\n";
                }
                else
                    output += "No such item exists in your inventory\n";
            }
            else
            {
                // Get nth instance of named item from PC's inventory to use
                int instance;
                if (int.TryParse(commandParams[1], out instance) && instance > 0)
                {
                    string suffix = GetOrdinalSuffix(instance);
                    instance--;     // We don't expect instance to be 0-based, so make it so
                    if (game.Player.HasItem(commandParams[0], instance))
                    {
                        Item i = game.Player.GetItem(commandParams[0], instance);

                        if (i is Weapon)
                        {
                            game.Player.EquipWeapon((Weapon)i);
                            int bonus = ((Weapon)i).DamageBonus;
                            output += "You equip " + i.Name + ", increasing your damage by " + bonus + "\n";
                        }
                        else if (i is Potion)
                        {
                            game.Player.UsePotion((Potion)i);
                            int restored = ((Potion)i).HealthRestored;
                            output += "You drink " + i.Name + ", restoring your health by " + restored + "\n";
                        }
                        else
                            output += "You cannot use the item, " + i.Name + "\n";
                    }
                    else
                        output += "No " + (instance + 1) + suffix +
                                  " item of that name is in your inventory\n";
                }
                else
                    output += "Second parameter must be an integer for the instance of the object to use\n";
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

                    // Increment start and current to point to the next parameter in the parameter list
                    start++;
                    current++;
                    resolvedParam.Clear();
                }

                // Prepare to return parameters
                toReturn = paramList.ToArray();
            }
            else
                // Parameters are unchanged
                toReturn = parameters;

            return toReturn;
        }
    }
}
