using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.Visualization.Actions
{

    public class HideAdditionalValueFast : ActionInterface
    {
        private IGraphPart target;

        public HideAdditionalValueFast(IGraphPart target)
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
