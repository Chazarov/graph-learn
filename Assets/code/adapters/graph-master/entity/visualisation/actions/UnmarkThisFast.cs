using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace GraphMaster.Visualization.Actions
{
    public class UnmarkThisFast : ActionInterface
    {
        private GraphPartInterface part;

        public UnmarkThisFast(GraphPartInterface part)
        {
            this.part = part;
        }

        public void Execute(object context)
        {
            if (context is PerformerInterface performer)
            {
                performer.UnmarkItFast(part);
            }
        }
    }
}

