﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneSimpleProcessMonsterSelect : BMonsterMenuSceneProcessState {
	/// <summary>
	/// 0:上 1:下
	/// </summary>
	private int menuSelectState_ = 0;

	public override MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();
		PlayerBattleData playerData = PlayerBattleData.GetInstance();

		if (eventMgr.EventUpdate()) {
			//モンスターがNoneだったら
			if (playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).tribesData_.monsterNumber_ == (int)MonsterTribesDataNumber.None) {
				//上を押していたら
				if (menuSelectState_ == 0) {
					//SE
					monsterMenuManager.GetInputSoundProvider().UpSelect();

					monsterMenuManager.GetMagazineParts().UpRollMagazineParts();

					//イベントの最後
					eventMgr.EventFinishSet();

					monsterMenuManager.GetBulletParts().UpRollStatusInfoParts();

					monsterMenuManager.selectMonsterNumber_ += 1;
					monsterMenuManager.selectMonsterNumber_ %= playerData.GetMonsterDatasLength();

					//最後のステータスインフォパーツに反映
					int referMonsterNumber = (monsterMenuManager.selectMonsterNumber_ + 2) % playerData.GetMonsterDatasLength();
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));

					//状態異常の表示、非表示
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetFirstAbnormalStateInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetSecondAbnormalStateInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

					//タイプの表示、非表示
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetFirstElementInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetSecondElementInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetFirstElementInfoParts().gameObject.SetActive(false);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetSecondElementInfoParts().gameObject.SetActive(false);
				}
				//下を押していたら
				else if (menuSelectState_ == 1) {
					//SE
					monsterMenuManager.GetInputSoundProvider().DownSelect();

					monsterMenuManager.GetMagazineParts().DownRollMagazineParts();

					//イベントの最後
					eventMgr.EventFinishSet();

					monsterMenuManager.GetBulletParts().DownRollStatusInfoParts();

					monsterMenuManager.selectMonsterNumber_ -= 1;
					monsterMenuManager.selectMonsterNumber_ = System.Math.Abs((monsterMenuManager.selectMonsterNumber_ + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength());

					//最初のステータスインフォパーツに反映
					int referMonsterNumber = System.Math.Abs(((monsterMenuManager.selectMonsterNumber_ - 2) + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength());
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));

					//状態異常の表示、非表示
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

					//タイプの表示、非表示
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetFirstElementInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetSecondElementInfoParts().gameObject.SetActive(true);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetFirstElementInfoParts().gameObject.SetActive(false);
					monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetSecondElementInfoParts().gameObject.SetActive(false);
				}
			}
			else {
				//操作の変更
				sceneMgr.inputProvider_ = new KeyBoardNormalInputProvider();

				//モンスターのパラメーターの反映
				monsterMenuManager.GetParameterInfoFrameParts().MonsterDataReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_));

				//モンスターの技の名前の反映
				for (int i = 0; i < monsterMenuManager.GetSkillCommandParts().GetCommandWindowTextsCount(); ++i) {
					monsterMenuManager.GetSkillCommandParts().CommandWindowChoiceTextChange(i, "　" + playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(i).skillName_);
				}
			}
		}

		if (sceneMgr.inputProvider_.UpSelect()
			|| sceneMgr.inputProvider_.MouseWheelValue() > 0) {
			//状態の保存
			menuSelectState_ = 0;

			//SE
			monsterMenuManager.GetInputSoundProvider().UpSelect();

			monsterMenuManager.GetMagazineParts().UpRollMagazineParts();

			//イベントの最後
			eventMgr.EventFinishSet();

			monsterMenuManager.GetBulletParts().UpRollStatusInfoParts();

			monsterMenuManager.selectMonsterNumber_ += 1;
			monsterMenuManager.selectMonsterNumber_ %= playerData.GetMonsterDatasLength();

			//最後のステータスインフォパーツに反映
			int referMonsterNumber = (monsterMenuManager.selectMonsterNumber_ + 2) % playerData.GetMonsterDatasLength();
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));

			//状態異常の表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetFirstAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetSecondAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

			//タイプの表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetFirstElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 2).GetSecondElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetFirstElementInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetSecondElementInfoParts().gameObject.SetActive(false);

			sceneMgr.inputProvider_ = new InactiveInputProvider();
		}
		else if (sceneMgr.inputProvider_.DownSelect()
			|| sceneMgr.inputProvider_.MouseWheelValue() < 0) {
			//状態の保存
			menuSelectState_ = 1;

			//SE
			monsterMenuManager.GetInputSoundProvider().DownSelect();

			monsterMenuManager.GetMagazineParts().DownRollMagazineParts();

			//イベントの最後
			eventMgr.EventFinishSet();

			monsterMenuManager.GetBulletParts().DownRollStatusInfoParts();

			monsterMenuManager.selectMonsterNumber_ -= 1;
			monsterMenuManager.selectMonsterNumber_ = System.Math.Abs((monsterMenuManager.selectMonsterNumber_ + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength());

			//最初のステータスインフォパーツに反映
			int referMonsterNumber = System.Math.Abs(((monsterMenuManager.selectMonsterNumber_ - 2) + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength());
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));

			//状態異常の表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

			//タイプの表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetFirstElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetSecondElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetFirstElementInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetSecondElementInfoParts().gameObject.SetActive(false);

			sceneMgr.inputProvider_ = new InactiveInputProvider();
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| sceneMgr.inputProvider_.SelectMouseLeftTrigger()) {
			//None以外だったら
			if (playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).tribesData_.monsterNumber_ != (int)MonsterTribesDataNumber.None) {
				//SE
				monsterMenuManager.GetInputSoundProvider().SelectEnter();

				//技の情報の反映
				monsterMenuManager.GetSkillInfoFrameParts().SkillInfoReflect(PlayerBattleData.GetInstance().GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(0));

				//技の選択肢の初期化
				monsterMenuManager.GetSkillCommandParts().CommandWindowChoicesColliderActive();

				monsterMenuManager.GetSkillCommandParts().GetCursorParts().gameObject.SetActive(true);

				//操作の変更
				AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalTriggerInputProvider();

				return MonsterMenuSceneProcess.SkillSelect;
			}
		}
		else if (sceneMgr.inputProvider_.SelectBack()
			|| sceneMgr.inputProvider_.SelectMouseRightTrigger()) {
			if (playerData.GetMonsterDatas(0).battleActive_) {
				sceneMgr.inputProvider_ = new InactiveInputProvider();

				playerData.changeMonsterNumber_ = 0;
				playerData.changeMonsterActive_ = true;

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

		return monsterMenuManager.GetNowProcessState().state_;
	}
}
