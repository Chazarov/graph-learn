

using Assets.code.adapters.graph_master.entity.interfaces;
using GraphMaster.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GraphMaster.Service
{
    internal class FromJSONConverter<TNode, TEdge> where TNode : IGraphNode where TEdge : IGraphEdge<TNode>
    {
        private IGraph<TNode, TEdge> graph;
        private IGraphObjectsFabric<TNode, TEdge> fabric;
        private IJsonMetadataParser metadataParser;

        public FromJSONConverter(IGraph<TNode, TEdge> graph, IGraphObjectsFabric<TNode, TEdge> fabric, IJsonMetadataParser metadataParser)
        {
            this.graph = graph;
            this.fabric = fabric;
            this.metadataParser = metadataParser;
        }

        public void ParseFromJson(string rawJson)
        {
            JObject jsonObject = JObject.Parse(rawJson);
            JObject adjMap = (JObject)jsonObject["adj_map"];
            
            Dictionary<string, TNode> nodeCache = new Dictionary<string, TNode>();

            foreach (var nodeEntry in adjMap)
            {
                string nodeName = nodeEntry.Key;
                JObject nodeData = (JObject)nodeEntry.Value;
                
                IGraphPartData nodeMeta = metadataParser.ParseNodeMeta(nodeData.ToString());
                TNode node = fabric.CreateNode(nodeMeta, nodeName);
                graph.AddNode(node);
                
                nodeCache[nodeName] = node;
            }

            foreach (var nodeEntry in adjMap)
            {
                string sourceNodeName = nodeEntry.Key;
                JObject nodeData = (JObject)nodeEntry.Value;
                JArray edges = (JArray)nodeData["edges"];
                
                TNode sourceNode = nodeCache[sourceNodeName];

                if (edges != null)
                {
                    foreach (JObject edgeData in edges)
                    {
                        string targetNodeName = edgeData["to"].ToString();
                        TNode targetNode = nodeCache[targetNodeName];
                        
                        IGraphPartData edgeMeta = metadataParser.ParseEdgeMeta(edgeData.ToString());
                        string edgeName = Guid.NewGuid().ToString();
                        TEdge edge = fabric.CreateEdge(edgeMeta, edgeName, sourceNode, targetNode);
                        graph.AddEdge(edge);
                    }
                }
            }
        }
    }
}
