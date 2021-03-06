using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer effectSprite_ = null;
	[SerializeField] private EventSpriteRenderer eventSpriteRenderer_ = null;

	public SpriteRenderer GetEffectSprite() { return effectSprite_; }
	public EventSpriteRenderer GetEventSpriteRenderer() { return eventSpriteRenderer_; }
}
