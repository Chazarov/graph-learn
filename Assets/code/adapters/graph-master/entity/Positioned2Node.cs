using Domain;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphMaster
{
    public class Positioned2Node: GraphNodeInterface
    {
        private Vector2 position;
        private GraphNodeInterface node;


        public Positioned2Node(Vector2 position, GraphNodeInterface node)
        {
            this.position = position;
            this.node = node;

        }

        public Positioned2Node(Vector2 position, GraphInterface graph): this(position, new GraphNode(graph))
        { }


        public Positioned2Node(GraphNodeInterface node) : this(Vector2.Zero, node)
        { }

        public Vector2 GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public GraphNodeInterface GetGraphNode()
        {
            return this.node;
        }

        public GraphInterface GetGraph()
        {
            return this.node.GetGraph();
        }

        public List<GraphEdgeInterface> GetNodeEdges()
        {
            return this.node.GetEdges();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            Positioned2Node other = (Positioned2Node)obj;

            return this.GetGraphNode().Equals(other.GetGraphNode());
        }

        public int GetNumber()
        {
            return this.node.GetNumber();
        }

        public string GetName()
        {
            return this.node.GetName();
        }

        public string GetDescription()
        {
            return this.node.GetDescription();
        }

        public void DisconnectEdge(GraphEdgeInterface edge)
        {
            this.node.DisconnectEdge(edge);
        }

        public List<GraphEdgeInterface> GetEdges()
        {
            return this.node.GetEdges();
        }

        public void AddEdge(GraphEdgeInterface edge)
        {
            this.AddEdge(edge);
        }

        public void DeleteNode()
        {
            this.node.DeleteNode();
        }

        public void SetName(string name)
        {
            this.SetName(name);
        }

        public void SetDescription(string description)
        {
            this.SetDescription(description);
        }
    }
}


