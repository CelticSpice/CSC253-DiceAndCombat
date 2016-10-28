/*
    This class represents an NPC, or a creature of some higher degree of
    intelligence
    10/28/2016
    CSC 253 0001 - CH8P1
    Author: James Alves, Shane McCann, Timothy Burns
*/

namespace Dice_and_Combat_Engine
{
    class NPC : Creature
    {
        // Fields
        private string[] responses;

        /*
            Constructor
        */

        public NPC(BaseStats stats, string desc, string[] responds)
            : base(stats, desc)
        {
            responses = new string[responds.Length];
            responds.CopyTo(responses, 0);
        }

        /*
            Copy Constructor
        */

        public NPC(NPC npc)
            : base(new BaseStats(npc.Stats), npc.Description)
        {
            responses = new string[npc.responses.Length];
            npc.responses.CopyTo(responses, 0);
        }

        /*
            The GetResponse method returns a random response from the NPC
        */

        public string GetResponse()
        {
            int originalSize = Stats.Damage.DieSize;
            Stats.Damage.DieSize = responses.Length;
            Stats.Damage.Roll();
            Stats.Damage.DieSize = originalSize;
            return responses[Stats.Damage.DieResult - 1];
        }
    }
}
