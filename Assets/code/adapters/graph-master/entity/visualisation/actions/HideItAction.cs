
namespace GraphMaster.Visualization.Actions
{
    public class HideItAction
    {
        GraphPartInterface target;

        public HideItAction(GraphPartInterface target)
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

