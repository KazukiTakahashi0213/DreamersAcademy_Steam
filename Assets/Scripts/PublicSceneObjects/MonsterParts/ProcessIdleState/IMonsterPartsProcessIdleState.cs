using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterPartsProcessIdleState {
	IMonsterPartsProcessIdleState Next();

	float addPos_ { get; }
}
