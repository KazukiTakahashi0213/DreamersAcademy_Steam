using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneSkillActionCommandExecuteBack : BMonsterMenuSceneSkillActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		monsterMenuManager.GetSkillActionCommandParts().gameObject.SetActive(false);

		monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.SkillSelect;
	}
}
