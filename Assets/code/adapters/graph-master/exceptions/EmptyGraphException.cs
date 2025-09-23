using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class EmptyGraphException : GraphMasterException
    {
        public EmptyGraphException() : base("Operation cannot be performed on an empty graph")
        {
        }

        public EmptyGraphException(string message) : base(message)
        {
        }

    }
}
