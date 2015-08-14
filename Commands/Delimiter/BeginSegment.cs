﻿using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class BeginSegment : Command
    {
        public BeginSegment(string identifier)
            : base(0, 6)
        {
            Identifier = identifier;
        }

        public string Identifier { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginSegment(this, parameter);
        }
    }
}
