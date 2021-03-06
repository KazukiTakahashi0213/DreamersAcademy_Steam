using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMenuSceneNormalProcessStateProvider : BBugMenuSceneProcessStateProvider {
	public BugMenuSceneNormalProcessStateProvider() {
		states_.Add(new BugMenuSceneNormalProcessNone());
		states_.Add(new BugMenuSceneNormalProcessSkillSelect());
	}

	public override void init(BugMenuManager bugMenuManager) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerTrainerData = PlayerTrainerData.GetInstance();

		//技の名前の反映
		for (int i = 0; i < bugMenuManager.GetCommandParts().GetCommandWindowTextsCount(); ++i) {
			if (i < playerTrainerData.GetSkillDatasCount()) {
				bugMenuManager.GetCommandParts().GetCommandWindowTexts(i).text = "　" + playerTrainerData.GetSkillDatas(i).skillName_;
			}
		}

		//技の情報の反映
		bugMenuManager.GetInfoFrameParts().SkillInfoReflect(playerTrainerData.GetSkillDatas(0));

		//技が表以上にあったら
		if (playerTrainerData.GetSkillDatasCount() > bugMenuManager.GetCommandParts().GetCommandWindowTextsCount()) {
			bugMenuManager.GetDownCursor().gameObject.SetActive(true);
		}
	}
}
