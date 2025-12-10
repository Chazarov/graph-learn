using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster
{
    public class InpossibleToChangeGraphTypeException : GraphMasterException
    {
        public InpossibleToChangeGraphTypeException() : base("It is impossible to change the graph type")
        {
        }

        public InpossibleToChangeGraphTypeException(string message) : base(message)
        {
        }

    }
}
