/*
    This class represents a container for other items
    10/30/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class Container : Item
    {
        // Fields
        private const ItemType TYPE = ItemType.Container;

        private Item[] _items;

        /*
            Constructor
        */

        public Container(string name, string desc, int durability, int value, int max)
            : base(name, desc, durability, value, TYPE)
        {
            _items = new Item[max];
        }

        /*
            Copy Constructor
        */

        public Container(Container container)
            : base(container.Name, container.Description, container.Durability, container.Value, TYPE)
        {
            _items = new Item[container.Items.Length];
            for (int i = 0; i < _items.Length; i++)
                if (container.Items[i] != null)
                {
                    if (container.Items[i] is Weapon)
                        _items[i] = new Weapon((Weapon)container.Items[i]);
                    else if (container.Items[i] is Potion)
                        _items[i] = new Potion((Potion)container.Items[i]);
                    else
                        _items[i] = new Treasure((Treasure)container.Items[i]);
                }
        }

        /*
            The AddItem method adds an item to the Container
        */

        public void AddItem(Item item)
        {
            bool added = false;
            for (int i = 0; i < _items.Length && !added; i++)
                if (_items[i] == null)
                {
                    _items[i] = item;
                    added = true;
                }
        }

        /*
            The GetItems method gets and returns every item from the container
        */

        public Item[] GetItems()
        {
            Item[] toReturn = new Item[_items.Length];
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] is Weapon)
                    toReturn[i] = new Weapon((Weapon)_items[i]);
                else if (_items[i] is Potion)
                    toReturn[i] = new Potion((Potion)_items[i]);
                else
                    toReturn[i] = new Treasure((Treasure)_items[i]);

                _items[i] = null;
            }
            return toReturn;
        }

        /*
            Items Property
        */

        public Item[] Items
        {
            get { return _items; }
        }
    }
}
