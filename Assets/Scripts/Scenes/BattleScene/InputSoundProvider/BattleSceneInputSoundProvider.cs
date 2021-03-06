using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleSceneInputSoundState {
	None
	, Normal
	, Max
}

public class BattleSceneInputSoundProvider {
	public BattleSceneInputSoundProvider() {
		states_.Add(new BattleSceneInputSoundNone());
		states_.Add(new BattleSceneInputSoundNormal());
	}

	public BattleSceneInputSoundState state_ = BattleSceneInputSoundState.None;

	private List<BBattleSceneInputSound> states_ = new List<BBattleSceneInputSound>();

	public void UpSelect() { states_[(int)state_].UpSelect(); }
	public void DownSelect() { states_[(int)state_].DownSelect(); }
	public void RightSelect() { states_[(int)state_].RightSelect(); }
	public void LeftSelect() { states_[(int)state_].LeftSelect(); }
	public void SelectEnter() { states_[(int)state_].SelectEnter(); }
}
