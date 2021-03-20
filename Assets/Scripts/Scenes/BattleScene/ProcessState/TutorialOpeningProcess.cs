using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOpeningProcess : IProcessState {
	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new OpeningProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		if (AllEventManager.GetInstance().EventUpdate()) {
			
		}

		if (allSceneMgr.inputProvider_.UpSelect()) {
		}
		else if (allSceneMgr.inputProvider_.DownSelect()) {
		}
		else if (allSceneMgr.inputProvider_.RightSelect()
			|| allSceneMgr.inputProvider_.RightSelectMouseButton()) {
			mgr.GetTutorialParts().RightButtonDown();
		}
		else if (allSceneMgr.inputProvider_.LeftSelect()
			|| allSceneMgr.inputProvider_.LeftSelectMouseButton()) {
			mgr.GetTutorialParts().LeftButtonDown();
		}
		else if (allSceneMgr.inputProvider_.SelectEnter()) {
		}
		else if (allSceneMgr.inputProvider_.SelectBack()
			|| allSceneMgr.inputProvider_.SelectBackMouseButton()) {
			AllEventManager.GetInstance().EventTriggerNext();

			mgr.TutorialOpeningEventSetNext();

			return mgr.nowProcessState().NextProcess();
		}
		else if (allSceneMgr.inputProvider_.SelectNovelWindowActive()) {
		}

		return this;
	}
}
