using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneNormalProcessSkillTradeEventExecute : BMonsterMenuSceneProcessState {
	public override MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		if (eventMgr.EventUpdate()) {
			return MonsterMenuSceneProcess.SkillSelect;
		}

		if (sceneMgr.inputProvider_.UpSelect()) {
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| sceneMgr.inputProvider_.SelectMouseLeftTrigger()) {
			eventMgr.EventTriggerNext();
		}
		else if (sceneMgr.inputProvider_.SelectBack()) {
		}

		return monsterMenuManager.GetNowProcessState().state_;
	}
}
