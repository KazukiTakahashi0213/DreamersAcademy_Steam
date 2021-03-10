using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType {
	None
	, Fire
	, Water
	, Tree
	, Holy
	, Dark
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

	//Fire
	static private bool SkillTradeCheckFire(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Fire) return true;

		return false;
	}

	//Water
	static private bool SkillTradeCheckWater(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Water) return true;

		return false;
	}

	//Tree
	static private bool SkillTradeCheckTree(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Tree) return true;
		if (elementType == ElementType.Fire) return true;

		return false;
	}

	//Holy
	static private bool SkillTradeCheckHoly(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Holy) return true;
		if (elementType == ElementType.Fire) return true;
		if (elementType == ElementType.Water) return true;
		if (elementType == ElementType.Tree) return true;

		return false;
	}

	//Dark
	static private bool SkillTradeCheckDark(ElementTypeState mine, ElementType elementType) {
		if (elementType == ElementType.Dark) return true;
		if (elementType == ElementType.Fire) return true;
		if (elementType == ElementType.Water) return true;
		if (elementType == ElementType.Tree) return true;

		return false;
	}

	private string[] names = new string[(int)ElementType.Max] {
		"None"
		, "ほのお"
		, "みず"
		, "くさ"
		, "ひかり"
		, "やみ"
	};
	public string GetName() { return names[(int)state_]; }

	private delegate bool SkillTradeCheckFunc(ElementTypeState mine, ElementType elementType);
	private SkillTradeCheckFunc[] skillTradeChecks_ = new SkillTradeCheckFunc[(int)ElementType.Max] {
		SkillTradeCheckNone
		, SkillTradeCheckFire
		, SkillTradeCheckWater
		, SkillTradeCheckTree
		, SkillTradeCheckHoly
		, SkillTradeCheckDark
	};
	public bool SkillTradeCheck(ElementType elementType) { return skillTradeChecks_[(int)state_](this, elementType); }
}
