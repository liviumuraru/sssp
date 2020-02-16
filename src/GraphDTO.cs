using System.Collections.Generic;

namespace SSSP
{
    public class GraphDTO
    {
        public List<GraphNodeDTO> nodes;
        public List<GraphEdgeDTO> edges;
        public RequiredPath path;
    }

    public class GraphNodeDTO
    {
        public string name;
    }

    public class GraphEdgeDTO
    {
        public string from;
        public string to;
    }

    public class RequiredPath
    {
        public string from;
        public string to;
    }
}
