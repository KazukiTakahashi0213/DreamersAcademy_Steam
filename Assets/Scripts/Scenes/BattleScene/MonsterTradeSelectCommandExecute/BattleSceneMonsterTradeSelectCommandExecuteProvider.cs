using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleSceneMonsterTradeSelectCommandExecuteState {
	None
	, Yes
	, No
}

public class BattleSceneMonsterTradeSelectCommandExecuteProvider {
	public BattleSceneMonsterTradeSelectCommandExecuteProvider(BattleSceneMonsterTradeSelectCommandExecuteState setState) {
		states_.Add(new BattleSceneMonsterTradeSelectCommandExecuteNone());
		states_.Add(new BattleSceneMonsterTradeSelectCommandExecuteYes());
		states_.Add(new BattleSceneMonsterTradeSelectCommandExecuteNo());

		state_ = setState;
	}

	public BattleSceneMonsterTradeSelectCommandExecuteState state_ = BattleSceneMonsterTradeSelectCommandExecuteState.None;

	private List<BBattleSceneMonsterTradeSelectCommandExecute> states_ = new List<BBattleSceneMonsterTradeSelectCommandExecute>();

	public IProcessState Execute(BattleManager battleManager) { return states_[(int)state_].Execute(battleManager); }
}
