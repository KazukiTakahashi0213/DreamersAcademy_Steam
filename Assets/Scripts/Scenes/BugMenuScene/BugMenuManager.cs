using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMenuManager : MonoBehaviour, ISceneManager {
	private BBugMenuSceneProcessStateProvider processProvider_ = new BugMenuSceneNormalProcessStateProvider();
	private BugMenuSceneInputSoundProvider inputSoundProvider_ = new BugMenuSceneInputSoundProvider();
	public BBugMenuSceneProcessStateProvider GetProcessProvider() { return processProvider_; }
	public BugMenuSceneInputSoundProvider GetInputSoundProvider() { return inputSoundProvider_; }

	private List<SkillData> skillTradeActiveSkills_ = new List<SkillData>();
	public SkillData GetSkillTradeActiveSkills(int number) { return skillTradeActiveSkills_[number]; }
	public int GetSkillTradeActiveSkillsCount() { return skillTradeActiveSkills_.Count; }
	public void SkillTradeActiveSkillsAdd(SkillData skillData) {
		if (!PlayerTrainerData.GetInstance().GetMonsterDatas(MonsterMenuManager.skillTradeSelectMonsterNumber_).SkillTradeCheck(skillData.elementType_.state_)) return;

		skillTradeActiveSkills_.Add(skillData);
	}


	//シーン上のオブジェクト
	[SerializeField] CommandParts commandParts_ = null;
	[SerializeField] SkillInfoFrameParts infoFrameParts_ = null;
	[SerializeField] SpriteRenderer downCursor_ = null;
	[SerializeField] SpriteRenderer upCursor_ = null;
	public CommandParts GetCommandParts() { return commandParts_; }
	public SkillInfoFrameParts GetInfoFrameParts() { return infoFrameParts_; }
	public SpriteRenderer GetDownCursor() { return downCursor_; }
	public SpriteRenderer GetUpCursor() { return upCursor_; }

	public void SceneStart() {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();

		//依存性注入
		processProvider_ = startProcessStateProvider_;
		processProvider_.state_ = BugMenuSceneProcess.SkillSelect;
		inputSoundProvider_.state_ = BugMenuSceneInputSoundState.Normal;

		//文字の初期化
		for (int i = 0; i < commandParts_.GetCommandWindowTextsCount(); ++i) {
			commandParts_.CommandWindowChoiceTextChange(i, "ーー");
		}

		//アップカーソルの初期化
		upCursor_.gameObject.SetActive(false);

		//選択肢の初期化
		commandParts_.SelectReset(new Vector3(-7.7f, 1.23f, -1));

		//初期化
		processProvider_.init(this);

		//フェードイン
		eventMgr.EventSpriteRendererSet(
			sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
			);
		eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		eventMgr.AllUpdateEventExecute(0.4f);

		//イベントの最後
		//操作の変更
		eventMgr.InputProviderChangeEventSet(new KeyBoardNormalTriggerInputProvider());
	}

	public void SceneUpdate() {
		processProvider_.state_ = processProvider_.Update(this);
	}

	public void SceneEnd() {
		skillTradeActiveSkills_.Clear();
	}

	public GameObject GetGameObject() { return gameObject; }

	static private BBugMenuSceneProcessStateProvider startProcessStateProvider_ = new BugMenuSceneNormalProcessStateProvider();
	static public void SetProcessStateProvider(BBugMenuSceneProcessStateProvider processStateProvider) {
		startProcessStateProvider_ = processStateProvider;
	}
}
