using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster
{
    public class DublicateException : GraphMasterException
    {
        public DublicateException() : base("Dublicate ")
        {
        }

        public DublicateException(string message) : base(message)
        {
        }
    }
}