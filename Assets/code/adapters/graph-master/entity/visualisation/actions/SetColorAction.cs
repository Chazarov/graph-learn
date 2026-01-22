
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;

namespace GraphMaster.Visualization.Actions
{
    public class SetColorAction: ActionInterface
    {

        IGraphPart target;
        Color color;

        public SetColorAction(IGraphPart target, Color color)
        {
            this.target = target;
            this.color = color;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.SetColor(target, color);
            }
        }
    }
}

