using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class NegativeEdgeNotAllowed : GraphMasterException
    {
        public NegativeEdgeNotAllowed() : base("Negative edge weights are not allowed in this graph")
        {
        }

        public NegativeEdgeNotAllowed(string message) : base(message)
        {
        }
    }
}
