namespace Grape
{
	internal class ElsewherePredicate<T>
	{
		private static ElsewherePredicate<T>? _instance;
		private ElsewherePredicate() => Predicate = _ => false;
		internal static ElsewherePredicate<T> GetInstance() => _instance ??= new ElsewherePredicate<T>();
		internal Predicate<T> Predicate { get; }
	}
}
