using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public abstract class GraphMasterException: System.Exception
    {
        public GraphMasterException() : base("GraphMasterException")
        {
        }

        public GraphMasterException(string message) : base(message)
        {
        }

    }
}
