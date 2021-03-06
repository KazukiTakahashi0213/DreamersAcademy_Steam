using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGaugeParts : MonoBehaviour {
	//EntryPoint
	void Update() {
		//メイン処理
		processState_.state_ = processState_.Update(this);
	}

	private HpGaugePartsProcessState processState_ = new HpGaugePartsProcessState(HpGaugePartsProcess.None);

	private t13.TimeFluct timeFluct_ = new t13.TimeFluct();
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	private float timeRegulation_ = 0;
	private IMonsterData referMonsterData_ = null;
	private float endFillAmount_ = 0;

	[SerializeField] private Image gauge_ = null;
	[SerializeField] private Text infoText_ = null;

	public t13.TimeFluct GetTimeFluct() { return timeFluct_; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public float GetTimeRegulation() { return timeRegulation_; }
	public IMonsterData GetReferMonsterData() { return referMonsterData_; }
	public float GetEndFillAmount() { return endFillAmount_; }

	public Image GetGauge() { return gauge_; }
	public Text GetInfoText() { return infoText_; }

	public void ProcessStateGaugeUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, IMonsterData referMonsterData, float endFillAmount) {
		timeRegulation_ = timeRegulation;
		referMonsterData_ = referMonsterData;
		endFillAmount_ = endFillAmount;
		timeFluct_.GetProcessState().state_ = timeFluctProcess;

		processState_.state_ = HpGaugePartsProcess.GaugeUpdate;
	}
}
