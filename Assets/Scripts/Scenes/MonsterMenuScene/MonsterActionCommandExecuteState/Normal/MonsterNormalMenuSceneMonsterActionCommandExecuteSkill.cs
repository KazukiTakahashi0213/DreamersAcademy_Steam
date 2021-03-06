using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNormalMenuSceneMonsterActionCommandExecuteSkill : BMonsterNormalMenuSceneMonsterActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		monsterMenuManager.GetMonsterActionCommandParts().gameObject.SetActive(false);

		monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.SkillSelect;

		monsterMenuManager.GetParameterInfoFrameParts().gameObject.SetActive(false);
		monsterMenuManager.GetSkillInfoFrameParts().gameObject.SetActive(true);

		//技の情報の反映
		monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(PlayerTrainerData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(0));

		monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(true);
	}
}
