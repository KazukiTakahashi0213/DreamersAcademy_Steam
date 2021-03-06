using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TitleSceneInputSoundState {
	None
	, Normal
	, Max
}

public class TitleSceneInputSoundProvider {
	public TitleSceneInputSoundProvider() {
		states_.Add(new TitleSceneInputSoundNone());
		states_.Add(new TitleSceneInputSoundNormal());
	}

	public TitleSceneInputSoundState state_ = TitleSceneInputSoundState.None;

	private List<BTitleSceneInputSound> states_ = new List<BTitleSceneInputSound>();

	public void UpSelect() { states_[(int)state_].UpSelect(); }
	public void DownSelect() { states_[(int)state_].DownSelect(); }
	public void RightSelect() { states_[(int)state_].RightSelect(); }
	public void LeftSelect() { states_[(int)state_].LeftSelect(); }
	public void SelectEnter() { states_[(int)state_].SelectEnter(); }
}
