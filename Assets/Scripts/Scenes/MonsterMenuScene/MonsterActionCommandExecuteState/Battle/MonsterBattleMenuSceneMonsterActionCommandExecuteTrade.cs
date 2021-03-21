using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleMenuSceneMonsterActionCommandExecuteTrade : BMonsterBattleMenuSceneMonsterActionCommandExecuteState {
	public override void Execute(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();

		//先頭がダウンしていたら
		if (!PlayerBattleData.GetInstance().GetMonsterDatas(0).battleActive_) {
			PlayerBattleData.GetInstance().changeMonsterNumber_ = monsterMenuManager.selectMonsterNumber_;
			PlayerBattleData.GetInstance().changeMonsterSkillNumber_ = monsterMenuManager.GetSkillCommandParts().SelectNumber();
			PlayerBattleData.GetInstance().changeMonsterActive_ = true;

			//操作の変更
			sceneMgr.inputProvider_ = new InactiveInputProvider();

			//技の選択肢の初期化
			monsterMenuManager.GetSkillCommandParts().SelectReset(new Vector3(-5.29f, 0.82f, 2));

			//フェードアウト
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
				);
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			//シーンの切り替え
			eventMgr.SceneChangeEventSet(SceneState.Battle, SceneChangeMode.Continue);
		}
		else {
			//モンスターが戦えて、Noneではなくて、先頭ではなかったら
			if (PlayerBattleData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).battleActive_
				&& PlayerBattleData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).tribesData_.monsterNumber_ != (int)MonsterTribesDataNumber.None
				&& monsterMenuManager.selectMonsterNumber_ != 0) {

				monsterMenuManager.GetMonsterActionCommandParts().gameObject.SetActive(false);

				monsterMenuManager.GetNowProcessState().state_ = MonsterMenuSceneProcess.SkillSelect;

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(PlayerBattleData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(0));

				//技の選択肢の初期化
				monsterMenuManager.GetSkillCommandParts().CommandWindowChoicesColliderActive();

				monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(true);

				//モンスターの交換中
				monsterMenuManager.monsterTradeSelectSkill_ = true;
			}
		}
	}
}
