using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBattleSceneStartCommandExecute {
	public virtual IProcessState Execute(BattleManager battleManager) { return battleManager.nowProcessState(); }
}
