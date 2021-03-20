using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContinueManager : MonoBehaviour, ISceneManager {
	[SerializeField] private NovelWindowParts novelWindowParts_ = null;

	public void SceneStart() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		PlayerTrainerData.GetInstance().prepareContinue_ = true;

		//フェードイン
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute();

		//ウェイト
		eventMgr.EventWaitSet(2.0f);

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("GameContinue/GameContinue1");
			EventMoveMap.NovelEvent(novelWindowParts_, context);
		}

		//ウェイト
		eventMgr.EventWaitSet(sceneMgr.GetEventWaitTime());

		//フェードアウト
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(0, 0, 0, 1)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//イベントの最後
		//シーンの切り替え
		MonsterMenuManager.SetProcessStateProvider(new MonsterMenuSceneNormalProcessStateProvider());
		eventMgr.SceneChangeEventSet(SceneState.MonsterMenu, SceneChangeMode.Slide);
	}

	public void SceneUpdate() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		if (!PlayerTrainerData.GetInstance().prepareContinue_) {
			novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
			novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

			//データのセーブ
			SaveDataTrasfer.GetInstance().ContinueDataSave();

			//プレイヤーデータの初期化
			PlayerTrainerData.ReleaseInstance();
			PlayerTrainerData.GetInstance().prepareContinue_ = true;

			//操作の変更
			sceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();

			//フェードイン
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
				);
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			//ノベル処理
			{
				string context = ResourcesTextsLoader.GetInstance().GetTexts("GameContinue/GameContinue2");
				EventMoveMap.NovelEvent(novelWindowParts_, context);
			}

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
			eventMgr.SceneChangeEventSet(SceneState.Title, SceneChangeMode.Change);
		}

		eventMgr.EventUpdate();

		if (sceneMgr.inputProvider_.UpSelect()) {
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| sceneMgr.inputProvider_.SelectMouseLeftTrigger()) {
			eventMgr.EventTriggerNext();
		}
		else if (sceneMgr.inputProvider_.SelectBack()) {
		}
	}

	public void SceneEnd() {
		novelWindowParts_.GetNovelWindowText().text = "";

		PlayerTrainerData.GetInstance().prepareContinue_ = false;
	}

	public GameObject GetGameObject() { return gameObject; }
}
