using System.Collections.Generic;

namespace Mox
{
	public static partial class Extensions
	{
		public static void Heapify<T>(this List<T> list, int last, int index, bool max = true) where T : IHasPriority
		{
			while (true)
			{
				var most = index;
				var left = 2 * index + 1;
				var right = 2 * index + 2;

				if (left < last && Compare(list[index], list[left], max)) most = left;
				if (right < last && Compare(list[most], list[right], max)) most = right;
				if (most == index) return;

				var temp = list[index];
				list[index] = list[most];
				list[most] = temp;
				index = most;
			}
		}

		private static bool Compare(IHasPriority one, IHasPriority two, bool max = true) 
			=> max ? one.Priority < two.Priority : one.Priority > two.Priority;

		[System.Obsolete]
		public static void MinHeapify<T>(this List<T> list, int last, int index) where T : IHasPriority 
			=> list.Heapify(last, index, false);
	}

	public interface IHasPriority
	{
		int Priority { get; }
	}

}