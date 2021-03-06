using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterBattleMenuSceneMonsterActionCommandExecute {
    None
    , Trade
    , Skill
    , Back
    , Max
}

public class MonsterBattleMenuSceneMonsterActionCommandExecuteStateProvider {
	public MonsterBattleMenuSceneMonsterActionCommandExecuteStateProvider(MonsterBattleMenuSceneMonsterActionCommandExecute setState = MonsterBattleMenuSceneMonsterActionCommandExecute.None) {
		states_.Add(new MonsterBattleMenuSceneMonsterActionCommandExecuteNone());
		states_.Add(new MonsterBattleMenuSceneMonsterActionCommandExecuteTrade());
		states_.Add(new MonsterBattleMenuSceneMonsterActionCommandExecuteSkill());
		states_.Add(new MonsterBattleMenuSceneMonsterActionCommandExecuteBack());

		state_ = setState;
	}

	public MonsterBattleMenuSceneMonsterActionCommandExecute state_;

	private List<BMonsterBattleMenuSceneMonsterActionCommandExecuteState> states_ = new List<BMonsterBattleMenuSceneMonsterActionCommandExecuteState>();

	public void Execute(MonsterMenuManager monsterMenuManager) { states_[(int)state_].Execute(monsterMenuManager); }
}
