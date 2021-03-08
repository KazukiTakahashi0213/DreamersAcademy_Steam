using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneCommandExecuteAttack : BBattleSceneCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		//SE
		battleManager.GetInputSoundProvider().SelectEnter();

		//こんらんの処理の初期化
		battleManager.ConfusionProcessStart();

		battleManager.InactiveUiCommand();
		battleManager.ActiveUiAttackCommand();

		return battleManager.nowProcessState().NextProcess();
	}
}
