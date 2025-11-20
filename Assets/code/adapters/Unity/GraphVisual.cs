using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphMaster;
using Domain;
using System.Linq;
using System.ComponentModel;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GraphMaster.UnityAdapter
{
    public class GraphVisual : MonoBehaviour
    {
        [SerializeField] private GameObject nodePrefab;


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


        public void DeleteNode(string nodeName)
        {
            Positioned2Node instance = sourse.GetNode(nodeName);
            sourse.DeleteNode(nodeName);
            Destroy(instance.gameObject);

        }



    }
}
