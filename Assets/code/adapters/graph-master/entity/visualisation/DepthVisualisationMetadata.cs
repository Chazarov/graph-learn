using Assets.code.adapters.graph_master.entity;
using Assets.code.adapters.graph_master.entity.positioned;
using GraphMaster.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphMaster.Visualization
{
    internal class DepthVisualisationMetadata: BaseNodeMetadata, IPositionedNodeData
    {
        private int depth;
        private Vector2 position;

        public DepthVisualisationMetadata(int depth, string title, string description): base(title, description)
        {
            this.depth = depth;
        }

        public string uniqueName => throw new NotImplementedException();

        public int GetDepth()
        {
            return depth;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }
    }
}
