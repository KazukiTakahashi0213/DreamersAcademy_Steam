using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapSceneInputSoundState {
	None
	, Normal
	, Max
}

public class MapSceneInputSoundProvider {
	public MapSceneInputSoundProvider() {
		states_.Add(new MapSceneInputSoundNone());
		states_.Add(new MapSceneInputSoundNormal());
	}

	public MapSceneInputSoundState state_ = MapSceneInputSoundState.None;

	private List<BMapSceneInputSound> states_ = new List<BMapSceneInputSound>();

	public void UpSelect() { states_[(int)state_].UpSelect(); }
	public void DownSelect() { states_[(int)state_].DownSelect(); }
	public void RightSelect() { states_[(int)state_].RightSelect(); }
	public void LeftSelect() { states_[(int)state_].LeftSelect(); }
	public void SelectEnter() { states_[(int)state_].SelectEnter(); }
	public void SelectMenu() { states_[(int)state_].SelectMenu(); }
}
