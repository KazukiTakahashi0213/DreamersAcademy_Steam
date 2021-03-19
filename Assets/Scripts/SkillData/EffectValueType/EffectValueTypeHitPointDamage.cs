using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectValueTypeHitPointDamage : BEffectValueType {
	public override bool EffectValueEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, DreamPointInfoParts attackDreamPointInfoParts, DreamPointInfoParts defenseDreamPointInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
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

		//攻撃の成功判定
		//技の命中率×命中補正値M×ランク補正
		bool skillSuccess = AllSceneManager.GetInstance().GetRandom().Next(0, 100) < (int)(attackSkillData.successRateValue_ * (4096 / 4096) * monsterHitRateValue);

		//技が失敗した時の説明
		if (!skillSuccess) {
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
		int realEffectValue = (int)(MonsterData.BattleDamageCalculate(attackMonsterData, defenseMonsterData, attackSkillData) * critical);
		defenseMonsterData.nowHitPoint_ -= realEffectValue;
		if (defenseMonsterData.nowHitPoint_ < 0) defenseMonsterData.nowHitPoint_ = 0;

		//DPの変動
		//PlayerBattleData.GetInstance().dreamPoint_ += playerSkillData.upDpValue_;
		int attackElementSimillarRsult = defenseMonsterData.ElementSimillarCheckerForValue(attackSkillData.elementType_);
		if (attackElementSimillarRsult == 3) {
			attackTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueBestSimillar());
		}
		else if (attackElementSimillarRsult == 2) {
			attackTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueNormalSimillar());
		}
		else if (attackElementSimillarRsult == 1) {
			attackTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueBadSimillar());
		}
		else if (attackElementSimillarRsult == 0) {
			attackTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueNotSimillar());
		}

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//DPの演出のイベント
		attackDreamPointInfoParts.DPEffectEventSet(attackTrainerBattleData.GetDreamPoint());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(defenseEffectParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//ダメージを受けていたら
		if (realEffectValue > 0) {
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
			int defenseElementSimillarResult = defenseMonsterData.ElementSimillarCheckerForValue(attackSkillData.elementType_);
			if (defenseElementSimillarResult == 3) {
				//DPの増加
				defenseTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueBestSimillar());

				//文字列の処理
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "こうかは　ばつぐんだ！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseElementSimillarResult == 2) {
				//DPの増加
				defenseTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueNormalSimillar());
			}
			else if (defenseElementSimillarResult == 1) {
				//DPの増加
				defenseTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueBadSimillar());

				//文字列の処理
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "こうかは　いまひとつの　ようだ");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}
			else if (defenseElementSimillarResult == 0) {
				//DPの増加
				defenseTrainerBattleData.DreamPointAddValue(AllSceneManager.GetInstance().GetUpDPValueNotSimillar());

				//文字列の処理
				AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "こうかが　ないようだ・・・");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
			}

			//DPの演出のイベント
			defenseDreamPointInfoParts.DPEffectEventSet(defenseTrainerBattleData.GetDreamPoint());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());
		}

		return true;
	}
}
