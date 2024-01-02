namespace Grape
{
	public record struct Path<TTransition>(
		string Source,
		string Destination,
		Predicate<TTransition> Condition
	);
}
