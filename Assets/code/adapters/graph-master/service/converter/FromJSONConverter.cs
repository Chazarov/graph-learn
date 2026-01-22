

using Assets.code.adapters.graph_master.entity.interfaces;
using GraphMaster.Entity;

namespace GraphMaster.Service
{
    internal class FromJSONConverter<TNode, TEdge> where TNode : IGraphNode where TEdge : IGraphEdge<TNode>
    {
        private IGraph<TNode, TEdge> graph;

        private IGraphObjectsFabric<TNode, TEdge> fabric;

        public FromJSONConverter(IGraph<TNode, TEdge> graph, IGraphObjectsFabric<TNode, TEdge> fabric)
        {
            this.graph = graph;
            this.fabric = fabric;
        }
    }
}
