﻿/* 
 * MoxLib Copyright (c) 2020 Paul Ping Kohler
 * 
 */

using UnityEngine;


namespace Mox
{
	public static partial class Extensions
	{

		public static Vector3 ZeroY(this Vector3 v) => new Vector3(v.x, 0f, v.z);

		public static Vector3 ExpandYtoZ(this Vector2 vec) => new Vector3(vec.x, 0, vec.y);
		public static Vector2 CollapseZtoY(this Vector3 vec) => new Vector2(vec.x, vec.z);

		public static Vector3 RemoveY(this Vector3 vec) => new Vector3(vec.x, 0, vec.z);

		public static int ManhattanNorm(this Vector2Int v) => Mathf.Abs(v.x) + Mathf.Abs(v.y);
		public static Vector3 MouseToEulerYaw(this Vector2 v) => new Vector3(0f, v.x, 0);
		public static Vector3 MouseToEulerPitch(this Vector2 v) => new Vector3(v.y, 0, 0);

		public static Vector3 Offset(this Vector3 position, Vector3 direction, float distance = 1f) => position + distance * direction.normalized;

		public static Vector3 Widen(this Vector3 v, float amount) => v + Vector3.right * amount;

		public static Vector3 Remap(this Vector3 v, Transform transform) => transform.forward * v.z + transform.up * v.y + transform.right * v.x;

	}
}