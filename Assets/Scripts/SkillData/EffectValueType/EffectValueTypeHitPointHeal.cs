using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectValueTypeHitPointHeal : BEffectValueType {
	public override bool EffectValueEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//攻撃の成功判定
		//技の命中率×命中補正値M×ランク補正
		bool skillSuccess = AllSceneManager.GetInstance().GetRandom().Next(0, 100) < (int)(attackSkillData.successRateValue_ * (4096 / 4096));

		//技が失敗した時の説明
		if (!skillSuccess) {
			AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), "しかし　" + attackTrainerBattleData.GetUniqueTrainerName() + attackMonsterData.uniqueName_ + "は\nうまく　きめられなかった！");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

			AllEventManager.GetInstance().EventWaitSet(1.0f);

			return false;
		}

		//ヒットポイントの変動
		attackMonsterData.nowHitPoint_ += (int)attackSkillData.effectValue_;
		if (attackMonsterData.nowHitPoint_ > attackMonsterData.RealHitPoint()) attackMonsterData.nowHitPoint_ = attackMonsterData.RealHitPoint();

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
		mgr.StatusInfoPartsDPEffectEventSet(attackTrainerBattleData, attackStatusInfoParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(attackEffectParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//ヒットポイントのゲージの変動イベントの設定
		float hpGaugeFillAmount = t13.Utility.ValueForPercentage(attackMonsterData.RealHitPoint(), attackMonsterData.nowHitPoint_, 1);
		AllEventManager.GetInstance().HpGaugePartsSet(attackStatusInfoParts.GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount, attackMonsterData);
		AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.5f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//文字列の処理
		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), attackTrainerBattleData.GetUniqueTrainerName() + attackMonsterData.uniqueName_ + "は\nたいりょくを　かいふくした！");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		return true;
	}
}
