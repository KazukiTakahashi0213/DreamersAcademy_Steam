using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillData {
	int skillNumber_ { get; }
	string skillName_ { get; }

	float effectValue_ { get; }

	List<AddParameterRankState> addPlayerParameterRanks_ { get; }
	List<AddParameterRankState> addEnemyParameterRanks_ { get; }

	List<AddAbnormalTypeState> addPlayerAbnormalStates_ { get; }
	List<AddAbnormalTypeState> addEnemyAbnormalStates_ { get; }
	
	int optionEffectTriggerRateValue_ { get; }
	int hitRateValue_ { get; }
	int upDpValue_ { get; }

	int playPoint_ { get; }
	int nowPlayPoint_ { get; set; }

	ElementTypeState elementType_ { get; }
	EffectTypeState effectType_ { get; }

	int triggerPriority_ { get; }
	int criticalParameterRank_ { get; }

	string effectInfo_ { get; }

	void EffectAnimetionEventSet(EffectParts targetEffectParts);
}
