using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndingManager : MonoBehaviour, ISceneManager {
	[SerializeField] private VideoPlayer videoPlayer_ = null;
	[SerializeField] private NovelWindowParts novelWindowParts_ = null;
	[SerializeField] private EventSpriteRenderer movieScreenEventSprite_ = null;

	public void SceneStart() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//BGMの停止
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().volume = 0;

		VideoClip loadData = Resources.Load("Video/" + "Dreamers_End") as VideoClip;
		videoPlayer_.clip = loadData;
		videoPlayer_.Play();

		//フェードイン
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(sceneMgr.GetEventWaitTime() * 4);

		//ウェイト
		eventMgr.EventWaitSet((float)loadData.length - 9.0f);

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			movieScreenEventSprite_
			, null
			, new Color(movieScreenEventSprite_.GetSpriteRenderer().color.r, movieScreenEventSprite_.GetSpriteRenderer().color.g, movieScreenEventSprite_.GetSpriteRenderer().color.b, 255)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(sceneMgr.GetEventWaitTime() * 3);

		//ウェイト
		eventMgr.EventWaitSet(1.0f);

		{
			//文字列の設定
			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetEventText(), "Thank You For Playing!");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(4.0f);
		}

		//Blinkの開始
		AllEventManager.GetInstance().EventSpriteRendererSet(novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//Enterの押下待ち
		AllEventManager.GetInstance().EventTriggerSet();

		//Blinkの終了
		AllEventManager.GetInstance().EventSpriteRendererSet(novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(sceneMgr.GetEventWaitTime() * 3);

		//イベントの最後
		//シーンの切り替え
		if (playerData.battleResult_) {
			eventMgr.SceneChangeEventSet(SceneState.GameContinue, SceneChangeMode.Change);
		}
		else {
			//プレイヤーデータの初期化
			PlayerTrainerData.ReleaseInstance();

			eventMgr.SceneChangeEventSet(SceneState.Title, SceneChangeMode.Change);
		}
	}

	public void SceneUpdate() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		eventMgr.EventUpdate();

		if (sceneMgr.inputProvider_.UpSelect()) {
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()) {
			eventMgr.EventTriggerNext();
		}
		else if (sceneMgr.inputProvider_.SelectBack()) {
		}
	}

	public void SceneEnd() {
		novelWindowParts_.GetNovelWindowText().text = "";

		movieScreenEventSprite_.GetSpriteRenderer().color = new Color(0, 0, 0, 0);
	}

	public GameObject GetGameObject() { return gameObject; }
}
