using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMenuSceneMonsterMenuSkillTradeProcessStateProvider : BBugMenuSceneProcessStateProvider {
	public BugMenuSceneMonsterMenuSkillTradeProcessStateProvider() {
		states_.Add(new BugMenuSceneMonsterMenuSkillTradeProcessNone());
		states_.Add(new BugMenuSceneMonsterMenuSkillTradeProcessSkillSelect());
	}

	public override void init(BugMenuManager bugMenuManager) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();

		//習得できる技の保存
		for(int i = 0;i < playerTrainerData.GetHaveSkillSize(); ++i) {
			bugMenuManager.SkillTradeActiveSkillsAdd(playerTrainerData.GetSkillDatas(i));
		}

		//技の名前の反映
		for (int i = 0; i < bugMenuManager.GetCommandParts().GetCommandWindowTextsCount(); ++i) {
			if (i < bugMenuManager.GetSkillTradeActiveSkillsCount()) {
				bugMenuManager.GetCommandParts().CommandWindowChoiceTextChange(i, "　" + bugMenuManager.GetSkillTradeActiveSkills(i).skillName_);
			}
		}

		//技の情報の反映
		bugMenuManager.GetInfoFrameParts().SkillInfoReflect(bugMenuManager.GetSkillTradeActiveSkills(0));

		//技が表以上にあったら
		if (bugMenuManager.GetSkillTradeActiveSkillsCount() > bugMenuManager.GetCommandParts().GetCommandWindowTextsCount()) {
			bugMenuManager.GetDownCursor().gameObject.SetActive(true);
		}
	}
}
