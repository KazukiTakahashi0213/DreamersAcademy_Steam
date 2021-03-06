using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateGameObjectEventManagerExecute {
	None
	, PosMove
	, RotMove
	, Max
}

public class UpdateGameObjectEventManagerExecuteState {
	public UpdateGameObjectEventManagerExecuteState(UpdateGameObjectEventManagerExecute setState) {
		state_ = setState;
	}

	public UpdateGameObjectEventManagerExecute state_;

	//None
	static private void NoneExecute(UpdateGameObjectEventManagerExecuteState mine, UpdateGameObjectEventManager updateGameObjectEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {

	}

	//PosMove
	static private void PosMoveExecute(UpdateGameObjectEventManagerExecuteState mine, UpdateGameObjectEventManager updateGameObjectEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < updateGameObjectEventManager.GetExecuteUpdateGameObjectsCount(); ++i) {
			updateGameObjectEventManager.GetExecuteUpdateGameObjects(i).ProcessStatePosMoveExecute(
				timeRegulation,
				timeFluctProcess,
				updateGameObjectEventManager.GetExecuteEndVec3s(i)
				);
		}
	}

	//RotMove
	static private void RotMoveExecute(UpdateGameObjectEventManagerExecuteState mine, UpdateGameObjectEventManager updateGameObjectEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < updateGameObjectEventManager.GetExecuteUpdateGameObjectsCount(); ++i) {
			updateGameObjectEventManager.GetExecuteUpdateGameObjects(i).ProcessStateRotMoveExecute(
				timeRegulation,
				timeFluctProcess,
				updateGameObjectEventManager.GetExecuteEndVec3s(i)
				);
		}
	}

	private delegate void ExecuteFunc(UpdateGameObjectEventManagerExecuteState mine, UpdateGameObjectEventManager updateGameObjectEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess);

	private ExecuteFunc[] executeFuncs_ = new ExecuteFunc[(int)UpdateGameObjectEventManagerExecute.Max] {
		NoneExecute
		, PosMoveExecute
		, RotMoveExecute
	};
	public void Execute(UpdateGameObjectEventManager updateGameObjectEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) { executeFuncs_[(int)state_](this, updateGameObjectEventManager, timeRegulation, timeFluctProcess); }
}
