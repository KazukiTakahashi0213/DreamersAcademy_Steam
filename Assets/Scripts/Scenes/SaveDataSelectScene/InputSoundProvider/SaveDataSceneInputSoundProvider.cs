using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SaveDataSceneInputSoundState {
	None
	, Normal
	, Max
}

public class SaveDataSceneInputSoundProvider {
	public SaveDataSceneInputSoundProvider() {
		states_.Add(new SaveDataSceneInputSoundNone());
		states_.Add(new SaveDataSceneInputSoundNormal());
	}

	public SaveDataSceneInputSoundState state_ = SaveDataSceneInputSoundState.None;

	private List<BSaveDataSceneInputSound> states_ = new List<BSaveDataSceneInputSound>();

	public void UpSelect() { states_[(int)state_].UpSelect(); }
	public void DownSelect() { states_[(int)state_].DownSelect(); }
	public void RightSelect() { states_[(int)state_].RightSelect(); }
	public void LeftSelect() { states_[(int)state_].LeftSelect(); }
	public void SelectEnter() { states_[(int)state_].SelectEnter(); }
}
