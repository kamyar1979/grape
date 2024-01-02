﻿namespace Grape
{
    public class TransitionContainer<TState, TTransition> where TState : notnull where TTransition : notnull
    {
        private Path<TState, TTransition> _path;
        private Graph<TState, Predicate<TTransition>> Holder { get; set; }
        private readonly List<TState> _stateTable;

        internal TransitionContainer(Graph<TState, Predicate<TTransition>> holder,
            IEnumerable<TState> stateTable)
        {
            Holder = holder;
            _stateTable = [..stateTable];
        }

        public TransitionContainer<TState, TTransition> From(TState sourceState)
        {
            _path = new Path<TState, TTransition>
            {
                Source = sourceState
            };
            return this;
        }

        public TransitionContainer<TState, TTransition> To(TState destState)
        {
            _path.Destination = destState;
            return this;
        }

        public TransitionContainer<TState, TTransition> Loop()
        {
            _path.Destination = _path.Source;
            return this;
        }

        public TransitionContainer<TState, TTransition> ElseWhere(TState state)
        {
            if (!_stateTable.Contains(_path.Source) || !_stateTable.Contains(state))
                throw new ApplicationException("The specified state name does not exist");
            Holder[_path.Source, state] = ElsewherePredicate<TTransition>.GetInstance().Predicate;;
            return this;
        }

        public TransitionContainer<TState, TTransition> If(Predicate<TTransition> pred)
        {
            if (!_stateTable.Contains(_path.Source) || !_stateTable.Contains(_path.Destination))
                throw new ApplicationException("The specified state name does not exist");
            Holder[_path.Source, _path.Destination] = pred;
            _path = new Path<TState, TTransition>
            {
                Condition = pred,
                Source = _path.Source
            };
            return this;
        }
    }
}