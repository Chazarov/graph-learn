using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class LoopsNotAllowed : GraphMasterException
    {
        public LoopsNotAllowed() : base("Self-loops (edges from a node to itself) are not allowed in this graph")
        {
        }

        public LoopsNotAllowed(string message) : base(message)
        {
        }
    }
}
