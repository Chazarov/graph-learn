using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster
{
    public class ImpossibleToChangeGraphTypeException : GraphMasterException
    {
        public ImpossibleToChangeGraphTypeException() : base("It is impossible to change the graph type") { }
        public ImpossibleToChangeGraphTypeException(string message) : base(message) { }

    }

    public class ImpossibleToSetGraphDirection : ImpossibleToChangeGraphTypeException
    {
        public ImpossibleToSetGraphDirection() : base("ImpossibleToSetDirection") { }
        public ImpossibleToSetGraphDirection(string message) : base(message) { }
    }

    public class ImpossibleToSetGraphParralel : ImpossibleToChangeGraphTypeException
    {
        public ImpossibleToSetGraphParralel() : base("ImpossibleToSetGraphParralel") { }
        public ImpossibleToSetGraphParralel(string message) : base(message) { }

    }
}
