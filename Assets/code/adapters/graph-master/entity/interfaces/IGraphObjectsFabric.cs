using Domain;
using GraphMaster.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.code.adapters.graph_master.entity.interfaces
{
    internal interface IGraphObjectsFabric<TNode, TEdge> where TNode: IGraphNode where TEdge: IGraphEdge<TNode>
    {
        public TNode CreateNode(IGraphPartData meta, string name);

        public TEdge CreateEdge(IGraphPartData meta, string name, TNode sourse, TNode target);
    }
}
