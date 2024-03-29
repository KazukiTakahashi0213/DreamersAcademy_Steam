﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectValueTypeNone : BEffectValueType {
	public override bool EffectValueEventSet(BattleManager mgr, BTrainerBattleData attackTrainerBattleData, BTrainerBattleData defenseTrainerBattleData, EffectParts attackEffectParts, EffectParts defenseEffectParts, MonsterParts defenseMonsterParts, StatusInfoParts attackStatusInfoParts, StatusInfoParts defenseStatusInfoParts, DreamPointInfoParts attackDreamPointInfoParts, DreamPointInfoParts defenseDreamPointInfoParts, IMonsterData attackMonsterData, ISkillData attackSkillData, IMonsterData defenseMonsterData) {
		return base.EffectValueEventSet(mgr, attackTrainerBattleData, defenseTrainerBattleData, attackEffectParts, defenseEffectParts, defenseMonsterParts, attackStatusInfoParts, defenseStatusInfoParts, attackDreamPointInfoParts, defenseDreamPointInfoParts, attackMonsterData, attackSkillData, defenseMonsterData);
	}
}
