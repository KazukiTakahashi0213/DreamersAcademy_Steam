using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HpGaugePartsEventManagerExecute {
	None
	, GaugeUpdate
	, Max
}

public class HpGaugePartsEventManagerExecuteState {
	public HpGaugePartsEventManagerExecuteState(HpGaugePartsEventManagerExecute setState) {
		state_ = setState;
	}

	public HpGaugePartsEventManagerExecute state_;

	//None
	static private void NoneExecute(HpGaugePartsEventManagerExecuteState mine, HpGaugePartsEventManager hpGaugePartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {

	}

	//GaugeUpdate
	static private void GaugeUpdateExecute(HpGaugePartsEventManagerExecuteState mine, HpGaugePartsEventManager hpGaugePartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		for (int i = 0; i < hpGaugePartsEventManager.GetExecuteHpGaugeParts().Count; ++i) {
			hpGaugePartsEventManager.GetExecuteHpGaugeParts()[i].ProcessStateGaugeUpdateExecute(
				timeRegulation
				, timeFluctProcess
				, hpGaugePartsEventManager.GetExecuteReferMonsterDatas()[i]
				, hpGaugePartsEventManager.GetExecuteEndFillAmounts()[i]
				);
		}
	}

	private delegate void ExecuteFunc(HpGaugePartsEventManagerExecuteState mine, HpGaugePartsEventManager hpGaugePartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess);

	private ExecuteFunc[] executeFuncs_ = new ExecuteFunc[(int)HpGaugePartsEventManagerExecute.Max] {
		NoneExecute
		, GaugeUpdateExecute
	};
	public void Execute(HpGaugePartsEventManager hpGaugePartsEventManager, float timeRegulation, t13.TimeFluctProcess timeFluctProcess) { executeFuncs_[(int)state_](this, hpGaugePartsEventManager, timeRegulation, timeFluctProcess); }
}
