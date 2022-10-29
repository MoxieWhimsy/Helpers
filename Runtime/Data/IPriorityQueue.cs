namespace Mox.Data
{
	public interface IPriorityQueue<T>
	{
		int Count { get; }
		void Enqueue(T item, int priority);
		bool Dequeue(out T item);
	}
}