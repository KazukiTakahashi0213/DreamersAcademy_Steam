using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneChangeMode {
	Change,
	Slide,
	Continue,
	Max
}

public class SceneChangeModeState {
	public SceneChangeModeState(SceneChangeMode setState) {
		state_ = setState;
	}

	public SceneChangeMode state_;

	//Change
	static private void ChangeChangeExecute(SceneChangeModeState mine, ISceneManager nowSceneManager, ISceneManager nextSceneManager) {
		//現在のシーンを非表示にする
		nowSceneManager.GetGameObject().SetActive(false);

		//現在のシーンの終了処理
		nowSceneManager.SceneEnd();

		//次のシーンを表示にする
		nextSceneManager.GetGameObject().SetActive(true);

		//次のシーンの開始処理
		nextSceneManager.SceneStart();
	}

	//Slide
	static private void SlideChangeExecute(SceneChangeModeState mine, ISceneManager nowSceneManager, ISceneManager nextSceneManager) {
		//現在のシーンを非表示にする
		nowSceneManager.GetGameObject().SetActive(false);

		//次のシーンを表示にする
		nextSceneManager.GetGameObject().SetActive(true);

		//次のシーンの開始処理
		nextSceneManager.SceneStart();
	}

	//Continue
	static private void ContinueChangeExecute(SceneChangeModeState mine, ISceneManager nowSceneManager, ISceneManager nextSceneManager) {
		//現在のシーンを非表示にする
		nowSceneManager.GetGameObject().SetActive(false);

		//現在のシーンの終了処理
		nowSceneManager.SceneEnd();

		//次のシーンを表示にする
		nextSceneManager.GetGameObject().SetActive(true);
	}

	private delegate void ChangeExecuteFunc(SceneChangeModeState mine, ISceneManager nowSceneManager, ISceneManager nextSceneManager);
	private ChangeExecuteFunc[] changeExecutes_ = new ChangeExecuteFunc[(int)SceneChangeMode.Max] {
		ChangeChangeExecute,
		SlideChangeExecute,
		ContinueChangeExecute
	};
	public void ChangeExecute(ISceneManager nowSceneManager, ISceneManager nextSceneManager) { changeExecutes_[(int)state_](this, nowSceneManager, nextSceneManager); }
}
