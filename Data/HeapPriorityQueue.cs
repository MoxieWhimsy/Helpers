using System.Collections.Generic;

namespace Mox.Data
{
	public class HeapPriorityQueue<T> : IPriorityQueue<T>
	{
		readonly List<HeapNode> nodes = new List<HeapNode>();

		public int Count => nodes.Count;

		public bool Dequeue(out T result)
		{
			if (nodes.Count <= 0)
			{
				result = default;
				return false;
			}
			result = nodes[0].Value;
			nodes[0] = nodes[Count - 1];
			nodes.RemoveAt(Count - 1);
			nodes.Heapify(Count, 0);
			return true;
		}

		public void Enqueue(T item, int priority)
		{
			nodes.Insert(0, new HeapNode(item, priority));
			nodes.Heapify(Count, 0);
		}

		class HeapNode : IHasPriority
		{
			public readonly T Value;
			public int Priority { get; }

			public HeapNode(T value, int priority)
			{
				this.Value = value;
				Priority = priority;
			}

			public override string ToString() => $"'{Value}' p{Priority}";
		}

		public override string ToString() => string.Join(", ", nodes);
	}
}