/*
    This class represents playable characters
    9/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_and_Combat_Engine
{
    class Player : Creature
    {
        // Fields
        private bool _leveledUp;
        private int _experience, _level;
        private List<Item> _inventory;
        private Weapon _equippedWeapon;

        /*
            Constructor
            Accepts stats
        */

        public Player(BaseStats stats, string desc)
            : base(new BaseStats(stats), desc)
        {
            _leveledUp = false;
            _experience = 0;
            _level = 1;
            _inventory = new List<Item>();
            _equippedWeapon = null;
        }

        /*
            The Attack method has the Player execute an attack against a Creature,
            reducing the defender's hitpoints and returning the damage dealt
        */

        public override int Attack(Creature defender)
        {
            Target = defender;
            Stats.Damage.Roll();
            int damage = Stats.Damage.DieResult;
            defender.Stats.HitPoints -= damage;

            if (defender.Stats.HitPoints <= 0)
            {
                Location.Denizens.Remove(defender);
                GainExperience(defender.Stats.XPValue);
                defender = null;
                Target = null;
            }

            if (_equippedWeapon != null)
            {
                _equippedWeapon.Durability -= 1;
                if (_equippedWeapon.Durability == 0)
                {
                    _inventory.Remove(_equippedWeapon);
                    UnequipWeapon();
                }
            }
            return damage;
        }

        /*
            The Drop method has the Player drop an item from its inventory into
            the Player's current location
        */

        public void Drop(Item item)
        {
            _inventory.Remove(item);
            Location.Contents.Add(item);
        }

        /*
            The EquipWeapon method has the player equip a weapon
        */

        public void EquipWeapon(Weapon weapon)
        {
            _equippedWeapon = weapon;
            Stats.Damage.DieSize += _equippedWeapon.DamageBonus;
        }

        /*
            The GainExperience method simulates the player gaining experience points
            The method accepts the number of experience points earned as an argument
        */

        public void GainExperience(int xp)
        {
            // Experience needed to level up modifier
            const int MODIFIER = 25;

            // Add experience to player
            _experience += xp;

            // Check for level up
            if (_experience >= _level * MODIFIER)
                // Set level up notifier to true
                _leveledUp = true;
        }

        /*
            The GetInventoryInformation method returns a string containing information
            about the number and kinds of items in the Player's inventory
        */

        public string GetInventoryInformation()
        {
            StringBuilder output = new StringBuilder();
            List<string> items = new List<string>();

            // Get item names
            foreach (Item item in _inventory)
                if (!items.Contains(item.Name))
                    items.Add(item.Name);

            // Build output
            if (items.Count > 0)
            {
                output.Append("Inventory:");
                for (int i = 0; i < items.Count; i++)
                {
                    int count = _inventory.Where(item => item.Name == items[i]).Count();
                    string name = (count > 1) ? items[i] + "s" : items[i];

                    // Format
                    if (items.Count == 1)
                        output.Append("\n-- " + count + " " + name);
                    else if (i < items.Count - 1)
                        output.Append("\n-- " + count + " " + name + ",");
                    else
                        output.Append("\n-- and " + count + " " + name);
                }
            }
            else
                output.Append("Your inventory is empty");
            return output.ToString();
        }

        /*
            The GetInventoryNames method returns the names of items contained
            in the Player's inventory
        */

        public string[] GetInventoryNames()
        {
            List<string> names = new List<string>();
            foreach (Item item in _inventory)
                if (!names.Contains(item.Name))
                    names.Add(item.Name);
            return names.ToArray();
        }

        /*
            The GetItem method returns the item with the specified name
            from the Player's inventory; If no item with that name exists,
            null is returned
        */

        public Item GetItem(string name)
        {
            return _inventory.Find(i => i.Name.ToLower() == name.ToLower());
        }

        /*
            The GetItem method returns the nth instance of the item
            with the specified name from the Player's inventory; if
            no item of the specified instance or name exists,
            null is returned
        */

        public Item GetItem(string name, int instance)
        {
            Item[] items = _inventory.Where(i => i.Name.ToLower() == name.ToLower()).ToArray();
            Item item = (items.Length > 0 && instance < items.Length) ? items[instance] : null;
            return item;
        }

        /*
            The GetItems method returns every item in the Player's inventory
            with the specified name
        */

        public Item[] GetItems(string name)
        {
            return _inventory.Where(i => i.Name.ToLower() == name.ToLower()).ToArray();
        }

        /*
           The Go method has the Player move from its current location to
           another location in the specifed direction
        */

        public void Go(Direction direction)
        {
            Location = Location.Links[(int)direction];
        }

        /*
            The LevelUp method levels up the player
        */

        public void LevelUp()
        {
            // Raise player's level
            _level += 1;

            // Upgrade player's max hitpoints
            Stats.MaxHitPoints += _level;

            // Reset leveled up notifier
            _leveledUp = false;
        }

        /*
            The Score method returns the total score the Player has earned, which
            will be the accumulated value of each Treasure item the player has
            in its inventory's value
        */

        public int GetScore()
        {
            int score = 0;
            Item[] items = _inventory.Where(i => (i is Treasure)).ToArray();
            foreach (Item item in items)
                score += item.Value;
            return score;
        }

        /*
            The Take method has the Player take an item from the Player's current location
            and place it into the Player's inventory
        */

        public void Take(Item item)
        {
            Location.Contents.Remove(item);
            _inventory.Add(item);
        }

        /*
            The TakeAll method has the Player take all items from the Player's current
            location and place them into the Player's inventory
        */

        public void TakeAll()
        {
            _inventory.AddRange(Location.Contents);
            Location.Contents.Clear();
        }

        /*
            The TakeAll method has the Player take all items of a specified name from
            the Player's current location and place them into the Player's inventory
        */

        public void TakeAll(string itemName)
        {
            _inventory.AddRange(Location.GetItems(itemName));
            Location.Contents.RemoveAll(i => i.Name.ToLower() == itemName.ToLower());
        }

        /*
            The UnequipWeapon method has the player unequip a weapon
        */

        public void UnequipWeapon()
        {
            Stats.Damage.DieSize -= _equippedWeapon.DamageBonus;
            _equippedWeapon = null;
        }

        /*
            The UsePotion method has the player use a potion
        */

        public void UsePotion(Potion potion)
        {
            Stats.HitPoints += potion.HealthRestored;
            if (Stats.HitPoints > Stats.MaxHitPoints)
                Stats.HitPoints = Stats.MaxHitPoints;
            potion.Durability -= 1;
            if (potion.Durability == 0)
                _inventory.Remove(potion);
        }

        /*
            LeveledUp property
        */

        public bool LeveledUp
        {
            get { return _leveledUp; }
        }

        /*
            Experience Property
        */

        public int Experience
        {
            get { return _experience; }
        }

        /*
            Level Property
        */

        public int Level
        {
            get { return _level; }
        }

        /*
            Inventory property
        */

        public List<Item> Inventory
        {
            get { return _inventory; }
        }

        /*
            EquippedWeapon property
        */

        public Weapon EquippedWeapon
        {
            get { return _equippedWeapon; }
        }
    }
}