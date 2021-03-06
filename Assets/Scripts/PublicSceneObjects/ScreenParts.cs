using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer screenSprite_ = null;

	public EventSpriteRenderer GetEventScreenSprite() { return screenSprite_; }
}
