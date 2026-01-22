using Assets.code.adapters.graph_master.entity.positioned;
using System.Numerics;
using GraphMaster.Entity;

namespace GraphMaster
{
    public class Positioned2Node: IGraphNode
    {
        private IPositionedNodeData data;

        private GraphNode baseNode;

        public Positioned2Node(IPositionedNodeData data, string name)
        {
            this.data = data;
            baseNode = new GraphNode(data, name);

        }

        public IPositionedNodeData GetData()
        {
            return data;
        }
        public IGraphPartData GetBaseData()
        {
            return data;
        }

        public string GetName()
        {
            return baseNode.GetName();
        }

        public Vector2 GetPosition()
        {
            return data.GetPosition();
        }


        public void SetPosition(Vector2 position)
        {
            data.SetPosition(position);
        }

    }
}


