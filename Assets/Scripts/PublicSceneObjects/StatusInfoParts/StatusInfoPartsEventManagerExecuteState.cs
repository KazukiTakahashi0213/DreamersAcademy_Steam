using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusInfoPartsEventManagerExecute {
	None
	, ColorUpdate
	, AllColorUpdate
	, IdleMoveStart
	, IdleMoveEnd
	, Max
}

public class StatusInfoPartsEventManagerExecuteState {
	public StatusInfoPartsEventManagerExecuteState(StatusInfoPartsEventManagerExecute setState) {
		state_ = setState;
	}

	public StatusInfoPartsEventManagerExecute state_;

	//None
	static private void NoneExecute(StatusInfoPartsEventManagerExecuteState mine, StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {

	}

	//ColorUpdate
	static private void ColorUpdateExecute(StatusInfoPartsEventManagerExecuteState mine, StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < statusInfoPartsEventManager.GetexecuteEventStatusInfosParts().Count; ++i) {
			statusInfoPartsEventManager.GetexecuteEventStatusInfosParts()[i].ProcessStateColorUpdateExecute(
				timeRegulation
				, timeFluctProcess
				, statusInfoPartsEventManager.GetExecuteEndColors()[i]
				);
		}
	}

	//AllColorUpdate
	static private void AllColorUpdateExecute(StatusInfoPartsEventManagerExecuteState mine, StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < statusInfoPartsEventManager.GetexecuteEventStatusInfosParts().Count; ++i) {
			statusInfoPartsEventManager.GetexecuteEventStatusInfosParts()[i].ProcessStateAllColorUpdateExecute(
				timeRegulation
				, timeFluctProcess
				, statusInfoPartsEventManager.GetExecuteEndColors()[i]
				);
		}
	}

	//IdleMoveStart
	static private void IdleMoveStartExecute(StatusInfoPartsEventManagerExecuteState mine, StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < statusInfoPartsEventManager.GetexecuteEventStatusInfosParts().Count; ++i) {
			statusInfoPartsEventManager.GetexecuteEventStatusInfosParts()[i].ProcessIdleStart();
		}
	}

	//IdleMoveEnd
	static private void IdleMoveEndExecute(StatusInfoPartsEventManagerExecuteState mine, StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < statusInfoPartsEventManager.GetexecuteEventStatusInfosParts().Count; ++i) {
			statusInfoPartsEventManager.GetexecuteEventStatusInfosParts()[i].ProcessIdleEnd();
		}
	}

	private delegate void ExecuteFunc(StatusInfoPartsEventManagerExecuteState mine, StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess);

	private ExecuteFunc[] executeFuncs_ = new ExecuteFunc[(int)StatusInfoPartsEventManagerExecute.Max] {
		NoneExecute
		, ColorUpdateExecute
		, AllColorUpdateExecute
		, IdleMoveStartExecute
		, IdleMoveEndExecute
	};
	public void Execute(StatusInfoPartsEventManager statusInfoPartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) { executeFuncs_[(int)state_](this, statusInfoPartsEventManager, timeRegulation, timeFluctProcess); }
}
