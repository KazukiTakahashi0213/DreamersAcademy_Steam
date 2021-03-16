using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuManager : MonoBehaviour, ISceneManager {
	public void SceneStart() {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		//依存性注入
		nowProcessState_ = startProcessStateProvider_;
		nowProcessState_.state_ = MonsterMenuSceneProcess.MonsterSelect;
		inputSoundProvider_.state_ = MonsterMenuSceneInputSoundState.MonsterSelect;

		selectMonsterNumber_ = 0;

		//モンスターの行動選択肢の初期化
		monsterActionCommandParts_.gameObject.SetActive(false);

		//技の選択肢の初期化

		//BulletPartsの初期化
		t13.UnityUtil.ObjectPosMove(bulletParts_.GetEventStatusInfosParts(0).gameObject, new Vector3(bulletParts_.GetEventStatusInfosParts(0).gameObject.transform.localPosition.x, 3.5f, 5));
		t13.UnityUtil.ObjectPosMove(bulletParts_.GetEventStatusInfosParts(1).gameObject, new Vector3(bulletParts_.GetEventStatusInfosParts(1).gameObject.transform.localPosition.x, 3.5f, 5));
		t13.UnityUtil.ObjectPosMove(bulletParts_.GetEventStatusInfosParts(2).gameObject, new Vector3(bulletParts_.GetEventStatusInfosParts(2).gameObject.transform.localPosition.x, 2.0f, 5));
		t13.UnityUtil.ObjectPosMove(bulletParts_.GetEventStatusInfosParts(3).gameObject, new Vector3(bulletParts_.GetEventStatusInfosParts(3).gameObject.transform.localPosition.x, 0.5f, 5));
		t13.UnityUtil.ObjectPosMove(bulletParts_.GetEventStatusInfosParts(4).gameObject, new Vector3(bulletParts_.GetEventStatusInfosParts(4).gameObject.transform.localPosition.x, 0.5f, 5));

		//MagazinePartsの初期化
		magazineParts_.Initialize();

		//StatusInfosPartsの色の変更
		for (int i = 0;i < (bulletParts_.GetEventStatusInfosPartsSize() / 2)+1; ++i) {
			if (i == 0) {
				bulletParts_.GetEventStatusInfosParts(i).ProcessStateAllColorUpdateExecute(0, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, 0));
				bulletParts_.GetEventStatusInfosParts(bulletParts_.GetEventStatusInfosPartsSize() - 1 - i).ProcessStateAllColorUpdateExecute(0, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, 0));
			}
			else {
				bulletParts_.GetEventStatusInfosParts(i).ProcessStateColorUpdateExecute(0, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, (byte)(255 / ((i % 2) + 1))));
				bulletParts_.GetEventStatusInfosParts(bulletParts_.GetEventStatusInfosPartsSize() - 1 - i).ProcessStateColorUpdateExecute(0, t13.TimeFluctProcess.Liner, new Color32(0, 0, 0, (byte)(255 / ((i % 2) + 1))));
			}
		}

		//状態異常の非表示
		bulletParts_.GetEventStatusInfosParts(0).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
		bulletParts_.GetEventStatusInfosParts(0).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);
		bulletParts_.GetEventStatusInfosParts(bulletParts_.GetEventStatusInfosPartsSize()-1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
		bulletParts_.GetEventStatusInfosParts(bulletParts_.GetEventStatusInfosPartsSize()-1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

		//タイプの非表示
		bulletParts_.GetEventStatusInfosParts(0).GetFirstElementInfoParts().gameObject.SetActive(false);
		bulletParts_.GetEventStatusInfosParts(0).GetSecondElementInfoParts().gameObject.SetActive(false);
		bulletParts_.GetEventStatusInfosParts(bulletParts_.GetEventStatusInfosPartsSize() - 1).GetFirstElementInfoParts().gameObject.SetActive(false);
		bulletParts_.GetEventStatusInfosParts(bulletParts_.GetEventStatusInfosPartsSize() - 1).GetSecondElementInfoParts().gameObject.SetActive(false);

		//初期化
		nowProcessState_.init(this);

		//フェードイン
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//操作の変更
		eventMgr.InputProviderChangeEventSet(new KeyBoardNormalInputProvider());
	}

	public void SceneUpdate() {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		nowProcessState_.state_ = nowProcessState_.Update(this);
	}

	public void SceneEnd() {

	}

	public GameObject GetGameObject() { return gameObject; }

	[SerializeField] private MagazineParts magazineParts_ = null;
	[SerializeField] private BulletParts bulletParts_ = null;
	[SerializeField] private SkillInfoFrameParts skillInfoFrameParts_ = null;
	[SerializeField] private ParameterInfoFrameParts parameterInfoFrameParts_ = null;
	[SerializeField] private CommandParts monsterActionCommandParts_ = null;
	[SerializeField] private CommandParts skillActionCommnadParts_ = null;
	[SerializeField] private CommandParts skillCommandParts_ = null;
	[SerializeField] private SkillInfoMenuParts skillInfoMenuParts_ = null;
	[SerializeField] private NovelWindowParts novelWindowParts_ = null;

	public MagazineParts GetMagazineParts() { return magazineParts_; }
	public BulletParts GetBulletParts() { return bulletParts_; }
	public SkillInfoFrameParts GetSkillInfoFrameParts() { return skillInfoFrameParts_; }
	public ParameterInfoFrameParts GetParameterInfoFrameParts() { return parameterInfoFrameParts_; }
	public CommandParts GetMonsterActionCommandParts() { return monsterActionCommandParts_; }
	public CommandParts GetSkillActionCommandParts() { return skillActionCommnadParts_; }
	public CommandParts GetSkillCommandParts() { return skillCommandParts_; }
	public SkillInfoMenuParts GetSkillInfoMenuParts() { return skillInfoMenuParts_; }
	public NovelWindowParts GetNovelWindowParts() { return novelWindowParts_; }

	//ステート
	private BMonsterMenuSceneProcessStateProvider nowProcessState_ = null;
	private MonsterMenuSceneInputSoundProvider inputSoundProvider_ = new MonsterMenuSceneInputSoundProvider();
	public BMonsterMenuSceneProcessStateProvider GetNowProcessState() { return nowProcessState_; }
	public MonsterMenuSceneInputSoundProvider GetInputSoundProvider() { return inputSoundProvider_; }

	public int selectMonsterNumber_ = 0;

	//入れ替え
	public bool swapActive_ = false;
	public int swapSelectNumber_ = 0;

	//技の習得
	static public bool skillTradeActive_ = false;
	static public SkillData skillTradeSkillData_ = null;
	static public int skillTradeSelectMonsterNumber_ = 0;

	static private BMonsterMenuSceneProcessStateProvider startProcessStateProvider_ = new MonsterMenuSceneBattleProcessStateProvider();
	static public void SetProcessStateProvider(BMonsterMenuSceneProcessStateProvider processStateProvider) {
		startProcessStateProvider_ = processStateProvider;
	}
}
