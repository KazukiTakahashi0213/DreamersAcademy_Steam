using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugMenuSceneInputSoundState {
	None
	, Normal
	, Max
}

public class BugMenuSceneInputSoundProvider {
	public BugMenuSceneInputSoundProvider() {
		states_.Add(new BugMenuSceneInputSoundNone());
		states_.Add(new BugMenuSceneInputSoundNormal());
	}

	public BugMenuSceneInputSoundState state_ = BugMenuSceneInputSoundState.None;

	private List<BBugMenuSceneInputSound> states_ = new List<BBugMenuSceneInputSound>();

	public void UpSelect() { states_[(int)state_].UpSelect(); }
	public void DownSelect() { states_[(int)state_].DownSelect(); }
	public void RightSelect() { states_[(int)state_].RightSelect(); }
	public void LeftSelect() { states_[(int)state_].LeftSelect(); }
	public void SelectEnter() { states_[(int)state_].SelectEnter(); }
}
