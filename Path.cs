namespace Grape
{
	public record struct Path<TState, TTransition>(
		TState Source,
		TState Destination,
		Predicate<TTransition> Condition
	);
}
