using GraphMaster.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.code.adapters.graph_master.entity.interfaces
{
    internal interface IJsonMetadataParser
    {
        public IGraphPartData ParseNodeMeta(string rawNodeJson);

        public IGraphPartData ParseEdgeMeta(string rawEdgeJson);
    }
}
