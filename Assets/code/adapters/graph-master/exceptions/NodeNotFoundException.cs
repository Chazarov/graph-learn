using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class NodeNotFoundException : GraphMasterException
    {
        public NodeNotFoundException() : base("The specified node was not found in the graph")
        {
        }

        public NodeNotFoundException(string message) : base(message)
        {
        }

        public NodeNotFoundException(int nodeId) : base($"Node with ID {nodeId} was not found in the graph")
        {
        }
    }
}
