using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneNormalProcessSkillActionSelect : BMonsterMenuSceneProcessState {
	private MonsterMenuSceneSkillActionCommandExecuteStateProvider nowSkillActionCommandExecuteStateProvider_ = new MonsterMenuSceneSkillActionCommandExecuteStateProvider(MonsterMenuSceneSkillActionCommandExecute.Swap);

	public override MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		eventMgr.EventUpdate();

		if (MonsterMenuManager.skillTradeActive_) {
			MonsterMenuManager.skillTradeActive_ = false;

			sceneMgr.inputProvider_ = new KeyBoardSelectInactiveTriggerInputProvider();

			//技が設定されていたら
			if (MonsterMenuManager.skillTradeSkillData_ == null) {
				//フェードイン
				eventMgr.EventSpriteRendererSet(
					sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
					, null
					, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
					);
				eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				eventMgr.AllUpdateEventExecute(0.4f);
			}
			else {
				monsterMenuManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
				monsterMenuManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

				//スキルの行動の選択肢の初期化
				nowSkillActionCommandExecuteStateProvider_.state_ = MonsterMenuSceneSkillActionCommandExecute.Swap;
				monsterMenuManager.GetSkillActionCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

				string context = "１..." + "\r\n\r\n"
					+ "２の..." + "\r\n\r\n"
					+ "ぽかん！" + "\r\n\r\n"
					+ playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).uniqueName_ + "は　\r\n"
					+ playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()).skillName_ + "を忘れて" + "\r\n\r\n"
					+ MonsterMenuManager.skillTradeSkillData_.skillName_ + "を　覚えた！";
				List<string> contexts = t13.Utility.ContextSlice(context, "\r\n\r\n");

				//技の反映
				playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).SkillSet(MonsterMenuManager.skillTradeSkillData_, monsterMenuManager.GetSkillCommandParts().SelectNumber());

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(PlayerTrainerData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(monsterMenuManager.GetSkillCommandParts().SelectNumber()));

				//モンスターの技の名前の反映
				for (int i = 0; i < monsterMenuManager.GetSkillCommandParts().GetCommandWindowTextsCount(); ++i) {
					monsterMenuManager.GetSkillCommandParts().CommandWindowChoiceTextChange(i, "　" + playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(i).skillName_);
				}

				//フェードイン
				eventMgr.EventSpriteRendererSet(
					sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
					, null
					, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 0)
					);
				eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				eventMgr.AllUpdateEventExecute(0.4f);

				//ウィンドウの表示
				eventMgr.UpdateGameObjectSet(monsterMenuManager.GetNovelWindowParts().GetUpdateGameObject());
				eventMgr.UpdateGameObjectsActiveSetExecute(true);

				for (int i = 0; i < contexts.Count; ++i) {
					//文字列の処理
					eventMgr.EventTextSet(monsterMenuManager.GetNovelWindowParts().GetNovelWindowEventText(), contexts[i]);
					eventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
					eventMgr.AllUpdateEventExecute(0.6f);

					//Blinkの開始
					eventMgr.EventSpriteRendererSet(monsterMenuManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
					eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
					eventMgr.AllUpdateEventExecute();

					//Enterの押下待ち
					eventMgr.EventTriggerSet();

					//Blinkの終了
					eventMgr.EventSpriteRendererSet(monsterMenuManager.GetNovelWindowParts().GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
					eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
					eventMgr.AllUpdateEventExecute();
				}

				//ウィンドウの初期化
				eventMgr.EventTextSet(monsterMenuManager.GetNovelWindowParts().GetNovelWindowEventText(), "");
				eventMgr.EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				eventMgr.AllUpdateEventExecute();

				//ウィンドウの非表示
				eventMgr.UpdateGameObjectSet(monsterMenuManager.GetNovelWindowParts().GetUpdateGameObject());
				eventMgr.UpdateGameObjectsActiveSetExecute(false);
			}

			//操作の変更
			eventMgr.InputProviderChangeEventSet(new KeyBoardNormalTriggerInputProvider());

			return MonsterMenuSceneProcess.SkillTradeEventExecute;
		}

		//カーソルが動いていたら
		int commandSelectNumber = monsterMenuManager.GetSkillActionCommandParts().CommandSelectForNumber(new Vector3(), new Vector3(0, 0.55f, 0));
		if (commandSelectNumber > -1) {
			//SE
			monsterMenuManager.GetInputSoundProvider().UpSelect();

			nowSkillActionCommandExecuteStateProvider_.state_ = (MonsterMenuSceneSkillActionCommandExecute)monsterMenuManager.GetSkillActionCommandParts().SelectNumber() + 1;
		}
		else if (sceneMgr.inputProvider_.UpSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillActionCommandParts().CommandSelectUp(new Vector3(0, 0.55f, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().UpSelect();

				nowSkillActionCommandExecuteStateProvider_.state_ = (MonsterMenuSceneSkillActionCommandExecute)monsterMenuManager.GetSkillActionCommandParts().SelectNumber()+1;
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()) {
			//選択肢が動かせたら
			if (monsterMenuManager.GetSkillActionCommandParts().CommandSelectDown(new Vector3(0, -0.55f, 0))) {
				//SE
				monsterMenuManager.GetInputSoundProvider().DownSelect();

				nowSkillActionCommandExecuteStateProvider_.state_ = (MonsterMenuSceneSkillActionCommandExecute)monsterMenuManager.GetSkillActionCommandParts().SelectNumber()+1;
			}
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| monsterMenuManager.GetSkillActionCommandParts().MouseLeftButtonTriggerActive()) {
			//SE
			monsterMenuManager.GetInputSoundProvider().SelectEnter();

			nowSkillActionCommandExecuteStateProvider_.Execute(monsterMenuManager);

			//スキルの行動の選択肢の初期化
			nowSkillActionCommandExecuteStateProvider_.state_ = MonsterMenuSceneSkillActionCommandExecute.Swap;
			monsterMenuManager.GetSkillActionCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));
		}
		else if (sceneMgr.inputProvider_.SelectBack()
			|| sceneMgr.inputProvider_.SelectMouseRightButton()) {
			monsterMenuManager.GetSkillActionCommandParts().gameObject.SetActive(false);

			//スキルの行動の選択肢の初期化
			nowSkillActionCommandExecuteStateProvider_.state_ = MonsterMenuSceneSkillActionCommandExecute.Swap;
			monsterMenuManager.GetSkillActionCommandParts().SelectReset(new Vector3(-0.6f, 0.85f, -4));

			monsterMenuManager.GetSkillCommandParts().commandWindowChoicesColliderActive();

			return MonsterMenuSceneProcess.SkillSelect;
		}

		return monsterMenuManager.GetNowProcessState().state_;
	}
}
