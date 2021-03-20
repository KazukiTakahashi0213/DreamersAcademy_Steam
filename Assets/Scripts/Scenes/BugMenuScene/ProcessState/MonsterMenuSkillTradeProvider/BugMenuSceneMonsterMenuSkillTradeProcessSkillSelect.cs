using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMenuSceneMonsterMenuSkillTradeProcessSkillSelect : BBugMenuSceneProcessState {
	//選択肢制御
	private int skillSelectNum_ = 0;

	public override BugMenuSceneProcess Update(BugMenuManager bugMenuManager) {
		AllEventManager eventMgr = AllEventManager.GetInstance();
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		eventMgr.EventUpdate();

		if (sceneMgr.inputProvider_.UpSelect()
			|| sceneMgr.inputProvider_.MouseWheelValue() > 0) {
			//表示する技がまだあったら
			if (skillSelectNum_ > 0) {
				//SE
				bugMenuManager.GetInputSoundProvider().UpSelect();

				--skillSelectNum_;

				//一番上からスクロールさせようとしたら
				if (bugMenuManager.GetCommandParts().SelectNumber() == 0) {
					//技の名前を更新する
					for (int i = skillSelectNum_, j = 0; i < skillSelectNum_ + bugMenuManager.GetCommandParts().GetCommandWindowTextsCount(); ++i) {
						bugMenuManager.GetCommandParts().CommandWindowChoiceTextChange(j, "　" + bugMenuManager.GetSkillTradeActiveSkills(i).skillName_);

						++j;
					}

					//カーソルを戻す
					//bugMenuManager.GetCommandParts().CommandSelect(1, new Vector3(0, -1.0f, 0));

					//downCursorの表示
					bugMenuManager.GetDownCursor().gameObject.SetActive(true);

					if (skillSelectNum_ == 0) {
						//upCursorの表示
						bugMenuManager.GetUpCursor().gameObject.SetActive(false);
					}
				}

				//選択肢の処理
				bugMenuManager.GetCommandParts().CommandSelectUp(new Vector3(0, 1.0f, 0));

				//技の情報の反映
				bugMenuManager.GetInfoFrameParts().SkillInfoReflect(bugMenuManager.GetSkillTradeActiveSkills(skillSelectNum_));
			}
		}
		else if (sceneMgr.inputProvider_.DownSelect()
			|| sceneMgr.inputProvider_.MouseWheelValue() < 0) {
			//表示する技がまだあったら
			if (skillSelectNum_ < bugMenuManager.GetSkillTradeActiveSkillsCount() - 1) {
				//SE
				bugMenuManager.GetInputSoundProvider().DownSelect();

				++skillSelectNum_;

				//一番下からスクロールさせようとしたら
				if (bugMenuManager.GetCommandParts().SelectNumber() == bugMenuManager.GetCommandParts().GetCommandWindowTextsCount()-1) {
					//技の名前を更新する
					for (int i = skillSelectNum_ - bugMenuManager.GetCommandParts().GetCommandWindowTextsCount() + 1, j = 0; i < skillSelectNum_ + 1; ++i) {
						bugMenuManager.GetCommandParts().CommandWindowChoiceTextChange(j, "　" + bugMenuManager.GetSkillTradeActiveSkills(i).skillName_);

						++j;
					}

					//カーソルを戻す
					//bugMenuManager.GetCommandParts().CommandSelect(-1, new Vector3(0, 1.0f, 0));

					//upCursorの表示
					bugMenuManager.GetUpCursor().gameObject.SetActive(true);

					if (skillSelectNum_ == bugMenuManager.GetSkillTradeActiveSkillsCount() - 1) {
						//downCursorの表示
						bugMenuManager.GetDownCursor().gameObject.SetActive(false);
					}
				}

				//選択肢の処理
				bugMenuManager.GetCommandParts().CommandSelectDown(new Vector3(0, -1.0f, 0));

				//技の情報の反映
				bugMenuManager.GetInfoFrameParts().SkillInfoReflect(bugMenuManager.GetSkillTradeActiveSkills(skillSelectNum_));
			}
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| sceneMgr.inputProvider_.SelectMouseLeftTrigger()) {
			//技を習得できるか
			if (playerData.GetMonsterDatas(MonsterMenuManager.skillTradeSelectMonsterNumber_).SkillTradeCheck(bugMenuManager.GetSkillTradeActiveSkills(skillSelectNum_).elementType_.state_)) {
				//SE
				bugMenuManager.GetInputSoundProvider().SelectEnter();

				MonsterMenuManager.skillTradeActive_ = true;
				MonsterMenuManager.skillTradeSkillData_ = new SkillData((SkillDataNumber)bugMenuManager.GetSkillTradeActiveSkills(skillSelectNum_).skillNumber_);

				sceneMgr.inputProvider_ = new InactiveInputProvider();

				//フェードアウト
				eventMgr.EventSpriteRendererSet(
					sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
					, null
					, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
					);
				eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				eventMgr.AllUpdateEventExecute(0.4f);

				//シーンの切り替え
				MonsterMenuManager.SetProcessStateProvider(new MonsterMenuSceneNormalProcessStateProvider());
				eventMgr.SceneChangeEventSet(SceneState.MonsterMenu, SceneChangeMode.Continue);
			}
		}
		else if (sceneMgr.inputProvider_.SelectBack()
			|| sceneMgr.inputProvider_.SelectMouseRightTrigger()) {
			MonsterMenuManager.skillTradeActive_ = true;
			MonsterMenuManager.skillTradeSkillData_ = null;

			sceneMgr.inputProvider_ = new InactiveInputProvider();

			//フェードアウト
			eventMgr.EventSpriteRendererSet(
				sceneMgr.GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, sceneMgr.GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
				);
			eventMgr.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			eventMgr.AllUpdateEventExecute(0.4f);

			//シーンの切り替え
			MonsterMenuManager.SetProcessStateProvider(new MonsterMenuSceneNormalProcessStateProvider());
			eventMgr.SceneChangeEventSet(SceneState.MonsterMenu, SceneChangeMode.Continue);
		}
		else if (sceneMgr.inputProvider_.SelectNovelWindowActive()) {
		}
		else if (sceneMgr.inputProvider_.SelectMenu()) {
		}

		return bugMenuManager.GetProcessProvider().state_;
	}
}
