using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    class TNode
    {
        char character;
        public char Character
        {
            get { return character; }
            set { character = value; }
        }

        int frequency;
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        
        TNode right;
        internal TNode Right
        {
            get { return right; }
            set { right = value; }
        }
        
        TNode left;
        internal TNode Left
        {
            get { return left; }
            set { left = value; }
        }

        public TNode()
        { }

        public TNode(char character, int frequency)
        {
            this.character = character;
            this.frequency = frequency;
        }
    }
}
