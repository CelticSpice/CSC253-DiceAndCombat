/*
    This class handles combat logic
    9/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

using System.Text;

namespace Dice_and_Combat_Engine
{
    class CombatEngine
    {
        // Fields
        private RandomDie d20Die;

        /*
            Constructor
        */

        public CombatEngine()
        {
            d20Die = new RandomDie(20);
        }

        /*
            The DoCombat method simulates a combat round between an attacker and a defender
            It accepts the attacking Creature and the defending Creature as arguments,
            and returns feedback
        */

        public string DoCombat(Creature attacker, Creature defender)
        {
            StringBuilder feedback = new StringBuilder();

            // Attack roll
            d20Die.Roll();
            int attackResult = d20Die.DieResult + attacker.Stats.AttackBonus;

            // Check against defender's AC
            if (attackResult >= defender.Stats.ArmorClass)
            {
                bool weaponUsed = false;
                string weaponName = "";
                if (attacker is Player && ((Player)attacker).EquippedWeapon != null)
                {
                    weaponUsed = true;
                    weaponName = ((Player)attacker).EquippedWeapon.Name;
                }

                // Attack
                int damageDealt = attacker.Attack(defender);
                feedback.Append(attacker.Stats.Name + " hit " + defender.Stats.Name +
                                " for " + damageDealt + "\n");

                if (defender.Stats.HitPoints <= 0)
                {
                    feedback.Append(defender.Stats.Name + " is dead\n");
                    feedback.Append(attacker.Stats.Name + " gains " + defender.Stats.XPValue + " xp\n");
                    if (attacker is Player && ((Player)attacker).LeveledUp)
                    {
                        ((Player)attacker).LevelUp();
                        feedback.Append("You leveled up to level " + ((Player)attacker).Level + "\n");
                    }
                }

                if (weaponUsed && attacker is Player && ((Player)attacker).EquippedWeapon == null)
                    feedback.Append("Your " + weaponName + " broke\n");
            }
            else
                feedback.Append(attacker.Stats.Name + " missed\n");
            return feedback.ToString();
        }
    }
}
