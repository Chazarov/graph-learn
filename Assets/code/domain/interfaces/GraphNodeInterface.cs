using GraphMaster;
using System.Collections;
using System.Collections.Generic;

namespace Domain
{
    public interface GraphNodeInterface: GraphPartInterface
    {
        public string GetDescription();

        public void SetName(string name);
        public void SetDescription(string description);

    }
}