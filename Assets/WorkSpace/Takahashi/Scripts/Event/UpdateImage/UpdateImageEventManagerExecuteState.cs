using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateImageEventManagerExecute {
	None
	, ChangeColor
	, FillAmountUpdate
	, Max
}

public class UpdateImageEventManagerExecuteState {
	public UpdateImageEventManagerExecuteState(UpdateImageEventManagerExecute setState) {
		state_ = setState;
	}

	public UpdateImageEventManagerExecute state_;

	//None
	static private void NoneExecute(UpdateImageEventManagerExecuteState mine, UpdateImageEventManager updateImageEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {

	}

	//ChangeColor
	static private void ChangeColorExecute(UpdateImageEventManagerExecuteState mine, UpdateImageEventManager updateImageEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < updateImageEventManager.GetExecuteUpdateImages().Count; ++i) {
			updateImageEventManager.GetExecuteUpdateImages()[i].ProcessStateChangeColorExecute(
				timeRegulation
				, timeFluctProcess
				, updateImageEventManager.GetExecuteChangeColorEnds()[i]
				);
		}
	}

	//FillAmountUpdate
	static private void FillAmountUpdateExecute(UpdateImageEventManagerExecuteState mine, UpdateImageEventManager updateImageEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < updateImageEventManager.GetExecuteUpdateImages().Count; ++i) {
			updateImageEventManager.GetExecuteUpdateImages()[i].ProcessStateFillAmountUpdateExecute(
				timeRegulation
				, timeFluctProcess
				, updateImageEventManager.GetExecuteEndFillAmounts()[i]
				);
		}
	}

	private delegate void ExecuteFunc(UpdateImageEventManagerExecuteState mine, UpdateImageEventManager updateImageEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess);

	private ExecuteFunc[] executeFuncs_ = new ExecuteFunc[(int)UpdateImageEventManagerExecute.Max] {
		NoneExecute
		, ChangeColorExecute
		, FillAmountUpdateExecute
	};
	public void Execute(UpdateImageEventManager updateImageEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) { executeFuncs_[(int)state_](this, updateImageEventManager, timeRegulation, timeFluctProcess); }
}
