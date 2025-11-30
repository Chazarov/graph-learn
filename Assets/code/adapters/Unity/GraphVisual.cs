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

        private GraphMaster.Graph<GraphMaster.UnityAdapter.UI.UIPositioned2Node, EdgeVisual> sourse = new Graph<UIPositioned2Node, EdgeVisual>();


        private List<UIPositioned2Node> selectedNodes = new List<UIPositioned2Node>();
        private bool addEdgesMode = false;


        private void Start()
        {

            CheckTheNodePrefabContent(nodePrefab);
        }

        private string NumberToLetters(int number)
        {
            string result = "";
            while (number > 0)
            {
                number--;
                result = (char)('A' + (number % 26)) + result;
                number /= 26;
            }
            return result;
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

        public  void SetAddEdgesMode()
        {
            if (addEdgesMode) { this.addEdgesMode = false; }
            else { this.addEdgesMode = true; }
        }

        private void OnAnyNodeSelected(string nodeName)
        {
            UIPositioned2Node node = this.sourse.GetNode(nodeName);
            if (addEdgesMode)
            {

                if (this.selectedNodes.Count == 1)
                {
                    try
                    {
                        Debug.Log("Try creaate nodes more them 1");
                        this.CreateEdgeObject(this.selectedNodes[0], node);
                    }
                    catch (ParralelEdgesNotAllowed e)
                    {
                        Debug.LogWarning(e.Message);
                        return;
                    }
                    catch (LoopsNotAllowed e)
                    {
                        Debug.LogWarning(e.Message);
                        return;
                    }
                    finally
                    {
                        if (this.selectedNodes[0].GetName() != nodeName)
                        {
                            this.selectedNodes[0].DeselectThisNode();
                            this.selectedNodes.Clear();
                            this.selectedNodes.Add(node);
                        }
                    }
                }
                if(this.selectedNodes.Count == 0)
                {
                    Debug.Log($"Add node in list count :{selectedNodes.Count}");
                    this.selectedNodes.Add(node);
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

        public void CreateEdgeObject(UIPositioned2Node sourseNode, UIPositioned2Node targetNode)
        {
            CheckTheEdgePrefabContent(edgePrefab);

            string edgeName = NumberToLetters(this.edgeNameSequense);

            
            this.sourse.CheckPossibilityOfAddingAnEdge(sourseNode.GetName(), targetNode.GetName(), edgeName);
           
            

            GameObject instance = Instantiate(edgePrefab);
            EdgeVisual edgeVisualComponent = instance.GetComponent<EdgeVisual>();
            edgeVisualComponent.Initialize(sourseNode, targetNode, edgeName);
            this.sourse.AddEdge(edgeVisualComponent);
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

            UIComponent.Initialize(NumberToLetters(nodeNameSequense), vector2);
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
