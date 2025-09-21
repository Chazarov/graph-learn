using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{   

    private List<GraphEdge> edges = new List<GraphEdge>();

    private bool hasParralelsEdges = false;

    private int number;

    private string name;

    private string description;


    public void AddEdge(GraphEdge edge)
    {
        foreach (GraphEdge childEdge in edges)
        {
            if (childEdge.IsParralel(edge)){
                hasParralelsEdges = true;
            }
        }
        this.edges.Add(edge);
    }


    public List<GraphEdge> GetEdges()
    {
        return edges;
    }

}
