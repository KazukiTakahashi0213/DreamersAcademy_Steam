using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterMenuSceneProcess {
	None
	, MonsterSelect
	, MonsterActionSelect
	, SkillSelect
	, SkillActionSelect
	, SkillTradeEventExecute
	, Max
}

public class BMonsterMenuSceneProcessStateProvider {
	public MonsterMenuSceneProcess state_ = MonsterMenuSceneProcess.None;

	protected List<BMonsterMenuSceneProcessState> states_ = new List<BMonsterMenuSceneProcessState>();

	public MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) { return states_[(int)state_].Update(monsterMenuManager); }

	public virtual void init(MonsterMenuManager monsterMenuManager) { }
}
