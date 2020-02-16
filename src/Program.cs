using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SSSP
{
    public static class Program
    {
        static void GenerateRandomGraph(int maxNodes)
        {
            var rand = new Random();
            string to = rand.Next(1, maxNodes / 2).ToString();
            string from = rand.Next(maxNodes / 2 + 1, maxNodes - 1).ToString();

            GraphDTO graphDto = new GraphDTO 
            { 
                edges = new List<GraphEdgeDTO>(),
                nodes = new List<GraphNodeDTO>(),
                path = new RequiredPath 
                { 
                    to = to,
                    from = from
                } 
            };
            
            foreach(var idx in Enumerable.Range(1, maxNodes))
            {
                graphDto.nodes.Add(new GraphNodeDTO { name = idx.ToString() });
                
            }

            foreach(var id in Enumerable.Range(1, maxNodes * 10))
            {
                string firstNode = rand.Next(1, maxNodes / 2).ToString();
                string secondNode = rand.Next(maxNodes / 2 + 1, maxNodes - 1).ToString();
                if (graphDto.edges.Where(edge => (edge.from == firstNode && edge.to == secondNode) || (edge.to == firstNode && edge.from == secondNode)).Count() <= 0)
                {
                    graphDto.edges.Add(new GraphEdgeDTO { from = firstNode, to = secondNode });
                }
            }

            var jsonString = JsonConvert.SerializeObject(graphDto);
            using StreamWriter writer = new StreamWriter("random.json", false);
            writer.WriteLine(jsonString);
        }

        static void Main(string[] args)
        {
            Graph graph;
            string from, to;
            if(args.Length > 0 && !string.IsNullOrEmpty(args[0]))
            {
                (graph, from, to) = ReadGraphFromFile(args[0]);
            }
            else
            {
                Console.WriteLine("No file has been given as an argument. Please input a json file path, or type random to generate a random graph(at most 50% connectivity) that will be saved as random.json :");
                var filePath = Console.ReadLine();
                if(filePath == "random")
                {
                    Console.WriteLine("How many nodes?:");
                    var nodeCount = int.Parse(Console.ReadLine());
                    GenerateRandomGraph(nodeCount);
                    (graph, from, to) = ReadGraphFromFile("random.json");
                }
                else
                    (graph, from, to) = ReadGraphFromFile(filePath);
            }

            if(graph.Nodes.Where(node => node.name == to || node.name == from).Count() != 2)
            {
                throw new Exception("One of the nodes that should be part of the path are not contained in the graph.");
            }

            var watch = new Stopwatch();
            watch.Start();
            var shortestPath = GetShortestPath(graph, to, from);
            watch.Stop();
            Console.WriteLine("Shortest path length: " + shortestPath);
            Console.WriteLine("Computed shortest path in " + watch.ElapsedMilliseconds + " ms.");
            Thread.Sleep(5000);
        }

        private struct QueueData
        {
            public Node currentNode;
            public int currentLength;
        }

        static int GetShortestPath(Graph graph, string to, string from)
        {
            if (to == from) return 0;
            var queue = new Queue<QueueData>();
            var traversed = new List<string>();
            var source = graph.Nodes.Where(node => node.name == from).First();
            queue.Enqueue(new QueueData { currentNode = source, currentLength = 0 });
            traversed.Add(source.name);
            while(queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.currentNode.name == to)
                {
                    return node.currentLength;
                }
                // syntax sugar, but adds complexity
                // for theoretical purposes, please consider this is a method of constant computational complexity (i.e. a hash table)
                var inboundEdges = graph.Edges.Where(edge => edge.from == node.currentNode.name);
                foreach(var edge in inboundEdges)
                {
                    if(!traversed.Contains(edge.to))
                    {
                        traversed.Add(edge.to);
                        var queueNodeData = new QueueData { currentNode = graph.Nodes.Where(node => node.name == edge.to).First(), currentLength = node.currentLength + 1};
                        queue.Enqueue(queueNodeData);
                    }
                }
            }
            return -1;
        }

        static Tuple<Graph, string, string> ReadGraphFromFile(string filePath)
        {
            var graph = new Graph();
            GraphDTO graphDto;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string graphJson = reader.ReadToEnd();
                graphDto = JsonConvert.DeserializeObject<GraphDTO>(graphJson);
            }

            graph.Nodes = graphDto.nodes.Select(node => new Node { name = node.name });
            graph.Edges = graphDto.edges.Select(edge => new Edge { from = edge.from, to = edge.to });

            return Tuple.Create(graph, graphDto.path.from, graphDto.path.to);
        }
    }
}
