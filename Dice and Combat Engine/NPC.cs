using System;
using System.Collections.Generic;

/*
    This class represents an NPC
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

        public NPC(BaseStats stats, Attributes attribs, string desc, string[] responds)
            : base(stats, attribs, desc)
        {
            responses = new string[responds.Length];
            responds.CopyTo(responses, 0);
        }

        /*
            Copy Constructor
        */

        public NPC(NPC npc)
            : base(npc.Stats, npc.Attributes, npc.Description)
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
