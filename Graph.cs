namespace Grape
{
    public class Graph<TNode, TEdge> where TNode : notnull where TEdge : notnull
    {
        private readonly TNode[] _nodes;
        private readonly TEdge[][] _adjacency;

        public Graph(ICollection<TNode> nodes)
        {
            _nodes = nodes.ToArray();
            _adjacency = new TEdge[nodes.Count][];
            for (var i = 0; i < nodes.Count; i++)
            {
                _adjacency[i] = new TEdge[nodes.Count];
            }
        }

        public TEdge this[TNode from, TNode to]
        {
            get => _adjacency[Array.IndexOf(_nodes, from)][Array.IndexOf(_nodes, to)];
            set => _adjacency[Array.IndexOf(_nodes, from)][Array.IndexOf(_nodes, to)] = value;
        }

        public IEnumerable<TEdge> EdgesFrom(TNode node) =>
            _nodes.Select((_, i) => _adjacency[Array.IndexOf(_nodes, node)][i]);


        public IEnumerable<TEdge?> EdgesTo(TNode node) =>
            _nodes.Select((_, i) => _adjacency[Array.IndexOf(_nodes, node)][i]);


        public TNode? Next(TNode node, TEdge edge)
        {
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (Equals(_adjacency[Array.IndexOf(_nodes, node)][i], edge))
                    return _nodes[i];
            }

            return default;
        }

        public TEdge GetEdge(TNode node1, TNode node2) =>
            _adjacency[Array.IndexOf(_nodes, node1)][Array.IndexOf(_nodes, node2)];
    }
}