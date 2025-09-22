using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class EmptyGraphException : Exception
    {
        public EmptyGraphException() : base("Operation cannot be performed on an empty graph")
        {
        }

        public EmptyGraphException(string message) : base(message)
        {
        }

        public EmptyGraphException(string operation) : base($"Cannot perform {operation} on an empty graph")
        {
        }
    }
}
