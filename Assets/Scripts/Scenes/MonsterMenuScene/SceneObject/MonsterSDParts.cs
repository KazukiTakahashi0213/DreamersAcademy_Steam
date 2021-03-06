using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSDParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer monsterSDEventSpriteRenderer_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public EventSpriteRenderer GetMonsterSDEventSpriteRenderer() { return monsterSDEventSpriteRenderer_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
}
