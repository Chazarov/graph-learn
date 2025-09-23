using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class WeightNotAllowedException : GraphMasterException
    {
        public WeightNotAllowedException() : base("Weights are not allowed for edges in this unweighted graph")
        {
        }

        public WeightNotAllowedException(string message) : base(message)
        {
        }
    }
}
