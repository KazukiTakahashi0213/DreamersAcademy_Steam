using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoMenuParts : MonoBehaviour {
	[SerializeField] private List<SpriteRenderer> skillInfoMenuSpriteRenderers_ = new List<SpriteRenderer>();

	public SpriteRenderer GetSkillInfoMenuSprite(int value) { return skillInfoMenuSpriteRenderers_[value]; }
	public int GetSkillInfoMenuSpriteRendererCount() { return skillInfoMenuSpriteRenderers_.Count; }
}
