using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillData {
	int skillNumber_ { get; }
	string skillName_ { get; }

	float effectValue_ { get; }

	List<AddParameterRankState> addSelfParameterRanks_ { get; }
	List<AddParameterRankState> addOtherParameterRanks_ { get; }

	List<AddAbnormalTypeState> addSelfAbnormalStates_ { get; }
	List<AddAbnormalTypeState> addOtherAbnormalStates_ { get; }
	
	int optionEffectTriggerRateValue_ { get; }
	int successRateValue_ { get; }
	int upDpValue_ { get; }

	int playPoint_ { get; }
	int nowPlayPoint_ { get; set; }

	ElementTypeState elementType_ { get; }
	EffectValueTypeProvider effectValueType_ { get; }

	int triggerPriority_ { get; }
	int criticalParameterRank_ { get; }

	string effectInfo_ { get; }

	void EffectAnimetionEventSet(EffectParts targetEffectParts);
}
