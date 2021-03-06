using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer commandWindowSprite_ = null;
	[SerializeField] private List<EventText> skillEventTexts_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public SpriteRenderer GetCommandWindowSprite() { return commandWindowSprite_; }
	public EventText GetSkillEventTexts(int number) { return skillEventTexts_[number]; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
}
