/*
    This class handles the combat logic
    9/5/2016
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
    class CombatEngine
    {
        // Fields
        private bool _combatActive;
        private List<Creature> combatants, friendlies, enemies;
        private List<string> _combatFeedback;
        private RandomDie d20Die;
        private string _victors;

        /*
            Constructor
        */

        internal CombatEngine()
        {
            _combatActive = false;
            combatants = new List<Creature>();
            friendlies = new List<Creature>();
            enemies = new List<Creature>();
            _combatFeedback = new List<string>();
            d20Die = new RandomDie(20);
            _victors = "";
        }

        /*
            The InitCombat method initializes combat
        */

        public void InitCombat(params Creature[] newCombatants)
        {
            _combatActive = true;

            // Get combatants and separate friendlies from enemies
            foreach (Creature creature in newCombatants)
            {
                combatants.Add(creature);

                if (creature.Stats.friendly)
                {
                    friendlies.Add(creature);
                }
                else
                {
                    enemies.Add(creature);
                }
            }

            // Initialize initiative
            int[] initiatives = new int[combatants.Count];         // A parallel relationship shall exist between
                                                                   // combatants and initiatives
            for (int i = 0; i < combatants.Count; i++)
            {
                int initiative;

                // Combatant rolls for initiative
                d20Die.Roll();
                initiative = d20Die.DieResult + combatants[i].Attributes.dexterity;

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
                            initiative = d20Die.DieResult + combatants[i].Attributes.dexterity;
                            initiatives[i] = initiative;

                            d20Die.Roll();
                            initiative = d20Die.DieResult + combatants[j].Attributes.dexterity;
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

                        Creature tmpCombatant = combatants[i];
                        combatants[i] = combatants[j];
                        combatants[j] = tmpCombatant;
                    }
                }
            }
        }

        /*
            The TakeCombatRound method simulates a combat round
            The method accepts an int specifying the target for the player

            -1 = Auto
        */

        public void TakeCombatRound(int chosenTarget = -1)
        {
            // Remove old feedback from list
            if (_combatFeedback.Count > 0)
            {
                _combatFeedback.Clear();
            }

            bool oppositionLives = true;
            int combatantIndex = 0;         // The index of the combatant moving

            while (oppositionLives && combatantIndex < combatants.Count)
            { 
                Creature target;
                int targetIndex;

                if (combatants[combatantIndex] is Player)
                {
                    Player playerCharacter = (Player) combatants[combatantIndex];

                    // Player acquires target
                    if (chosenTarget > -1)
                    {
                        target = enemies[chosenTarget];
                        targetIndex = combatants.IndexOf(target);
                    }
                    else
                    {
                        target = SeekTarget(playerCharacter);
                        targetIndex = combatants.IndexOf(target);
                    }

                    // Attack roll for player
                    d20Die.Roll();

                    // Determine if player's attack is successful
                    int attackResult;

                    attackResult = d20Die.DieResult + playerCharacter.Stats.attackBonus;
                    

                    if (attackResult >= target.Stats.armorClass)
                    {
                        // Player rolls for damage
                        int damageResult;

                        playerCharacter.Stats.damage.Roll();

                        damageResult = playerCharacter.Stats.damage.DieResult;

                        // Use player's weapon
                        if (playerCharacter.EquippedWeapon != null)
                        {
                            playerCharacter.EquippedWeapon.Use();

                            // Check weapon's durability
                            if (playerCharacter.EquippedWeapon.Durability == 0)
                            {
                                playerCharacter.Inventory.Remove(playerCharacter.EquippedWeapon);
                                playerCharacter.UnequipWeapon();
                            }
                        }

                        // Deal damage
                        BaseStats newTargetStats = target.Stats;
                        newTargetStats.hitPoints -= damageResult;

                        // Set target's new stats
                        target.Stats = newTargetStats;

                        // Update feedback
                        _combatFeedback.Add(playerCharacter.Stats.name + " hit " + target.Stats.name +
                                           " for " + damageResult + " damage!");

                        // Check if target is dead
                        if (target.Stats.hitPoints == 0)
                        {
                            // Update feedback to inform that target is dead
                            _combatFeedback.Add(target.Stats.name + " is dead!");

                            // Remove target from list of combatants and enemies
                            combatants.Remove(target);
                            enemies.Remove(target);
                        }
                    }
                    else
                    {
                        // Update feedback to inform that player missed
                        _combatFeedback.Add(playerCharacter.Stats.name + " missed!");
                    }
                }
                else
                {
                    // Attacker acquires target
                    target = SeekTarget(combatants[combatantIndex]);
                    targetIndex = combatants.IndexOf(target);

                    // Attack roll for attacker
                    d20Die.Roll();

                    // Determine if attack successful
                    if (d20Die.DieResult + combatants[combatantIndex].Stats.attackBonus >= target.Stats.armorClass)
                    {
                        // Roll for damage
                        combatants[combatantIndex].Stats.damage.Roll();

                        // Deal damage
                        BaseStats newTargetStats = target.Stats;
                        newTargetStats.hitPoints -= combatants[combatantIndex].Stats.damage.DieResult;

                        // Set target's new stats
                        target.Stats = newTargetStats;

                        // Update feedback
                        _combatFeedback.Add(combatants[combatantIndex].Stats.name + " hit " + target.Stats.name +
                                           " for " + combatants[combatantIndex].Stats.damage.DieResult + " damage!");

                        // Check if target is dead
                        if (target.Stats.hitPoints <= 0)
                        {
                            // Update feedback to inform that target is dead
                            _combatFeedback.Add(target.Stats.name + " is dead!");

                            // Remove target from list of combatants
                            combatants.Remove(target);

                            if (target.Stats.friendly)
                            {
                                friendlies.Remove(target);
                            }
                            else
                            {
                                enemies.Remove(target);
                            }
                        }
                    }
                    else
                    {
                        // Update feedback to inform that attacker missed
                        _combatFeedback.Add(combatants[combatantIndex].Stats.name + " missed!");
                    }
                }

                // Check if the opposition lives
                if (friendlies.Count == 0 || enemies.Count == 0)
                {
                    oppositionLives = false;
                    _combatActive = false;

                    // Set the victors for the combat
                    if (enemies.Count == 0)
                    {
                        _victors = "Friendlies";
                    }
                    else
                    {
                        _victors = "Enemies";
                    }
                }
                else
                {
                    // Next combatant takes turn
                    if ((target.Stats.hitPoints <= 0 && !(targetIndex < combatantIndex)) || target.Stats.hitPoints > 0)
                    {
                        combatantIndex++;
                    }
                }
            }
        }

        /*
            The SeekTarget method automates a combatant's choice of who to attack
        */

        private Creature SeekTarget(Creature combatant)
        {
            // Narrow choice of targets down to those opposed to combatant
            List<Creature> possibleTargets = new List<Creature>();

            foreach (Creature creature in combatants)
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
            CombatActive property
        */

        public bool CombatActive
        {
            get { return _combatActive; }
        }

        /*
            CombatFeedback property
        */

        public List<string> CombatFeedback
        {
            get { return _combatFeedback; }
        }

        /*
            Victors property
        */

        public string Victors
        {
            get { return _victors; }
        }
    }
}
