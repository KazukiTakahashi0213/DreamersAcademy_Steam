using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningProcess : IProcessState {
	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new CommandSelectProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		if (AllEventManager.GetInstance().EventUpdate()) {
			mgr.GetPlayerStatusInfoParts().ProcessIdleStart();
			mgr.GetPlayerMonsterParts().ProcessIdleStart();

			return mgr.nowProcessState().NextProcess();
		}

		if (allSceneMgr.inputProvider_.UpSelect()) {
		}
		else if (allSceneMgr.inputProvider_.DownSelect()) {
		}
		else if (allSceneMgr.inputProvider_.RightSelect()) {
		}
		else if (allSceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (allSceneMgr.inputProvider_.SelectEnter()) {
			AllEventManager.GetInstance().EventTriggerNext();
		}
		else if (allSceneMgr.inputProvider_.SelectNovelWindowActive()) {
		}

		return this;
	}
}
