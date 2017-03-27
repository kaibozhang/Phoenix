﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TriCheer.Phoenix.SequenceFile
{
    public class SequenceFactory
    {
        public static ISequence CreateSequence(SequenceTypes sequenceType)
        {
            if (sequenceType == SequenceTypes.Normal)
            {
                return new NormalSequence();
            }
            return null;
        }
    }
}