using UnityEngine;

namespace GraphMaster.UnityAdapter.Visualization.Actions
{
    public class SetAdditionalValueFast : ActionInterface
    {
        public string newValue;
        public GraphPartInterface target;

        public SetAdditionalValueFast(string newValue, GraphPartInterface target)
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
