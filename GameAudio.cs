using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Mox
{
	public class GameAudio : GameSingleton<GameAudio>, ISerializationCallbackReceiver
	{
		[SerializeField] private AudioMixer mixer;
		[SerializeField, Min(1f)] private float multiplier = 20f;
		[SerializeField] private List<string> mixerParameters = new List<string>();
		[Tooltip("The default decibels when mute toggled off, if player hasn't toggled on mute yet")]
		[Range(MinimumDecibels, MaximumDecibels)] [SerializeField]
		private float defaultUnmuteDecibels = -8f;
		private const float MinimumDecibels = -80f;
		private const float MaximumDecibels = -0.05f;
		public const float MinimumValue = 0.0001f;

		private static AudioMixer MainAudioMixer => Instance.mixer;
		public static System.Action<string> ParameterChanged;

		protected override void Initialize()
		{
			Invoke(nameof(LoadAllMixerDecibels), float.Epsilon);
		}

		private void LoadAllMixerDecibels()
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

			value = MinimumValue;
			return false;
		}
		
		public static bool SetVolume(string parameterName, float volume)
		{
			volume = Mathf.Clamp(volume, MinimumValue, ToVolume(MaximumDecibels));
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
			var defaultDecibels = PlayerPrefs.GetFloat($"{parameterName}_Default", Instance.defaultUnmuteDecibels);

			decibels = muteSound ? ToDecibels(MinimumValue) : defaultDecibels;
			PlayerPrefs.SetFloat(parameterName, decibels);
			PlayerPrefs.Save();
			return Ready && MainAudioMixer.SetFloat(parameterName, decibels);
		}
		
		private static float GetDecibelsParameter(string parameterName)
		{
			var mixerSet = Instance.mixer.GetFloat(parameterName, out var decibels);
			decibels = PlayerPrefs.GetFloat(parameterName, mixerSet ? decibels : MinimumDecibels);
			decibels = Mathf.Clamp(decibels, MinimumDecibels, MaximumDecibels);
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
			decibels = Mathf.Clamp(decibels, MinimumDecibels, MaximumDecibels);
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
