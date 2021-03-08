using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBattleSceneCommandExecute {
	public virtual IProcessState Execute(BattleManager battleManager) { return battleManager.nowProcessState(); }
}
