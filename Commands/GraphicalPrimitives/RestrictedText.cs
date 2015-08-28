﻿using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class RestrictedText : Command
    {
        public RestrictedText(double deltaWidth, double deltaHeight, double positionX, double positionY, int final, string text)
            : base(4, 5)
        {
            DeltaWidth = deltaWidth;
            DeltaHeight = deltaHeight;
            Position = new PointF((float)positionX, (float)positionY);
            Final = (FinalFlag)final;
            Text = text;
        }
        public double DeltaWidth { get; private set; }
        public double DeltaHeight { get; private set; }
        public PointF Position { get; private set; }
        public FinalFlag Final { get; private set; }
        public string Text { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveRestrictedText(this, parameter);
        }
    }
}