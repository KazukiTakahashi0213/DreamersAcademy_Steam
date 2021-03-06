using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneSkillActionCommandExecuteSwap : BMonsterMenuSceneSkillActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		//入れ替え状態に変更
		monsterMenuManager.swapSelectNumber_ = monsterMenuManager.GetSkillCommandParts().GetSelectNumber();
		monsterMenuManager.swapActive_ = true;

		//技の画像の色の変更
		monsterMenuManager.GetSkillInfoMenuParts().GetSkillInfoMenuSprite(monsterMenuManager.swapSelectNumber_).color = new Color32(222, 255, 0, 255);

		monsterMenuManager.GetSkillActionCommandParts().gameObject.SetActive(false);

		monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.SkillSelect;
	}
}
