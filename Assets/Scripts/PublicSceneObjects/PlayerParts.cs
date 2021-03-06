using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer eventSprite_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public EventSpriteRenderer GetEventSprite() { return eventSprite_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
}
