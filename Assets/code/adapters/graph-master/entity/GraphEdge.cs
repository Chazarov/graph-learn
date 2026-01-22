using Domain;
using GraphMaster.Entity;

namespace GraphMaster
{
    public class GraphEdge<TNode>: IGraphEdge<TNode> where TNode : IGraphNode
    {
        private TNode targetNode;
        private TNode sourseNode;

        private IEdgeData data;

        private string name;

        

 
        public GraphEdge(TNode sourceNode, TNode targetNode, string name, IEdgeData data)
        {
            this.targetNode = targetNode;
            this.sourseNode = sourceNode;
            this.data = data;
            this.name = name;
        }

   

        public IEdgeData GetData()
        {
            return data;
        }
        public string GetSourseName()
        {
            return this.sourseNode.GetName();
        }

        public string GetTargetName()
        {
            return this.targetNode.GetName();
        }

        public string GetName()
        {
            return this.name;
        }


        public void SetSourseNode(TNode node)
        {
            this.sourseNode = node;
        }

        public void SetTargetNode(TNode node)
        {
            this.targetNode = node;
        }

        public TNode GetSourseNode()
        {
            return sourseNode;
        }

        public TNode GetTargetNode()
        {
            return targetNode;
        }

        public IGraphPartData GetBaseData()
        {
            return data;
        }
    }

}
