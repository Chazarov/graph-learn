using GraphMaster;
using System.Collections.Generic;

namespace Domain
{
    public interface GraphTraversalServiceInterface<TNode, TEdge> 
        where TNode : GraphNodeInterface, GraphPartInterface
        where TEdge : GraphEdgeInterface<TNode>, GraphPartInterface
    {
        List<ActionInterface> Traverse(GraphInterface<TNode, TEdge> graph);
    }
}
