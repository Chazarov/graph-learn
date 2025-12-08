using System.Collections.Generic;
using UnityEngine;
using GraphMaster.UnityAdapter.UI;
using UnityEngine.Events;




#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GraphMaster.UnityAdapter
{
    public class GraphUI: MonoBehaviour 
    {
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject edgePrefab;

        private int nodeNameSequense = 1;
        private int edgeNameSequense = 1;

        private GraphMaster.Graph<GraphMaster.UnityAdapter.UI.NodeUI, EdgeUI> sourse = new Graph<NodeUI, EdgeUI>();


        private List<NodeUI> selectedNodes = new List<NodeUI>();
        private List<EdgeUI> selectedEdges = new List<EdgeUI>();
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
            NodeUI uIPositioned2Node = null;
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
            EdgeUI edge = null;
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
            NodeUI node = this.sourse.GetNode(nodeName);
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
                            this.selectedNodes[0].Deselect();
                            this.selectedNodes.Clear();
                            this.selectedNodes.Add(node);
                        }
                    }
                }
                if(this.selectedNodes.Count == 0)
                {
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
                            this.selectedNodes[i].Deselect();
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



        private void OnAnyEdgeSelected(EdgeUI edge)
        {

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            string temp = "";
            Debug.Log($"Select edge: {edge.GetName()}");
            foreach (var ed in this.selectedEdges)
            {
                temp += "----" + ed.GetName();
            }
            Debug.Log($" Selected Edges: {temp}");
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


            if (deletingMode)
            {
                DeleteEdge(edge);
            }
            else if (selectedEdges.Count == 1)
            {
                if (edge == selectedEdges[0])
                {
                    Debug.Log("Breaking");
                    return;
                }
                Debug.Log($"  Replaced {selectedEdges[0].GetName()} ");
                selectedEdges[0].Deselect();
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
            foreach (var edge in selectedEdges)
            {
                edge.SetWeight(weight);
                
            }
        }

        private void OnAnyEdgeDeselected(EdgeUI edge)
        {
            Debug.Log($" Edge deselected {edge.GetName()}");
            selectedEdges.Remove(edge);
            EdgeDeselected.Invoke(edge.GetName());
        }

        public void CreateEdgeObject(NodeUI sourseNode, NodeUI targetNode)
        {
            CheckTheEdgePrefabContent(edgePrefab);

            string edgeName = NumberToLetters(this.edgeNameSequense);

            
            this.sourse.CheckPossibilityOfAddingAnEdge(sourseNode.GetName(), targetNode.GetName(), edgeName);
           
            

            GameObject instance = Instantiate(edgePrefab);
            EdgeUI edgeVisualComponent = instance.GetComponent<EdgeUI>();
            edgeVisualComponent.IsSelected += OnAnyEdgeSelected;
            edgeVisualComponent.IsDeselected += OnAnyEdgeDeselected;
            edgeVisualComponent.Initialize(sourseNode, targetNode, edgeName, edgeNameSequense);
            this.sourse.AddEdge(edgeVisualComponent);
            edgeNameSequense += 1;
        }

        


        public void CreateNodeObject()
        {
            CheckTheNodePrefabContent(nodePrefab);
            GameObject instance = Instantiate(nodePrefab);
            Vector3 instancePosition = new Vector3(UnityEngine.Random.Range(3, -3), UnityEngine.Random.Range(3, -3), instance.transform.position.z);

            NodeUI UIComponent = instance.GetComponent<NodeUI>();
            UIComponent.IsSelected += OnAnyNodeSelected;
            UIComponent.IsDeselected += OnAnyNodeDeselected;

            UIComponent.Initialize(NumberToLetters(nodeNameSequense), instancePosition, nodeNameSequense);
            sourse.AddNode(UIComponent);
            nodeNameSequense++;

        }


        public void DeleteNode(string nodeName)
        {
            NodeUI instance = sourse.GetNode(nodeName);
            
            List<EdgeUI> edgesToDelete = new List<EdgeUI>();
            foreach (EdgeUI edge in sourse.GetEdges())
            {
                if (edge.GetSourceNode().GetName() == nodeName || edge.GetTargetNode().GetName() == nodeName)
                {
                    edgesToDelete.Add(edge);
                }
            }
            
            foreach (EdgeUI edge in edgesToDelete)
            {
                DeleteEdge(edge);
                Destroy(edge.gameObject);
            }
            
            sourse.DeleteNode(nodeName);
            selectedNodes.Remove(instance);

            Destroy(instance.gameObject);

        }

        public void DeleteEdge(string edgeName)
        {

            EdgeUI edge = sourse.GetEdge(edgeName);
            DeleteEdge(edge);
        }

        public void DeleteEdge(EdgeUI edge)
        {
            this.selectedEdges.Remove(edge);
            sourse.DeleteEdge(edge);
            Destroy(edge.gameObject);
        }


        public EdgeUI GetEdge(string name)
        {
            return sourse.GetEdge(name);
        }


    }
}
