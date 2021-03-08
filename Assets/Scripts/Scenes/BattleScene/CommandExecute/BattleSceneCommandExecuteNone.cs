using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneCommandExecuteNone : BBattleSceneCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		return base.Execute(battleManager);
	}
}
