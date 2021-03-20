using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour, ISceneManager {
	//シーンのオブジェクト
	[SerializeField] private EventSpriteRenderer pressKeyLogoEventSprite_ = null;

	//プロバイダー
	private TitleSceneInputSoundProvider inputSoundProvider_ = new TitleSceneInputSoundProvider();

	public void SceneStart() {
		//依存性注入
		inputSoundProvider_.state_ = TitleSceneInputSoundState.Normal;

		pressKeyLogoEventSprite_.blinkTimeRegulation_ = 0.8f;
		pressKeyLogoEventSprite_.GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		pressKeyLogoEventSprite_.ProcessStateBlinkStartExecute();

		AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalTriggerInputProvider();

		AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite().ProcessStateChangeColorExecute(
			5.0f
			, t13.TimeFluctProcess.Liner
			, new Color(0, 0, 0, 0)
			);

		//BGMの再生
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().volume = 0.3f;
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Title());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();
	}

	public void SceneUpdate() {
		AllEventManager.GetInstance().EventUpdate();

		if (AllSceneManager.GetInstance().inputProvider_.AnyKeyTrigger()) {
			AllEventManager eventMgr = AllEventManager.GetInstance();
			AllSceneManager sceneMgr = AllSceneManager.GetInstance();

			//SE
			AllSceneManager.GetInstance().GetPublicAudioParts().GetSEAudioSource().volume = 1.0f;
			inputSoundProvider_.SelectEnter();

			pressKeyLogoEventSprite_.blinkTimeRegulation_ = 0.1f;

			//操作の変更
			sceneMgr.inputProvider_ = new InactiveInputProvider();

			//スクリーンの非表示
			AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite().ProcessReset();
			AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color = new Color(0, 0, 0, 0);

			//ウェイト
			eventMgr.EventWaitSet(1.0f);

			//フェードアウト
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
				) ;
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			//シーンの切り替え
			eventMgr.SceneChangeEventSet(SceneState.SaveDataSelect, SceneChangeMode.Change);
		}
	}

	public void SceneEnd() {
		pressKeyLogoEventSprite_.ProcessStateBlinkEndExecute();
		pressKeyLogoEventSprite_.gameObject.SetActive(true);
	}

	public GameObject GetGameObject() { return gameObject; }
}
