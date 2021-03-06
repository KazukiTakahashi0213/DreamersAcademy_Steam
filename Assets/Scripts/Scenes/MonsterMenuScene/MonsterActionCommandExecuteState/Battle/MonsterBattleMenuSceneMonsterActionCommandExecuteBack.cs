using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleMenuSceneMonsterActionCommandExecuteBack : BMonsterBattleMenuSceneMonsterActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		monsterMenuManager.GetMonsterActionCommandParts().gameObject.SetActive(false);

		//操作の変更
		AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalInputProvider();

		monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.MonsterSelect;
	}
}
