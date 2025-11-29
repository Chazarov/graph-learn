using Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphMaster.UnityAdapter
{
    public class Positioned2Node : MonoBehaviour, GraphNodeInterface
    {

        private GraphMaster.Positioned2Node sourse = new GraphMaster.Positioned2Node(new System.Numerics.Vector2(0, 0), "");

        [SerializeField] private string nodeName;


        private void Start()
        {
           
        }
        

        private void OnValidate()
        {
            if (sourse == null)
            {
                return;
            }
            SetPosition(transform.position);
 
            sourse.SetName(this.nodeName);
        }


        public void Initialize(string name)
        {
            SetName(name);
            SetPosition(new Vector2 (0, 0));
        }


        public void Initialize(string name, Vector2 position)
        {
            SetName(name);
            SetPosition(position);
        }


        public void SetPosition(Vector2 position)
        {
            transform.position = position;
            Vector2 newPosition = transform.position;
            sourse.SetPosition(new System.Numerics.Vector2(newPosition.x, newPosition.y));
        }


        public void SetName(string name)
        {
            nodeName = name;
            this.name = "Node " + name;
            sourse.SetName(name);
        }


        public string GetName()
        {
            return nodeName;
        }

        public string GetDescription()
        {
            throw new System.NotImplementedException();
        }


        public void SetDescription(string description)
        {
            throw new System.NotImplementedException();
        }
    }

}
