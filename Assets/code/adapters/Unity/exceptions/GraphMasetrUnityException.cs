using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace GraphMaster.UnityAdapter
{
    public abstract class GraphMasetrUnityException : System.Exception
    {
            public GraphMasetrUnityException() : base("GraphMasterUnityException")
            {
            }

            public GraphMasetrUnityException(string message) : base(message)
            {
            }

    }

}



