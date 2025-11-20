using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphMaster;
using Domain;
using System.Linq;

namespace GraphMaster.UnityAdapter
{
    public class GraphVisual : MonoBehaviour
    {
        [SerializeField] private GameObject nodePrefab; 
        [SerializeField] private List<string>nodes = new List<string>();

        private List<string> oldNodes = new List<string>();

        private GraphMaster.Graph<GraphMaster.UnityAdapter.Positioned2Node, GraphMaster.GraphEdge> sourse = new Graph<GraphMaster.UnityAdapter.Positioned2Node, GraphEdge>();

        private void Start()
        {
            if (nodePrefab == null)
            {
                throw new System.Exception("Prefab can't be a null. Please add a prefab for the node");
            }
        }


        public GameObject CreateNodeObject(string name)
        {
            GameObject instance = Instantiate(nodePrefab);
            Positioned2Node component = instance.AddComponent<Positioned2Node>();
            component.Initialize(name);
            sourse.AddNode(component);

            return instance;
        }

        private void ImplementListOfNodes()
        {

            if (nodePrefab == null)
            {
                nodes.Clear();
                throw new System.Exception("Prefab can't be a null. Please add a prefab for the node");
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                string currentNode = nodes[i];
                if (currentNode == ""){
                    nodes[i] =  i.ToString(); 
                    break;
                }
            }

            List<string> toDelete = oldNodes.Except(nodes).ToList();
            foreach (string nodeName in toDelete)
            {
                DeleteNode(nodeName);
            }

            List<string> toCreate = nodes.Except(oldNodes).ToList();
            foreach(string nodeName in toCreate)
            {
                CreateNodeObject(nodeName);
            }
        }

        public void DeleteNode(string nodeName)
        {
            oldNodes.Remove(nodeName);
            nodes.Remove(nodeName);
            Positioned2Node instance = sourse.GetNode(nodeName);
            sourse.DeleteNode(nodeName);
            Destroy(instance.gameObject);

        }

        private void OnValidate()
        {
            ImplementListOfNodes();
        }


    }
}
