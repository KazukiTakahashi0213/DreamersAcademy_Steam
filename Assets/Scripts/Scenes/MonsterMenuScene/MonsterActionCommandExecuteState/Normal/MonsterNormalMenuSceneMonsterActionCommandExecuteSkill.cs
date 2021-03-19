using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNormalMenuSceneMonsterActionCommandExecuteSkill : BMonsterNormalMenuSceneMonsterActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		monsterMenuManager.GetMonsterActionCommandParts().gameObject.SetActive(false);

		monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.SkillSelect;

		//技の情報の反映
		monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(PlayerTrainerData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(0));

		//技の選択肢の初期化
		monsterMenuManager.GetSkillCommandParts().commandWindowChoicesColliderActive();

		monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(true);
	}
}
