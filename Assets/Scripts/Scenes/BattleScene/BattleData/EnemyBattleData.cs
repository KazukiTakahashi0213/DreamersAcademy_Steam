using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleData : TrainerBattleData {
	public override void monsterAdd(IMonsterData addMonster) {
		if (haveMonsterSize_ == MONSTER_MAX_SIZE) return;

		monsterDatas_[haveMonsterSize_] = addMonster;
		haveMonsterSize_ += 1;
		battleActiveMonsterSize_ += 1;
	}

	public override IMonsterData GetMonsterDatas(int num) { return monsterDatas_[num]; }
	public override int GetMonsterDatasLength() { return monsterDatas_.Length; }
	public override int GetHaveMonsterSize() { return haveMonsterSize_; }
	public override string GetUniqueTrainerName() { return "あいての　"; }
	public bool GetThinkingEnd() { return thikingEnd_; }

	//手持ちのモンスターのデータ
	private const int MONSTER_MAX_SIZE = 3;
	private int haveMonsterSize_ = 0;
	private IMonsterData[] monsterDatas_ = new IMonsterData[MONSTER_MAX_SIZE] {
		new MonsterData(new MonsterTribesData(MonsterTribesDataNumber.None), 0, 50)
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

	//思考時間
	private const float THINKING_TIME_REGURATION = 3.0f;
	//思考処理が終わったかのフラグ
	private bool thikingEnd_ = false;
	//思考時間のタイマー
	private t13.TimeCounter thikingTimeCounter_ = new t13.TimeCounter();

	//どくダメージの時間
	private const float POISON_COUNTER_TIME_REGULATION = 1.5f;
	//どく状態のカウンター
	private float poisonCounter_ = 0;

	//倒れた時の処理
	public override void MonsterDownEventSet(BattleManager manager) {
		battleActiveMonsterSize_ -= 1;

		dreamPoint_ += 45;

		//戦闘のモンスターをダウンさせる
		monsterDatas_[0].battleActive_ = false;

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(1.0f);

		//エネミーの画像の非表示
		AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyMonsterParts().GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterDown()));

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		//DPの演出のイベント
		manager.StatusInfoPartsDPEffectEventSet(this, manager.GetEnemyStatusInfoParts());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		//エネミーのステータスインフォの退場
		AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyStatusInfoParts().GetEventGameObject(), new Vector3(-13.5f, manager.GetEnemyStatusInfoParts().transform.position.y, manager.GetEnemyStatusInfoParts().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);

		//文字列の処理
		AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetEventText(), "あいての　" + monsterDatas_[0].uniqueName_ + "は　たおれた！");
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		if (battleActiveMonsterSize_ == 0) {
			//BGMの再生
			AllEventManager.GetInstance().BGMAudioClipChangeEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamers_Win()));
			AllEventManager.GetInstance().BGMAudioPlayEventSet();

			//文字列の処理
			AllEventManager.GetInstance().EventTextSet(
				manager.GetNovelWindowParts().GetEventText()
				, EnemyTrainerData.GetInstance().job() + "の　" + EnemyTrainerData.GetInstance().name() + "\n"
				+ "との　しょうぶに　かった！");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime() * 2);

			//エネミーの入場
			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyParts().GetEventGameObject(), new Vector3(3.5f, manager.GetEnemyParts().transform.position.y, manager.GetEnemyParts().transform.position.z));
			AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
			AllEventManager.GetInstance().AllUpdateEventExecute(1.5f);

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

			//勝ちの設定
			PlayerTrainerData.GetInstance().battleEnd_ = true;
			PlayerTrainerData.GetInstance().battleResult_ = true;

			return;
		}

		//タイプ相性の測定
		int[] typeSimillarResult = new int[3] { 0, 0, 0 };
		int[] monsterNumbers = new int[3] { 0, 1, 2 };


		for (int i = 0; i < monsterDatas_.Length; ++i) {
			//戦えたら
			if(monsterDatas_[i].battleActive_
				&& monsterDatas_[i].tribesData_.monsterNumber_ != 0) {
				{
					int simillarResult = PlayerBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarCheckerForValue(monsterDatas_[i].tribesData_.firstElement_);

					if (simillarResult == 0) typeSimillarResult[i] += 3;
					else if (simillarResult == 1) typeSimillarResult[i] += 1;
					else if (simillarResult == 2) typeSimillarResult[i] += 0;
					else if (simillarResult == 3) typeSimillarResult[i] += 2;
				}
				{
					int simillarResult = PlayerBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarCheckerForValue(monsterDatas_[i].tribesData_.secondElement_);

					if (simillarResult == 0) typeSimillarResult[i] += 3;
					else if (simillarResult == 1) typeSimillarResult[i] += 1;
					else if (simillarResult == 2) typeSimillarResult[i] += 0;
					else if (simillarResult == 3) typeSimillarResult[i] += 2;
				}
			}
		}

		t13.Utility.SimpleHiSort2Index(typeSimillarResult, monsterNumbers);

		//モンスターデータの入れ替え
		IMonsterData temp = monsterDatas_[0];
		monsterDatas_[0] = monsterDatas_[monsterNumbers[0]];
		monsterDatas_[monsterNumbers[0]] = temp;

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(1.0f);

		//文字列の処理
		AllEventManager.GetInstance().EventTextSet(
			manager.GetNovelWindowParts().GetEventText(), EnemyTrainerData.GetInstance().name() + "は\n" 
			+ monsterDatas_[0].uniqueName_ + "を　くりだした！"
			);
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterSet()));

		//モンスターの登場演出
		{
			Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterSetEffect");
			List<Sprite> animeSprites = new List<Sprite>();
			for (int i = 0; i < sprites.Length; ++i) {
				animeSprites.Add(sprites[i]);
			}
			AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetEnemyEffectParts().GetEventSpriteRenderer(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.35f);
		}

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime() / 2);

		//画像の設定
		if (monsterDatas_[0].battleData_.HaveAbnormalType(AbnormalType.Hero)) {
			List<Sprite> sprites = new List<Sprite>();
			sprites.Add(monsterDatas_[0].tribesData_.frontDreamTex_);

			AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetEnemyMonsterParts().GetEventMonsterSprite(), sprites, new Color32());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
			AllEventManager.GetInstance().AllUpdateEventExecute();
		}
		else {
			List<Sprite> sprites = new List<Sprite>();
			sprites.Add(monsterDatas_[0].tribesData_.frontTex_);

			AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetEnemyMonsterParts().GetEventMonsterSprite(), sprites, new Color32());
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
			AllEventManager.GetInstance().AllUpdateEventExecute();
		}

		//名前とレベルをTextに反映
		string monsterViewName = t13.Utility.StringFullSpaceBackTamp(monsterDatas_[0].uniqueName_, 6);
		AllEventManager.GetInstance().EventTextSet(manager.GetEnemyStatusInfoParts().GetBaseParts().GetInfoEventText(), monsterViewName + "　　Lｖ" + t13.Utility.HarfSizeForFullSize(monsterDatas_[0].level_.ToString()));
		AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//HPをTextに反映
		//HPゲージの調整
		float hpGaugeFillAmount = t13.Utility.ValueForPercentage(monsterDatas_[0].RealHitPoint(), monsterDatas_[0].nowHitPoint_, 1);
		AllEventManager.GetInstance().HpGaugePartsSet(manager.GetEnemyStatusInfoParts().GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount, monsterDatas_[0]);
		AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//状態異常の反映
		monsterDatas_[0].battleData_.AbnormalSetStatusInfoPartsEventSet(manager.GetEnemyStatusInfoParts());

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		//エネミーの表示
		AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyMonsterParts().GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		//エネミーのステータスインフォの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyStatusInfoParts().GetEventGameObject(), new Vector3(-3.5f, manager.GetEnemyStatusInfoParts().transform.position.y, manager.GetEnemyStatusInfoParts().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		manager.ActiveUiCommand();
		manager.InactiveUiCommand();
	}

	//交換処理
	public override void MonsterChangeEventSet(BattleManager manager) {
		//モンスターの変更が行われていたら
		if (changeMonsterNumber_ > 0) {
			IMonsterData md = monsterDatas_[changeMonsterNumber_];

			//先頭のパラメーターをリセット
			monsterDatas_[0].battleData_.RankReset();

			AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetEventText(), EnemyTrainerData.GetInstance().name() + "は\n"
				+ monsterDatas_[0].uniqueName_ + "を　ひっこめた！");
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(manager.GetEventContextUpdateTime());

			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyMonsterParts().GetEventGameObject());
			AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyStatusInfoParts().GetEventGameObject(), new Vector3(-13.5f, manager.GetEnemyStatusInfoParts().transform.position.y, manager.GetEnemyStatusInfoParts().transform.position.z));
			AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);

			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			//モンスターデータの入れ替え
			IMonsterData temp = monsterDatas_[0];
			monsterDatas_[0] = monsterDatas_[changeMonsterNumber_];
			monsterDatas_[changeMonsterNumber_] = temp;

			AllEventManager.GetInstance().EventTextSet(manager.GetNovelWindowParts().GetEventText(), EnemyTrainerData.GetInstance().name() + "は\n"
				+ md.uniqueName_ + "を　くりだした！");
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
				AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetEnemyEffectParts().GetEventSpriteRenderer(), animeSprites);
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
				AllEventManager.GetInstance().AllUpdateEventExecute(0.35f);
			}

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime() / 2);

			//画像の設定
			if (monsterDatas_[0].battleData_.HaveAbnormalType(AbnormalType.Hero)) {
				List<Sprite> sprites = new List<Sprite>();
				sprites.Add(monsterDatas_[0].tribesData_.frontDreamTex_);

				AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetEnemyMonsterParts().GetEventMonsterSprite(), sprites, new Color32());
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
				AllEventManager.GetInstance().AllUpdateEventExecute();
			}
			else {
				List<Sprite> sprites = new List<Sprite>();
				sprites.Add(monsterDatas_[0].tribesData_.frontTex_);
				AllEventManager.GetInstance().EventSpriteRendererSet(manager.GetEnemyMonsterParts().GetEventMonsterSprite(), sprites, new Color32());
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
				AllEventManager.GetInstance().AllUpdateEventExecute();
			}

			//名前とレベルをTextに反映
			string monsterViewName = t13.Utility.StringFullSpaceBackTamp(monsterDatas_[0].uniqueName_, 6);
			AllEventManager.GetInstance().EventTextSet(manager.GetEnemyStatusInfoParts().GetBaseParts().GetInfoEventText(), monsterViewName + "　　Lｖ" + t13.Utility.HarfSizeForFullSize(monsterDatas_[0].level_.ToString()));
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//HPをTextに反映
			//HPゲージの調整
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(monsterDatas_[0].RealHitPoint(), monsterDatas_[0].nowHitPoint_, 1);
			AllEventManager.GetInstance().HpGaugePartsSet(manager.GetEnemyStatusInfoParts().GetFrameParts().GetHpGaugeParts(), hpGaugeFillAmount, monsterDatas_[0]);
			AllEventManager.GetInstance().HpGaugePartsUpdateExecuteSet(HpGaugePartsEventManagerExecute.GaugeUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//状態異常の反映
			md.battleData_.AbnormalSetStatusInfoPartsEventSet(manager.GetEnemyStatusInfoParts());

			//エネミーの表示
			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyMonsterParts().GetEventGameObject());
			AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

			//エネミーのステータスインフォの入場
			AllEventManager.GetInstance().UpdateGameObjectSet(manager.GetEnemyStatusInfoParts().GetEventGameObject(), new Vector3(-3.5f, manager.GetEnemyStatusInfoParts().transform.position.y, manager.GetEnemyStatusInfoParts().transform.position.z));
			AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);
		}

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(manager.GetEventWaitTime());

		changeMonsterNumber_ = 0;
	}

	public bool ThinkingTimeEnd() {
		//思考時間が終わっていたら
		if(thikingEnd_ == true) {
			thikingEnd_ = false;

			return true;
		}

		return thikingEnd_;
	}
	public void ThinkingTimeCounter() {
		float addRegulationTime = 0;
		if (monsterDatas_[0].battleData_.HaveAbnormalType(AbnormalType.Sleep)) addRegulationTime += 3.0f;
		if (monsterDatas_[0].battleData_.HaveAbnormalType(AbnormalType.Confusion)) addRegulationTime += 2.0f;

		//思考時間が終わっていなかったら
		if (thikingEnd_ == false) {
			//時間のカウント
			if (thikingTimeCounter_.measure(Time.deltaTime, THINKING_TIME_REGURATION + addRegulationTime)) {
				//思考時間の終了
				thikingEnd_ = true;
			}
		}
	}

	public bool PoinsonCounter() {
		poisonCounter_ += Time.deltaTime;

		if(poisonCounter_ > POISON_COUNTER_TIME_REGULATION) {
			poisonCounter_ = 0;

			return true;
		}

		return false;
	}

	//シングルトン
	private EnemyBattleData() { }

	static private EnemyBattleData instance_ = null;
	static public EnemyBattleData GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new EnemyBattleData();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
