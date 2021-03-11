using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
	None,
	Attack,
	Support,
	Max
}

public class EffectTypeState {
	public EffectTypeState(EffectType setState, EffectAttackType effectAttackType = EffectAttackType.Normal) {
		state_ = setState;
		effectAttackTypeState_.state_ = effectAttackType;
	}

	public EffectType state_;

	//None
	static private bool NoneExecuteEventSet(EffectTypeState mine, BattleManager mgr, BTrainerBattleData attackTrainerBattleData, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		return false;
	}

	//Attack
	static private bool AttackExecuteEventSet(EffectTypeState mine, BattleManager mgr, BTrainerBattleData attackTrainerBattleData, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//ランク補正の計算
		float monsterHitRateValue = 0;
		{
			//回避率と命中率のランク
			int monsterHitRank = attackMonsterData.battleData_.GetHitRateParameterRank() - defenseMonsterData.battleData_.GetAvoidRateParameterRank();

			//分子,分母
			float numerator = 3, denominator = 3;

			if (monsterHitRank < 0) denominator -= monsterHitRank;
			else numerator += monsterHitRank;

			//回避率と命中率のランクの倍率
			monsterHitRateValue = numerator / denominator;
		}

		//攻撃のヒット判定
		//技の命中率×命中補正値M×ランク補正
		bool skillHit = AllSceneManager.GetInstance().GetRandom().Next(0, 100) < (int)(attackSkillData.hitRateValue_ * (4096 / 4096) * monsterHitRateValue);

		//攻撃がはずれた時の説明
		if (!skillHit) {
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "しかし　" + attackTrainerBattleData.GetUniqueTrainerName() + attackMonsterData.uniqueName_ + "の\nこうげきは　はずれた！");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

			AllEventManager.GetInstance().EventWaitSet(1.0f);

			return false;
		}

		//急所の判定
		float critical = 1;
		if (attackSkillData.criticalParameterRank_ <= 0) {
			if (AllSceneManager.GetInstance().GetRandom().Next(0, 24) == 13) critical = 1.5f;
		}
		else if (attackSkillData.criticalParameterRank_ == 1) {
			if (AllSceneManager.GetInstance().GetRandom().Next(0, 8) == 4) critical = 1.5f;
		}
		else if (attackSkillData.criticalParameterRank_ == 2) {
			if (AllSceneManager.GetInstance().GetRandom().Next(0, 2) == 0) critical = 1.5f;
		}
		else critical = 1.5f;

		//ヒットポイントの変動
		int realDamage = (int)(MonsterData.BattleDamageCalculate(attackMonsterData, defenseMonsterData, attackSkillData) * critical);
		defenseMonsterData.nowHitPoint_ -= realDamage;
		if (defenseMonsterData.nowHitPoint_ < 0) defenseMonsterData.nowHitPoint_ = 0;

		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(defenseEffectParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//ダメージを受けていたら
		if (realDamage > 0) {
			//ダメージアクション（点滅）
			defenseMonsterParts.GetEventMonsterSprite().blinkTimeRegulation_ = 0.06f;

			//点滅の開始
			AllEventManager.GetInstance().EventSpriteRendererSet(defenseMonsterParts.GetEventMonsterSprite());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.48f);

			//点滅の終了
			AllEventManager.GetInstance().EventSpriteRendererSet(defenseMonsterParts.GetEventMonsterSprite());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.2f);

			//ヒットポイントのゲージの変動イベントの設定
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(defenseMonsterData.RealHitPoint(), defenseMonsterData.nowHitPoint_, 1);
			AllEventManager.GetInstance().HpGaugePartsSet(defenseStatusInfoParts.GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount, defenseMonsterData);
			AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.5f);

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

			if (critical > 1.0f) {
				//急所の説明
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "きゅうしょに　あたった！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//効果の説明
			if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) > 1.0f) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "こうかは　ばつぐんだ！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) < 1.0f
				&& defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) > 0) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "こうかは　いまひとつの　ようだ");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) < 0.1f) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "こうかが　ないようだ・・・");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}

		return true;
	}

	//Support
	static private bool SupportExecuteEventSet(EffectTypeState mine, BattleManager mgr, BTrainerBattleData attackTrainerBattleData, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(defenseEffectParts);
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		return true;
	}

	private delegate bool ExecuteEventSetFunc(EffectTypeState mine, BattleManager mgr, BTrainerBattleData attackTrainerBattleData, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData);
	private ExecuteEventSetFunc[] executePlayerEventSets_ = new ExecuteEventSetFunc[(int)EffectType.Max] {
		NoneExecuteEventSet,
		AttackExecuteEventSet,
		SupportExecuteEventSet
	};
	public bool ExecuteEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) { return executePlayerEventSets_[(int)state_](this, mgr, attackTrainerBattleData, defenseEffectParts, defenseMonsterParts, defenseStatusInfoParts, attackMonsterData, attackSkillData, defenseMonsterData); }

	private EffectAttackTypeState effectAttackTypeState_ = new EffectAttackTypeState(EffectAttackType.Normal);
	public EffectAttackTypeState GetEffectAttackTypeState() { return effectAttackTypeState_; }
}
