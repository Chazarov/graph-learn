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
            SynchronizePosition(true);
        }
        

        private void OnValidate()
        {
            if (sourse == null)
            {
                return;
            }

            SynchronizePosition();
 
            sourse.SetName(this.nodeName);
        }


        public void Initialize(string name)
        {
            SetName(name);
            SynchronizePosition();
        }


        public void Initialize(string name, Vector2 position)
        {
            SetName(name);
            SetPosition(position);
        }

        private void SynchronizePosition(bool fromSourse = false)
        {
            if (!fromSourse)
            {
                Vector2 newPosition = transform.position;
                sourse.SetPosition(new System.Numerics.Vector2(newPosition.x, newPosition.y));
            }
            else
            {
                transform.position = new Vector2(sourse.GetPosition().X, sourse.GetPosition().Y);
            }
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
            SynchronizePosition();
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

        public void DisconnectEdge(GraphEdgeInterface edge)
        {
            throw new System.NotImplementedException();
        }

        public List<GraphEdgeInterface> GetEdges()
        {
            throw new System.NotImplementedException();
        }

        public void AddEdge(GraphEdgeInterface edge)
        {
            throw new System.NotImplementedException();
        }


        public void SetDescription(string description)
        {
            throw new System.NotImplementedException();
        }
    }

}
