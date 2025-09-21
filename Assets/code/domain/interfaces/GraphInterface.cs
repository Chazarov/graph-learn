using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace domain
{
    public interface GraphInterface
    {
        public bool AllowedParrallelEdges();

        public bool AllowedNegativeEdges();

        public bool AllowedLoops();
    
        public bool IsOriented();

        public bool IsWeighed();



        public void MakeOriented();

        public void MakeParralel();

        public void MakeWeighed();

        public void AllowNegative();

        public void AllowLoops();
    





    }

}


