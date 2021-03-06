using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPartsProcessNone : IMonsterPartsProcessState {
	public IMonsterPartsProcessState Update(MonsterParts monsterParts) {
		return this;
	}
}
