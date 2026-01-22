using UnityEngine;

namespace GraphMaster.Visualization.Actions
{
    public class SetAdditionalValueFast : ActionInterface
    {
        public string newValue;
        public IGraphPart target;

        public SetAdditionalValueFast(string newValue, IGraphPart target)
        {
            this.newValue = newValue;
            this.target = target;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.SetAdditionalValueFast(target, newValue);
            }
        }

        public override string ToString()
        {
            return "SetAdditionalValueFast" + "   " + target.GetName() + "  " + newValue;
        }
    }

}
