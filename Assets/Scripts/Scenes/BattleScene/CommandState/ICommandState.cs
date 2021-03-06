using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandState {
	ICommandState RightSelect(BattleManager mgr);
	ICommandState LeftSelect(BattleManager mgr);
	ICommandState UpSelect(BattleManager mgr);
	ICommandState DownSelect(BattleManager mgr);

	IProcessState Execute(BattleManager mgr);
}
