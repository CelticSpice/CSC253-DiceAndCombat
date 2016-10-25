using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice_and_Combat_Engine
{
    class NonPlayerCharacter : Creature
    {
        private List<string> _responces;

        public NonPlayerCharacter(List<string> responces, string desc) :
            base(desc)
        {
            _responces = responces;
        }
    }
}
