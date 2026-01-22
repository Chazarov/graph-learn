using GraphMaster;
using System.Collections.Generic;

namespace Domain
{
    public interface GraphTraversalServiceInterface<TNode, TEdge> 
        where TNode : IGraphNode, IGraphPart
        where TEdge : IGraphEdge<TNode>, IGraphPart
    {
        List<ActionInterface> Traverse(GraphInterface<TNode, TEdge> graph);
    }
}
