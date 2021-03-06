using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMenuSceneSkillActionCommandExecute {
	None
	, Swap
	, Trade
	, Back
	, Max
}

public class MonsterMenuSceneSkillActionCommandExecuteStateProvider {
	public MonsterMenuSceneSkillActionCommandExecuteStateProvider(MonsterMenuSceneSkillActionCommandExecute setState = MonsterMenuSceneSkillActionCommandExecute.None) {
		states_.Add(new MonsterMenuSceneSkillActionCommandExecuteNone());
		states_.Add(new MonsterMenuSceneSkillActionCommandExecuteSwap());
		states_.Add(new MonsterMenuSceneSkillActionCommandExecuteTrade());
		states_.Add(new MonsterMenuSceneSkillActionCommandExecuteBack());

		state_ = setState;
	}

	public MonsterMenuSceneSkillActionCommandExecute state_;

	private List<BMonsterMenuSceneSkillActionCommandExecuteState> states_ = new List<BMonsterMenuSceneSkillActionCommandExecuteState>();

	public void Execute(MonsterMenuManager monsterMenuManager) { states_[(int)state_].Execute(monsterMenuManager); }
}
