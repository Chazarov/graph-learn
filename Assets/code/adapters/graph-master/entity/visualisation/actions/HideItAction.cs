
namespace GraphMaster.Visualization.Actions
{
    public class HideItAction
    {
        IGraphPart target;

        public HideItAction(IGraphPart target)
        {
            this.target = target;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.HideIt(target);
            }
        }
    }
}

