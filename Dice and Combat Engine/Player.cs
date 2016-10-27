/*
    This class represents playable characters
    9/22/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dice_and_Combat_Engine
{
    // Structs
    struct PlayerStats
    {
        public string playerClass,
                      race;

        public int experience, level;
    }

    class Player : Creature
    {
        // Fields
        private PlayerStats _playerStats;
        private List<Item> _inventory;
        private Weapon _equippedWeapon;
        private bool _leveledUp;

        /*
            Constructor
            
            Accepts Player-specific stats, base stats, attributes, and a portrait of the player
        */

        public Player(PlayerStats playerStats, BaseStats baseStats, Attributes attribs, string desc)
            : base(baseStats, attribs, desc)
        {
            _playerStats = playerStats;
            _inventory = new List<Item>();
            _equippedWeapon = null;
            _leveledUp = false;
        }

        /*
            The Attack method has the Player execute an attack against a Creature,
            reducing the defender's hitpoints and returning the damage dealt
        */

        public override int Attack(Creature defender)
        {
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
                _equippedWeapon.Use();
                if (_equippedWeapon.Durability == 0)
                {
                    _inventory.Remove(_equippedWeapon);
                    UnequipWeapon();
                }
            }
            return damage;
        }

        /*
            The Drop method has the Player drop an item from the Player's inventory into
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
            _playerStats.experience += xp;

            // Check for level up
            if (_playerStats.experience >= _playerStats.level * MODIFIER)
            {
                // Set level up notifier to true
                _leveledUp = true;
            }
        }

        /*
            The Use method has the Player use an item from its inventory
        */

        public void Use(Item item)
        {
            if (item is Weapon)
            {
                EquipWeapon((Weapon)item);
            }
            else if (item is Potion)
            {
                UsePotion((Potion)item);
            }
        }

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
                        output.Append("\n-- " + count + " " + name);
                    else
                        output.Append("\n-- and " + count + " " + name);
                }
            }
            else
                output.Append("Your inventory is lacking of items");
            return output.ToString();
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
            if (items.Length > 0 && instance < items.Length)
                return _inventory[instance];
            else
                return null;
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
            _playerStats.level += 1;

            // Upgrade player's max hitpoints
            Stats.MaxHitPoints += _playerStats.level;

            // Reset leveled up notifier
            _leveledUp = false;
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

        private void UsePotion(Potion potion)
        {
            Stats.HitPoints += potion.Use();
            if (Stats.HitPoints > Stats.MaxHitPoints)
                Stats.HitPoints = Stats.MaxHitPoints;
            if (potion.Durability == 0)
                _inventory.Remove(potion);
        }

        /*
            PlayerStats property
        */

        public PlayerStats PlayerStats
        {
            get { return _playerStats; }
            set { _playerStats = value; }
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

        /*
            LeveledUp property
        */

        public bool LeveledUp
        {
            get { return _leveledUp; }
        }
    }
}
