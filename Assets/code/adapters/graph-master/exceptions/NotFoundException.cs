using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GraphMaster
{
    public class NotFoundException : GraphMasterException
    {
        public NotFoundException() : base("Object Not found")
        {
        }


        public NotFoundException(string objectType, string name, string sourse) : base($"{objectType} with name {name} was not found in {sourse}")
        {
        }
    }
}
