using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusInfoPartsProcessIdleUp : IStatusInfoPartsProcessIdleState {
	public StatusInfoPartsProcessIdleUp() {
		addPos_ = 0.1f;
	}

	public float addPos_ { get; }

	public IStatusInfoPartsProcessIdleState Next() {
		return new StatusInfoPartsProcessIdleDown();
	}
}
