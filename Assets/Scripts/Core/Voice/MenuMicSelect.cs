using System.Linq;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;

namespace ProjectNet.Core.Voice
{
	public class MenuMicSelect : MonoBehaviour
	{
		public TMP_Dropdown dropdown;
		public Recorder recorder;

		private void Start()
		{
			var devices = Microphone.devices;
			if (devices.Length == 0) return;
			var list = devices.ToList();
			dropdown.AddOptions(list);
			SetMic(0);
		}

		public void SetMic(int i)
		{
			var devices = Microphone.devices;
			if (devices.Length > i)
			{
				recorder.UnityMicrophoneDevice = devices[i];
			}
		}
	}
}
