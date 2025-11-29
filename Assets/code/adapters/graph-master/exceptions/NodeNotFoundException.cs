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


        public NodeNotFoundException(string nodeName) : base($"Node with name {nodeName} was not found in the graph")
        {
        }
    }
}
