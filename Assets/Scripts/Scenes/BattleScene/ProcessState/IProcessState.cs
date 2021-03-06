using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProcessState {
	IProcessState NextProcess();
	IProcessState BackProcess();

	IProcessState Update(BattleManager mgr);
}
