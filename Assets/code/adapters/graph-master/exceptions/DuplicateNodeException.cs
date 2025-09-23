using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class DuplicateNodeException : GraphMasterException
    {
        public DuplicateNodeException() : base("A node with this identifier already exists in the graph")
        {
        }

        public DuplicateNodeException(string message) : base(message)
        {
        }

        public DuplicateNodeException(int nodeId) : base($"Node with ID {nodeId} already exists in the graph")
        {
        }
    }
}
