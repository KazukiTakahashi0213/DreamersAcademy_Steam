using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer monsterSprite_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public SpriteRenderer GetMonsterSprite() { return monsterSprite_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
}
