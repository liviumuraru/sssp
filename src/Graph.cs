using System;
using System.Collections.Generic;
using System.Text;

namespace SSSP
{
    public class Node
    {
        public string name;
        public List<Node> neighbors;
    }

    public class Edge
    {
        public string from;
        public string to;
    }

    public class Graph
    {
        public IEnumerable<Node> Nodes { get; set; }
        public IEnumerable<Edge> Edges { get; set; }
    }
}
