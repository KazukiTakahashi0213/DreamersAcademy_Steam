using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPartsProcessIdleUp : IMonsterPartsProcessIdleState {
	public MonsterPartsProcessIdleUp() {
		addPos_ = 0.1f;
	}

	public float addPos_ { get; }

	public IMonsterPartsProcessIdleState Next() {
		return new MonsterPartsProcessIdleDown();
	}
}
