using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphMaster;
using Domain;
using System.Linq;
using System.ComponentModel;
using GraphMaster.UnityAdapter.UI;
using UnityEngine.Events;




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
        private List<EdgeVisual> selectedEdges = new List<EdgeVisual>();
        private bool addEdgesMode = false;
        private bool deletingMode = false;


        [SerializeField] public  UnityEvent<string> EdgeSelected;
        [SerializeField] public  UnityEvent<string> EdgeDeselected;



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

        public void SetDeletingMode()
        {
            if (deletingMode) { this.deletingMode = false; }
            else { this.deletingMode = true; }
        }


        private void OnAnyNodeSelected(string nodeName)
        {
            UIPositioned2Node node = this.sourse.GetNode(nodeName);
            if (deletingMode)
            {
                DeleteNode(nodeName);
                return;
            }
            if (addEdgesMode)
            {

                if (this.selectedNodes.Count == 1)
                {
                    try
                    {
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



        private void OnAnyEdgeSelected(EdgeVisual edge)
        {
            
            if (deletingMode)
            {
                DeleteEdge(edge.GetName());
            }
            else if (selectedEdges.Count == 1)
            {
                if (edge != selectedEdges[0])
                {
                    selectedEdges[0].DeselectThisEdge();
                }
                
                selectedEdges.Clear();
                selectedEdges.Add(edge);
                EdgeSelected.Invoke(edge.GetName());
            }
            else if (selectedEdges.Count == 0)
            {
                selectedEdges.Add(edge);
                EdgeSelected.Invoke(edge.GetName());
            }
            
        }

        public void SetSelectedEdgesWeight(float weight)
        {
            Debug.Log($" Set selected weight {weight}");
            foreach (var edge in selectedEdges)
            {
                edge.SetWeight(weight);
                
            }
        }

        private void OnAnyEdgeDeselected(EdgeVisual edge)
        {
            EdgeDeselected.Invoke(edge.GetName());
        }

        public void CreateEdgeObject(UIPositioned2Node sourseNode, UIPositioned2Node targetNode)
        {
            CheckTheEdgePrefabContent(edgePrefab);

            string edgeName = NumberToLetters(this.edgeNameSequense);

            
            this.sourse.CheckPossibilityOfAddingAnEdge(sourseNode.GetName(), targetNode.GetName(), edgeName);
           
            

            GameObject instance = Instantiate(edgePrefab);
            EdgeVisual edgeVisualComponent = instance.GetComponent<EdgeVisual>();
            edgeVisualComponent.IsSelected += OnAnyEdgeSelected;
            edgeVisualComponent.IsDeselected += OnAnyEdgeDeselected;
            edgeVisualComponent.Initialize(sourseNode, targetNode, edgeName);
            this.sourse.AddEdge(edgeVisualComponent);
            edgeNameSequense += 1;
        }

        


        public void CreateNodeObject()
        {
            CheckTheNodePrefabContent(nodePrefab);
            GameObject instance = Instantiate(nodePrefab);
            Vector3 instancePosition = new Vector3(UnityEngine.Random.Range(3, -3), UnityEngine.Random.Range(3, -3), instance.transform.position.z);

            UIPositioned2Node UIComponent = instance.GetComponent<UIPositioned2Node>();
            UIComponent.IsSelected += OnAnyNodeSelected;
            UIComponent.IsDeselected += OnAnyNodeDeselected;

            UIComponent.Initialize(NumberToLetters(nodeNameSequense), instancePosition);
            sourse.AddNode(UIComponent);
            nodeNameSequense++;

        }


        public void DeleteNode(string nodeName)
        {
            UIPositioned2Node instance = sourse.GetNode(nodeName);
            
            List<EdgeVisual> edgesToDelete = new List<EdgeVisual>();
            foreach (EdgeVisual edge in sourse.GetEdges())
            {
                if (edge.GetSourceNode().GetName() == nodeName || edge.GetTargetNode().GetName() == nodeName)
                {
                    edgesToDelete.Add(edge);
                }
            }
            
            foreach (EdgeVisual edge in edgesToDelete)
            {
                sourse.DeleteEdge(edge);
                Destroy(edge.gameObject);
            }
            
            sourse.DeleteNode(nodeName);
            selectedNodes.Remove(instance);

            Destroy(instance.gameObject);

        }

        public void DeleteEdge(string edgeName)
        {
            EdgeVisual edge = sourse.GetEdge(edgeName);
            sourse.DeleteEdge(edge);
            Destroy(edge.gameObject);
        }



    }
}
