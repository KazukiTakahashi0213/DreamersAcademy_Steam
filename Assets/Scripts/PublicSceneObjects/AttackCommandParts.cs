using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommandParts : MonoBehaviour {
	[SerializeField] CommandParts commandParts_ = null;
	[SerializeField] CommandWindowParts skillInfoParts_ = null;

	public CommandParts GetCommandParts() { return commandParts_; }
	public CommandWindowParts GetSkillInfoParts() { return skillInfoParts_; }
}
