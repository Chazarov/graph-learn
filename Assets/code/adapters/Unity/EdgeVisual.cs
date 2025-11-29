using Domain;
using GraphMaster.UnityAdapter.UI;
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

        public void Initialize(UIPositioned2Node sourse, UIPositioned2Node target)
        {
            GraphEdgeInterface edge = new GraphEdge(sourse, target);
            this.sourse = edge;

            line.positionCount = 2;
            line.SetPosition(0, sourse.transform.position);
            line.SetPosition(1, target.transform.position);
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

