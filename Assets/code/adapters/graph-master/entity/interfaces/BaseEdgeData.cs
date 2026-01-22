using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.code.adapters.graph_master.entity.interfaces
{
    internal class BaseEdgeData
    {
        private float weight;
        private bool hasWeight = false;
        private string name;
        public float GetWeight()
        {
            return this.weight;
        }

        public void SetWeight(float weight)
        {
            this.weight = weight;
            this.hasWeight = true;
        }


        public bool HasWeight()
        {
            return hasWeight;
        }
    }
}
