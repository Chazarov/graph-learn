using Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class EdgeVisual : MonoBehaviour, Domain.GraphEdgeInterface
    {

        [SerializeField] LineRenderer line;

        GraphEdgeInterface sourse;

        void Start()
        {

        }

        void Update()
        {

        }

        public void Initialize(Positioned2Node sourse, Positioned2Node target)
        {
            GraphEdgeInterface edge = new GraphEdge(sourse, target);
            this.sourse = edge;
        }

        public GraphNodeInterface GetSourceNode()
        {
            return sourse.GetSourceNode();
        }

        public GraphNodeInterface GetTargetNode()
        {
            return sourse.GetTargetNode();
        }

        public float GetWeight()
        {
            return sourse.GetWeight();
        }

        public bool HasWeight()
        {
            return sourse.HasWeight();
        }

        public bool IsParralel(GraphEdgeInterface other)
        {
            return sourse.IsParralel(other);
        }

        public void SetWeight(float weight)
        {
            sourse.SetWeight(weight);
        }

       

        
    }

}

