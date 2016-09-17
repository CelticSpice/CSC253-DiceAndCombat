/*
    This class handles the combat logic
    9/5/2016
    CSC 253 0001 - Dice and Combat Engine
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Collections.Generic;

namespace Dice_and_Combat_Engine
{
    class CombatEngine
    {
        // Fields
        private bool _combatActive;
        private Creature[] _combatants;
        private int _playerSelectedTarget;
        private List<string> _combatFeedback;
        private RandomDie d20Die;

        /*
            Constructor
        */

        public CombatEngine()
        {
            _combatActive = false;
            _combatants = null;
            _playerSelectedTarget = -1;
            _combatFeedback = new List<string>();
            d20Die = new RandomDie(20);
        }

        /*
            The InitCombat method initializes combat
        */

        public void InitCombat(Creature[] newCombatants)
        {
            _combatActive = true;

            _combatants = new Creature[newCombatants.Length];

            // Get combatants
            for (int i = 0; i < _combatants.Length; i++)
            {
                _combatants[i] = newCombatants[i];
            }

            // Initialize initiative
            int[] initiatives = new int[_combatants.Length];         // A parallel relationship shall exist between
                                                                    // combatants and initiatives
            for (int i = 0; i < _combatants.Length; i++)
            {                                                       // That is, combatants shall be ordered
                int initiative;                                     // by initiative

                // Combatant rolls for initiative
                d20Die.Roll();
                initiative = d20Die.DieResult + _combatants[i].Attributes.dexterity;

                // Record initiative result for combatant
                initiatives[i] = initiative;
            }

            // Check for ties in initiative rolls
            bool tie;

            do
            {
                tie = false;
                int i = 0;
                int j = 1;

                while (i < (initiatives.Length - 1) && !tie)
                {
                    if (initiatives[i] == initiatives[j])
                    {
                        tie = true;

                        // Reroll for combatants tied until tie is broken
                        while (initiatives[i] == initiatives[j])
                        {
                            int initiative;

                            d20Die.Roll();
                            initiative = d20Die.DieResult + _combatants[i].Attributes.dexterity;
                            initiatives[i] = initiative;

                            d20Die.Roll();
                            initiative = d20Die.DieResult + _combatants[j].Attributes.dexterity;
                            initiatives[j] = initiative;
                        }
                    }
                    else
                    {
                        i++;
                        j++;
                    }
                }
            } while (tie);

            // Order combatants and initiatives sequentially, in descending order of initiative
            for (int i = 0; i < (initiatives.Length - 1); i++)
            {
                for (int j = (i + 1); j < initiatives.Length; j++)
                {
                    if (initiatives[j] > initiatives[i])
                    {
                        int tmp = initiatives[i];
                        initiatives[i] = initiatives[j];
                        initiatives[j] = tmp;

                        Creature tmpCombatant = _combatants[i];
                        _combatants[i] = _combatants[j];
                        _combatants[j] = tmpCombatant;
                    }
                }
            }
        }

        /*
            The TakeCombatRound method simulates a combat round
        */

        public void TakeCombatRound()
        {
            // Remove old feedback from list
            if (_combatFeedback.Count > 0)
            {
                _combatFeedback.Clear();
            }

            bool oppositionLives = true;

            for (int i = 0; i < _combatants.Length && oppositionLives; i++)
            { 
                // Check if combatant lives
                if (_combatants[i].Stats.hitPoints > 0)
                {
                    Creature target;

                    if (_combatants[i] is Player && _playerSelectedTarget > -1)
                    {
                        target = _combatants[_playerSelectedTarget];
                    }
                    else
                    {
                        target = SeekTargetFor(_combatants[i]);
                    }

                    // Attack roll
                    d20Die.Roll();

                    // Determine if attack successful
                    int attackResult = d20Die.DieResult + _combatants[i].Stats.attackBonus;

                    if (attackResult >= target.Stats.armorClass)
                    {
                        // Damage roll
                        _combatants[i].Stats.damage.Roll();

                        int damageResult = _combatants[i].Stats.damage.DieResult;

                        // Use weapon if player is attacking
                        if (_combatants[i] is Player && ((Player)_combatants[i]).EquippedWeapon != null)
                        {
                            Player player = (Player)_combatants[i];

                            player.EquippedWeapon.Use();

                            // Check weapon's durability
                            if (player.EquippedWeapon.Durability == 0)
                            {
                                player.Inventory.Remove(player.EquippedWeapon);
                                player.UnequipWeapon();
                            }
                        }

                        // Deal damage
                        BaseStats newTargetStats = target.Stats;
                        newTargetStats.hitPoints -= damageResult;

                        // Set target's new stats
                        target.Stats = newTargetStats;

                        // Update feedback
                        _combatFeedback.Add(_combatants[i].Stats.name + " hit " + target.Stats.name +
                                            " for " + damageResult + " damage!");

                        // Check if target is dead
                        if (target.Stats.hitPoints <= 0)
                        {
                            // Ensure clean 0
                            newTargetStats.hitPoints = 0;
                            target.Stats = newTargetStats;

                            // Update feedback to inform that target is dead
                            _combatFeedback.Add(target.Stats.name + " is dead!");
                        }
                    }
                    else
                    {
                        // Update feedback to inform that combatant missed
                        _combatFeedback.Add(_combatants[i].Stats.name + " missed!");
                    }

                    // Check if the opposition lives
                    int livingFriendlies = 0,
                        livingEnemies = 0;

                    for (int j = 0; j < _combatants.Length; j++)
                    {
                        if (_combatants[j].Stats.friendly && _combatants[j].Stats.hitPoints > 0)
                        {
                            livingFriendlies++;
                        }
                        else if (!(_combatants[j].Stats.friendly) && _combatants[j].Stats.hitPoints > 0)
                        {
                            livingEnemies++;
                        }
                    }

                    if (livingFriendlies == 0 || livingEnemies == 0)
                    {
                        oppositionLives = false;
                        _combatActive = false;
                    }
                }
            }
        }

        /*
            The SeekTargetFor method gets a target for a combatant automatically
        */

        private Creature SeekTargetFor(Creature combatant)
        {
            // Narrow choice of targets down to those opposed to combatant
            List<Creature> possibleTargets = new List<Creature>();

            foreach (Creature creature in _combatants)
            {
                if (creature.Stats.friendly != combatant.Stats.friendly)
                {
                    possibleTargets.Add(creature);
                }
            }

            // Roll to determine which target to attack
            RandomDie die = new RandomDie(possibleTargets.Count);
            die.Roll();

            return possibleTargets[die.DieResult - 1];
        }

        /*
            The IsCombatant method checks if a Creature is a combatant for the currently
            initialized combat
        */

        public bool IsCombatant(Creature creature)
        {
            bool isCombatant = false;
            for (int i = 0; i < _combatants.Length && !isCombatant; i++)
            {
                if (_combatants[i] == creature)
                {
                    isCombatant = true;
                }
            }

            return isCombatant;
        }

        /*
            CombatActive property
        */

        public bool CombatActive
        {
            get { return _combatActive; }
        }

        /*
            PlayerSelectedTarget property
        */

        public int PlayerSelectedTarget
        {
            get { return _playerSelectedTarget; }
            set { _playerSelectedTarget = value; }
        }

        /*
            CombatFeedback property
        */

        public List<string> CombatFeedback
        {
            get { return _combatFeedback; }
        }
    }
}
