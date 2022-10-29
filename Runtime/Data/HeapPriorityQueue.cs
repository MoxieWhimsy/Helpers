using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Mox.Data
{
	[System.Serializable]
	public class HeapPriorityQueue<T> : IPriorityQueue<T>
	{
		readonly List<HeapNode> nodes = new List<HeapNode>();

		public HeapPriorityQueue(bool withMaxHeap = true)
		{
			UsesMaxHeap = withMaxHeap;
		}

		[ShowInInspector] public bool UsesMaxHeap { get; private set; }

		[ShowInInspector] private IEnumerable<HeapNode> Nodes => nodes;

		[ShowInInspector]
		public int Count => nodes.Count;

		[Button]
		public bool Dequeue(out T item)
		{
			if (nodes.Count <= 0)
			{
				item = default;
				return false;
			}
			item = nodes[0].Value;
			nodes[0] = nodes[Count - 1];
			nodes.RemoveAt(Count - 1);
			nodes.Heapify(Count, 0, UsesMaxHeap);
			return true;
		}

		[Button]
		public void Enqueue(T item, int priority)
		{
			nodes.Insert(0, new HeapNode(item, priority));
			nodes.Heapify(Count, 0, UsesMaxHeap);
		}

		class HeapNode : IHasPriority
		{
			[LabelWidth(40), HorizontalGroup()] 
			public readonly T Value;
			[LabelWidth(50), ShowInInspector, HorizontalGroup()] 
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