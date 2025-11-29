using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class NodeNotSelectedException : GraphMasetrUnityException
    {
        public NodeNotSelectedException() : base("Node is not selected. You need to select a node for this operation.")
        {
        }
    }
}

