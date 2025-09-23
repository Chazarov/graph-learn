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

        public void DeleteNode(int nodeNumber);

        // Методы для работы с алгоритмами
        public List<GraphMaster.GraphNode> MakeDijkstra();
        public List<GraphMaster.GraphNode> MakeBreadth();
        public List<GraphMaster.GraphNode> MakeDepth();
        public List<GraphMaster.GraphNode> MakeBF();
    }
}