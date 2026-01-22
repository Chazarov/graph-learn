using GraphMaster.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMaster.Entity
{
    internal class BaseNodeMetadata: IGraphPartData
    {
        private string title;
        private string description;
        private string uniqueName;

        public BaseNodeMetadata(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

        public string GetTitle()
        {
            return title;
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetUniqueName()
        {
            return uniqueName;
        }
    }
}
