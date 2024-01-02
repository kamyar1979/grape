namespace Grape
{
    public class TransitionContainer<TState, TTransition> where TState : notnull where TTransition : notnull
    {
        private Path<TTransition> _path;
        private Graph<TState, Predicate<TTransition>> Holder { get; set; }
        private readonly IDictionary<string, TState> _stateTable;

        internal TransitionContainer(Graph<TState, Predicate<TTransition>> holder,
            IDictionary<string, TState> stateTable)
        {
            Holder = holder;
            _stateTable = stateTable;
        }

        public TransitionContainer<TState, TTransition> From(string sourceState)
        {
            _path = new Path<TTransition>
            {
                Source = sourceState
            };
            return this;
        }

        public TransitionContainer<TState, TTransition> To(string destState)
        {
            _path.Destination = destState;
            return this;
        }

        public TransitionContainer<TState, TTransition> Loop()
        {
            _path.Destination = _path.Source;
            return this;
        }

        public TransitionContainer<TState, TTransition> ElseWhere()
        {
            if (!_stateTable.TryGetValue(_path.Source, out var source) ||
                !_stateTable.TryGetValue(_path.Destination, out var dest))
                throw new ApplicationException("The specified state name does not exist");
            var pred = ElsewherePredicate<TTransition>.GetInstance().Predicate;
            Holder[source, dest] = pred;
            _path = new Path<TTransition>
            {
                Condition = pred,
                Source = _path.Source
            };
            return this;
        }

        public TransitionContainer<TState, TTransition> If(Predicate<TTransition> pred)
        {
            if (!_stateTable.TryGetValue(_path.Source, out var source) ||
                !_stateTable.TryGetValue(_path.Destination, out var dest))
                throw new ApplicationException("The specified state name does not exist");
            Holder[source, dest] = pred;
            _path = new Path<TTransition>
            {
                Condition = pred,
                Source = _path.Source
            };
            return this;
        }
    }
}