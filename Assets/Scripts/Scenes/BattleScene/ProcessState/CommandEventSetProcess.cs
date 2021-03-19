using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandEventSetProcess : IProcessState {
	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new CommandEventExecuteProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		////DPの演出のイベント
		//mgr.PlayerEnemyStatusInfoPartsDPEffect();

		//ウィンドウの表示
		mgr.GetNovelWindowParts().gameObject.SetActive(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//パワーアップしていたら
		if (PlayerBattleData.GetInstance().dreamSyncronize_ == true) {
			//dpの初期化
			PlayerBattleData.GetInstance().DreamPointReset();
			PlayerBattleData.GetInstance().dreamSyncronize_ = false;

			//ゆめの文字色の変更
			mgr.GetDreamCommandSprite().color = new Color32(255, 255, 255, 255);

			//DPの演出のイベント
			mgr.GetPlayerDreamPointInfoParts().DPEffectEventSet(PlayerBattleData.GetInstance().GetDreamPoint());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

			//BGMの音量の減少
			AllEventManager.GetInstance().BGMAudioVolumeChangeEventSet(0.15f);

			//パワーアップの演出
			mgr.GetDreamEffectParts().DreamSyncronizeEventSet(PlayerBattleData.GetInstance().GetMonsterDatas(0), new Vector3(-13.0f, 0.8f, -1.0f), new Vector3(13.0f, 0.8f, -1.0f));
			
			//画像の変更
			{
				List<Sprite> sprites = new List<Sprite>();
				sprites.Add(PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.backDreamTex_);
			
				AllEventManager.GetInstance().EventSpriteRendererSet(mgr.GetPlayerMonsterParts().GetEventMonsterSprite(), sprites);
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
				AllEventManager.GetInstance().AllUpdateEventExecute();
			}

			//BGMの変更
			AllEventManager.GetInstance().BGMAudioClipChangeEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Rock()));
			
			//BGMの再生
			AllEventManager.GetInstance().BGMAudioVolumeChangeEventSet(0.3f);
			AllEventManager.GetInstance().BGMAudioPlayEventSet();

			//能力変化の更新
			string abnormalStateContext = new AddAbnormalTypeState(AddAbnormalType.Hero).AddAbnormalTypeExecute(PlayerBattleData.GetInstance().GetMonsterDatas(0));

			//文字列のイベント
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), abnormalStateContext);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

			//状態異常のイベントのセット
			PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.AbnormalSetStatusInfoPartsEventSet(mgr.GetPlayerStatusInfoParts());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

			//ねむりの終了処理
			mgr.SleepProcessEnd();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
		}

		//パワーアップしていたら
		if (EnemyBattleData.GetInstance().dreamSyncronize_ == true) {
			//dpの初期化
			EnemyBattleData.GetInstance().DreamPointReset();
			EnemyBattleData.GetInstance().dreamSyncronize_ = false;

			//DPの演出のイベント
			mgr.GetEnemyDreamPointInfoParts().DPEffectEventSet(EnemyBattleData.GetInstance().GetDreamPoint());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

			//BGMの音量の減少
			AllEventManager.GetInstance().BGMAudioVolumeChangeEventSet(0.15f);

			//パワーアップの演出
			mgr.GetDreamEffectParts().DreamSyncronizeEventSet(EnemyBattleData.GetInstance().GetMonsterDatas(0), new Vector3(13.0f, 0.8f, -1.0f), new Vector3(-13.0f, 0.8f, -1.0f));

			//画像の変更
			{
				List<Sprite> sprites = new List<Sprite>();
				sprites.Add(EnemyBattleData.GetInstance().GetMonsterDatas(0).tribesData_.frontDreamTex_);

				AllEventManager.GetInstance().EventSpriteRendererSet(mgr.GetEnemyMonsterParts().GetEventMonsterSprite(), sprites);
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
				AllEventManager.GetInstance().AllUpdateEventExecute();
			}

			//BGMの再生
			AllEventManager.GetInstance().BGMAudioVolumeChangeEventSet(0.3f);

			//能力変化の更新
			string abnormalStateContext = new AddAbnormalTypeState(AddAbnormalType.Hero).AddAbnormalTypeExecute(EnemyBattleData.GetInstance().GetMonsterDatas(0));

			//文字列のイベント
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), abnormalStateContext);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

			//状態異常のイベントのセット
			EnemyBattleData.GetInstance().GetMonsterDatas(0).battleData_.AbnormalSetStatusInfoPartsEventSet(mgr.GetEnemyStatusInfoParts());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
		}

		//交換されていたら
		if (PlayerBattleData.GetInstance().changeMonsterActive_ == true) {
			PlayerBattleData.GetInstance().MonsterChangeEventSet(mgr);
		}

		//交換されていたら
		if (EnemyBattleData.GetInstance().changeMonsterActive_ == true) {
			EnemyBattleData.GetInstance().MonsterChangeEventSet(mgr);
		}

		//現在、場に出ているモンスターのデータの取得
		IMonsterData enemyMonsterData = EnemyBattleData.GetInstance().GetMonsterDatas(0);
		IMonsterData playerMonsterData = PlayerBattleData.GetInstance().GetMonsterDatas(0);

		//現在、場に出ているモンスターの選択技のデータの取得
		ISkillData enemySkillData = enemyMonsterData.GetSkillDatas(mgr.enemySelectSkillNumber_);
		ISkillData playerSkillData = playerMonsterData.GetSkillDatas(mgr.playerSelectSkillNumber_);

		//技の優先度で行動順を決める
		if (enemySkillData.triggerPriority_ < playerSkillData.triggerPriority_) {
			//プレイヤーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetPlayerEffectParts()
				, mgr.GetEnemyEffectParts()
				, mgr.GetEnemyMonsterParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, PlayerBattleData.GetInstance()
				, EnemyBattleData.GetInstance()
				, playerMonsterData, playerSkillData, enemyMonsterData
				);
			//エネミーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetEnemyEffectParts()
				, mgr.GetPlayerEffectParts()
				, mgr.GetPlayerMonsterParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, EnemyBattleData.GetInstance()
				, PlayerBattleData.GetInstance()
				, enemyMonsterData, enemySkillData, playerMonsterData
				);
		}
		else if (enemySkillData.triggerPriority_ > playerSkillData.triggerPriority_) {
			//エネミーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetEnemyEffectParts()
				, mgr.GetPlayerEffectParts()
				, mgr.GetPlayerMonsterParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, EnemyBattleData.GetInstance()
				, PlayerBattleData.GetInstance()
				, enemyMonsterData, enemySkillData, playerMonsterData
				);
			//プレイヤーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetPlayerEffectParts()
				, mgr.GetEnemyEffectParts()
				, mgr.GetEnemyMonsterParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, PlayerBattleData.GetInstance()
				, EnemyBattleData.GetInstance()
				, playerMonsterData, playerSkillData, enemyMonsterData
				);
		}
		//素早さで行動順を決める
		else if ((enemyMonsterData.RealSpeed() * enemyMonsterData.battleData_.RealSpeedParameterRank()) < (playerMonsterData.RealSpeed() * playerMonsterData.battleData_.RealSpeedParameterRank())) {
			//プレイヤーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetPlayerEffectParts()
				, mgr.GetEnemyEffectParts()
				, mgr.GetEnemyMonsterParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, PlayerBattleData.GetInstance()
				, EnemyBattleData.GetInstance()
				, playerMonsterData, playerSkillData, enemyMonsterData
				);
			//エネミーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetEnemyEffectParts()
				, mgr.GetPlayerEffectParts()
				, mgr.GetPlayerMonsterParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, EnemyBattleData.GetInstance()
				, PlayerBattleData.GetInstance()
				, enemyMonsterData, enemySkillData, playerMonsterData
				);
		}
		else if ((enemyMonsterData.RealSpeed() * enemyMonsterData.battleData_.RealSpeedParameterRank()) == (playerMonsterData.RealSpeed() * playerMonsterData.battleData_.RealSpeedParameterRank())) {
			if (AllSceneManager.GetInstance().GetRandom().Next(0, 2) < 1) {
				//プレイヤーの戦闘処理
				SkillResultSet(
					mgr
					, mgr.GetPlayerEffectParts()
					, mgr.GetEnemyEffectParts()
					, mgr.GetEnemyMonsterParts()
					, mgr.GetPlayerStatusInfoParts()
					, mgr.GetEnemyStatusInfoParts()
					, mgr.GetPlayerDreamPointInfoParts()
					, mgr.GetEnemyDreamPointInfoParts()
					, PlayerBattleData.GetInstance()
					, EnemyBattleData.GetInstance()
					, playerMonsterData, playerSkillData, enemyMonsterData
					);
				//エネミーの戦闘処理
				SkillResultSet(
					mgr
					, mgr.GetEnemyEffectParts()
					, mgr.GetPlayerEffectParts()
					, mgr.GetPlayerMonsterParts()
					, mgr.GetEnemyStatusInfoParts()
					, mgr.GetPlayerStatusInfoParts()
					, mgr.GetEnemyDreamPointInfoParts()
					, mgr.GetPlayerDreamPointInfoParts()
					, EnemyBattleData.GetInstance()
					, PlayerBattleData.GetInstance()
					, enemyMonsterData, enemySkillData, playerMonsterData
					);
			}
			else {
				//エネミーの戦闘処理
				SkillResultSet(
					mgr
					, mgr.GetEnemyEffectParts()
					, mgr.GetPlayerEffectParts()
					, mgr.GetPlayerMonsterParts()
					, mgr.GetEnemyStatusInfoParts()
					, mgr.GetPlayerStatusInfoParts()
					, mgr.GetEnemyDreamPointInfoParts()
					, mgr.GetPlayerDreamPointInfoParts()
					, EnemyBattleData.GetInstance()
					, PlayerBattleData.GetInstance()
					, enemyMonsterData, enemySkillData, playerMonsterData
					);
				//プレイヤーの戦闘処理
				SkillResultSet(
					mgr
					, mgr.GetPlayerEffectParts()
					, mgr.GetEnemyEffectParts()
					, mgr.GetEnemyMonsterParts()
					, mgr.GetPlayerStatusInfoParts()
					, mgr.GetEnemyStatusInfoParts()
					, mgr.GetPlayerDreamPointInfoParts()
					, mgr.GetEnemyDreamPointInfoParts()
					, PlayerBattleData.GetInstance()
					, EnemyBattleData.GetInstance()
					, playerMonsterData, playerSkillData, enemyMonsterData
					);
			}
		}
		else {
			//エネミーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetEnemyEffectParts()
				, mgr.GetPlayerEffectParts()
				, mgr.GetPlayerMonsterParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, EnemyBattleData.GetInstance()
				, PlayerBattleData.GetInstance()
				, enemyMonsterData, enemySkillData, playerMonsterData
				);
			//プレイヤーの戦闘処理
			SkillResultSet(
				mgr
				, mgr.GetPlayerEffectParts()
				, mgr.GetEnemyEffectParts()
				, mgr.GetEnemyMonsterParts()
				, mgr.GetPlayerStatusInfoParts()
				, mgr.GetEnemyStatusInfoParts()
				, mgr.GetPlayerDreamPointInfoParts()
				, mgr.GetEnemyDreamPointInfoParts()
				, PlayerBattleData.GetInstance()
				, EnemyBattleData.GetInstance()
				, playerMonsterData, playerSkillData, enemyMonsterData
				);
		}

		//白紙に
		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), " ");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute();
		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(0.5f);

		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();

		//交換判定の初期化
		PlayerBattleData.GetInstance().changeMonsterActive_ = false;
		EnemyBattleData.GetInstance().changeMonsterActive_ = false;

		return mgr.nowProcessState().NextProcess();
	}


	private void SkillResultSet(BattleManager mgr, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, DreamPointInfoParts attackDreamPointInfoParts, DreamPointInfoParts defenseDreamPointInfoParts, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//ダウンしていたら
		if (!attackMonsterData.battleActive_) return;

		//交換されていたら
		//if (attackTrainerBattleData.changeMonsterActive_) return;

		//ねむりターンの消費
		mgr.SleepProcessUse(attackTrainerBattleData, attackStatusInfoParts);

		//こんらんターンの消費
		mgr.ConfusionProcessUse(attackTrainerBattleData, attackStatusInfoParts);

		//文字列の設定
		string skillUseContext = attackTrainerBattleData.GetUniqueTrainerName() + attackMonsterData.uniqueName_ + "の\n"
			+ attackSkillData.skillName_ + "！";

		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), skillUseContext);
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//技のイベントの設定
		bool skillResult = attackSkillData.effectValueType_.EffectValueEventSet(
			mgr
			, attackTrainerBattleData
			, defenseTrainerBattleData
			, attackEffectParts
			, defenseEffectParts
			, defenseMonsterParts
			, attackStatusInfoParts
			, defenseStatusInfoParts
			, attackDreamPointInfoParts
			, defenseDreamPointInfoParts
			, attackMonsterData, attackSkillData, defenseMonsterData
			);
		//技が当たっていなかったら
		if (!skillResult) return;

		//追加効果の判定
		bool optionEffectTrigger = AllSceneManager.GetInstance().GetRandom().Next(0, 100) < attackSkillData.optionEffectTriggerRateValue_;

		//追加効果の処理
		if (optionEffectTrigger) {
			//能力変化の処理
			ParameterRankEventSet(
				mgr
				, attackTrainerBattleData
				, attackMonsterData, attackSkillData, defenseMonsterData
				);

			//状態異常の処理
			AbnormalEventSet(
				mgr
				, attackStatusInfoParts
				, defenseStatusInfoParts
				, attackTrainerBattleData
				, defenseTrainerBattleData
				, attackMonsterData, attackSkillData, defenseMonsterData);
		}

		if (defenseMonsterData.nowHitPoint_ <= 0) {
			//モンスターが倒れた時のイベント
			defenseTrainerBattleData.MonsterDownEventSet(mgr);

			AllEventManager.GetInstance().EventFinishSet();
		}
	}

	private void ParameterRankEventSet(BattleManager mgr, BTrainerBattleData trainerBattleData, IMonsterData selfMonsterData, ISkillData skillData, IMonsterData otherMonsterData) {
		if (skillData.addSelfParameterRanks_[0].state_ != AddParameterRank.None) {
			if (skillData.addSelfParameterRanks_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//自分への能力変化の処理
			for (int i = 0; i < skillData.addSelfParameterRanks_.Count; ++i) {
				//能力変化の更新
				string parameterRankContext = skillData.addSelfParameterRanks_[i].AddParameterExecute(selfMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), trainerBattleData.GetUniqueTrainerName() + parameterRankContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}

		if (skillData.addOtherParameterRanks_[0].state_ != AddParameterRank.None) {
			if (skillData.addOtherParameterRanks_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//相手への能力変化の処理
			for (int i = 0; i < skillData.addOtherParameterRanks_.Count; ++i) {
				//能力変化の更新
				string parameterRankContext = skillData.addOtherParameterRanks_[i].AddParameterExecute(otherMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), parameterRankContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}
	}

	private void AbnormalEventSet(BattleManager mgr, StatusInfoParts selfStatusInfoParts, StatusInfoParts otherStatusInfoParts, BTrainerBattleData selfTrainerBattleData, BTrainerBattleData otherTrainerBattleData, IMonsterData selfMonsterData, ISkillData skillData, IMonsterData otherMonsterData) {
		if (skillData.addSelfAbnormalStates_[0].state_ != AddAbnormalType.None) {

			if (skillData.addSelfAbnormalStates_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//自分への状態異常の処理
			for (int i = 0; i < skillData.addSelfAbnormalStates_.Count; ++i) {
				//能力変化の更新
				string abnormalStateContext = skillData.addSelfAbnormalStates_[i].AddAbnormalTypeExecute(selfMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), selfTrainerBattleData.GetUniqueTrainerName() + abnormalStateContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//状態異常のイベントのセット
			selfMonsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(selfStatusInfoParts);

			//ねむりの処理の初期化
			mgr.SleepProcessStart();
			mgr.SleepUseStart(selfTrainerBattleData);

			//こんらんの状態の初期化
			mgr.ConfusionUseStart(selfTrainerBattleData);
		}

		if (skillData.addOtherAbnormalStates_[0].state_ != AddAbnormalType.None) {

			if (skillData.addOtherAbnormalStates_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//相手への状態異常の処理
			for (int i = 0; i < skillData.addOtherAbnormalStates_.Count; ++i) {
				//能力変化の更新
				string abnormalStateContext = skillData.addOtherAbnormalStates_[i].AddAbnormalTypeExecute(otherMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), abnormalStateContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//状態異常のイベントのセット
			otherMonsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(otherStatusInfoParts);

			//ねむりの処理の初期化
			mgr.SleepProcessStart();
			mgr.SleepUseStart(otherTrainerBattleData);

			//こんらんの状態の初期化
			mgr.ConfusionUseStart(otherTrainerBattleData);
		}
	}

}
