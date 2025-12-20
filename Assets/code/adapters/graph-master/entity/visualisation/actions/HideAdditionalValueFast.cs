using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.Visualization.Actions
{

    public class HideAdditionalValueFast : ActionInterface
    {
        private GraphPartInterface target;

        public HideAdditionalValueFast(GraphPartInterface target)
        {
            this.target = target;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.HideAdditionalValueFast(target);
            }
        }

        public override string ToString()
        {
            return "HideAdditionalValueFast " + target.GetName();
        }
    }

}
