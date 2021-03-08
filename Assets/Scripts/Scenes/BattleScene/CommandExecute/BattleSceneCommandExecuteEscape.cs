using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneCommandExecuteEscape : BBattleSceneCommandExecute {
	public override IProcessState Execute(BattleManager battleManager) {
		//文字列の設定
		AllEventManager.GetInstance().EventTextSet(battleManager.GetNovelWindowParts().GetNovelWindowEventText(), "いまは　にげられない！");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(battleManager.GetEventContextUpdateTime());
		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();

		return battleManager.nowProcessState();
	}
}
