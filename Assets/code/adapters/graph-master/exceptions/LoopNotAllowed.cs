using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class LoopNotAllowed : GraphMasterException
    {
        public LoopNotAllowed() : base("Self-loops (edges from a node to itself) are not allowed in this graph")
        {
        }

        public LoopNotAllowed(string message) : base(message)
        {
        }
    }
}
