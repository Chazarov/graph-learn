using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class TestFailedException : GraphMasterException
    {
        public TestFailedException() : base("Test failed")
        {
        }

        public TestFailedException(string message) : base(message)
        {
        }

    }
}
