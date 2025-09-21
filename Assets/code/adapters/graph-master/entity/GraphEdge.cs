using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEdge
{
    private float weight;
    private GraphNode graphNode;
   
    public float GetWeight()
    {
        return this.weight;
    }

    public GraphNode GetGraphNode()
    {
        return this.graphNode;
    }

    public bool IsParralel(GraphEdge other)
    {
        if (other == null) return false;
        if (other.GetGraphNode() == this.GetGraphNode()){
            return true;
        };
        return true;
    }
}
