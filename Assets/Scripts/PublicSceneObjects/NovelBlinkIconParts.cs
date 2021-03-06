using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelBlinkIconParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer novelBlinkIconEventSprite_ = null;

	public EventSpriteRenderer GetNovelBlinkIconEventSprite() { return novelBlinkIconEventSprite_; }
}
