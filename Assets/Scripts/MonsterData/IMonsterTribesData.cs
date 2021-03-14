using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//種族値、番号、デフォルトの名前、属性、画像
public interface IMonsterTribesData {
	int monsterNumber_ { get; }
	string monsterName_ { get; }

	int tribesHitPoint_ { get; }
	int tribesAttack_ { get; }
	int tribesDefense_ { get; }
	int tribesSpeed_ { get; }

	float tribesDreamAttack_ { get; }
	float tribesDreamDefense_ { get; }
	float tribesDreamSpeed_ { get; }

	ElementTypeState firstElement_ { get; }
	ElementTypeState secondElement_ { get; }

	Sprite frontTex_ { get; }
	Sprite backTex_ { get; }
	Sprite SDTex_ { get; }
	Sprite frontDreamTex_ { get; }
	Sprite backDreamTex_ { get; }
}
