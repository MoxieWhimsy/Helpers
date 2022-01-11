using UnityEngine;

namespace Mox
{
	/// <summary>
	/// MonoBehaviour-based singleton component.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GameSingleton<T> : MonoBehaviour where T : GameSingleton<T>
	{
		protected static T Instance { get; private set; }
		public static bool Ready => Instance;
		
		private void Awake()
		{
			if (Instance == this) 
				return;
			
			if (Instance)
			{
				Destroy(this);
				return;
			}

			Instance = this as T;
			Initialize();
		}
		
		protected virtual void Initialize()
		{
		}
	}
}