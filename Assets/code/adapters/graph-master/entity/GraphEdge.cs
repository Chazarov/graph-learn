using Domain;

namespace GraphMaster
{
    public class GraphEdge<TNode>: Domain.GraphEdgeInterface<TNode> where TNode : GraphNodeInterface
    {
        private float weight;
        private bool hasWeight = false;
        private string name;

        private TNode targetNode;
        private TNode sourseNode;

        


        // Конструктор для взвешенного ребра
        // 1: 
        public GraphEdge(TNode sourceNode, TNode targetNode, float weight)
        {
            this.targetNode = targetNode;
            this.sourseNode = sourceNode;
            SetWeight(weight);
        }
        // Конструктор для невзвешенного ребра
        public GraphEdge(TNode sourceNode, TNode targetNode): this(sourceNode, targetNode, 1){}
   
        public float GetWeight()
        {
            return this.weight;
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
            this.hasWeight = true;
        }


        public bool HasWeight()
        {
            return hasWeight;
        }

        public TNode GetSourseNode()
        {
            return this.sourseNode;
        }

        public TNode GetTargetNode()
        {
            return this.targetNode;
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
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetSourseNode(TNode node)
        {
            this.sourseNode = node;
        }

        public void SetTargetNode(TNode node)
        {
            this.targetNode= node;
        }
    }

}
