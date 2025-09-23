using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class WeightRequiredException : GraphMasterException
    {
        public WeightRequiredException() : base("Weight is required for edges in this weighted graph")
        {
        }

        public WeightRequiredException(string message) : base(message)
        {
        }
    }
}
