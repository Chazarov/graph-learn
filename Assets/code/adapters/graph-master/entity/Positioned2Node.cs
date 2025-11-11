using Domain;
using GrapMaster;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphMaster
{
    public class Positioned2Node: GraphNode
    {
        private Vector2 position;


        public Positioned2Node(Vector2 position, IMutableGraph<GraphNodeInterface> graph, string name, string description) : base(graph, name, description)
        {
            this.position = position;
        }

        public Positioned2Node(Vector2 position, IMutableGraph<GraphNodeInterface> graph, string name) : base(graph, name)
        {
            this.position = position;
        }

        public Positioned2Node(Vector2 position, IMutableGraph<GraphNodeInterface> graph): base(graph)
        {
            this.position = position;
        }

        public Positioned2Node(IMutableGraph<GraphNodeInterface> graph) : this(Vector2.Zero, graph)
        { }

        public Vector2 GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }



    }
}


