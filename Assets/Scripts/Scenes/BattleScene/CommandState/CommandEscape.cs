using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEscape : ICommandState {
	public ICommandState DownSelect(BattleManager mgr) {
		return this;
	}
	public ICommandState LeftSelect(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().LeftSelect();

		//どくのダメージ処理
		mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		mgr.CommandLeftCursorMove();
		return new CommandMonsterTrade();
	}
	public ICommandState RightSelect(BattleManager mgr) {
		return this;
	}
	public ICommandState UpSelect(BattleManager mgr) {
		//SE
		mgr.GetInputSoundProvider().UpSelect();

		//どくのダメージ処理
		mgr.PoisonDamageProcess(PlayerBattleData.GetInstance(), mgr.GetPlayerStatusInfoParts(), mgr.GetPlayerMonsterParts());

		mgr.CommandUpCursorMove();
		return new CommandDream();
	}

	public IProcessState Execute(BattleManager mgr) {
		//文字列の設定
		AllEventManager.GetInstance().EventTextSet(mgr.GetNovelWindowParts().GetEventText(), "いまは　にげられない！");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(mgr.GetEventContextUpdateTime());
		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();

		return mgr.nowProcessState();
	}
}
