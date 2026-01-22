

namespace GraphMaster.Entity
{
    public interface IGraphPart
    {
        public string GetName();
        public IGraphPartData GetBaseData();
    }
}
