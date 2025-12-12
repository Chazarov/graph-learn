using System.Collections.Generic;
using UnityEngine;
using GraphMaster.UnityAdapter.UI;
using UnityEngine.Events;
using System.Linq;





#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GraphMaster.UnityAdapter
{
    public class GraphUI: MonoBehaviour 
    {
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private GameObject edgePrefab;


        [SerializeField] private Vector2 upperCreationRange =  Vector2.zero;
        [SerializeField] private Vector2 lowerCreationRange = Vector2.zero;

        [SerializeField] private Transform cursorTransform;

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

        private void SetRoot(NodeUI root)
        {
            var prev = sourse.GetRoot();
            if (prev != null)
            {
                prev.RemoveRoot();
            }
            root.MarkAsRootWithoutNotify();
            sourse.SetRoot(root);

        }
        public void CreateNodeObject()
        {
            CheckTheNodePrefabContent(nodePrefab);
            GameObject instance = Instantiate(nodePrefab);
            Vector3 instancePosition = new Vector3(UnityEngine.Random.Range(upperCreationRange.x, lowerCreationRange.x), UnityEngine.Random.Range(upperCreationRange.y, lowerCreationRange.y), instance.transform.position.z);

            NodeUI UIComponent = instance.GetComponent<NodeUI>();
            UIComponent.IsSelected += OnAnyNodeSelected;
            UIComponent.IsDeselected += OnAnyNodeDeselected;
            UIComponent.IsRootMarking += SetRoot;

            UIComponent.Initialize(NumberToLetters(nodeNameSequense), instancePosition, nodeNameSequense);

            bool thisNOdeFirst = !sourse.HasNodes();

            sourse.AddNode(UIComponent);
            nodeNameSequense++;
            if (thisNOdeFirst)
            {
                Debug.Log("This Node first");
                this.SetRoot(UIComponent);
            }

        }



        private void OnAnyEdgeSelected(EdgeUI edge)
        {



            if (deletingMode)
            {
                DeleteEdge(edge);
            }
            else if (selectedEdges.Count == 1)
            {
                if (edge == selectedEdges[0])
                {
                    return;
                }
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

            edgeVisualComponent.Initialize(sourseNode, targetNode, edgeName, edgeNameSequense, this.GetIsDirected());
            this.sourse.AddEdge(edgeVisualComponent);
            edgeNameSequense += 1;



            this.UpdateDirectedEdgesViews(sourseNode, targetNode);
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
            this.UpdateDirectedEdgesViews(edge.GetSourceNode(), edge.GetTargetNode());
        }


        public EdgeUI GetEdge(string name)
        {
            return sourse.GetEdge(name);
        }

        public bool GetIsDirected()
        {
            return sourse.GetIsDirected();
        }

        public bool GetIsParallel()
        {
            return sourse.GetIsParralel();
        }

        public void SetParralel(bool value)
        {
            sourse.SetParralelEdgesAllowed(value);
        }

        public void SetDirected(bool value)
        {

            sourse.SetDirected(value);

            var edges = sourse.GetEdges();
            foreach (EdgeUI edge in edges)
            {
                edge.SetDirected(value);
            }
        }


        public void UpdateDirectedEdgesViews(NodeUI node1, NodeUI node2)
        {
            

            Debug.Log(this.GetAdjacencyMapAsJson());
            string name1 = node1.GetName();
            string name2 = node2.GetName();
            List<EdgeUI> edgesToUpdate = this.sourse.GetEdgesBetween(name1, name2);

            UpdateDirectedEdgesViews(edgesToUpdate);
        }
        public void UpdateDirectedEdgesViews(List<EdgeUI> edges)
        {

            int l = edges.Count;
            if(l == 2)
            {
                edges[0].SetEdgeCenterOffset(1);
                edges[1].SetEdgeCenterOffset(2);
                return;
            }

            int l2 = l;
            if (l % 2 == 0) l2 += 1;

            int counter = l;
            int b = 0;
            int c = 0;

            for (int i = 0; i < l; i++) 
            {
                b += 2;
                c = (b - 1) % l2;

                if (l2 > b)
                {
                    edges[i].SetEdgeCenterOffset(l2 - b);
                }
                else
                {
                    edges[i].SetEdgeCenterOffset(c);
                }
            }
        }

        /// <summary>
        /// Возвращает внутренний граф для использования в алгоритмах.
        /// </summary>
        public Graph<NodeUI, EdgeUI> GetGraph()
        {
            return sourse;
        }

        /// <summary>
        /// Возвращает AdjacencyMap в виде отформатированной JSON-строки.
        /// </summary>
        public string GetAdjacencyMapAsJson()
        {
            var adjMap = sourse.GetAdjacencyMap();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("{");

            var sourceKeys = adjMap.Keys.ToList();
            for (int i = 0; i < sourceKeys.Count; i++)
            {
                string sourceNode = sourceKeys[i];
                sb.AppendLine($"  \"{sourceNode}\": {{");

                var targetKeys = adjMap[sourceNode].Keys.ToList();
                for (int j = 0; j < targetKeys.Count; j++)
                {
                    string targetNode = targetKeys[j];
                    var edges = adjMap[sourceNode][targetNode];
                    var edgeNames = edges.Select(e => $"\"{e.GetName()}\"");
                    
                    sb.Append($"    \"{targetNode}\": [{string.Join(", ", edgeNames)}]");
                    sb.AppendLine(j < targetKeys.Count - 1 ? "," : "");
                }

                sb.Append("  }");
                sb.AppendLine(i < sourceKeys.Count - 1 ? "," : "");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

    }
}
