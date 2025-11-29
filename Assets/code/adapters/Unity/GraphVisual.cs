using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphMaster;
using Domain;
using System.Linq;
using System.ComponentModel;
using GraphMaster.UnityAdapter.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GraphMaster.UnityAdapter
{
    public class GraphVisual: MonoBehaviour 
    {
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject edgePrefab;

        private int nodeNameSequense = 1;
        private int edgeNameSequense = 1;

        private GraphMaster.Graph<GraphMaster.UnityAdapter.UI.UIPositioned2Node, GraphMaster.GraphEdge<UIPositioned2Node>> sourse = new Graph<UIPositioned2Node, GraphEdge<UIPositioned2Node>>();


        private List<UIPositioned2Node> selectedNodes = new List<UIPositioned2Node>();
        private bool addEdgeStarted = false;


        private void Start()
        {

            CheckTheNodePrefabContent(nodePrefab);
        }

        public void  CheckTheNodePrefabContent(GameObject nodePrefab)
        {
            if (nodePrefab == null)
            {
                throw new System.Exception("Node prefab can't be a null. Please add a prefab for the node");
            }
            Positioned2Node positioned2Node = null;
            if (!nodePrefab.TryGetComponent(out positioned2Node))
            {
                throw new System.Exception("The node prefab must contain the Positioned2Node class.");
            }
            UIPositioned2Node uIPositioned2Node = null;
            if (!nodePrefab.TryGetComponent(out uIPositioned2Node))
            {
                throw new System.Exception("The node prefab must contain the UIPositioned2Node class.");
            }
        }

        public void CheckTheEdgePrefabContent(GameObject edgePrefab)
        {
            if(edgePrefab == null)
            {
                throw new System.Exception("Edge prefab can't be a null. Please add a prefab for the edge");
            }
            EdgeVisual edge = null;
            if(!edgePrefab.TryGetComponent(out edge))
            {
                throw new System.Exception("The edge prefab must contain the EdgeVisual class.");
            }
        }

        public  void StartAddEdge()
        {
            if(selectedNodes.Count > 0)
            {
                addEdgeStarted = true;
            }
            else
            {
                throw new NodeNotSelectedException();
            }
        }

        private void OnAnyNodeSelected(string nodeName)
        {
            UIPositioned2Node node = this.sourse.GetNode(nodeName);
            if (addEdgeStarted)
            {
                if (this.selectedNodes.Count == 1)
                {
                    this.CreateEdgeObject(this.selectedNodes[0], node);
                    node.DeselectThisNode();
                    this.selectedNodes[0].DeselectThisNode();
                    this.selectedNodes.Clear();
                    this.addEdgeStarted = false;
                }

            }
            else
            {
                if (this.selectedNodes.Count > 0)
                {
                    for (int i = this.selectedNodes.Count - 1; i >= 0; i--)
                    {
                        if (this.selectedNodes[i].GetName() != nodeName)
                        {
                            this.selectedNodes[i].DeselectThisNode();
                        }
                        
                    }
                    this.selectedNodes.Clear();
                }
                this.selectedNodes.Add(node);
            }
        }

        private void OnAnyNodeDeselected(string nodeName)
        {
            for (int i = 0; i < this.selectedNodes.Count; i++)
            {
                if (this.selectedNodes[i].GetName() == nodeName)
                {
                    this.selectedNodes.Remove(this.selectedNodes[i]);
                }
            }
            
        }

        public void CreateEdgeObject(UIPositioned2Node sourse, UIPositioned2Node target)
        {
            CheckTheEdgePrefabContent(edgePrefab);

            string edgeName = "" + this.edgeNameSequense;

            try
            {
                this.sourse.CheckPossibilityOfAddingAnEdge(sourse.GetName(), target.GetName(), edgeName);
            }
            catch (ParralelEdgesNotAllowed e)
            {
                Debug.Log(e.Message);
            }
            

            GameObject instance = Instantiate(edgePrefab);
            EdgeVisual edgeVisualComponent = instance.GetComponent<EdgeVisual>();
            edgeVisualComponent.Initialize(sourse, target, edgeName);
            edgeNameSequense += 1;
        }

        


        public void CreateNodeObject()
        {
            CheckTheNodePrefabContent(nodePrefab);
            GameObject instance = Instantiate(nodePrefab);
            Vector2 vector2 = new Vector2(Random.Range(3, -3), Random.Range(3, -3));

            UIPositioned2Node UIComponent = instance.GetComponent<UIPositioned2Node>();
            UIComponent.IsSelected += OnAnyNodeSelected;
            UIComponent.IsDeselected += OnAnyNodeDeselected;

            UIComponent.Initialize(nodeNameSequense.ToString(), vector2);
            sourse.AddNode(UIComponent);
            nodeNameSequense++;

        }


        public void DeleteNode(string nodeName)
        {
            UIPositioned2Node instance = sourse.GetNode(nodeName);
            sourse.DeleteNode(nodeName);
            Destroy(instance.gameObject);

        }



    }
}
