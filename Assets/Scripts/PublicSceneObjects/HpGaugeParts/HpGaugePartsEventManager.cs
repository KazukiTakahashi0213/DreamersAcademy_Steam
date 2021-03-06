using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGaugePartsEventManager {
	private HpGaugePartsEventManagerExecuteState executeState_ = new HpGaugePartsEventManagerExecuteState(HpGaugePartsEventManagerExecute.None);

	private int hpGaugesPartsExecuteCounter_ = 0;
	private List<HpGaugeParts> hpGaugesParts_ = new List<HpGaugeParts>();
	private List<IMonsterData> referMonsterDatas_ = new List<IMonsterData>();
	private List<float> endFillAmounts_ = new List<float>();
	private List<List<HpGaugeParts>> executeHpGaugesParts_ = new List<List<HpGaugeParts>>();
	private List<List<IMonsterData>> executeReferMonsterDatas_ = new List<List<IMonsterData>>();
	private List<List<float>> executeEndFillAmounts_ = new List<List<float>>();
	private List<HpGaugePartsEventManagerExecute> hpGaugePartsEventManagerExecutes_ = new List<HpGaugePartsEventManagerExecute>();

	public HpGaugePartsEventManagerExecuteState GetExecuteState() { return executeState_; }

	public List<HpGaugeParts> GetExecuteHpGaugeParts() { return executeHpGaugesParts_[hpGaugesPartsExecuteCounter_]; }
	public List<IMonsterData> GetExecuteReferMonsterDatas() { return executeReferMonsterDatas_[hpGaugesPartsExecuteCounter_]; }
	public List<float> GetExecuteEndFillAmounts() { return executeEndFillAmounts_[hpGaugesPartsExecuteCounter_]; }

	public void HpGaugePartsSet(HpGaugeParts hpGaugeParts, IMonsterData monsterData, float endFillAmount) {
		hpGaugesParts_.Add(hpGaugeParts);
		referMonsterDatas_.Add(monsterData);
		endFillAmounts_.Add(endFillAmount);
	}
	public void HpGaugesPartsExecuteSet(HpGaugePartsEventManagerExecute setExecute = HpGaugePartsEventManagerExecute.None) {
		List<HpGaugeParts> addHpGaugesParts = new List<HpGaugeParts>();
		List<IMonsterData> addReferMonsterDatas = new List<IMonsterData>();
		List<float> addEndFillAmounts = new List<float>();

		for (int i = 0; i < hpGaugesParts_.Count; ++i) {
			addHpGaugesParts.Add(hpGaugesParts_[i]);
			addReferMonsterDatas.Add(referMonsterDatas_[i]);
			addEndFillAmounts.Add(endFillAmounts_[i]);
		}

		executeHpGaugesParts_.Add(addHpGaugesParts);
		executeReferMonsterDatas_.Add(addReferMonsterDatas);
		executeEndFillAmounts_.Add(addEndFillAmounts);
		hpGaugePartsEventManagerExecutes_.Add(setExecute);

		hpGaugesParts_.Clear();
		referMonsterDatas_.Clear();
		endFillAmounts_.Clear();
	}

	public void HpGaugesPartsUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		executeState_.state_ = hpGaugePartsEventManagerExecutes_[hpGaugesPartsExecuteCounter_];

		executeState_.Execute(this, timeRegulation, timeFluctProcess);

		hpGaugesPartsExecuteCounter_ += 1;
	}

	public void HpGaugesPartsClear() {
		hpGaugesParts_.Clear();
		referMonsterDatas_.Clear();
		endFillAmounts_.Clear();
		executeHpGaugesParts_.Clear();
		executeReferMonsterDatas_.Clear();
		executeEndFillAmounts_.Clear();
		hpGaugePartsEventManagerExecutes_.Clear();

		hpGaugesPartsExecuteCounter_ = 0;
	}
}
