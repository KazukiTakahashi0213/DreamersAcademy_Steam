using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType {
	None
	, Normal
	, Fire
	, Water
	, Tree
	, Insect
	, Steel
	, Ghost
	, Max
}

public class ElementTypeState {
	public ElementTypeState(ElementType setState) {
		state_ = setState;
	}

	public ElementType state_;

	//None
	static private bool SkillTradeCheckNone(ElementTypeState mine, ElementType elementType) {
		return false;
	}

	//Normal
	static private bool SkillTradeCheckNormal(ElementTypeState mine, ElementType elementType) {
		if (elementType != ElementType.Ghost) return true;

		return false;
	}

	//Fire
	static private bool SkillTradeCheckFire(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Normal) return true;
		if (elementType == ElementType.Fire) return true;
		if (elementType == ElementType.Steel) return true;
		if (elementType == ElementType.Ghost) return true;

		return false;
	}

	//Water
	static private bool SkillTradeCheckWater(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Normal) return true;
		if (elementType == ElementType.Water) return true;
		if (elementType == ElementType.Steel) return true;
		if (elementType == ElementType.Ghost) return true;

		return false;
	}

	//Tree
	static private bool SkillTradeCheckTree(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Normal) return true;
		if (elementType == ElementType.Tree) return true;
		if (elementType == ElementType.Insect) return true;
		if (elementType == ElementType.Fire) return true;
		if (elementType == ElementType.Ghost) return true;

		return false;
	}

	//Insect
	static private bool SkillTradeCheckInsect(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Normal) return true;
		if (elementType == ElementType.Insect) return true;
		if (elementType == ElementType.Tree) return true;

		return false;
	}

	//Steel
	static private bool SkillTradeCheckSteel(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Normal) return true;
		if (elementType == ElementType.Steel) return true;
		if (elementType == ElementType.Fire) return true;

		return false;
	}

	//Ghost
	static private bool SkillTradeCheckGhost(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Ghost) return true;
		if (elementType == ElementType.Fire) return true;
		if (elementType == ElementType.Water) return true;
		if (elementType == ElementType.Tree) return true;

		return false;
	}

	private string[] names = new string[(int)ElementType.Max] {
		"None",
		"ノーマル",
		"ほのお",
		"みず",
		"くさ",
		"むし",
		"はがね",
		"ゴースト"
	};
	public string GetName() { return names[(int)state_]; }

	private delegate bool SkillTradeCheckFunc(ElementTypeState mine, ElementType elementType);
	private SkillTradeCheckFunc[] skillTradeChecks_ = new SkillTradeCheckFunc[(int)ElementType.Max] {
		SkillTradeCheckNone
		, SkillTradeCheckNormal
		, SkillTradeCheckFire
		, SkillTradeCheckWater
		, SkillTradeCheckTree
		, SkillTradeCheckInsect
		, SkillTradeCheckSteel
		, SkillTradeCheckGhost
	};
	public bool SkillTradeCheck(ElementType elementType) { return skillTradeChecks_[(int)state_](this, elementType); }
}
