using System.Collections.Generic;

namespace Mox
{
	public static partial class Extensions
	{
		public static void Heapify<T>(this List<T> list, int last, int index) where T : IHasPriority
		{
			int largest = index;
			int left = 2 * index + 1;
			int right = 2 * index + 2;

			if (left < last && list[index].Priority < list[left].Priority)
				largest = left;
			if (right < last && list[largest].Priority < list[right].Priority)
				largest = right;
			if (largest != index)
			{
				var temp = list[index];
				list[index] = list[largest];
				list[largest] = temp;
				list.Heapify(last, largest);
			}
		}
	}

	public interface IHasPriority
	{
		int Priority { get; }
	}

}