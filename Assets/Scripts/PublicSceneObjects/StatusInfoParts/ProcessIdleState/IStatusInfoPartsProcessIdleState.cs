using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusInfoPartsProcessIdleState {
	IStatusInfoPartsProcessIdleState Next();

	float addPos_ { get; }
}
