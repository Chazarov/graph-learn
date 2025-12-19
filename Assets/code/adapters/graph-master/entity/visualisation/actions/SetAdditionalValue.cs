using UnityEngine;

namespace GraphMaster.Visualization.Actions
{
    public class SetAdditionalValue : ActionInterface
    {
        public string newValue;
        public GraphPartInterface target;

        public SetAdditionalValue(string newValue, GraphPartInterface target)
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
