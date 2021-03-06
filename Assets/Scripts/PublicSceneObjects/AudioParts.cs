using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParts : MonoBehaviour {
	[SerializeField] private AudioSource BGMAudioSource_ = null;
	[SerializeField] private AudioSource SEAudioSource_ = null;

	public AudioSource GetBGMAudioSource() { return BGMAudioSource_; }
	public AudioSource GetSEAudioSource() { return SEAudioSource_; }
}
