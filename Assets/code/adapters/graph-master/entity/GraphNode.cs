using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Domain;

namespace GraphMaster
{

    public class GraphNode : GraphNodeInterface
    {

        private string name;

        private string description = "";



        public GraphNode(string name)
        {
            this.name = name;
        }

        public GraphNode(string name, string description) : this(name)
        {
            this.description = description;
        }


        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetDescription(string description)
        {
            this.description = description;
        }
    }

}