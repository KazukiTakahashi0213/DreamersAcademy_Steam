using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataSelectManager : MonoBehaviour, ISceneManager {
	//シーンのオブジェクト
	[SerializeField] Text _start_text = null;
	[SerializeField] Text _continue_text = null;
	[SerializeField] GameObject _move_cursor = null;

	//プロバイダー
	SaveDataSceneInputSoundProvider inputSoundProvider_ = new SaveDataSceneInputSoundProvider();

	enum SELECT_STATUS {START,CONTINUE, }

	SELECT_STATUS _select_num = SELECT_STATUS.START;

	public void SceneStart() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//依存性注入
		inputSoundProvider_.state_ = SaveDataSceneInputSoundState.Normal;

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

		if (sceneMgr.inputProvider_.UpSelect())
		{
			//SE
			inputSoundProvider_.UpSelect();

			_select_num = SELECT_STATUS.START;
			_move_cursor.transform.position = new Vector3(0.75f, 1.45f, -1);
		}

		if (sceneMgr.inputProvider_.DownSelect())
		{
			//SE
			inputSoundProvider_.DownSelect();

			_select_num = SELECT_STATUS.CONTINUE;
			_move_cursor.transform.position = new Vector3(0.75f, -0.55f, -1);
		}

		if (sceneMgr.inputProvider_.SelectEnter()) {
			//SE
			inputSoundProvider_.SelectEnter();

			//データの初期化
			PlayerTrainerData.ReleaseInstance();
			EnemyTrainerData.ReleaseInstance();

			if (_select_num == SELECT_STATUS.START)
			{
				Debug.Log("はじめから");

				sceneMgr.inputProvider_ = new InactiveInputProvider();

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
			if (_select_num == SELECT_STATUS.CONTINUE) {
				Debug.Log("つづきから");

				//データのロード
				if (SaveDataTrasfer.GetInstance().DataLoad()) {
					//操作の変更
					sceneMgr.inputProvider_ = new InactiveInputProvider();

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
