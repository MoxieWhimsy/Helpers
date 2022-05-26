using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Mox
{
	public class GameAudio : GameSingleton<GameAudio>, ISerializationCallbackReceiver
	{
		[SerializeField] private AudioMixer mixer;
		[SerializeField, Min(1f)] private float multiplier = 30f;
		[SerializeField] private List<string> mixerParameters = new List<string>();
		private const float DefaultDecibels = -8f;
		private const float MaximumDecibels = -0.05f;
		public const float MinimumValue = 0.001f;

		private static AudioMixer MainAudioMixer => Instance.mixer;
		public static System.Action<string> ParameterChanged;

		protected override void Initialize()
		{
			foreach (var parameter in mixerParameters.Where(parameter => Load(mixer, parameter)))
			{
				ParameterChanged?.Invoke(parameter);
			}
		}
		
		internal static bool GetVolume(string parameterName, out float value)
		{
			if (Ready)
			{
				var decibels = GetDecibelsParameter(parameterName);
				value = Mathf.Clamp(ToVolume(decibels), 0, ToVolume(MaximumDecibels));
				return true;
			}

			value = Ready ? ToVolume(DefaultDecibels) : MinimumValue;
			return false;
		}
		
		public static bool SetVolume(string parameterName, float volume)
		{
			volume = Mathf.Clamp(volume, 0, ToVolume(MaximumDecibels));
			var decibels = ToDecibels(volume);
			PlayerPrefs.SetFloat(parameterName, decibels);
			PlayerPrefs.Save();

			var result = Ready && MainAudioMixer.SetFloat(parameterName, decibels);
			if (result) ParameterChanged?.Invoke(parameterName);
			return result;
		}

		public static bool ToggleMute(string parameterName, bool muteSound, bool skipEvent = false)
		{
			if (muteSound && MainAudioMixer.GetFloat(parameterName, out var decibels) && decibels > ToDecibels(MinimumValue))
				PlayerPrefs.SetFloat($"{parameterName}_Default", decibels);
			var defaultDecibels = PlayerPrefs.GetFloat($"{parameterName}_Default", DefaultDecibels);

			decibels = muteSound ? ToDecibels(MinimumValue) : defaultDecibels;
			PlayerPrefs.SetFloat(parameterName, decibels);
			PlayerPrefs.Save();
			return Ready && MainAudioMixer.SetFloat(parameterName, decibels);
		}
		
		[System.Obsolete]
		private static bool GetParameterDecibels(string parameterName, out float decibels)
		{
			decibels = PlayerPrefs.GetFloat(parameterName, DefaultDecibels);
			decibels = Mathf.Min(decibels, MaximumDecibels);
			return true;
		}

		private static float GetDecibelsParameter(string parameterName)
		{
			var mixerSet = Instance.mixer.GetFloat(parameterName, out var decibels);
			decibels = PlayerPrefs.GetFloat(parameterName, mixerSet ? decibels : DefaultDecibels);
			decibels = Mathf.Min(decibels, MaximumDecibels);
			return decibels;
		}

		#if UNITY_EDITOR
		private static IEnumerable<string> GetExposedParameters(AudioMixer audioMixer)
		{
			if (!audioMixer) 
				yield break;

			var propertyInfo = audioMixer.GetType().GetProperty("exposedParameters");
			if (propertyInfo == null) 
				yield break;
			
			var exposedParameters = (System.Array)propertyInfo.GetValue(audioMixer, null);

			foreach (var parameter in exposedParameters)
			{
				yield return parameter.GetType().GetField("name").GetValue(parameter).ToString();
			}
		}
		#endif

		private static bool Load(AudioMixer mixer, string parameterName)
		{
			var decibels = GetDecibelsParameter(parameterName);
			return mixer.SetFloat(parameterName, decibels);
		}

		private static float ToDecibels(float value)
		{
			value = Mathf.Max(value, MinimumValue);
			return Mathf.Log10(value) * Instance.multiplier;
		}

		private static float ToVolume(float decibels)
		{
			decibels = Mathf.Min(decibels, MaximumDecibels);
			return Mathf.Pow(10, decibels / Instance.multiplier);
		}

		public void OnBeforeSerialize()
		{
			#if UNITY_EDITOR
			mixerParameters = GetExposedParameters(mixer).ToList();
			#endif
		}

		public void OnAfterDeserialize()
		{
		}
	}
}
