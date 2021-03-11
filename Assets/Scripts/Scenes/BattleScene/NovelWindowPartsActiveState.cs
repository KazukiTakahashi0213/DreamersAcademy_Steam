using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NovelWindowPartsActive {
	Active,
	Inactive,
	Max
}

public class NovelWindowPartsActiveState {
	public NovelWindowPartsActiveState(NovelWindowPartsActive setState) {
		state_ = setState;
	}

	public NovelWindowPartsActive state_;

	//Active
	static private NovelWindowPartsActive ActiveNext(NovelWindowPartsActiveState mine, BattleManager battleManager) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		allSceneMgr.inputProvider_ = new KeyBoardNovelWindowInactiveInputProvider();

		battleManager.GetNovelWindowParts().gameObject.SetActive(false);
		battleManager.GetCommandCommandParts().gameObject.SetActive(!battleManager.GetCommandCommandParts().gameObject.activeSelf);

		return NovelWindowPartsActive.Inactive;
	}

	//Inactive
	static private NovelWindowPartsActive InactiveNext(NovelWindowPartsActiveState mine, BattleManager battleManager) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		allSceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();

		battleManager.GetNovelWindowParts().gameObject.SetActive(true);
		battleManager.GetCommandCommandParts().gameObject.SetActive(!battleManager.GetCommandCommandParts().gameObject.activeSelf);

		return NovelWindowPartsActive.Active;
	}

	private delegate NovelWindowPartsActive NextFunc(NovelWindowPartsActiveState mine, BattleManager battleManager);
	private NextFunc[] nexts_ = new NextFunc[(int)NovelWindowPartsActive.Max] {
		ActiveNext,
		InactiveNext
	};
	public NovelWindowPartsActive Next(BattleManager battleManager) { return nexts_[(int)state_](this, battleManager); }
}
