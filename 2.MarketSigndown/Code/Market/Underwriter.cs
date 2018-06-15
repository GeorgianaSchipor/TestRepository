using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Market
{
    public class Underwriter
    {

        public int SeqNum { get; set; }
        public string Name { get; set; }
        public float WrittenLine { get; set; }
        public float MinLine { get; set; }
        public float SignedLine { get; set; }

        public Underwriter(int seqNum, string name, float writtenLine, float minLine, float signedLine)
        {
            SeqNum = seqNum;
            Name = name;
            WrittenLine = writtenLine;
            MinLine = minLine;
            SignedLine = signedLine;
        }
    }
}