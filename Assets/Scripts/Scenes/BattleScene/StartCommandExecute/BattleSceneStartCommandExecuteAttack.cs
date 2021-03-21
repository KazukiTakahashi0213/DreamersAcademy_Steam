using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneStartCommandExecuteAttack : BBattleSceneStartCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		//戦えるモンスターが１体だったら
		if (PlayerBattleData.GetInstance().GetBattleActiveMonsterSize() == 1) {
			//SE
			battleManager.GetInputSoundProvider().SelectEnter();

			battleManager.InactiveUiStartCommand();
			battleManager.ActiveUiCommand();

			return new CommandSelectProcess();
		}
		else {
			//SE
			battleManager.GetInputSoundProvider().SelectEnter();

			battleManager.InactiveUiStartCommand();
			battleManager.ActiveUiMonsterTradeSelectCommand();

			return battleManager.nowProcessState().NextProcess();
		}
	}
}
