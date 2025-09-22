using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class InvalidGraphOperationException : Exception
    {
        public InvalidGraphOperationException() : base("Invalid operation for the current graph configuration")
        {
        }

        public InvalidGraphOperationException(string message) : base(message)
        {
        }
    }
}
