using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleSceneStartCommandExecuteState {
	None
	, Attack
	, Monster
}

public class BattleSceneStartCommandExecuteProvider {
	public BattleSceneStartCommandExecuteProvider(BattleSceneStartCommandExecuteState setState) {
		states_.Add(new BattleSceneStartCommandExecuteNone());
		states_.Add(new BattleSceneStartCommandExecuteAttack());
		states_.Add(new BattleSceneStartCommandExecuteMonster());

		state_ = setState;
	}

	public BattleSceneStartCommandExecuteState state_ = BattleSceneStartCommandExecuteState.None;

	private List<BBattleSceneStartCommandExecute> states_ = new List<BBattleSceneStartCommandExecute>();

	public IProcessState Execute(BattleManager battleManager) { return states_[(int)state_].Execute(battleManager); }
}
