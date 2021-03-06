﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSeedTutorialDocter : MonoBehaviour {
	//EntryPoint
	void Start() {
		EventMoveMap eventMoveMap_ = GetComponent<EventMoveMap>();

		eventMoveMap_.GetEventSetFuncs().Add(BattleStart);
		eventMoveMap_.GetEventSetFuncs().Add(BattleVictory);
		eventMoveMap_.GetEventSetFuncs().Add(BattleLose);
		eventMoveMap_.GetEventSetFuncs().Add(BattleAfter);
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
			md.SkillAdd(new SkillData(SkillDataNumber.Totugeki));
			md.SkillAdd(new SkillData(SkillDataNumber.Suimenngiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Dokunokiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Suiryuuuchi));
			//エネミーの手持ちに追加
			enemyBattleData.monsterAdd(md);
		}
		{
			//データの生成
			IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Furiruma), 0, 50);
			//技の取得
			md.SkillAdd(new SkillData(SkillDataNumber.Shougekiha));
			md.SkillAdd(new SkillData(SkillDataNumber.Happabiimu));
			md.SkillAdd(new SkillData(SkillDataNumber.Honoonokiri));
			md.SkillAdd(new SkillData(SkillDataNumber.Ibaranotoge));
			//エネミーの手持ちに追加
			enemyBattleData.monsterAdd(md);
		}

		//エネミーの設定
		enemyTrainerData.SetTrainerData("はかせ", "ヴィクター", ResourcesGraphicsLoader.GetInstance().GetGraphics("Enemy/Victer0"));

		//1週目だったら
		if (playerTrainerData.clearTimes_ == 0) {
			{
				//データの生成
				IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Handreon), 0, 50);
				//技の取得
				md.SkillAdd(new SkillData(SkillDataNumber.Totugeki));
				md.SkillAdd(new SkillData(SkillDataNumber.Baandoraibu));
				md.SkillAdd(new SkillData(SkillDataNumber.Faiasoodo));
				md.SkillAdd(new SkillData(SkillDataNumber.Kenbu));
				//プレイヤーの手持ちに追加
				playerTrainerData.MonsterAdd(md);
			}
			{
				//データの生成
				IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Bauporisu), 0, 50);
				//技の取得
				md.SkillAdd(new SkillData(SkillDataNumber.Totugeki));
				md.SkillAdd(new SkillData(SkillDataNumber.Suimenngiri));
				md.SkillAdd(new SkillData(SkillDataNumber.Dokunokiri));
				md.SkillAdd(new SkillData(SkillDataNumber.Suiryuuuchi));
				//プレイヤーの手持ちに追加
				playerTrainerData.MonsterAdd(md);
			}
			{
				//データの生成
				IMonsterData md = new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.Furiruma), 0, 50);
				//技の取得
				md.SkillAdd(new SkillData(SkillDataNumber.Shougekiha));
				md.SkillAdd(new SkillData(SkillDataNumber.Happabiimu));
				md.SkillAdd(new SkillData(SkillDataNumber.Honoonokiri));
				md.SkillAdd(new SkillData(SkillDataNumber.Ibaranotoge));
				//プレイヤーの手持ちに追加
				playerTrainerData.MonsterAdd(md);
			}
		}

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleStart1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//ウェイト
		allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime() * 2.0f);
		

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleStart2");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//ウェイト
		allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime() * 2.0f);

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleStart3");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//ウェイト
		allEventMgr.EventWaitSet(allSceneMgr.GetEventWaitTime() * 2.0f);

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleStart4");
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
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//技の取得
		for(int i = 1;i < 29; ++i) {
			playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)i));
		}
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)43));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)44));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)45));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)46));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)48));

		//BGMの再生
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

		//バトル後のイベントへ
		eventMoveMap.executeEventNum_ = 4;

		//階層の移動の解放
		mapManager.GetFloorObjects().GetEventMoveMaps(1).executeEventNum_ = 1;
		mapManager.GetFloorObjects().GetEventMoveMaps(2).executeEventNum_ = 1;

		//ドアの解放
		mapManager.GetFloorObjects().GetEventMoveMaps(1).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
		mapManager.GetFloorObjects().GetEventMoveMaps(2).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
		mapManager.GetFloorObjects().GetEventMoveMaps(3).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;
		mapManager.GetFloorObjects().GetEventMoveMaps(4).GetEventSpriteRenderer().GetSpriteRenderer().sprite = null;

		//手持ちモンスターの回復
		for (int i = 0;i < playerTrainerData.GetHaveMonsterSize(); ++i) {
			playerTrainerData.GetMonsterDatas(i).battleActive_ = true;
			playerTrainerData.GetMonsterDatas(i).nowHitPoint_ = playerTrainerData.GetMonsterDatas(i).RealHitPoint();
		}

		//クリア階層の増加
		playerTrainerData.clearMapFloor_ += 1;

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleVictory1");
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
	private static void BattleLose(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//技の取得
		for (int i = 1; i < 29; ++i) {
			playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)i));
		}
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)43));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)44));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)45));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)46));
		playerTrainerData.SkillAdd(new SkillData((SkillDataNumber)48));

		//BGMの再生
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().clip = ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Map());
		AllSceneManager.GetInstance().GetPublicAudioParts().GetBGMAudioSource().Play();

		//バトル後のイベントへ
		eventMoveMap.executeEventNum_ = 4;

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

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleLose1");
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
	private static void BattleAfter(EventMoveMap eventMoveMap, MapManager mapManager) {
		AllEventManager allEventMgr = AllEventManager.GetInstance();
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();
		EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
		PlayerBattleData playerBattleData = PlayerBattleData.GetInstance();
		EnemyBattleData enemyBattleData = EnemyBattleData.GetInstance();

		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		mapManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//ノベル処理
		{
			string context = ResourcesTextsLoader.GetInstance().GetTexts("TutorialDocter/BattleAfter1");
			EventMoveMap.NovelEvent(mapManager.GetNovelWindowParts(), context);
		}

		//イベントの最後
		allEventMgr.EventFinishSet();
	}
}
