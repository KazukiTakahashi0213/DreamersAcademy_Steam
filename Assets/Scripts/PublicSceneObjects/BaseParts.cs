using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer baseSprite_ = null;
	[SerializeField] private EventText infoNameEventText_ = null;
	[SerializeField] private Text hpLogoText_ = null;

	public SpriteRenderer GetBaseSprite() { return baseSprite_; }
	public Text GetHpLogoText() { return hpLogoText_; }
	public EventText GetInfoNameEventText() { return infoNameEventText_; }
}
