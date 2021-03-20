using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMenuSceneNormalProcessMonsterSelect : BMonsterMenuSceneProcessState {
	public override MonsterMenuSceneProcess Update(MonsterMenuManager monsterMenuManager) {
		AllSceneManager sceneMgr = AllSceneManager.GetInstance();
		AllEventManager eventMgr = AllEventManager.GetInstance();
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		eventMgr.EventUpdate();

		if (sceneMgr.inputProvider_.UpSelect()
			|| sceneMgr.inputProvider_.MouseWheelValue() > 0) {
			//SE
			monsterMenuManager.GetInputSoundProvider().UpSelect();

			monsterMenuManager.GetMagazineParts().UpRollMagazineParts();

			//操作の変更
			eventMgr.InputProviderChangeEventSet(new KeyBoardNormalInputProvider());

			monsterMenuManager.GetBulletParts().UpRollStatusInfoParts();

			monsterMenuManager.selectMonsterNumber_ += 1;
			monsterMenuManager.selectMonsterNumber_ %= playerData.GetMonsterDatasLength();

			//モンスターのパラメーターの反映
			monsterMenuManager.GetParameterInfoFrameParts().MonsterDataReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_));

			//モンスターの技の名前の反映
			for (int i = 0; i < monsterMenuManager.GetSkillCommandParts().GetCommandWindowTextsCount(); ++i) {
				monsterMenuManager.GetSkillCommandParts().CommandWindowChoiceTextChange(i, "　" + playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(i).skillName_);
			}

			//最後のステータスインフォパーツに反映
			int referMonsterNumber = (monsterMenuManager.selectMonsterNumber_ + 2) % playerData.GetMonsterDatasLength();
			if (monsterMenuManager.swapActive_ && referMonsterNumber == monsterMenuManager.swapSelectNumber_) {
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetBaseParts().GetBaseSprite().color = new Color32(222, 255, 0, 0);
			}
			else {
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() - 1).GetBaseParts().GetBaseSprite().color = new Color32(255, 255, 255, 0);
			}

			//状態異常の表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-2).GetFirstAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-2).GetSecondAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

			//タイプの表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-2).GetFirstElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-2).GetSecondElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetFirstElementInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetSecondElementInfoParts().gameObject.SetActive(false);

			sceneMgr.inputProvider_ = new InactiveInputProvider();
		}
		else if (sceneMgr.inputProvider_.DownSelect()
			|| sceneMgr.inputProvider_.MouseWheelValue() < 0) {
			//SE
			monsterMenuManager.GetInputSoundProvider().DownSelect();

			monsterMenuManager.GetMagazineParts().DownRollMagazineParts();

			//操作の変更
			eventMgr.InputProviderChangeEventSet(new KeyBoardNormalInputProvider());

			monsterMenuManager.GetBulletParts().DownRollStatusInfoParts();

			monsterMenuManager.selectMonsterNumber_ -= 1;
			monsterMenuManager.selectMonsterNumber_ = System.Math.Abs((monsterMenuManager.selectMonsterNumber_ + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength());

			//モンスターのパラメーターの反映
			monsterMenuManager.GetParameterInfoFrameParts().MonsterDataReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_));

			//モンスターの技の名前の反映
			for (int i = 0; i < monsterMenuManager.GetSkillCommandParts().GetCommandWindowTextsCount(); ++i) {
				monsterMenuManager.GetSkillCommandParts().CommandWindowChoiceTextChange(i, "　" + playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(i).skillName_);
			}

			//最後のステータスインフォパーツに反映
			int referMonsterNumber = System.Math.Abs(((monsterMenuManager.selectMonsterNumber_ - 2) + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength());
			if (monsterMenuManager.swapActive_ && referMonsterNumber == monsterMenuManager.swapSelectNumber_) {
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetBaseParts().GetBaseSprite().color = new Color32(222, 255, 0, 0);
			}
			else {
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).MonsterStatusInfoSet(playerData.GetMonsterDatas(referMonsterNumber));
				monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(0).GetBaseParts().GetBaseSprite().color = new Color32(255, 255, 255, 0);
			}

			//状態異常の表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-1).GetFirstAbnormalStateInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-1).GetSecondAbnormalStateInfoParts().gameObject.SetActive(false);

			//タイプの表示、非表示
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetFirstElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(1).GetSecondElementInfoParts().gameObject.SetActive(true);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-1).GetFirstElementInfoParts().gameObject.SetActive(false);
			monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize()-1).GetSecondElementInfoParts().gameObject.SetActive(false);

			sceneMgr.inputProvider_ = new InactiveInputProvider();
		}
		else if (sceneMgr.inputProvider_.RightSelect()) {
		}
		else if (sceneMgr.inputProvider_.LeftSelect()) {
		}
		else if (sceneMgr.inputProvider_.SelectEnter()
			|| sceneMgr.inputProvider_.SelectMouseLeftTrigger()) {
			//None以外だったら
			if(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).tribesData_.monsterNumber_ != (int)MonsterTribesDataNumber.None) {
				//SE
				monsterMenuManager.GetInputSoundProvider().SelectEnter();

				//スワップ中
				if (monsterMenuManager.swapActive_) {
					//スワップ選択中のモンスターと選択中のモンスターが一緒ではなかったら
					if (monsterMenuManager.swapSelectNumber_ != monsterMenuManager.selectMonsterNumber_) {
						//スワップ処理
						playerData.MonsterSwap(monsterMenuManager.selectMonsterNumber_, monsterMenuManager.swapSelectNumber_);

						//ステータスインフォへの反映
						for (int i = 0; i < (monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize() / 2) + 1; ++i) {
							if (i == 0) {
								monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(i + 2).MonsterStatusInfoSet(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_));
							}
							else {
								monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(i + 2).MonsterStatusInfoSet(playerData.GetMonsterDatas((monsterMenuManager.selectMonsterNumber_ + i) % playerData.GetMonsterDatasLength()));
								monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(-i + 2).MonsterStatusInfoSet(playerData.GetMonsterDatas(System.Math.Abs((monsterMenuManager.selectMonsterNumber_ - i + playerData.GetMonsterDatasLength()) % playerData.GetMonsterDatasLength())));
							}
						}

						//MagazinePartsのSDの画像の変更
						for (int i = 0; i < monsterMenuManager.GetMagazineParts().GetMonsterSDsPartsCount(); ++i) {
							monsterMenuManager.GetMagazineParts().GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().sprite = playerData.GetMonsterDatas(i).tribesData_.SDTex_;
						}

						//モンスターのパラメーターの反映
						monsterMenuManager.GetParameterInfoFrameParts().MonsterDataReflect(playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_));

						//モンスターの技の名前の反映
						for (int i = 0; i < monsterMenuManager.GetSkillCommandParts().GetCommandWindowTextsCount(); ++i) {
							monsterMenuManager.GetSkillCommandParts().CommandWindowChoiceTextChange(i, "　" + playerData.GetMonsterDatas(monsterMenuManager.selectMonsterNumber_).GetSkillDatas(i).skillName_);
						}
					}

					monsterMenuManager.swapActive_ = false;

					//バレットの色の変更
					for (int i = 0; i < monsterMenuManager.GetBulletParts().GetEventStatusInfosPartsSize(); ++i) {
						monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(i).GetBaseParts().GetBaseSprite().color = new Color(1.0f, 1.0f, 1.0f, monsterMenuManager.GetBulletParts().GetEventStatusInfosParts(i).GetBaseParts().GetBaseSprite().color.a);
					}
				}
				else {
					monsterMenuManager.GetMonsterActionCommandParts().gameObject.SetActive(true);

					//操作の変更
					AllSceneManager.GetInstance().inputProvider_ = new KeyBoardNormalTriggerInputProvider();

					return MonsterMenuSceneProcess.MonsterActionSelect;
				}
			}
		}
		else if (sceneMgr.inputProvider_.SelectBack()
			|| sceneMgr.inputProvider_.SelectMouseRightTrigger()) {
			//スワップ中じゃなかったら
			if (!monsterMenuManager.swapActive_) {
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
				if (playerData.prepareContinue_) {
					playerData.prepareContinue_ = false;
					eventMgr.SceneChangeEventSet(SceneState.GameContinue, SceneChangeMode.Continue);
				}
				else {
					eventMgr.SceneChangeEventSet(SceneState.Map, SceneChangeMode.Change);
				}
			}
		}

		return monsterMenuManager.GetNowProcessState().state_;
	}
}
