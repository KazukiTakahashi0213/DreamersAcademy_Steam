using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateImage : MonoBehaviour {
	//EntryPoint
	void Update() {
		//メイン処理
		processState_.state_ = processState_.Update(this);
	}

	private UpdateImageProcessState processState_ = new UpdateImageProcessState(UpdateImageProcess.None);

	private t13.TimeFluct[] timeFlucts_ = new t13.TimeFluct[4]{
		new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
	};
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	private float timeRegulation_ = 0;

	private Color32 changeEndColor_ = new Color32();

	private float endFillAmount_ = 0;

	[SerializeField] private Image image_ = null;

	public t13.TimeFluct GetTimeFlucts(int value) { return timeFlucts_[value]; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public float GetTimeRegulation() { return timeRegulation_; }

	public Color32 GetChangeEndColor() { return changeEndColor_; }

	public float GetEndFillAmount() { return endFillAmount_; }

	public Image GetImage() { return image_; }

	public void ProcessStateChangeColorExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Color color) {
		timeRegulation_ = timeRegulation;
		changeEndColor_ = color;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = UpdateImageProcess.ChangeColor;
	}
	public void ProcessStateFillAmountUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, float endFillAmount) {
		timeRegulation_ = timeRegulation;
		endFillAmount_ = endFillAmount;
		timeFlucts_[0].GetProcessState().state_ = timeFluctProcess;

		processState_.state_ = UpdateImageProcess.FillAmountUpdate;
	}
}
