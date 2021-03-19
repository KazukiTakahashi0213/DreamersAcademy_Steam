using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneMonsterTradeSelectCommandExecuteNo : BBattleSceneMonsterTradeSelectCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		//SE
		battleManager.GetInputSoundProvider().SelectEnter();

		battleManager.InactiveUiMonsterTradeSelectCommand();
		battleManager.ActiveUiCommand();

		return battleManager.nowProcessState().NextProcess();
	}
}
