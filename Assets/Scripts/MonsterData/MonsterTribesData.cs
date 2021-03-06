﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MonsterTribesDataNumber {
	None
	, Maikon
	, Bauporisu
	, Furiruma
	, Furiruga
	, Sorushaaku
	, Biinaasu
	, DJKong
	, Naitobea
	, Bakutaa
	, Rabinobia
	, Handreon
	, Max
}

public class MonsterTribesData : IMonsterTribesData {
	public MonsterTribesData(MonsterTribesDataNumber monsterTribesNumber) {
		ResourcesGraphicsLoader graphicsLoader = ResourcesGraphicsLoader.GetInstance();

		ResourcesMonsterTribesData data = ResourcesMonsterTribesDatasLoader.GetInstance().GetMonsterDatas((int)monsterTribesNumber);

		monsterNumber_ = (int)monsterTribesNumber;
		monsterName_ = data.monsterName_;

		tribesHitPoint_ = data.tribesHitPoint_;
		tribesAttack_ = data.tribesAttack_;
		tribesDefense_ = data.tribesDefense_;
		tribesSpecialAttack_ = data.tribesSpecialAttack_;
		tribesSpecialDefense_ = data.tribesSpecialDefense_;
		tribesSpeed_ = data.tribesSpeed_;

		firstElement_ = new ElementTypeState((ElementType)data.firstElement_);
		secondElement_ = new ElementTypeState((ElementType)data.secondElement_);

		frontTex_ = graphicsLoader.GetGraphics("Monster/" + data.texName_ + "/" + data.texName_ + "_Front");
		backTex_ = graphicsLoader.GetGraphics("Monster/" + data.texName_ + "/" + data.texName_ + "_Back");
		SDTex_ = graphicsLoader.GetGraphics("Monster/" + data.texName_ + "/" + data.texName_ + "_SD");
		frontDreamTex_ = graphicsLoader.GetGraphics("Monster/" + data.texName_ + "/" + data.texName_ + "_FrontDream");
		backDreamTex_ = graphicsLoader.GetGraphics("Monster/" + data.texName_ + "/" + data.texName_ + "_BackDream");
	}

	public int monsterNumber_ { get; }
	public string monsterName_ { get; }

	public int tribesHitPoint_ { get; }
	public int tribesAttack_ { get; }
	public int tribesDefense_ { get; }
	public int tribesSpecialAttack_ { get; }
	public int tribesSpecialDefense_ { get; }
	public int tribesSpeed_ { get; }

	public ElementTypeState firstElement_ { get; }
	public ElementTypeState secondElement_ { get; }

	public Sprite frontTex_ { get; }
	public Sprite backTex_ { get; }
	public Sprite SDTex_ { get; }
	public Sprite frontDreamTex_ { get; }
	public Sprite backDreamTex_ { get; }
}
