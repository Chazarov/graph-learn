using Domain;
using GrapMaster;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphMaster
{
    public class Positioned2Node: GraphNodeInterface
    {
        private Vector2 position;

        private GraphNode baseNode;

        public Positioned2Node(Vector2 position,  string name, string description)
        {
            this.position = position;
            baseNode = new GraphNode(name, description);

        }

        public Positioned2Node(Vector2 position, string name) : this(position, name, "") { }


        public void AddEdge(GraphEdgeInterface edge)
        {
            baseNode.AddEdge(edge);
        }

        public void DisconnectEdge(GraphEdgeInterface edge)
        {
            baseNode.DisconnectEdge(edge);
        }

        public string GetDescription()
        {
            return baseNode.GetDescription();
        }

        public List<GraphEdgeInterface> GetEdges()
        {
            return baseNode.GetEdges();
        }

        public string GetName()
        {
            return baseNode.GetName();
        }

        public void SetDescription(string description)
        {
            baseNode.SetDescription(description);
        }

        public void SetName(string name)
        {
            baseNode.SetName(name);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(position.X, position.Y);
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}


