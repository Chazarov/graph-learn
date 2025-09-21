using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using domain;

namespace GraphMaster
{
    public class Graph : domain.GraphInterface
    {
        private bool isOriented = false;
        private bool isWeighed = false;
        private bool allowParallelEdges = false;
        private bool allowNegativeEdges = false;
        private bool allowLoops = false;

        public bool AllowedParrallelEdges()
        {
            return allowParallelEdges;
        }

        public bool AllowedNegativeEdges()
        {
            return allowNegativeEdges;
        }

        public bool AllowedLoops()
        {
            return allowLoops;
        }

        public bool IsOriented()
        {
            return isOriented;
        }

        public bool IsWeighed()
        {
            return isWeighed;
        }

        public void MakeOriented()
        {
            isOriented = true;
        }

        public void MakeParralel()
        {
            allowParallelEdges = true;
        }

        public void MakeWeighed()
        {
            isWeighed = true;
        }

        public void AllowNegative()
        {
            allowNegativeEdges = true;
        }

        public void AllowLoops()
        {
            allowLoops = true;
        }
    }

}

