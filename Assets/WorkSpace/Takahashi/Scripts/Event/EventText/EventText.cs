using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventText : MonoBehaviour {
	//EntryPoint
	void Update() {
		//メイン処理
		processState_.state_ = processState_.Update(this);
	}

	private EventTextProcessState processState_ = new EventTextProcessState(EventTextProcess.None);

	private t13.TimeFluct[] timeFlucts_ = new t13.TimeFluct[4]{
		new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
	};
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	private float timeRegulation_ = 0;

	private string updateContext_ = "";

	private Color32 changeEndColor_ = new Color32();

	[SerializeField] private Text text_ = null;

	public t13.TimeFluct GetTimeFlucts(int value) { return timeFlucts_[value]; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public float GetTimeRegulation() { return timeRegulation_; }

	public string GetUpdateContext() { return updateContext_; }

	public Color32 GetChangeEndColor() { return changeEndColor_; }

	public Text GetText() { return text_; }

	public void ProcessStateCharaUpdateExecute(float timeRegulation, string updateContext) {
		timeRegulation_ = timeRegulation;
		updateContext_ = updateContext;

		processState_.state_ = EventTextProcess.CharaUpdate;
	}
	public void ProcessStateChangeColorExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Color color) {
		timeRegulation_ = timeRegulation;
		changeEndColor_ = color;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = EventTextProcess.ChangeColor;
	}
}
