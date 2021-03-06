using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSeedFinalDocter : MonoBehaviour {
	//EntryPoint
	void Start() {
		EventMoveMap eventMoveMap_ = GetComponent<EventMoveMap>();

		eventMoveMap_.GetEventSetFuncs().Add(BattleStart);
		eventMoveMap_.GetEventSetFuncs().Add(BattleVictory);
		eventMoveMap_.GetEventSetFuncs().Add(BattleLose);
	}

	private static void BattleStart(EventMoveMap eventMoveMap, MapManager mapManager) {
		PlayerBattleData.ReleaseInstance();
		EnemyBattleData.ReleaseInstance();

		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//エネミーのモンスター設定
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Bauporisu), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Faiasoodo));
			md.SkillAdd(new SkillData(SkillDataNumber.Suiryuuuchi));
			md.SkillAdd(new SkillData(SkillDataNumber.Midaresasi));
			md.SkillAdd(new SkillData(SkillDataNumber.Aiansoodo));
			//エネミーの手持ちに追加
			enemyBattleData.monsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Furiruma), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Tainetuhakka));
			md.SkillAdd(new SkillData(SkillDataNumber.Taihuun));
			md.SkillAdd(new SkillData(SkillDataNumber.Reerugan));
			md.SkillAdd(new SkillData(SkillDataNumber.Usikoku));
			//エネミーの手持ちに追加
			enemyBattleData.monsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Bakutaa), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Honoonokiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Dokunokiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Tetunokokoro));
			md.SkillAdd(new SkillData(SkillDataNumber.Sinkousin));
			//エネミーの手持ちに追加
			enemyBattleData.monsterAdd(md);
		}

		//エネミーの設定
		enemyTrainerData.SetTrainerData("はかせ", "ヴィクター", ResourcesGraphicsLoader.GetInstance().GetGraphics("Enemy/FinalVicter"));

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("FinalDocter/BattleStart1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//BGMの再生
		allEventMgr.BGMAudioClipChangeEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Dead()));
		allEventMgr.BGMAudioPlayEventSet();

		//戦闘の処理
		EventMoveMap.BattleEvent();
	}
	private static void BattleVictory(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//BGMの再生
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

		//手持ちモンスターの回復
		for (int i = 0; i < playerTrainerData.GetHaveMonsterSize(); ++i) {
			playerTrainerData.GetMonsterDatas(i).battleActive_ = true;
			playerTrainerData.GetMonsterDatas(i).nowHitPoint_ = playerTrainerData.GetMonsterDatas(i).RealHitPoint();
		}

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("FinalDocter/BattleVictory1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//フェードアウト
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(2.0f);

		//イベントの最後
		//シーンの切り替え
		if (playerTrainerData.clearTimes_ > 0) {
			allEventMgr.SceneChangeEventSet(SceneState.GameContinue, SceneChangeMode.Change);
		}
		else {
			allEventMgr.SceneChangeEventSet(SceneState.Ending, SceneChangeMode.Change);
		}
	}
	private static void BattleLose(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//BGMの再生
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

		//手持ちモンスターの回復
		for (int i = 0; i < playerTrainerData.GetHaveMonsterSize(); ++i) {
			playerTrainerData.GetMonsterDatas(i).battleActive_ = true;
			playerTrainerData.GetMonsterDatas(i).nowHitPoint_ = playerTrainerData.GetMonsterDatas(i).RealHitPoint();
		}

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("FinalDocter/BattleLose1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//プレイヤーデータの初期化
		PlayerTrainerData.ReleaseInstance();

		//選択肢の初期化
		mapManager.GetCommandParts().gameObject.SetActive(false);
		mapManager.GetCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

		//フェードアウト
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
			);
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(3.5f);

		//イベントの最後
		//シーンの切り替え
		allEventMgr.SceneChangeEventSet(SceneState.Title, SceneChangeMode.Change);
	}
}
