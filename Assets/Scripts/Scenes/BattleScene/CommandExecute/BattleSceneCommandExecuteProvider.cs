using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleSceneCommandExecuteState {
    None
    , Attack
    , MonsterTrade
    , Dream
    , Escape
    , Max
}

public class BattleSceneCommandExecuteProvider {
	public BattleSceneCommandExecuteProvider(BattleSceneCommandExecuteState setState) {
		states_.Add(new BattleSceneCommandExecuteNone());
		states_.Add(new BattleSceneCommandExecuteAttack());
		states_.Add(new BattleSceneCommandExecuteMonsterTrade());
		states_.Add(new BattleSceneCommandExecuteDream());
		states_.Add(new BattleSceneCommandExecuteEscape());

		state_ = setState;
	}

	public BattleSceneCommandExecuteState state_ = BattleSceneCommandExecuteState.None;

	private List<BBattleSceneCommandExecute> states_ = new List<BBattleSceneCommandExecute>();

	public IProcessState Execute(BattleManager battleManager) { return states_[(int)state_].Execute(battleManager); }
}
