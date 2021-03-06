using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMenuSceneInputSoundState {
	None
	, Normal
	, MonsterSelect
	, Max
}

public class MonsterMenuSceneInputSoundProvider {
	public MonsterMenuSceneInputSoundProvider() {
		states_.Add(new MonsterMenuSceneInputSoundNone());
		states_.Add(new MonsterMenuSceneInputSoundNormal());
		states_.Add(new MonsterMenuSceneInputSoundMonsterSelect());
	}

	public MonsterMenuSceneInputSoundState state_ = MonsterMenuSceneInputSoundState.None;

	private List<BMonsterMenuSceneInputSound> states_ = new List<BMonsterMenuSceneInputSound>();

	public void UpSelect() { states_[(int)state_].UpSelect(); }
	public void DownSelect() { states_[(int)state_].DownSelect(); }
	public void RightSelect() { states_[(int)state_].RightSelect(); }
	public void LeftSelect() { states_[(int)state_].LeftSelect(); }
	public void SelectEnter() { states_[(int)state_].SelectEnter(); }
}
