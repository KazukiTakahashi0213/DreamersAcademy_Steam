using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommandParts : MonoBehaviour {
	[SerializeField] SkillParts skillParts_ = null;
	[SerializeField] CommandWindowParts skillInfoParts_ = null;

	public SkillParts GetSkillParts() { return skillParts_; }
	public CommandWindowParts GetSkillInfoParts() { return skillInfoParts_; }
}
