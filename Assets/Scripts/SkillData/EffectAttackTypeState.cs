using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectAttackType {
	None,
	Normal,
	Max
}

public class EffectAttackTypeState {
	public EffectAttackTypeState(EffectAttackType setState) {
		state_ = setState;
	}

	public EffectAttackType state_;
}
