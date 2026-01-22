using Domain;
using GraphMaster.Entity;

namespace GraphMaster
{

    public class GraphNode : IGraphNode
    {

        private IGraphPartData data;

        private string name;

        internal GraphNode(IGraphPartData data, string name)
        {
            this.data = data;
            this.name = name;   
        }



        public string GetName()
        {
            return name;
        }

        public IGraphPartData GetBaseData()
        {
            return data;
        }

    }

}