namespace Grape
{
    public class StateMachine<TState, TTransition>(
        bool ignoreCase,
        Graph<TState, Predicate<TTransition>> machineGraph) 
        where TState : notnull where TTransition : notnull
    {
        private readonly Dictionary<string, TState> _stateTable = ignoreCase
            ? new Dictionary<string, TState>(StringComparer.InvariantCultureIgnoreCase)
            : new Dictionary<string, TState>();

        private Graph<TState, Predicate<TTransition>> MachineGraph { get; set; } = machineGraph;

        public TState this[string name]
        {
            get => _stateTable[name];
            set => _stateTable[name] = value;
        }

        public StateMachine(Graph<TState, Predicate<TTransition>> machineGraph) : this(true, machineGraph)
        {
        }

        public void DeclareStates(IDictionary<string, TState> states)
        {
            foreach (var item in states)
            {
                _stateTable.Add(item.Key, item.Value);
            }
            MachineGraph = new Graph<TState, Predicate<TTransition>>(states.Values);
        }

        public void DeclareTransitions(Action<TransitionContainer<TState, TTransition>> proc)
        {
            var container = new TransitionContainer<TState, TTransition>(MachineGraph, _stateTable);
            proc(container);
        }

        public TState? Pass(TState currentState, TTransition transition)
        {
            foreach (var item in MachineGraph.EdgesFrom(currentState))
            {
                if (item == ElsewherePredicate<TTransition>.GetInstance().Predicate) continue;
                if (item(transition))
                {
                    return MachineGraph.Next(currentState, item);
                }
            }

            return currentState;
        }
    }
}