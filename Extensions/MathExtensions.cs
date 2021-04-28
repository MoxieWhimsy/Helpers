using UnityEngine;

namespace Mox
{
    public static partial class Extensions
    {
        public static int MoveTowards(this int value, int toward, int amount)
		{
            if (Mathf.Abs(toward - value) <= amount)
                return toward;

            if (value > toward)
                return value - amount;

            return value + amount;
		}
    }
}