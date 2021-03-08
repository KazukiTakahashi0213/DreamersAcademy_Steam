using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSeedGirl : MonoBehaviour {
	//EntryPoint
	void Start() {
		EventMoveMap eventMoveMap_ = GetComponent<EventMoveMap>();

		eventMoveMap_.GetEventSetFuncs().Add(BattleStart);
		eventMoveMap_.GetEventSetFuncs().Add(BattleVictory);
		eventMoveMap_.GetEventSetFuncs().Add(BattleLose);
		eventMoveMap_.GetEventSetFuncs().Add(BattleAfter);
		eventMoveMap_.GetEventSetFuncs().Add(TradeSuccess);
		eventMoveMap_.GetEventSetFuncs().Add(TradeAfter);
	}

	private static void BattleStart(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//エネミーのモンスター設定
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Rabinobia), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Happabureedo));
			md.SkillAdd(new SkillData(SkillDataNumber.Heddoatakku));
			md.SkillAdd(new SkillData(SkillDataNumber.Tetunokokoro));
			md.SkillAdd(new SkillData(SkillDataNumber.Nemurinokiri));
			//エネミーの手持ちに追加
			enemyTrainerData.MonsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Furiruga), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Happabiimu));
			md.SkillAdd(new SkillData(SkillDataNumber.Dairinshou));
			md.SkillAdd(new SkillData(SkillDataNumber.Musinokona));
			md.SkillAdd(new SkillData(SkillDataNumber.Sinkousin));
			//エネミーの手持ちに追加
			enemyTrainerData.MonsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Bauporisu), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Totugeki));
			md.SkillAdd(new SkillData(SkillDataNumber.Suimenngiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Dokunokiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Suiryuuuchi));
			//エネミーの手持ちに追加
			enemyTrainerData.MonsterAdd(md);
		}

		//エネミーの設定
		enemyTrainerData.SetTrainerData("アイドル志望", "あき", ResourcesGraphicsLoader.GetInstance().GetGraphics("Enemy/Girl"));

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("Girl/BattleStart1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}
		//BGMの再生
		allEventMgr.BGMAudioClipChangeEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Battle()));
		allEventMgr.BGMAudioPlayEventSet();

		//戦闘の処理
		EventMoveMap.BattleEvent();
	}
	private static void BattleVictory(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//BGMの再生
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

		//選択肢の有り
		mapManager.monsterTradeSelectActive_ = true;
		
		//階層の移動の解放
		mapManager.GetFloorObjects().GetEventMoveMaps(1).executeEventNum_ = 1;
		mapManager.GetFloorObjects().GetEventMoveMaps(2).executeEventNum_ = 1;
		
		//ドアの解放
		mapManager.GetFloorObjects().GetEventMoveMaps(1).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
		mapManager.GetFloorObjects().GetEventMoveMaps(2).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
		mapManager.GetFloorObjects().GetEventMoveMaps(3).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
		mapManager.GetFloorObjects().GetEventMoveMaps(4).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;

		//手持ちモンスターの回復
		for (int i = 0; i < playerTrainerData.GetHaveMonsterSize(); ++i) {
			playerTrainerData.GetMonsterDatas(i).battleActive_ = true;
			playerTrainerData.GetMonsterDatas(i).nowHitPoint_ = playerTrainerData.GetMonsterDatas(i).RealHitPoint();
		}

		//クリア階層の増加
		playerTrainerData.clearMapFloor_ += 1;

		//エネミーのモンスター設定
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Rabinobia), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Happabureedo));
			md.SkillAdd(new SkillData(SkillDataNumber.Heddoatakku));
			md.SkillAdd(new SkillData(SkillDataNumber.Tetunokokoro));
			md.SkillAdd(new SkillData(SkillDataNumber.Nemurinokiri));
			//エネミーの手持ちに追加
			enemyTrainerData.MonsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Furiruga), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Happabiimu));
			md.SkillAdd(new SkillData(SkillDataNumber.Dairinshou));
			md.SkillAdd(new SkillData(SkillDataNumber.Musinokona));
			md.SkillAdd(new SkillData(SkillDataNumber.Sinkousin));
			//エネミーの手持ちに追加
			enemyTrainerData.MonsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Bauporisu), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Totugeki));
			md.SkillAdd(new SkillData(SkillDataNumber.Suimenngiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Hyakutonpuresu));
			md.SkillAdd(new SkillData(SkillDataNumber.Baasutoappu));
			//エネミーの手持ちに追加
			enemyTrainerData.MonsterAdd(md);
		}

		//技の取得
		for (int i = 29; i < 43; ++i) {
			playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)i));
		}
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)47));

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("Girl/BattleVictory1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}
		//イベントの最後
		allEventMgr.EventFinishSet();
	}
	private static void BattleLose(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//BGMの再生
		allSceneMgr.GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
		allSceneMgr.GetPublicAudioParts().GetBGMAudioSource().Play();

		//手持ちモンスターの回復
		for (int i = 0; i < playerTrainerData.GetHaveMonsterSize(); ++i) {
			playerTrainerData.GetMonsterDatas(i).battleActive_ = true;
			playerTrainerData.GetMonsterDatas(i).nowHitPoint_ = playerTrainerData.GetMonsterDatas(i).RealHitPoint();
		}

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("Girl/BattleLose1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}
		//フェードアウト
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(0, 0, 0, 1)
			);
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.4f);

		//ウェイト
		allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime() * 2);

		//フェードイン
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(0, 0, 0, 0)
			);
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.4f);

		//イベントの最後
		allEventMgr.EventFinishSet();
	}
	private static void BattleAfter(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("Girl/BattleAfter1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//イベントの最後
		allEventMgr.EventFinishSet();
	}
	private static void TradeSuccess(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("Girl/TradeSuccess1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}
	}
	private static void TradeAfter(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//バトル後のイベントへ
		eventMoveMap.executeEventNum_ = 4;

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("Girl/TradeAfter1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//フェードアウト
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(0, 0, 0, 1)
			);
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.4f);

		//ウェイト
		allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime());

		//フェードイン
		allEventMgr.EventSpriteRendererSet(
			allSceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
			, null
			, new Color(0, 0, 0, 0)
			);
		allEventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		allEventMgr.AllUpdateEventExecute(0.4f);

		//イベントの最後
		allEventMgr.EventFinishSet();
	}
}
