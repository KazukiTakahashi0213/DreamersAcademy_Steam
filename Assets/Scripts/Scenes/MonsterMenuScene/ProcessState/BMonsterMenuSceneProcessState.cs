using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMonsterMenuSceneProcessState {
	public virtual MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) { return monsterMenuManager.GetNowProcessState().state_; }
}
