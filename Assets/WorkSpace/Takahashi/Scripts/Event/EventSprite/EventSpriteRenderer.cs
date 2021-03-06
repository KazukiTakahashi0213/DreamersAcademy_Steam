using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpriteRenderer : MonoBehaviour {
	//EntryPoint
	void Update() {
		//メイン処理
		processState_.state_ = processState_.Update(this);
	}

	private EventSpriteRendererProcessState processState_ = new EventSpriteRendererProcessState(EventSpriteRendererProcess.None);
	private UpdateSpriteRendererProcessBlinkState blinkState_ = new UpdateSpriteRendererProcessBlinkState(UpdateSpriteRendererProcessBlink.Out);

	private t13.TimeFluct[] timeFlucts_ = new t13.TimeFluct[4]{
		new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
		, new t13.TimeFluct()
	};
	private t13.TimeCounter timeCounter_ = new t13.TimeCounter();

	private float timeRegulation_ = 0;
	private List<Sprite> animeSprites_ = null;
	private int nowAnimeSpriteNumber_ = 0;
	private Color32 changeEndColor_ = new Color32();

	public float blinkTimeRegulation_ = 0.06f;
	private float blinkAlphaValueArchive_ = 0;

	[SerializeField] private SpriteRenderer spriteRenderer_ = null;

	public UpdateSpriteRendererProcessBlinkState GetBlinkState() { return blinkState_; }

	public t13.TimeFluct GetTimeFlucts(int value) { return timeFlucts_[value]; }
	public t13.TimeCounter GetTimeCounter() { return timeCounter_; }

	public float GetTimeRegulation() { return timeRegulation_; }
	public List<Sprite> GetAnimeSprites() { return animeSprites_; }
	public void SetNowAnimeSpriteNumber(int number) { nowAnimeSpriteNumber_ = number; }
	public int GetNowAnimeSpriteNumber() { return nowAnimeSpriteNumber_; }
	public Color32 GetChangeEndColor() { return changeEndColor_; }

	public SpriteRenderer GetSpriteRenderer() { return spriteRenderer_; }

	public void ProcessStateAnimeExecute(float timeRegulation, List<Sprite> sprites) {
		timeRegulation_ = timeRegulation;
		animeSprites_ = sprites;

		spriteRenderer_.sprite = animeSprites_[nowAnimeSpriteNumber_];

		processState_.state_ = EventSpriteRendererProcess.Anime;
	}
	public void ProcessStateChangeColorExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess, Color color) {
		timeRegulation_ = timeRegulation;
		changeEndColor_ = color;
		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].GetProcessState().state_ = timeFluctProcess;
		}

		processState_.state_ = EventSpriteRendererProcess.ChangeColor;
	}
	public void ProcessStateBlinkStartExecute() {
		blinkAlphaValueArchive_ = spriteRenderer_.color.a;

		processState_.state_ = EventSpriteRendererProcess.Blink;
	}
	public void ProcessStateBlinkEndExecute() {
		spriteRenderer_.color = new Color(spriteRenderer_.color.r, spriteRenderer_.color.g, spriteRenderer_.color.b, blinkAlphaValueArchive_);

		timeCounter_.reset();

		processState_.state_ = EventSpriteRendererProcess.None;
	}
	public void ProcessReset() {
		timeCounter_.reset();

		for (int i = 0; i < timeFlucts_.Length; ++i) {
			timeFlucts_[i].Reset();
		}

		processState_.state_ = EventSpriteRendererProcess.None;
	}

	public void SpriteSet(List<Sprite> sprites) {
		spriteRenderer_.sprite = sprites[nowAnimeSpriteNumber_];
	}
}
