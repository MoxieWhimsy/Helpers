/* 
 * MoxLib Copyright (c) 2020 Paul Ping Kohler
 * 
 */

using System.Linq;
using UnityEngine;
using Component = UnityEngine.Component;


namespace Mox
{
	public static partial class Extensions
	{

        public static string ByKeyword(this string[] args, string keyword, string nullOrEmpty = "")
        {
            string result = args.FirstOrDefault(a => a.StartsWith(keyword));
            result = string.IsNullOrEmpty(result) ? nullOrEmpty : result.Substring(keyword.Length).Trim();
            return result;
        }

        public static T FindComponent<T>(this Component component, params string[] paths) where T : UnityEngine.Component
		{
            if (component.FindComponent(out T result))
                return result;

            Debug.LogWarning( $"Debug: {nameof(FindComponent)} did not find a component among {paths}", component);
            return default;
		}

		public static bool FindComponent<T>(this Component component, out T result, params string[] names) where T : UnityEngine.Component
		{
			if (component == null)
			{
				result = null;
				return false;
			}

			if (names.Length == 0)
			{
				return component.TryGetComponent(out result);
			}

			names = names.Select(n => n.ToLower()).ToArray();

			var components = component.GetComponentsInChildren<T>();
			result = components.FirstOrDefault(c => names.Contains(c.name.ToLower()));
			return result;
		}

		public static bool GetValidGameObject(this Component component, out GameObject gameObject, params string[] paths)
        {
            if (component == null)
            {
                gameObject = null;
                return false;
            }

            Transform transform = component.transform;

            if (paths.Length <= 0)
			{
                gameObject = component.gameObject;
                return true;
			}

            foreach(string path in paths)
            {
                var findTransform = transform.Find(path);
                if (findTransform != null)
                {
                    gameObject = findTransform.gameObject;
                    return true;
                }
            }

            gameObject = null;
            return false;
        }

		public static bool HasComponent<T>(this GameObject gameObject) where T : Component
			=> gameObject && gameObject.TryGetComponent<T>(out _);

		public static bool HasComponent<T>(this Component component) where T : Component
			=> component && component.TryGetComponent<T>(out _);

        public static float AsFloat(this int value) => value;

        public static string ShortName(this KeyCode keyCode)
        {
            switch(keyCode)
            {
                case KeyCode.Alpha0:
                    return "0";
                case KeyCode.Alpha1:
                    return "1";
                case KeyCode.Alpha2:
                    return "2";
                case KeyCode.Alpha3:
                    return "3";
                case KeyCode.Alpha4:
                    return "4";
                case KeyCode.Alpha5:
                    return "5";
                case KeyCode.Alpha6:
                    return "6";
                case KeyCode.Alpha7:
                    return "7";
                case KeyCode.Alpha8:
                    return "8";
                case KeyCode.Alpha9:
                    return "9";
            }

            return keyCode.ToString();
        }
	}


}