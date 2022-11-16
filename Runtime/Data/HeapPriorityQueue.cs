using System.Collections.Generic;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Mox.Data
{
	[System.Serializable]
	public class HeapPriorityQueue<T> : IPriorityQueue<T>
	{
		readonly List<HeapNode> _nodes = new List<HeapNode>();

		public HeapPriorityQueue(bool withMaxHeap = true)
		{
			UsesMaxHeap = withMaxHeap;
		}

		#if ODIN_INSPECTOR
		[ShowInInspector]
		#endif
		public bool UsesMaxHeap { get; private set; }

		#if ODIN_INSPECTOR
		[ShowInInspector]
		#endif
		private IEnumerable<HeapNode> Nodes => _nodes;

		#if ODIN_INSPECTOR
		[ShowInInspector]
		#endif
		public int Count => _nodes.Count;

		#if ODIN_INSPECTOR
		[Button]
		#endif
		public bool Dequeue(out T item)
		{
			if (_nodes.Count <= 0)
			{
				item = default;
				return false;
			}
			item = _nodes[0].Value;
			_nodes[0] = _nodes[Count - 1];
			_nodes.RemoveAt(Count - 1);
			_nodes.Heapify(Count, 0, UsesMaxHeap);
			return true;
		}

		#if ODIN_INSPECTOR
		[Button]
		#endif
		public void Enqueue(T item, int priority)
		{
			_nodes.Insert(0, new HeapNode(item, priority));
			_nodes.Heapify(Count, 0, UsesMaxHeap);
		}

		private class HeapNode : IHasPriority
		{
			#if ODIN_INSPECTOR
			[LabelWidth(50), ShowInInspector, HorizontalGroup()]
			#endif
			public T Value { get; }
			#if ODIN_INSPECTOR
			[LabelWidth(50), ShowInInspector, HorizontalGroup()] 
			#endif
			public int Priority { get; }

			public HeapNode(T value, int priority)
			{
				Value = value;
				Priority = priority;
			}

			public override string ToString() => $"'{Value}' p{Priority}";
		}

		public override string ToString() => string.Join(", ", _nodes);
	}
}