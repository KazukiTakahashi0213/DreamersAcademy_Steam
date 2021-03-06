using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterNormalMenuSceneMonsterActionCommandExecute {
	None
	, Swap
	, Skill
	, Back
	, Max
}

public class MonsterNormalMenuSceneMonsterActionCommandExecuteStateProvider {
	public MonsterNormalMenuSceneMonsterActionCommandExecuteStateProvider(MonsterNormalMenuSceneMonsterActionCommandExecute setState = MonsterNormalMenuSceneMonsterActionCommandExecute.None) {
		states_.Add(new MonsterNormalMenuSceneMonsterActionCommandExecuteNone());
		states_.Add(new MonsterNormalMenuSceneMonsterActionCommandExecuteSwap());
		states_.Add(new MonsterNormalMenuSceneMonsterActionCommandExecuteSkill());
		states_.Add(new MonsterNormalMenuSceneMonsterActionCommandExecuteBack());

		state_ = setState;
	}

	public MonsterNormalMenuSceneMonsterActionCommandExecute state_;

	private List<BMonsterNormalMenuSceneMonsterActionCommandExecuteState> states_ = new List<BMonsterNormalMenuSceneMonsterActionCommandExecuteState>();

	public void Execute(MonsterMenuManager monsterMenuManager) { states_[(int)state_].Execute(monsterMenuManager); }
}
