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
	static private bool NoneExecutePlayerEventSet(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		return false;
	}
	static private bool NoneExecuteEnemyEventSet(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		return false;
	}

	//Attack
	static private bool AttackExecutePlayerEventSet(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
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
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "しかし　" + attackMonsterData.uniqueName_ + "の\nこうげきは　はずれた！");
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

		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(mgr.GetEnemyEffectParts());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//ダメージを受けていたら
		if (realDamage > 0) {
			//ダメージアクション（点滅）
			mgr.GetEnemyMonsterParts().GetEventMonsterSprite().blinkTimeRegulation_ = 0.06f;

			//点滅の開始
			AllEventManager.GetInstance().EventSpriteRendererSet(mgr.GetEnemyMonsterParts().GetEventMonsterSprite());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.48f);

			//点滅の終了
			AllEventManager.GetInstance().EventSpriteRendererSet(mgr.GetEnemyMonsterParts().GetEventMonsterSprite());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.2f);

			//ヒットポイントのゲージの変動イベントの設定
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(defenseMonsterData.RealHitPoint(), defenseMonsterData.nowHitPoint_, 1);
			AllEventManager.GetInstance().HpGaugePartsSet(mgr.GetEnemyStatusInfoParts().GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount);
			AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.5f);

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

			if (critical > 1.0f) {
				//急所の説明
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "きゅうしょに　あたった！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//効果の説明
			if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) > 1.0f) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "こうかは　ばつぐんだ！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) < 1.0f
				&& defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) > 0) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "こうかは　いまひとつの　ようだ");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) < 0.1f) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "こうかは　ないようだ・・・");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}

		return true;
	}
	static private bool AttackExecuteEnemyEventSet(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
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
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "しかし　あいての　" + attackMonsterData.uniqueName_ + "の\nこうげきは　はずれた！");
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

		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(mgr.GetPlayerEffectParts());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//ダメージを受けていたら
		if (realDamage > 0) {
			//ダメージアクション（点滅）
			mgr.GetPlayerMonsterParts().GetEventMonsterSprite().blinkTimeRegulation_ = 0.06f;

			//点滅の開始
			AllEventManager.GetInstance().EventSpriteRendererSet(mgr.GetPlayerMonsterParts().GetEventMonsterSprite());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.48f);

			//点滅の終了
			AllEventManager.GetInstance().EventSpriteRendererSet(mgr.GetPlayerMonsterParts().GetEventMonsterSprite());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.2f);

			//ヒットポイントのゲージの変動イベントの設定
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(defenseMonsterData.RealHitPoint(), defenseMonsterData.nowHitPoint_, 1);
			AllEventManager.GetInstance().HpGaugePartsSet(mgr.GetPlayerStatusInfoParts().GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount, defenseMonsterData);
			AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.5f);

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

			if (critical > 1.0f) {
				//急所の説明
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "きゅうしょに　あたった！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//効果の説明
			if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) > 1.0f) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "こうかは　ばつぐんだ！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) < 1.0f
				&& defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) > 0) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "こうかは　いまひとつの　ようだ");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseMonsterData.ElementSimillarChecker(attackSkillData.elementType_) < 0.1f) {
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "こうかが　ないようだ・・・");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
		}

		return true;
	}

	//Support
	static private bool SupportExecutePlayerEventSet(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(mgr.GetEnemyEffectParts());
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		return true;
	}
	static private bool SupportExecuteEnemyEventSet(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(mgr.GetPlayerEffectParts());
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		return true;
	}

	private delegate bool ExecutePlayerEventSetFunc(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData);
	private ExecutePlayerEventSetFunc[] executePlayerEventSets_ = new ExecutePlayerEventSetFunc[(int)EffectType.Max] {
		NoneExecutePlayerEventSet,
		AttackExecutePlayerEventSet,
		SupportExecutePlayerEventSet
	};
	public bool ExecutePlayerEventSet(BattleManager battleManager, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) { return executePlayerEventSets_[(int)state_](this, battleManager, attackMonsterData, attackSkillData, defenseMonsterData); }

	private delegate bool ExecuteEnemyEventSetFunc(EffectTypeState mine, BattleManager mgr, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData);
	private ExecuteEnemyEventSetFunc[] executeEnemyEventSets_ = new ExecuteEnemyEventSetFunc[(int)EffectType.Max] {
		NoneExecuteEnemyEventSet,
		AttackExecuteEnemyEventSet,
		SupportExecuteEnemyEventSet
	};
	public bool ExecuteEnemyEventSet(BattleManager battleManager, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) { return executeEnemyEventSets_[(int)state_](this, battleManager, attackMonsterData, attackSkillData, defenseMonsterData); }

	private EffectAttackTypeState effectAttackTypeState_ = new EffectAttackTypeState(EffectAttackType.Normal);
	public EffectAttackTypeState GetEffectAttackTypeState() { return effectAttackTypeState_; }
}
