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
		//DPの演出のイベント
		mgr.PlayerEnemyStatusInfoPartsDPEffect();

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//パワーアップしていたら
		if (PlayerBattleData.GetInstance().dreamSyncronize_ == true) {
			//dpの初期化
			PlayerBattleData.GetInstance().dreamPoint_ = 0;
			PlayerBattleData.GetInstance().dreamSyncronize_ = false;

			//ゆめの文字色の変更
			mgr.GetCommandCommandParts().GetCommandWindowTexts(1).color = new Color32(50, 50, 50, 255);

			//DPの演出のイベント
			mgr.StatusInfoPartsDPEffectEventSet(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts());

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
			EnemyBattleData.GetInstance().dreamPoint_ = 0;
			EnemyBattleData.GetInstance().dreamSyncronize_ = false;

			//DPの演出のイベント
			mgr.StatusInfoPartsDPEffectEventSet(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts());

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
			PlayerSkillResultSet(mgr, playerMonsterData, playerSkillData, enemyMonsterData);
			//エネミーの戦闘処理
			EnemySkillResultSet(mgr, enemyMonsterData, enemySkillData, playerMonsterData);
		}
		else if (enemySkillData.triggerPriority_ > playerSkillData.triggerPriority_) {
			//エネミーの戦闘処理
			EnemySkillResultSet(mgr, enemyMonsterData, enemySkillData, playerMonsterData);
			//プレイヤーの戦闘処理
			PlayerSkillResultSet(mgr, playerMonsterData, playerSkillData, enemyMonsterData);
		}
		//素早さで行動順を決める
		else if (enemyMonsterData.RealSpeed() < playerMonsterData.RealSpeed()) {
			//プレイヤーの戦闘処理
			PlayerSkillResultSet(mgr, playerMonsterData, playerSkillData, enemyMonsterData);
			//エネミーの戦闘処理
			EnemySkillResultSet(mgr, enemyMonsterData, enemySkillData, playerMonsterData);
		}
		else if (enemyMonsterData.RealSpeed() == playerMonsterData.RealSpeed()) {
			if (AllSceneManager.GetInstance().GetRandom().Next(0, 2) == 0) {
				//プレイヤーの戦闘処理
				PlayerSkillResultSet(mgr, playerMonsterData, playerSkillData, enemyMonsterData);
				//エネミーの戦闘処理
				EnemySkillResultSet(mgr, enemyMonsterData, enemySkillData, playerMonsterData);
			}
			else {
				//エネミーの戦闘処理
				EnemySkillResultSet(mgr, enemyMonsterData, enemySkillData, playerMonsterData);
				//プレイヤーの戦闘処理
				PlayerSkillResultSet(mgr, playerMonsterData, playerSkillData, enemyMonsterData);
			}
		}
		else {
			//エネミーの戦闘処理
			EnemySkillResultSet(mgr, enemyMonsterData, enemySkillData, playerMonsterData);
			//プレイヤーの戦闘処理
			PlayerSkillResultSet(mgr, playerMonsterData, playerSkillData, enemyMonsterData);
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

	//HACK 同じ事をしている
	private void EnemySkillResultSet(BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//ダウンしていたら
		if (attackMonsterData.battleActive_ == false) return;

		//交換されていたら
		if (EnemyBattleData.GetInstance().changeMonsterActive_) return;

		//ねむりターンの消費
		mgr.SleepProcessUse(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts());

		//こんらんターンの消費
		mgr.ConfusionProcessUse(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts());

		//エネミーの文字列の設定
		string skillUseContext = "あいての　" + attackMonsterData.uniqueName_ + "の\n"
			+ attackSkillData.skillName_ + "！";

		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), skillUseContext);
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//技のイベントの設定
		bool skillResult = attackSkillData.effectType_.ExecuteEnemyEventSet(mgr, attackMonsterData, attackSkillData, defenseMonsterData);
		if (!skillResult) return;

		//追加効果の判定
		bool optionEffectTrigger = AllSceneManager.GetInstance().GetRandom().Next(1, 101) <= attackSkillData.optionEffectTriggerRateValue_;

		//追加効果の処理
		if (optionEffectTrigger) {
			//能力変化の処理
			EnemyParameterRankEventSet(mgr, defenseMonsterData, attackSkillData, attackMonsterData);

			//状態異常の処理
			EnemyAbnormalEventSet(mgr, defenseMonsterData, attackSkillData, attackMonsterData);
		}

		if (defenseMonsterData.nowHitPoint_ <= 0) {
			//モンスターが倒れた時のイベント
			PlayerBattleData.GetInstance().MonsterDownEventSet(mgr);

			AllEventManager.GetInstance().EventFinishSet();
		}
	}
	private void PlayerSkillResultSet(BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//ダウンしていたら
		if (attackMonsterData.battleActive_ == false) return;

		//交換されていたら
		if (PlayerBattleData.GetInstance().changeMonsterActive_) return;

		//ねむりターンの消費
		mgr.SleepProcessUse(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts());

		//こんらんターンの消費
		mgr.ConfusionProcessUse(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts());

		//プレイヤーの文字列の設定
		string skillUseContext = attackMonsterData.uniqueName_ + "の\n"
			+ attackSkillData.skillName_ + "！";

		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), skillUseContext);
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//技のイベントの設定
		bool skillResult = attackSkillData.effectType_.ExecutePlayerEventSet(mgr, attackMonsterData, attackSkillData, defenseMonsterData);
		if (!skillResult) return;

		//追加効果の判定
		bool optionEffectTrigger = AllSceneManager.GetInstance().GetRandom().Next(1, 101) <= attackSkillData.optionEffectTriggerRateValue_;

		//追加効果の処理
		if (optionEffectTrigger) {
			//能力変化の処理
			PlayerParameterRankEventSet(mgr, attackMonsterData, attackSkillData, defenseMonsterData);

			//状態異常の処理
			PlayerAbnormalEventSet(mgr, attackMonsterData, attackSkillData, defenseMonsterData);
		}

		if (defenseMonsterData.nowHitPoint_ <= 0) {
			//モンスターが倒れた時のイベント
			EnemyBattleData.GetInstance().MonsterDownEventSet(mgr);

			//白紙に
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), " ");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.5f);

			AllEventManager.GetInstance().EventFinishSet();
		}
	}

	private void PlayerParameterRankEventSet(BattleManager mgr, IMonsterData playerMonsterData, ISkillData attackSkillData, IMonsterData enemyMonsterData) {
		if (attackSkillData.addPlayerParameterRanks_[0].state_ != AddParameterRank.None) {

			if (attackSkillData.addPlayerParameterRanks_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//プレイヤーへの能力変化の処理
			for (int i = 0; i < attackSkillData.addPlayerParameterRanks_.Count; ++i) {
				//能力変化の更新
				string parameterRankContext = attackSkillData.addPlayerParameterRanks_[i].AddParameterExecute(playerMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), parameterRankContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}

		if (attackSkillData.addEnemyParameterRanks_[0].state_ != AddParameterRank.None) {

			if (attackSkillData.addEnemyParameterRanks_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//エネミーへの能力変化の処理
			for (int i = 0; i < attackSkillData.addEnemyParameterRanks_.Count; ++i) {
				//能力変化の更新
				string parameterRankContext = attackSkillData.addEnemyParameterRanks_[i].AddParameterExecute(enemyMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "あいての　" + parameterRankContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}
	}
	private void EnemyParameterRankEventSet(BattleManager mgr, IMonsterData playerMonsterData, ISkillData attackSkillData, IMonsterData enemyMonsterData) {
		if (attackSkillData.addPlayerParameterRanks_[0].state_ != AddParameterRank.None) {

			if (attackSkillData.addPlayerParameterRanks_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//エネミーへの能力変化の処理
			for (int i = 0; i < attackSkillData.addEnemyParameterRanks_.Count; ++i) {
				//能力変化の更新
				string parameterRankContext = attackSkillData.addPlayerParameterRanks_[i].AddParameterExecute(enemyMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "あいての　" + parameterRankContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}

		if (attackSkillData.addEnemyParameterRanks_[0].state_ != AddParameterRank.None) {

			if (attackSkillData.addEnemyParameterRanks_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//プレイヤーへの能力変化の処理
			for (int i = 0; i < attackSkillData.addPlayerParameterRanks_.Count; ++i) {
				//能力変化の更新
				string parameterRankContext = attackSkillData.addEnemyParameterRanks_[i].AddParameterExecute(playerMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), parameterRankContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}
	}

	private void PlayerAbnormalEventSet(BattleManager mgr, IMonsterData playerMonsterData, ISkillData attackSkillData, IMonsterData enemyMonsterData) {
		if (attackSkillData.addPlayerAbnormalStates_[0].state_ != AddAbnormalType.None) {

			if (attackSkillData.addPlayerAbnormalStates_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//プレイヤーへの状態異常の処理
			for (int i = 0; i < attackSkillData.addPlayerAbnormalStates_.Count; ++i) {
				//能力変化の更新
				string abnormalStateContext = attackSkillData.addPlayerAbnormalStates_[i].AddAbnormalTypeExecute(playerMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), abnormalStateContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//状態異常のイベントのセット
			playerMonsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(mgr.GetPlayerStatusInfoParts());

			//ねむりの処理の初期化
			mgr.SleepProcessStart();
			mgr.SleepUseStart(PlayerBattleData.GetInstance());

			//こんらんの状態の初期化
			mgr.ConfusionUseStart(PlayerBattleData.GetInstance());
		}

		if (attackSkillData.addEnemyAbnormalStates_[0].state_ != AddAbnormalType.None) {

			if (attackSkillData.addEnemyAbnormalStates_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//エネミーへの状態異常の処理
			for (int i = 0; i < attackSkillData.addEnemyAbnormalStates_.Count; ++i) {
				//能力変化の更新
				string abnormalStateContext = attackSkillData.addEnemyAbnormalStates_[i].AddAbnormalTypeExecute(enemyMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "あいての　" + abnormalStateContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//状態異常のイベントのセット
			enemyMonsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(mgr.GetEnemyStatusInfoParts());

			//ねむりの処理の初期化
			mgr.SleepProcessStart();
			mgr.SleepUseStart(EnemyBattleData.GetInstance());

			//こんらんの状態の初期化
			mgr.ConfusionUseStart(EnemyBattleData.GetInstance());
		}
	}
	private void EnemyAbnormalEventSet(BattleManager mgr, IMonsterData playerMonsterData, ISkillData attackSkillData, IMonsterData enemyMonsterData) {
		if (attackSkillData.addPlayerAbnormalStates_[0].state_ != AddAbnormalType.None) {

			if (attackSkillData.addPlayerAbnormalStates_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//エネミーへの状態異常の処理
			for (int i = 0; i < attackSkillData.addEnemyAbnormalStates_.Count; ++i) {
				//能力変化の更新
				string abnormalStateContext = attackSkillData.addPlayerAbnormalStates_[i].AddAbnormalTypeExecute(enemyMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "あいての　" + abnormalStateContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//状態異常のイベントのセット
			enemyMonsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(mgr.GetEnemyStatusInfoParts());

			//ねむりの処理の初期化
			mgr.SleepProcessStart();
			mgr.SleepUseStart(EnemyBattleData.GetInstance());

			//こんらんの状態の初期化
			mgr.ConfusionUseStart(EnemyBattleData.GetInstance());
		}

		if (attackSkillData.addEnemyAbnormalStates_[0].state_ != AddAbnormalType.None) {

			if (attackSkillData.addEnemyAbnormalStates_.Count > 0) {
				//アニメーション
				AllEventManager.GetInstance().EventWaitSet(1.0f);
			}

			//プレイヤーへの状態異常の処理
			for (int i = 0; i < attackSkillData.addPlayerAbnormalStates_.Count; ++i) {
				//能力変化の更新
				string abnormalStateContext = attackSkillData.addEnemyAbnormalStates_[i].AddAbnormalTypeExecute(playerMonsterData);

				//文字列のイベント
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), abnormalStateContext);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//状態異常のイベントのセット
			playerMonsterData.battleData_.AbnormalSetStatusInfoPartsEventSet(mgr.GetPlayerStatusInfoParts());

			//ねむりの処理の初期化
			mgr.SleepProcessStart();
			mgr.SleepUseStart(PlayerBattleData.GetInstance());

			//こんらんの状態の初期化
			mgr.ConfusionUseStart(PlayerBattleData.GetInstance());
		}
	}

}
