namespace Grape
{
    public class StateMachine<TState, TTransition>
        where TState : notnull where TTransition : notnull
    {

        private readonly List<TState> _stateTable;
        private readonly Graph<TState, Predicate<TTransition>> _machineGraph;
        public StateMachine(IEnumerable<TState> states, bool ignoreCase = false)
        {
            _stateTable = [..states];
            _machineGraph = new Graph<TState, Predicate<TTransition>>(_stateTable);
        }

        public void DeclareTransitions(Action<TransitionContainer<TState, TTransition>> proc)
        {
            var container = new TransitionContainer<TState, TTransition>(_machineGraph, _stateTable);
            proc(container);
        }

        public TState? Pass(TState currentState, TTransition transition)
        {
            foreach (var item in _machineGraph.EdgesFrom(currentState))
            {
                if (item == ElsewherePredicate<TTransition>.GetInstance().Predicate) continue;
                if (item(transition))
                {
                    return _machineGraph.Next(currentState, item);
                }
            }

            return currentState;
        }
    }
}