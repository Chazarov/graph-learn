using GraphMaster.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.code.adapters.graph_master.entity.positioned
{
    public interface IPositionedNodeData: IGraphPartData
    {
        public Vector2 GetPosition();

        public void SetPosition(Vector2 position);
    }
}
