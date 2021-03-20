using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneBattleProcessSkillSelect : BMonsterMenuSceneProcessState {
    public override MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();
		PlayerBattleData playerData = PlayerBattleData.GetInstance();

		eventMgr.EventUpdate();

		//カーソルが動いていたら
		int commandSelectNumber = monsterMenuManager.GetSkillCommandParts().CommandSelectForNumber(new Vector3(6.08f, 0, 0), new Vector3(0, 1.72f, 0));
		if (commandSelectNumber > -1) {
			//SE
			monsterMenuManager.GetInputSoundProvider().UpSelect();

			//技の情報の反映
			monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()));
		}
		else if (sceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelectUp(new Vector3(0, 1.72f, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().UpSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelectDown(new Vector3(0, -1.72f, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().DownSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelectRight(new Vector3(6.08f, 0, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().RightSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillCommandParts().CommandSelectLeft(new Vector3(-6.08f, 0, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().LeftSelect();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()));
			}
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| monsterMenuManager.GetSkillCommandParts().MouseLeftButtonTriggerActive()) {
			//モンスターの交換中なら
			if (monsterMenuManager.monsterTradeSelectSkill_) {
				monsterMenuManager.monsterTradeSelectSkill_ = false;

				PlayerBattleData.GetInstance().changeMonsterNumber_ = monsterMenuManager.selectMonsterNumber_;
				PlayerBattleData.GetInstance().changeMonsterSkillNumber_ = monsterMenuManager.GetSkillCommandParts().SelectNumber();
				PlayerBattleData.GetInstance().changeMonsterActive_ = true;

				monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(false);
				monsterMenuManager.GetSkillCommandParts().commandWindowChoicesColliderInactive();

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
		}
		else if (sceneMgr.inputProvider_.SelectBack()
			|| sceneMgr.inputProvider_.SelectMouseRightTrigger()) {
			monsterMenuManager.monsterTradeSelectSkill_ = false;

			monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(false);
			monsterMenuManager.GetSkillCommandParts().commandWindowChoicesColliderInactive();

			monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReset();

			//操作の変更
			AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalInputProvider();

			//技の選択肢の初期化
			monsterMenuManager.GetSkillCommandParts().SelectReset(new Vector3(-5.29f, 0.82f, 2));

			return MonsterMenuSceneProcess.MonsterSelect;
		}

		return monsterMenuManager.GetNowProcessState().state_;
	}
}
