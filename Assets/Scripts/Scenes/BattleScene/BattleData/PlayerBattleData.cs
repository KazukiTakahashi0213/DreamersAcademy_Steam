﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleData : TrainerBattleData {
	public override void monsterAdd(IMonsterData addMonster) {
		if (haveMonsterSize_ == MONSTER_MAX_SIZE) return;

		monsterDatas_[haveMonsterSize_] = addMonster;
		haveMonsterSize_ += 1;
		battleActiveMonsterSize_ += 1;
	}

	public override IMonsterData GetMonsterDatas(int num) { return monsterDatas_[num]; }
	public override int GetMonsterDatasLength() { return monsterDatas_.Length; }
	public override int GetHaveMonsterSize() { return haveMonsterSize_; }
	public override string GetUniqueTrainerName() { return ""; }

	//手持ちのモンスターのデータ
	private const int MONSTER_MAX_SIZE = 6;
	private int haveMonsterSize_ = 0;
	private IMonsterData[] monsterDatas_ = new IMonsterData[MONSTER_MAX_SIZE] {
		new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
		, new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
		, new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
		, new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
		, new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
		, new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
	};

	//戦えるモンスターの数
	private int battleActiveMonsterSize_ = 0;

	//交換するか否かのフラグ
	public bool changeMonsterActive_ = false;
	//交換する手持ちの番号
	public int changeMonsterNumber_ = 0;

	//パワーアップするか否かのフラグ
	public bool dreamSyncronize_ = false;

	//倒れた時の処理
	public override void MonsterDownEventSet(BattleManager manager) {
		battleActiveMonsterSize_ -= 1;

		dreamPoint_ += 45;

		//戦闘のモンスターをダウンさせる
		monsterDatas_[0].battleActive_ = false;

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(1.0f);

		//モンスターの画像の非表示
		AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetPlayerMonsterParts().GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterDown()));

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		//DPの演出のイベント
		manager.StatusInfoPartsDPEffectEventSet(this, manager.GetPlayerStatusInfoParts());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		//モンスターのステータスインフォの退場
		AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetPlayerStatusInfoParts().GetEventGameObject(), new Vector3(13.5f, manager.GetPlayerStatusInfoParts().transform.position.y, manager.GetPlayerStatusInfoParts().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);

		//文字列の処理
		AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetEventText(), monsterDatas_[0].uniqueName_ + "は　たおれた！");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		if (battleActiveMonsterSize_ == 0) {
			//BGMの再生
			AllEventManager.GetInstance().BGMAudioClipChangeEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Lose()));
			AllEventManager.GetInstance().BGMAudioPlayEventSet();

			//文字列の処理
			AllEventManager.GetInstance().EventTextSet(
				manager.GetNovelWindowParts().GetEventText()
				, EnemyTrainerData.GetInstance().job() + "の　" + EnemyTrainerData.GetInstance().name() + "\n"
				+ "との　しょうぶに　まけた");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime() * 2);

			//エネミーの入場
			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyParts().GetEventGameObject(), new Vector3(7.5f, manager.GetEnemyParts().transform.position.y, manager.GetEnemyParts().transform.position.z));
			AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime() * 2);

			//フェードアウト
			AllEventManager.GetInstance().EventSpriteRendererSet(
				AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite()
				, null
				, new Color(AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.r, AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.g, AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite().GetSpriteRenderer().color.b, 255)
				);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute(2.0f);

			//シーンの切り替え
			AllEventManager.GetInstance().SceneChangeEventSet(SceneState.Map, SceneChangeMode.Continue);

			//負けの設定
			PlayerTrainerData.GetInstance().battleEnd_ = true;
			PlayerTrainerData.GetInstance().battleResult_ = false;

			return;
		}

		//シーンの切り替え
		MonsterMenuManager.SetProcessStateProvider(new MonsterMenuSceneBattleProcessStateProvider());
		AllEventManager.GetInstance().SceneChangeEventSet(SceneState.MonsterMenu, SceneChangeMode.Slide);
	}

	//交換処理
	public override void MonsterChangeEventSet(BattleManager manager) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();
		
		//モンスターの変更が行われていたら
		if (changeMonsterNumber_ > 0) {
			IMonsterData md = monsterDatas_[changeMonsterNumber_];

			//先頭のパラメーターをリセット
			monsterDatas_[0].battleData_.RankReset();

			AllEventManager.GetInstance().EventStatusInfoPartsSet(manager.GetPlayerStatusInfoParts(), new Color32(0, 0, 0, 0));
			AllEventManager.GetInstance().StatusInfoPartsUpdateExecuteSet(StatusInfoPartsEventManagerExecute.IdleMoveEnd);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//先頭がダウンしていなかったら
			if (monsterDatas_[0].battleActive_ == true) {
				AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetEventText(), monsterDatas_[0].uniqueName_ + "\n"
					+ "もどれ！");
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

				AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

				AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetPlayerMonsterParts().GetEventGameObject());
				AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

				AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

				AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetPlayerStatusInfoParts().GetEventGameObject(), new Vector3(13.5f, manager.GetPlayerStatusInfoParts().transform.position.y, manager.GetPlayerStatusInfoParts().transform.position.z));
				AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
				AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);
			}

			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetEventText(), "ゆけ！　" + md.uniqueName_ + "！");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			//SE
			AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterSet()));

			//モンスターの登場演出
			{
				Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterSetEffect");
				List<Sprite> animeSprites = new List<Sprite>();
				for (int i = 0; i < sprites.Length; ++i) {
					animeSprites.Add(sprites[i]);
				}
				AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetPlayerEffectParts().GetEventSpriteRenderer(), animeSprites);
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
				AllEventManager.GetInstance().AllUpdateEventExecute(0.35f);
			}

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime() / 2);

			//画像の設定
			if (md.battleData_.HaveAbnormalType(AbnormalType.Hero)) {
				List<Sprite> sprites = new List<Sprite>();
				sprites.Add(md.tribesData_.backDreamTex_);

				AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetPlayerMonsterParts().GetEventMonsterSprite(), sprites, new Color32());
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
				AllEventManager.GetInstance().AllUpdateEventExecute();
			}
			else {
				List<Sprite> sprites = new List<Sprite>();
				sprites.Add(md.tribesData_.backTex_);

				AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetPlayerMonsterParts().GetEventMonsterSprite(), sprites, new Color32());
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
				AllEventManager.GetInstance().AllUpdateEventExecute();
			}

			//名前とレベルをTextに反映
			string monsterViewName = t13.Utility.StringFullSpaceBackTamp(md.uniqueName_, 6);
			AllEventManager.GetInstance().EventTextSet(manager.GetPlayerStatusInfoParts().GetBaseParts().GetInfoEventText(), monsterViewName + "　　Lｖ" + t13.Utility.HarfSizeForFullSize(md.level_.ToString()));
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//HPをTextに反映
			//HPゲージの調整
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(md.RealHitPoint(), md.nowHitPoint_, 1);
			AllEventManager.GetInstance().HpGaugePartsSet(manager.GetPlayerStatusInfoParts().GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount, md);
			AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//技をTextに反映
			for (int i = 0; i < 4; ++i) {
				AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i), "　" + t13.Utility.StringFullSpaceBackTamp(md.GetSkillDatas(i).skillName_, 7));
			}
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//文字の色の変更
			for (int i = 0; i < 4; ++i) {
				if (EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) > 1.0f) {
					manager.GetNovelWindowParts().GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(207, 52, 112, 255);
				}
				else if (EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) < 1.0f
					&& EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) > 0) {
					manager.GetNovelWindowParts().GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(52, 130, 207, 255);
				}
				else if (EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) < 0.1f) {
					manager.GetNovelWindowParts().GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(195, 195, 195, 255);
				}
				else {
					manager.GetNovelWindowParts().GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(50, 50, 50, 255);
				}
			}

			//状態異常の反映
			md.battleData_.AbnormalSetStatusInfoPartsEventSet(manager.GetPlayerStatusInfoParts());

			//ねむりの終了処理
			manager.SleepProcessEnd();

			IMonsterData temp = monsterDatas_[0];
			monsterDatas_[0] = monsterDatas_[changeMonsterNumber_];
			monsterDatas_[changeMonsterNumber_] = temp;

			//ねむりの開始処理
			manager.SleepProcessStart();
			manager.SleepUseStart(this);

			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetPlayerMonsterParts().GetEventGameObject());
			AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetPlayerStatusInfoParts().GetEventGameObject(), new Vector3(4.0f, manager.GetPlayerStatusInfoParts().transform.position.y, manager.GetPlayerStatusInfoParts().transform.position.z));
			AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);
		}
		
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		manager.ActiveUiCommand();
		manager.InactiveUiCommand();

		allSceneMgr.inputProvider_ = new InactiveInputProvider();

		changeMonsterNumber_ = 0;
	}

	//シングルトン
	private PlayerBattleData() { }

	static private PlayerBattleData instance_ = null;
	static public PlayerBattleData GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new PlayerBattleData();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
