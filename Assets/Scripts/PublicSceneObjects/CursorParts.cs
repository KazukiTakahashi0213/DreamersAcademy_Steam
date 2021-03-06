using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer cursorSprite_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public SpriteRenderer GetCursorSprite() { return cursorSprite_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
}
