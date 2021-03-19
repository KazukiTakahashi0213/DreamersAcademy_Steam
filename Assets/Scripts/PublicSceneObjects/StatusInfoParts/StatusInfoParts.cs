using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfoParts : MonoBehaviour {
	//EntryPoint
	void Update() {
		//メイン処理
		processState_.state_ = processState_.Update(this);
	}

	private StatusInfoPartsProcessState processState_ = new StatusInfoPartsProcessState(StatusInfoPartsProcess.None);
	private IStatusInfoPartsProcessIdleState processIdleState_ = new StatusInfoPartsProcessIdleDown();

	private t13.TimeFluct[] timeFlucts_ = new t13.TimeFluct[7] {
		new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
	};
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	private float timeRegulation_ = 0;
	private Color32 endColor_ = new Color32();

	[SerializeField] FrameParts frameParts_ = null;
	[SerializeField] BaseParts baseParts_ = null;
	[SerializeField] AbnormalStateInfoParts firstAbnormalStateInfoParts_ = null;
	[SerializeField] AbnormalStateInfoParts secondAbnormalStateInfoParts_ = null;
	[SerializeField] ElementInfoParts firstElementInfoParts_ = null;
	[SerializeField] ElementInfoParts secondElementInfoParts_ = null;
	[SerializeField] UpdateGameObject eventGameObject_ = null;
	[SerializeField] float idleTimeRegulation_ = 0.5f;
	[SerializeField] private float entryPosY_ = 0;

	public void SetProcessIdleState(IStatusInfoPartsProcessIdleState state) { processIdleState_ = state; }
	public IStatusInfoPartsProcessIdleState GetProcessIdleState() { return processIdleState_; }

	public t13.TimeFluct GetTimeFlucts(int number) { return timeFlucts_[number]; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public float GetTimeRegulation() { return timeRegulation_; }
	public Color32 GetEndColor() { return endColor_; }

	public FrameParts GetFrameParts() { return frameParts_; }
	public BaseParts GetBaseParts() { return baseParts_; }
	public AbnormalStateInfoParts GetFirstAbnormalStateInfoParts() { return firstAbnormalStateInfoParts_; }
	public AbnormalStateInfoParts GetSecondAbnormalStateInfoParts() { return secondAbnormalStateInfoParts_; }
	public ElementInfoParts GetFirstElementInfoParts() { return firstElementInfoParts_; }
	public ElementInfoParts GetSecondElementInfoParts() { return secondElementInfoParts_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
	public float GetIdleTimeRegulation() { return idleTimeRegulation_; }

	public void ProcessStateColorUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Color32 endColor) {
		timeRegulation_ = timeRegulation;
		endColor_ = endColor;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = StatusInfoPartsProcess.ColorUpdate;
	}
	public void ProcessStateAllColorUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Color32 endColor) {
		timeRegulation_ = timeRegulation;
		endColor_ = endColor;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = StatusInfoPartsProcess.AllColorUpdate;
	}

	public void ProcessIdleStart() {
		processState_.state_ = StatusInfoPartsProcess.IdleMove;
	}
	public void ProcessIdleEnd() {
		t13.UnityUtil.ObjectPosMove(gameObject, new Vector3(transform.position.x, entryPosY_, transform.position.z));

		processIdleState_ = new StatusInfoPartsProcessIdleDown();

		timeCounter_.reset();

		processState_.state_ = StatusInfoPartsProcess.None;
	}

	public void MonsterStatusInfoSet(IMonsterData monsterData) {
		//名前をTextに反映
		string monsterViewName = t13.Utility.StringFullSpaceBackTamp(monsterData.uniqueName_, 6);
		baseParts_.GetInfoNameEventText().GetText().text = monsterViewName;

		//HPをTextに反映
		//HPゲージの調整
		float hpGaugeFillAmount = t13.Utility.ValueForPercentage(monsterData.RealHitPoint(), monsterData.nowHitPoint_, 1);
		frameParts_.GetHpGaugeParts().ProcessStateGaugeUpdateExecute(0, t13.TimeFluctProcess.Liner, monsterData, hpGaugeFillAmount);

		//状態異常の反映
		monsterData.battleData_.AbnormalSetStatusInfoParts(this);

		//タイプの反映
		firstElementInfoParts_.ElementReflect(monsterData.tribesData_.firstElement_);
		secondElementInfoParts_.ElementReflect(monsterData.tribesData_.secondElement_);
	}
	public void MonsterStatusInfoSetEventSet(IMonsterData monsterData) {
		//名前をTextに反映
		string monsterViewName = t13.Utility.StringFullSpaceBackTamp(monsterData.uniqueName_, 6);
		AllEventManager.GetInstance().EventTextSet(baseParts_.GetInfoNameEventText(), monsterViewName);
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//HPをTextに反映
		//HPゲージの調整
		float hpGaugeFillAmount = t13.Utility.ValueForPercentage(monsterData.RealHitPoint(), monsterData.nowHitPoint_, 1);
		AllEventManager.GetInstance().HpGaugePartsSet(frameParts_.GetHpGaugeParts(), hpGaugeFillAmount, monsterData);
		AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//状態異常の反映
		monsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(this);


		//タイプの反映
		firstElementInfoParts_.ElementReflectEventSet(monsterData.tribesData_.firstElement_);
		secondElementInfoParts_.ElementReflectEventSet(monsterData.tribesData_.secondElement_);
	}
}
