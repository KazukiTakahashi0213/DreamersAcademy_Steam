using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackCommandState {
	IAttackCommandState RightSelect(BattleManager mgr);
	IAttackCommandState LeftSelect(BattleManager mgr);
	IAttackCommandState UpSelect(BattleManager mgr);
	IAttackCommandState DownSelect(BattleManager mgr);

	IProcessState Execute(BattleManager mgr);
}
