using UnityEngine;

namespace GraphMaster.Visualization.Actions
{
    public class SetAdditionalValue : ActionInterface
    {
        public string newValue;
        public IGraphPart target;

        public SetAdditionalValue(string newValue, IGraphPart target)
        {
            this.newValue = newValue;
            this.target = target;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.SetAdditionalValue(target, newValue);
            }
        }

        public override string ToString()
        {
            return "SetAdditionalValue" + "   " + target.GetName() + "  " + newValue;
        }
    }
      
}
