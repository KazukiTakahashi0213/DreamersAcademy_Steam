using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneState {
	Title,
	SaveDataSelect,
	Map,
	BugMenu,
	MonsterMenu,
	Battle,
	Ending,
	GameContinue,
	Max
}

[System.Serializable]
public class SerializeMonsterData {
	[SerializeField] public MonsterTribesDataNumber monsterTribesDataNumber_ = MonsterTribesDataNumber.None;
	[SerializeField] public List<SkillDataNumber> skillDataNumbers_ = new List<SkillDataNumber>();
}

public class AllSceneManager : MonoBehaviour {
	//EntryPoint
	public AllSceneManager() {
		instance_ = this;
	}

	//init
	void Start() {
		//プレイヤーのバトルの手持ちの反映
		for (int i = 0; i < startPlayerMonsterDatas_.Count; ++i) {
			//モンスターの生成
			MonsterData monsterData = new MonsterData(new MonsterTribesData(startPlayerMonsterDatas_[i].monsterTribesDataNumber_), 0, 50);

			//技の習得
			for(int j = 0;j < startPlayerMonsterDatas_[i].skillDataNumbers_.Count; ++j) {
				monsterData.SkillAdd(new SkillData(startPlayerMonsterDatas_[i].skillDataNumbers_[j]));
			}

			//モンスターの追加
			PlayerTrainerData.GetInstance().MonsterAdd(monsterData);
		}

		//エネミーのバトルの手持ちの反映
		for (int i = 0; i < startEnemyMonsterDatas_.Count; ++i) {
			//モンスターの生成
			MonsterData monsterData = new MonsterData(new MonsterTribesData(startEnemyMonsterDatas_[i].monsterTribesDataNumber_), 0, 50);

			//技の習得
			for (int j = 0; j < startEnemyMonsterDatas_[i].skillDataNumbers_.Count; ++j) {
				monsterData.SkillAdd(new SkillData(startEnemyMonsterDatas_[i].skillDataNumbers_[j]));
			}

			//モンスターの追加
			EnemyTrainerData.GetInstance().MonsterAdd(monsterData);
		}

		//各シーンを生成し、非表示にする
		for (int i = 0; i < (int)SceneState.Max; ++i) {
			GameObject load = Resources.Load("Prefabs/Scenes/" + sceneStateString[i]) as GameObject;
			load = Instantiate(load, new Vector3(0, 0, 0), Quaternion.identity);

			sceneState[i] = load.GetComponent<ISceneManager>();
			load.SetActive(false);
		}

		//現在のシーンを表示にし、ISceneManagerを取得する
		sceneState[(int)nowSceneState_].GetGameObject().SetActive(true);

		//現在のシーンの開始処理
		sceneState[(int)nowSceneState_].SceneStart();
	}
	//MainLoop
	void Update() {
		//Escapeキーの振る舞い
		if (Input.GetKeyDown(KeyCode.Escape)) t13.UnityUtil.GameQuit();

		//現在のシーンの実装処理
		sceneState[(int)nowSceneState_].SceneUpdate();

		//SceneChangeが呼ばれていたら
		if(SceneActive_ == false) {
			//シーンの切り替え処理
			sceneChangeModeState.ChangeExecute(sceneState[(int)nowSceneState_], sceneState[(int)nextSceneState_]);

			//シーンのステートの変更
			nowSceneState_ = nextSceneState_;

			SceneActive_ = true;
		}
	}

	[SerializeField] private AudioParts publicAudioParts_ = null;
	[SerializeField] private ScreenParts publicFrontScreen_ = null;
	public AudioParts GetPublicAudioParts() { return publicAudioParts_; }
	public ScreenParts GetPublicFrontScreen() { return publicFrontScreen_; }

	[SerializeField] private SceneState nowSceneState_ = SceneState.Battle;

	[SerializeField, Header("バトルのシーンの演出の速度(０～１)")] private float battleEffectSpeed_ = 1.0f;
	public float GetBattleEffectSpeed() { return battleEffectSpeed_; }

	[SerializeField, Header("バトルのプレイヤーの手持ちモンスター")] private List<SerializeMonsterData> startPlayerMonsterDatas_ = new List<SerializeMonsterData>();

	[SerializeField, Header("バトルのエネミーの手持ちモンスター")] private List<SerializeMonsterData> startEnemyMonsterDatas_ = new List<SerializeMonsterData>();

	private string[] sceneStateString = new string[(int)SceneState.Max] {
		"TitleScene",
		"SaveDataSelectScene",
		"MapScene",
		"BugMenuScene",
		"MonsterMenuScene",
		"BattleScene",
		"EndingScene",
		"GameContinueScene"
	};

	private ISceneManager[] sceneState = new ISceneManager[(int)SceneState.Max];

	private SceneChangeModeState sceneChangeModeState = new SceneChangeModeState(SceneChangeMode.Change);
	private bool SceneActive_ = true;
	private SceneState nextSceneState_;

	private System.Random random = new System.Random();
	public System.Random GetRandom() { return random; }

	public IInputProvider inputProvider_ = new InactiveInputProvider();

	public float GetEventContextUpdateTime() { return 0.4f; }
	public float GetEventWaitTime() { return 0.8f; }

	public void SceneChange(SceneState nextScene, SceneChangeMode sceneChangeMode) {
		SceneActive_ = false;

		nextSceneState_ = nextScene;

		sceneChangeModeState.state_ = sceneChangeMode;
	}

	static private AllSceneManager instance_;
	static public AllSceneManager GetInstance() { return instance_; }
}
