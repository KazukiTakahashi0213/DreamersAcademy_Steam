using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectValueTypeDreamPointDrain : BEffectValueType {
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

		//ドリームポイントの変動
		attackTrainerBattleData.DreamPointAddValue((int)attackSkillData.effectValue_);
		defenseTrainerBattleData.DreamPointAddValue(-(int)attackSkillData.effectValue_);

		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(attackEffectParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//DPの演出のイベント
		mgr.StatusInfoPartsDPEffectEventSet(defenseTrainerBattleData, defenseStatusInfoParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//DPの演出のイベント
		mgr.StatusInfoPartsDPEffectEventSet(attackTrainerBattleData, attackStatusInfoParts);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		//文字列の処理
		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetNovelWindowEventText(), attackTrainerBattleData.GetUniqueTrainerName() + attackMonsterData.uniqueName_ + "は\nドリームポイントを　うばった！");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		return true;
	}
}
