using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbnormalStateInfoParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer baseEventSprite_ = null;
	[SerializeField] private EventText infoEventText_ = null;
	[SerializeField] private UpdateGameObject updateGameObject_ = null;

	public EventSpriteRenderer GetBaseEventSprite() { return baseEventSprite_; }
	public EventText GetInfoEventText() { return infoEventText_; }
	public UpdateGameObject GetUpdateGameObject() { return updateGameObject_; }
}
