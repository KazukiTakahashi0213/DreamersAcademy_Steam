using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateSpriteRendererProcessBlink {
	None
	, In
	, Out
	, Max
}

public class UpdateSpriteRendererProcessBlinkState {
	public UpdateSpriteRendererProcessBlinkState(UpdateSpriteRendererProcessBlink setState) {
		state_ = setState;
	}

	public UpdateSpriteRendererProcessBlink state_;

	//None
	private static UpdateSpriteRendererProcessBlink NoneNext(UpdateSpriteRendererProcessBlinkState mine) {
		return mine.state_;
	}

	//In
	private static UpdateSpriteRendererProcessBlink InNext(UpdateSpriteRendererProcessBlinkState mine) {
		return UpdateSpriteRendererProcessBlink.Out;
	}

	//Out
	private static UpdateSpriteRendererProcessBlink OutNext(UpdateSpriteRendererProcessBlinkState mine) {
		return UpdateSpriteRendererProcessBlink.In;
	}

	private float[] alphaValues_ = new float[(int)UpdateSpriteRendererProcessBlink.Max] {
		0
		, 255
		, 0
	};
	public float AlphaValue() { return alphaValues_[(int)state_]; }

	private delegate UpdateSpriteRendererProcessBlink NextFunc(UpdateSpriteRendererProcessBlinkState mine);

	private NextFunc[] nextFuncs_ = new NextFunc[(int)UpdateSpriteRendererProcessBlink.Max] {
		NoneNext
		, InNext
		, OutNext
	};
	public UpdateSpriteRendererProcessBlink Next() { return nextFuncs_[(int)state_](this); }
}
