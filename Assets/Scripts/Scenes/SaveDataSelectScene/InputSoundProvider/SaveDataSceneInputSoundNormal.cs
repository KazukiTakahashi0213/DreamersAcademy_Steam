using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataSceneInputSoundNormal : BSaveDataSceneInputSound {
	public override void UpSelect() {
		AudioClip data = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathCursor1());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().PlayOneShot(data);
	}
	public override void DownSelect() {
		AudioClip data = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathCursor1());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().PlayOneShot(data);
	}
	public override void RightSelect() {
		AudioClip data = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathCursor1());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().PlayOneShot(data);
	}
	public override void LeftSelect() {
		AudioClip data = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathCursor1());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().PlayOneShot(data);
	}
	public override void SelectEnter() {
		AudioClip data = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathSelect1());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().PlayOneShot(data);
	}
}
