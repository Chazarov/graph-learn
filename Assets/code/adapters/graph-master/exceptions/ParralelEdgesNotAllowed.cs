using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class ParralelEdgesNotAllowed : GraphMasterException
    {
        public ParralelEdgesNotAllowed() : base("Parallel edges are not allowed in this oriented graph")
        {
        }

        public ParralelEdgesNotAllowed(string message) : base(message)
        {
        }
    }

}
