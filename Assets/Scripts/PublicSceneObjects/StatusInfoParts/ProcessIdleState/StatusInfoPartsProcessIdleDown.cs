using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusInfoPartsProcessIdleDown : IStatusInfoPartsProcessIdleState {
	public StatusInfoPartsProcessIdleDown() {
		addPos_ = -0.1f;
	}

	public float addPos_ { get; }

	public IStatusInfoPartsProcessIdleState Next() {
		return new StatusInfoPartsProcessIdleUp();
	}
}
