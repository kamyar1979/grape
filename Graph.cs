namespace Grape
{
    public class Graph<TState, TEdge> where TState : notnull where TEdge : notnull
    {
        private readonly TState[] _nodes;
        private readonly TEdge[][] _adjacency;

        public Graph(ICollection<TState> nodes)
        {
            _nodes = nodes.ToArray();
            _adjacency = new TEdge[nodes.Count][];
            for (var i = 0; i < nodes.Count; i++)
            {
                _adjacency[i] = new TEdge[nodes.Count];
            }
        }

        public TEdge this[TState from, TState to]
        {
            get => _adjacency[Array.IndexOf(_nodes, from)][Array.IndexOf(_nodes, to)];
            set => _adjacency[Array.IndexOf(_nodes, from)][Array.IndexOf(_nodes, to)] = value;
        }

        public IEnumerable<TEdge> EdgesFrom(TState node) =>
            _nodes.Select((_, i) => _adjacency[Array.IndexOf(_nodes, node)][i]).Where(c => c is not null);


        public IEnumerable<TEdge?> EdgesTo(TState node) =>
            _nodes.Select((_, i) => _adjacency[Array.IndexOf(_nodes, node)][i]);


        public TState? Next(TState node, TEdge edge)
        {
            for (var i = 0; i < _nodes.Length; i++)
            {
                if (Equals(_adjacency[Array.IndexOf(_nodes, node)][i], edge))
                    return _nodes[i];
            }

            return default;
        }

        public TEdge GetEdge(TState node1, TState node2) =>
            _adjacency[Array.IndexOf(_nodes, node1)][Array.IndexOf(_nodes, node2)];
    }
}