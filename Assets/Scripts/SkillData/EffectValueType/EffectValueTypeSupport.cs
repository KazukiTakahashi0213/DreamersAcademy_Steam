using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectValueTypeSupport : BEffectValueType {
	public override bool EffectValueEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, DreamPointInfoParts attackDreamPointInfoParts, DreamPointInfoParts defenseDreamPointInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		//技のアニメーション
		attackSkillData.EffectAnimetionEventSet(defenseEffectParts);
		AllEventManager.GetInstance().EventWaitSet(mgr.GetEventWaitTime());

		return true;
	}
}
