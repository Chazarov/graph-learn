

using UnityEngine;

namespace GraphMaster.Visualization.Actions
{
    
    public class MarkThis : ActionInterface
    {
        private IGraphPart target;

        public MarkThis(IGraphPart target)
        {
            this.target = target;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.MarkThis(target);
            }
        }

        public override string ToString()
        {
            return "MarkThis " + target.GetName();
        }
    }

}
