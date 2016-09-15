/*
    This class represents dice to be cast
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
    class RandomDie
    {
        // Fields
        private int _dieSize, _dieResult;
        private static Random rng;

        /*
            Constructor
            Accepts an integer specifiying the die size
            Defaults to die size of 6
        */

        public RandomDie(int size = 6)
        {
            _dieSize = size;
            _dieResult = 0;
            if (rng == null)
            {
                rng = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);
            }
        }

        /*
            The Roll method simulates rolling the dice
        */

        public void Roll()
        {
            _dieResult = (rng.Next(_dieSize) + 1);
        }

        /*
            DieSize property
        */

        public int DieSize
        {
            get { return _dieSize; }
            set { _dieSize = value; }
        }

        /*
            DieResult property
        */

        public int DieResult
        {
            get { return _dieResult; }
        }
    }
}
