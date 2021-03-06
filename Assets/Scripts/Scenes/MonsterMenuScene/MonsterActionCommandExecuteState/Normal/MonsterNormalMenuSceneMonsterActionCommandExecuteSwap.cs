using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNormalMenuSceneMonsterActionCommandExecuteSwap : BMonsterNormalMenuSceneMonsterActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		//入れ替え状態に変更
		monsterMenuManager.swapSelectNumber_ = monsterMenuManager.selectMonsterNumber_;
		monsterMenuManager.swapActive_ = true;

		//バレットの色の変更
		monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(2).GetBaseParts().GetBaseSprite().color = new Color32(222, 255, 0, 255);

		monsterMenuManager.GetMonsterActionCommandParts().gameObject.SetActive(false);

		//操作の変更
		AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalInputProvider();

		monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.MonsterSelect;
	}
}
