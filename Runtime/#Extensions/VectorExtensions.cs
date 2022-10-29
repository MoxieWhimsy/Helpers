/* 
 * MoxLib Copyright (c) 2020 Paul Ping Kohler
 * 
 */

using System.Collections.Generic;
using UnityEngine;


namespace Mox
{
	public static partial class Extensions
	{

		public static Vector3 ZeroY(this Vector3 v) => new Vector3(v.x, 0f, v.z);

		public static Vector3 ExpandAddZ(this Vector2 vec, float z) => new Vector3(vec.x, vec.y, z);
		public static Vector3 ExpandYtoZ(this Vector2 vec) => new Vector3(vec.x, 0, vec.y);
		public static Vector3 ExpandYtoZ(this Vector2Int vec) => new Vector3(vec.x, 0, vec.y);
		public static Vector2 CollapseZtoY(this Vector3 vec) => new Vector2(vec.x, vec.z);

		public static Vector3 RemoveY(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);

		public static int ManhattanNorm(this Vector2Int v) => Mathf.Abs(v.x) + Mathf.Abs(v.y);

		public static int SquareOneDistance(this Vector2Int v)
		{
			var xDistance = Mathf.Abs(v.x);
			var yDistance = Mathf.Abs(v.y);
			var remaining = Mathf.Abs(xDistance - yDistance);
			return Mathf.Min(xDistance, yDistance) + remaining;
		}

		public static Vector2Int ToEightDirection(this Vector2Int v)
		{
			if (v.x == 0 && v.y == 0)
				return Vector2Int.zero;

			var x = v.x > 0 ? 1 : v.x < 0 ? -1 : 0;
			var y = v.y > 0 ? 1 : v.y < 0 ? -1 : 0;
			
			return new Vector2Int(x, y);
		}
		public static Vector3 MouseToEulerYaw(this Vector2 v) => new Vector3(0f, v.x, 0);
		public static Vector3 MouseToEulerPitch(this Vector2 v) => new Vector3(v.y, 0, 0);

		public static Vector3 Offset(this Vector3 position, Vector3 direction, float distance = 1f) => position + distance * direction.normalized;

		public static Vector2Int PickRandom(this List<Vector2Int> list) => list[Random.Range(0, list.Count)];
		public static Vector3 PickRandom(this List<Vector3> list) => list[Random.Range(0, list.Count)];
		public static Vector3 Widen(this Vector3 v, float amount) => v + Vector3.right * amount;

		public static Vector3 Remap(this Vector3 v, Transform transform) => transform.forward * v.z + transform.up * v.y + transform.right * v.x;

	}
}