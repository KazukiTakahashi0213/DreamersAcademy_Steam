using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataSelectManager : MonoBehaviour, ISceneManager {
	//シーンのオブジェクト
	[SerializeField] private CommandParts commandParts_ = null;
	[SerializeField] private SpriteRenderer dataSelectInfoSprite_ = null;

	//プロバイダー
	SaveDataSceneInputSoundProvider inputSoundProvider_ = new SaveDataSceneInputSoundProvider();

	public void SceneStart() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//依存性注入
		inputSoundProvider_.state_ = SaveDataSceneInputSoundState.Normal;

		//初期化
		dataSelectInfoSprite_.sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics(GraphicsPathSupervisor.GetInstance().GetPathGameStartInfo());

		commandParts_.SelectReset(new Vector3(3.32f, 0.81f, -4));

		//フェードイン
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//イベントの最後
		eventMgr.EventFinishSet();
	}

	public void SceneUpdate() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		if (AllEventManager.GetInstance().EventUpdate()) {
			sceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();
		}

		//カーソルが動いていたら
		int commandSelectNumber = commandParts_.CommandSelectForNumber(new Vector3(), new Vector3(0, 1.9f, 0));
		if(commandSelectNumber > -1) {
			//説明の画像
			if (commandSelectNumber == 0) dataSelectInfoSprite_.sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics(GraphicsPathSupervisor.GetInstance().GetPathGameStartInfo());
			if (commandSelectNumber == 1) dataSelectInfoSprite_.sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics(GraphicsPathSupervisor.GetInstance().GetPathGameContinueInfo());

			//SE
			inputSoundProvider_.UpSelect();

		}
		else if (sceneMgr.inputProvider_.UpSelect()) {
			//カーソルが動かせたら
			if(commandParts_.CommandSelectUp(new Vector3(0, 1.9f, 0))) {
				//説明の画像
				dataSelectInfoSprite_.sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics(GraphicsPathSupervisor.GetInstance().GetPathGameStartInfo());

				//SE
				inputSoundProvider_.UpSelect();
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			//カーソルが動かせたら
			if (commandParts_.CommandSelectDown(new Vector3(0, -1.9f, 0))) {
				//説明の画像
				dataSelectInfoSprite_.sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics(GraphicsPathSupervisor.GetInstance().GetPathGameContinueInfo());

				//SE
				inputSoundProvider_.DownSelect();
			}
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| commandParts_.MouseLeftButtonTriggerActive()) {
			//データの初期化
			PlayerTrainerData.ReleaseInstance();
			EnemyTrainerData.ReleaseInstance();

			if (commandParts_.SelectNumber() == 0) {
				Debug.Log("はじめから");

				//操作の変更
				sceneMgr.inputProvider_ = new InactiveInputProvider();

				//SE
				inputSoundProvider_.SelectEnter();

				//ウェイト
				eventMgr.EventWaitSet(sceneMgr.GetEventWaitTime());

				//フェードアウト
				eventMgr.EventSpriteRendererSet(
					sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
					, null
					, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
					);
				eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				eventMgr.AllUpdateEventExecute(0.4f);

				//シーンの切り替え
				eventMgr.SceneChangeEventSet(SceneState.Map, SceneChangeMode.Change);
			}
			else if (commandParts_.SelectNumber() == 1) {
				Debug.Log("つづきから");

				//データのロード
				if (SaveDataTrasfer.GetInstance().DataLoad()) {
					//操作の変更
					sceneMgr.inputProvider_ = new InactiveInputProvider();

					//SE
					inputSoundProvider_.SelectEnter();

					//ウェイト
					eventMgr.EventWaitSet(sceneMgr.GetEventWaitTime());

					//フェードアウト
					eventMgr.EventSpriteRendererSet(
						sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
						, null
						, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
						);
					eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
					eventMgr.AllUpdateEventExecute(0.4f);

					//シーンの切り替え
					eventMgr.SceneChangeEventSet(SceneState.Map, SceneChangeMode.Change);
				}
			}
		}
	}

	public void SceneEnd() {

	}

	public GameObject GetGameObject() { return gameObject; }
}
