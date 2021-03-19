using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneStartCommandExecuteAttack : BBattleSceneStartCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		//SE
		battleManager.GetInputSoundProvider().SelectEnter();

		battleManager.InactiveUiStartCommand();
		battleManager.ActiveUiMonsterTradeSelectCommand();

		return battleManager.nowProcessState().NextProcess();
	}
}
