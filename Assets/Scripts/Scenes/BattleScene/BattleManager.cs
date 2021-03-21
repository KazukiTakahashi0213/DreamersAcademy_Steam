using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour, ISceneManager {
	//EntryPoint
	//Init
	public void SceneStart() {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//依存性注入
		allSceneMgr.inputProvider_ = new KeyBoardNormalTriggerInputProvider();
		inputSoundProvider_.state_ = BattleSceneInputSoundState.Normal;

		//エネミー、プレイヤーの位置の初期化
		playerParts_.transform.localPosition = new Vector3(13.0f, -1.4f, 5);
		enemyParts_.transform.localPosition = new Vector3(-13.5f, 3.0f, 5);

		//エネミー、プレイヤーのステータスインフォの初期化
		playerStatusInfoParts_.transform.localPosition = new Vector3(-13.5f, -4.2f, 4);
		enemyStatusInfoParts_.transform.localPosition = new Vector3(-13.5f, 3.4f, 4);
		playerDreamPointInfoParts_.GaugeReset();
		enemyDreamPointInfoParts_.GaugeReset();
		playerStatusInfoParts_.ProcessIdleEnd();
		playerMonsterParts_.ProcessIdleEnd();

		//エネミー、プレイヤーのモンスターの非表示
		playerMonsterParts_.gameObject.SetActive(false);
		enemyMonsterParts_.gameObject.SetActive(false);

		//ウィンドウの初期化
		novelWindowParts_.gameObject.SetActive(false);

		//睡眠スクリーンの初期化
		sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color = new Color(sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.r, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.g, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.b, 0);

		//コマンドパーツの初期化
		InactiveUiCommand();
		InactiveUiAttackCommand();
		InactiveUiStartCommand();
		InactiveUiMonsterTradeSelectCommand();
		commandCommandParts_.SelectReset(new Vector3(0.4f, 2.48f, -4));
		attackCommandParts_.GetCommandParts().SelectReset(new Vector3(6.77f, -1.57f, -4.0f));
		startCommandParts_.SelectReset(new Vector3(0.4f, 2.48f, -4));
		monsterTradeSelectCommandParts_.SelectReset(new Vector3(-1.73f, 0.94f, -4));

		//ゆめの文字の色の変更
		dreamCommandSprite_.color = new Color32(255, 255, 255, 255);

		//プレイヤー、エネミーの画像の設定
		playerParts_.GetEventSprite().GetSpriteRenderer().sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet0");
		enemyParts_.GetMonsterSprite().sprite = EnemyTrainerData.GetInstance().GetSprite();

		//ドリームポイントゲージの初期化
		playerDreamPointInfoParts_.gameObject.SetActive(false);
		enemyDreamPointInfoParts_.gameObject.SetActive(false);

		//エネミーモンスターの読み込み
		{
			//エネミーのモンスターの読み込み
			if (EnemyTrainerData.GetInstance().GetHaveMonsterSize() < 4) {
				for (int i = 0; i < EnemyTrainerData.GetInstance().GetHaveMonsterSize(); ++i) {
					EnemyBattleData.GetInstance().MonsterAdd(EnemyTrainerData.GetInstance().GetMonsterDatas(i));
				}
			}
			else {
				for (int i = 0; i < 3; ++i) {
					EnemyBattleData.GetInstance().MonsterAdd(EnemyTrainerData.GetInstance().GetMonsterDatas(i));
				}
			}

			//エネミーの先頭のモンスターの取得
			IMonsterData md = EnemyBattleData.GetInstance().GetMonsterDatas(0);

			//画像の設定
			enemyMonsterParts_.GetMonsterSprite().sprite = md.tribesData_.frontTex_;

			//ステータスインフォに反映
			enemyStatusInfoParts_.MonsterStatusInfoSet(md);
		}

		//プレイヤーモンスターの読み込み
		{
			//プレイヤーのモンスターの読み込み
			if (PlayerTrainerData.GetInstance().GetHaveMonsterSize() < 4) {
				for (int i = 0; i < PlayerTrainerData.GetInstance().GetHaveMonsterSize(); ++i) {
					PlayerBattleData.GetInstance().MonsterAdd(PlayerTrainerData.GetInstance().GetMonsterDatas(i));
				}
			}
			else {
				for (int i = 0; i < 3; ++i) {
					PlayerBattleData.GetInstance().MonsterAdd(PlayerTrainerData.GetInstance().GetMonsterDatas(i));
				}
			}

			//プレイヤーの先頭のモンスターの取得
			IMonsterData md = PlayerBattleData.GetInstance().GetMonsterDatas(0);

			//画像の設定
			playerMonsterParts_.GetMonsterSprite().sprite = md.tribesData_.backTex_;

			//ステータスインフォに反映
			playerStatusInfoParts_.MonsterStatusInfoSet(md);

			//攻撃技の反映
			attackCommandParts_.MonsterDataReflect(md, EnemyBattleData.GetInstance().GetMonsterDatas(0));
		}

		//初期Stateを設定
		if (!PlayerTrainerData.GetInstance().clearTutorial_) {
			nowProcessState_ = new TutorialOpeningProcess();

			//イベントのセット
			TutorialOpeningEventSet();

			PlayerTrainerData.GetInstance().clearTutorial_ = true;
		}
		else {
			nowProcessState_ = new OpeningProcess();

			//イベントのセット
			OpeningEventSet();
		}
	}
	//MainLoop
	public void SceneUpdate() {
		nowProcessState_ = nowProcessState_.Update(this);
	}

	public void SceneEnd() {
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		for(int i = 0;i < playerData.GetHaveMonsterSize(); ++i) {
			playerData.GetMonsterDatas(i).BattleDataReset();
		}
	}

	public GameObject GetGameObject() { return gameObject; }

	private const float CONFUSION_TIME = 0.2f;
	private t13.TimeCounter confusionCounter_ = new t13.TimeCounter();

	private const int POISON_DAMAGE = 5;
	private bool poisonMonsterDown_ = false;

	[SerializeField] private NovelWindowParts novelWindowParts_ = null;
	[SerializeField] private CommandParts commandCommandParts_ = null;
	[SerializeField] private AttackCommandParts attackCommandParts_ = null;
	[SerializeField] private CommandParts startCommandParts_ = null;
	[SerializeField] private CommandParts monsterTradeSelectCommandParts_ = null;
	[SerializeField] private MonsterParts playerMonsterParts_ = null;
	[SerializeField] private MonsterParts enemyMonsterParts_ = null;
	[SerializeField] private PlayerParts playerParts_ = null;
	[SerializeField] private EnemyParts enemyParts_ = null;
	[SerializeField] private StatusInfoParts playerStatusInfoParts_ = null;
	[SerializeField] private StatusInfoParts enemyStatusInfoParts_ = null;
	[SerializeField] private EffectParts playerEffectParts_ = null;
	[SerializeField] private EffectParts enemyEffectParts_ = null;
	[SerializeField] private AudioParts playerAudioParts_ = null;
	[SerializeField] private AudioParts enemyAudioParts_ = null;
	[SerializeField] private ScreenParts sleepScreenParts_ = null;
	[SerializeField] private MagazineParts playerMagazineParts_ = null;
	[SerializeField] private MagazineParts enemyMagazineParts_ = null;
	[SerializeField] private EventSpriteRenderer dreamEffectScreenEventSprite_ = null;
	[SerializeField] private DreamEffectParts dreamEffectParts_ = null;
	[SerializeField] private DreamPointInfoParts playerDreamPointInfoParts_ = null;
	[SerializeField] private DreamPointInfoParts enemyDreamPointInfoParts_ = null;
	[SerializeField] private SpriteRenderer dreamCommandSprite_ = null;
	[SerializeField] private BattleTutorialParts tutorialParts_ = null;

	public MonsterParts GetEnemyMonsterParts() { return enemyMonsterParts_; }
	public MonsterParts GetPlayerMonsterParts() { return playerMonsterParts_; }
	public StatusInfoParts GetPlayerStatusInfoParts() { return playerStatusInfoParts_; }
	public StatusInfoParts GetEnemyStatusInfoParts() { return enemyStatusInfoParts_; }
	public EffectParts GetPlayerEffectParts() { return playerEffectParts_; }
	public EffectParts GetEnemyEffectParts() { return enemyEffectParts_; }
	public NovelWindowParts GetNovelWindowParts() { return novelWindowParts_; }
	public CommandParts GetCommandCommandParts() { return commandCommandParts_; }
	public AttackCommandParts GetAttackCommandParts() { return attackCommandParts_; }
	public CommandParts GetStartCommandParts() { return startCommandParts_; }
	public CommandParts GetMonsterTradeSelectCommandParts() { return monsterTradeSelectCommandParts_; }
	public AudioParts GetPlayerAudioParts() { return playerAudioParts_; }
	public AudioParts GetEnemyAudioParts() { return enemyAudioParts_; }
	public PlayerParts GetPlayerParts() { return playerParts_; }
	public EnemyParts GetEnemyParts() { return enemyParts_; }
	public ScreenParts GetSleepScreenParts() { return sleepScreenParts_; }
	public MagazineParts GetPlayerMagazineParts() { return playerMagazineParts_; }
	public MagazineParts GetEnemyMagazineParts() { return enemyMagazineParts_; }
	public EventSpriteRenderer GetDreamEffectScreenEventSprite() { return dreamEffectScreenEventSprite_; }
	public DreamEffectParts GetDreamEffectParts() { return dreamEffectParts_; }
	public DreamPointInfoParts GetPlayerDreamPointInfoParts() { return playerDreamPointInfoParts_; }
	public DreamPointInfoParts GetEnemyDreamPointInfoParts() { return enemyDreamPointInfoParts_; }
	public SpriteRenderer GetDreamCommandSprite() { return dreamCommandSprite_; }
	public BattleTutorialParts GetTutorialParts() { return tutorialParts_; }

	public void AttackCommandSkillInfoTextSet(int number) {
		IMonsterData monsterData = PlayerBattleData.GetInstance().GetMonsterDatas(0);
		IMonsterData enemyMonsterData = EnemyBattleData.GetInstance().GetMonsterDatas(0);
		string elementSimillarContext = "";

		int simillarResult = enemyMonsterData.ElementSimillarCheckerForValue(monsterData.GetSkillDatas(number).elementType_);

		if (simillarResult == 3) elementSimillarContext = "こうかばつぐん！";
		else if (simillarResult == 2) elementSimillarContext = "いいかんじ！";
		else if (simillarResult == 1) elementSimillarContext = "いまひとつ...";
		else if (simillarResult == 0) elementSimillarContext = "こうかなし";

		attackCommandParts_.GetSkillInfoParts().GetCommandWindowText().text =
			"ぞくせい／" + monsterData.GetSkillDatas(number).elementType_.GetName() + "\n"
			+ elementSimillarContext;
	}

	public void ActiveUiCommand() {
		commandCommandParts_.gameObject.SetActive(true);

		commandCommandParts_.SelectReset(new Vector3(0.4f, 2.48f, -4));

		//dpが100以上だったら
		//if (PlayerBattleData.GetInstance().GetDreamPoint() >= 100) {
		//	novelWindowParts_.GetNovelWindowText().text =
		//		"ゆめたちが　\n"
		//		+ "きょうめいしている・・・";
		//}
		//else {
		//	string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
		//	string context_ = playerFirstMonsterName + "は　どうする？";
		//	novelWindowParts_.GetNovelWindowText().text = context_;
		//}
	}
	public void ActiveUiAttackCommand() {
		attackCommandParts_.gameObject.SetActive(true);

		attackCommandParts_.GetCommandParts().SelectReset(new Vector3(6.77f, -1.57f, -4.0f));

		AttackCommandSkillInfoTextSet(0);

		playerSelectSkillNumber_ = 0;
	}
	public void ActiveUiStartCommand() {
		startCommandParts_.gameObject.SetActive(true);

		startCommandParts_.SelectReset(new Vector3(0.4f, 2.48f, -4));
	}
	public void ActiveUiMonsterTradeSelectCommand() {
		monsterTradeSelectCommandParts_.gameObject.SetActive(true);

		monsterTradeSelectCommandParts_.SelectReset(new Vector3(-1.73f, 0.94f, -4));
	}
	public void InactiveUiCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		commandCommandParts_.gameObject.SetActive(false);
	}
	public void InactiveUiAttackCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		attackCommandParts_.gameObject.SetActive(false);
	}
	public void InactiveUiStartCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		startCommandParts_.gameObject.SetActive(false);
	}
	public void InactiveUiMonsterTradeSelectCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		monsterTradeSelectCommandParts_.gameObject.SetActive(false);
	}

	public void PoisonDamageProcess(BTrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts, MonsterParts monsterParts) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//どく状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Poison
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Poison) {
			//ダメージ
			trainerBattleData.GetMonsterDatas(0).nowHitPoint_ -= POISON_DAMAGE;
			if (trainerBattleData.GetMonsterDatas(0).nowHitPoint_ < 0) trainerBattleData.GetMonsterDatas(0).nowHitPoint_ = 0;

			//ヒットポイントのゲージの変動
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(trainerBattleData.GetMonsterDatas(0).RealHitPoint(), trainerBattleData.GetMonsterDatas(0).nowHitPoint_, 1);
			statusInfoParts.GetFrameParts().GetHpGaugeParts().ProcessStateGaugeUpdateExecute(0, t13.TimeFluctProcess.Liner, trainerBattleData.GetMonsterDatas(0), hpGaugeFillAmount);

			if (trainerBattleData.GetMonsterDatas(0).nowHitPoint_ <= 0) {
				//入力の非アクティブ
				allSceneMgr.inputProvider_ = new InactiveInputProvider();

				//アイドル状態の停止
				playerStatusInfoParts_.ProcessIdleEnd();
				playerMonsterParts_.ProcessIdleEnd();

				//UIの非表示
				InactiveUiAttackCommand();
				InactiveUiCommand();

				//ウィンドウの表示
				novelWindowParts_.gameObject.SetActive(true);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

				//モンスターが倒れた時のイベント
				trainerBattleData.MonsterDownEventSet(this);

				AllEventManager.GetInstance().EventFinishSet();

				poisonMonsterDown_ = true;
			}
		}
	}
	public bool PoisonDamageDown() {
		if (poisonMonsterDown_) {
			poisonMonsterDown_ = false;

			return true;
		}

		return poisonMonsterDown_;
	}

	public bool BurnsDamageProcess(BTrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts, MonsterParts monsterParts) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//やけど状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Burns
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Burns) {
			//やけどダメージのカウント
			if (trainerBattleData.GetMonsterDatas(0).battleData_.BurnsCounter()) {
				//ダメージ
				trainerBattleData.GetMonsterDatas(0).nowHitPoint_ -= 1;
				if (trainerBattleData.GetMonsterDatas(0).nowHitPoint_ < 0) trainerBattleData.GetMonsterDatas(0).nowHitPoint_ = 0;
			}

			//ヒットポイントのゲージの変動
			float hpGaugeFillAmount = t13.Utility.ValueForPercentage(trainerBattleData.GetMonsterDatas(0).RealHitPoint(), trainerBattleData.GetMonsterDatas(0).nowHitPoint_, 1);
			statusInfoParts.GetFrameParts().GetHpGaugeParts().ProcessStateGaugeUpdateExecute(0, t13.TimeFluctProcess.Liner, trainerBattleData.GetMonsterDatas(0), hpGaugeFillAmount);

			if (trainerBattleData.GetMonsterDatas(0).nowHitPoint_ <= 0) {
				//入力の非アクティブ
				allSceneMgr.inputProvider_ = new InactiveInputProvider();

				//アイドル状態の停止
				playerStatusInfoParts_.ProcessIdleEnd();
				playerMonsterParts_.ProcessIdleEnd();

				//UIの非表示
				InactiveUiAttackCommand();
				InactiveUiCommand();

				//ウィンドウの表示
				novelWindowParts_.gameObject.SetActive(true);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

				//モンスターが倒れた時のイベント
				trainerBattleData.MonsterDownEventSet(this);

				AllEventManager.GetInstance().EventFinishSet();

				return true;
			}
		}

		return false;
	}

	public void ConfusionProcessStart() {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//こんらん状態なら
		if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
			|| PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {
			//選択の不可
			allSceneMgr.inputProvider_ = new KeyBoardSelectInactiveTriggerInputProvider();

			//カウンターのリセット
			confusionCounter_.reset();
		}
	}
	public void ConfusionProcess() {
		//こんらん状態なら
		if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
			|| PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {
			if (confusionCounter_.measure(Time.deltaTime, CONFUSION_TIME)) {
				if (attackCommandParts_.GetCommandParts().SelectNumber() == 0) {
					attackCommandParts_.GetCommandParts().CommandSelectRight(new Vector3(3.56f, 0, 0));

					//SE
					inputSoundProvider_.RightSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
				else if (attackCommandParts_.GetCommandParts().SelectNumber() == 1) {
					attackCommandParts_.GetCommandParts().CommandSelectDown(new Vector3(0, -1.42f, 0));

					//SE
					inputSoundProvider_.DownSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
				else if (attackCommandParts_.GetCommandParts().SelectNumber() == 2) {
					attackCommandParts_.GetCommandParts().CommandSelectUp(new Vector3(0, 1.42f, 0));

					//SE
					inputSoundProvider_.UpSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
				else if (attackCommandParts_.GetCommandParts().SelectNumber() == 3) {
					attackCommandParts_.GetCommandParts().CommandSelectLeft(new Vector3(-3.56f, 0, 0));

					//SE
					inputSoundProvider_.LeftSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
			}
		}
	}
	public void ConfusionUseStart(BTrainerBattleData trainerBattleData) {
		//こんらん状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {
			//ターン数のセット
			trainerBattleData.GetMonsterDatas(0).battleData_.ConfusionTurnSeedCreate();
		}
	}
	public void ConfusionProcessUse(BTrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts) {
		//こんらん状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {

			//こんらんターンの消費
			if (trainerBattleData.GetMonsterDatas(0).battleData_.UseConfusionTurn()) {
				//状態異常の回復
				trainerBattleData.GetMonsterDatas(0).battleData_.RefreshAbnormalType(AbnormalType.Confusion);

				//StatusInfoPartsへ反映
				trainerBattleData.GetMonsterDatas(0).battleData_.AbnormalSetStatusInfoPartsEventSet(statusInfoParts);

				//メッセージ処理
				AllEventManager.GetInstance().EventTextSet(
					novelWindowParts_.GetNovelWindowEventText()
					, trainerBattleData.GetUniqueTrainerName() + trainerBattleData.GetMonsterDatas(0).uniqueName_ + "の\n"
					+ "こんらんが　とけた！"
					);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);
			}
			else {
				//メッセージ処理
				AllEventManager.GetInstance().EventTextSet(
					novelWindowParts_.GetNovelWindowEventText()
					, trainerBattleData.GetUniqueTrainerName() + trainerBattleData.GetMonsterDatas(0).uniqueName_ + "は\n"
					+ "こんらんしている"
					);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);
			}
		}
	}

	public void SleepUseStart(BTrainerBattleData trainerBattleData) {
		//ねむり状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Sleep
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Sleep) {
			//ターン数のセット
			trainerBattleData.GetMonsterDatas(0).battleData_.SleepTurnSeedCreate();
		}
	}
	public void SleepProcessUse(BTrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts) {
		//ねむり状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Sleep
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Sleep) {

			//ねむりターンの消費
			if (trainerBattleData.GetMonsterDatas(0).battleData_.UseSleepTurn()) {
				//状態異常の回復
				trainerBattleData.GetMonsterDatas(0).battleData_.RefreshAbnormalType(AbnormalType.Sleep);

				//StatusInfoPartsへ反映
				trainerBattleData.GetMonsterDatas(0).battleData_.AbnormalSetStatusInfoPartsEventSet(statusInfoParts);

				//フェードイン
				AllEventManager.GetInstance().EventSpriteRendererSet(sleepScreenParts_.GetEventScreenSprite(), null, new Color(sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.r, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.g, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.b, 0));
				AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
				AllEventManager.GetInstance().AllUpdateEventExecute(1.0f);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

				//メッセージ処理
				AllEventManager.GetInstance().EventTextSet(
					novelWindowParts_.GetNovelWindowEventText()
					, trainerBattleData.GetUniqueTrainerName() + trainerBattleData.GetMonsterDatas(0).uniqueName_ + "は\n"
					+ "めをさました！"
					);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);
			}
			else {
				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

				//メッセージ処理
				AllEventManager.GetInstance().EventTextSet(
					novelWindowParts_.GetNovelWindowEventText()
					, trainerBattleData.GetUniqueTrainerName() + trainerBattleData.GetMonsterDatas(0).uniqueName_ + "は\n"
					+ "ねむっている"
					);
				AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
				AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);

				//ウェイト
				AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);
			}
		}
	}
	public void SleepProcessStart() {
		//ねむり状態なら
		if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Sleep
			|| PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Sleep) {
			//フェードアウト
			AllEventManager.GetInstance().EventSpriteRendererSet(sleepScreenParts_.GetEventScreenSprite(), null, new Color(sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.r, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.g, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.b, 1));
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute(1.0f);
		}
	}
	public void SleepProcessEnd() {
		//ねむり状態なら
		if (PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Sleep
			|| PlayerBattleData.GetInstance().GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Sleep) {

			//フェードイン
			AllEventManager.GetInstance().EventSpriteRendererSet(sleepScreenParts_.GetEventScreenSprite(), null, new Color(sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.r, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.g, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.b, 0));
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute(1.0f);
		}
	}

	//ステート
	private IProcessState nowProcessState_;
	private NovelWindowPartsActiveState novelWindowPartsActiveState_ = new NovelWindowPartsActiveState(NovelWindowPartsActive.Active);
	private BattleSceneInputSoundProvider inputSoundProvider_ = new BattleSceneInputSoundProvider();
	public IProcessState nowProcessState() { return nowProcessState_; }
	public NovelWindowPartsActiveState GetNovelWindowPartsActiveState() { return novelWindowPartsActiveState_; }
	public BattleSceneInputSoundProvider GetInputSoundProvider() { return inputSoundProvider_; }

	public int playerSelectSkillNumber_ { get; set; }
	public int enemySelectSkillNumber_ { get; set; }

	[SerializeField] private float eventContextUpdateTime_ = 0.4f;
	[SerializeField] private float eventWaitTime_ = 0.8f;
	public float GetEventContextUpdateTime() { return eventContextUpdateTime_ * AllSceneManager.GetInstance().GetBattleEffectSpeed(); }
	public float GetEventWaitTime() { return eventWaitTime_ * AllSceneManager.GetInstance().GetBattleEffectSpeed(); }

	void OpeningEventSet() {
		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		//フェードイン
		AllEventManager.GetInstance().EventSpriteRendererSet(AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite(), null, new Color(0, 0, 0, 0));
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		//プレイヤーとエネミーの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyParts_.GetEventGameObject(), new Vector3(3.5f, enemyParts_.GetEventGameObject().transform.position.y, enemyParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectSet(playerParts_.GetEventGameObject(), new Vector3(-4.5f, playerParts_.GetEventGameObject().transform.position.y, playerParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);

		AllEventManager.GetInstance().AllUpdateEventExecute(2.0f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		//ウィンドウの表示
		AllEventManager.GetInstance().UpdateGameObjectSet(novelWindowParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//プレイヤーのマガジンの出現
		AllEventManager.GetInstance().EventSpriteRendererSet(
			playerMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
			);
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		//エネミーのマガジンの出現
		AllEventManager.GetInstance().EventSpriteRendererSet(
			enemyMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
			);
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		for (int i = 0;i < 3; ++i) {
			//プレイヤーのマガジンの保有演出
			if(i < PlayerBattleData.GetInstance().GetHaveMonsterSize()) {
				AllEventManager.GetInstance().EventSpriteRendererSet(
					playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
					, null
					, new Color(playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
					);
			}

			//エネミーのマガジンの保有演出
			if (i < EnemyBattleData.GetInstance().GetHaveMonsterSize()) {
				AllEventManager.GetInstance().EventSpriteRendererSet(
					enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
					, null
					, new Color(enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
					);
			}

			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.25f);
		}

		{
			//文字列の設定
			EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
			string context = enemyTrainerData.GetJob() + "の　" + enemyTrainerData.GetName() + "が\nしょうぶを　しかけてきた！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);
		}

		//Blinkの開始
		AllEventManager.GetInstance().EventSpriteRendererSet(novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//Enterの押下待ち
		AllEventManager.GetInstance().EventTriggerSet();

		//Blinkの終了
		AllEventManager.GetInstance().EventSpriteRendererSet(novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		{
			//文字列の設定
			string enemyFirstMonsterName = EnemyBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
			string context = enemyTrainerData.GetJob() + "の　" + enemyTrainerData.GetName() + "は\n" + enemyFirstMonsterName + "を　くりだした！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);
		}

		//エネミーのマガジンの消滅
		AllEventManager.GetInstance().EventSpriteRendererSet(
			enemyMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
			);

		//エネミーのマガジンの消滅演出
		for (int i = 0; i < EnemyBattleData.GetInstance().GetHaveMonsterSize(); ++i) {
			AllEventManager.GetInstance().EventSpriteRendererSet(
				enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
				, null
				, new Color(enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
				);
		}
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//エネミーの退場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyParts_.GetEventGameObject(), new Vector3(3.5f + 9.5f, enemyParts_.GetEventGameObject().transform.position.y, enemyParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterSet()));

		//モンスターの登場演出
		{
			Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterSetEffect");
			List<Sprite> animeSprites = new List<Sprite>();
			for (int i = 0; i < sprites.Length; ++i) {
				animeSprites.Add(sprites[i]);
			}
			AllEventManager.GetInstance().EventSpriteRendererSet(enemyEffectParts_.GetEventSpriteRenderer(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		}

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		//エネミーモンスターの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyMonsterParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//エネミーモンスターのインフォメーションの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyStatusInfoParts_.GetEventGameObject(), new Vector3(-3.5f, enemyStatusInfoParts_.GetEventGameObject().transform.position.y, enemyStatusInfoParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.4f);

		//エネミーのDPゲージの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyDreamPointInfoParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		{
			//文字列の設定
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context = "ゆけっ！　" + playerFirstMonsterName + "！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);
		}

		//プレイヤーのマガジンの消滅
		AllEventManager.GetInstance().EventSpriteRendererSet(
			playerMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
			);

		//プレイヤーのマガジンの消滅演出
		for (int i = 0; i < PlayerBattleData.GetInstance().GetHaveMonsterSize(); ++i) {
			AllEventManager.GetInstance().EventSpriteRendererSet(
				playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
				, null
				, new Color(playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
				);
		}
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//プレイヤーの退場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerParts_.GetEventGameObject(), new Vector3(-4.5f - 9.5f, playerParts_.GetEventGameObject().transform.position.y, playerParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);

		//プレイヤーのアニメーション
		{
			List<Sprite> animeSprites = new List<Sprite>();
			animeSprites.Add(ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet1"));
			animeSprites.Add(ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet2"));
			AllEventManager.GetInstance().EventSpriteRendererSet(playerParts_.GetEventSprite(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		}

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterSet()));

		//モンスターの登場演出
		{
			Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterSetEffect");
			List<Sprite> animeSprites = new List<Sprite>();
			for (int i = 0; i < sprites.Length; ++i) {
				animeSprites.Add(sprites[i]);
			}
			AllEventManager.GetInstance().EventSpriteRendererSet(playerEffectParts_.GetEventSpriteRenderer(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		}

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		//プレイヤーモンスターの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerMonsterParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//プレイヤーモンスターのインフォメーションの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerStatusInfoParts_.GetEventGameObject(), new Vector3(-6.12f, playerStatusInfoParts_.GetEventGameObject().transform.position.y, playerStatusInfoParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.4f);

		//プレイヤーのDPゲージの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerDreamPointInfoParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		//ウィンドウの非表示
		AllEventManager.GetInstance().UpdateGameObjectSet(novelWindowParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

		//コマンドの選択肢とカーソルの出現
		AllEventManager.GetInstance().UpdateGameObjectSet(startCommandParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();
	}
	void TutorialOpeningEventSet() {
		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().blinkTimeRegulation_ = 0.5f;
		novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite().GetBlinkState().state_ = UpdateSpriteRendererProcessBlink.In;

		tutorialParts_.TutorialReset();

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		//フェードイン
		AllEventManager.GetInstance().EventSpriteRendererSet(AllSceneManager.GetInstance().GetPublicFrontScreen().GetEventScreenSprite(), null, new Color(0, 0, 0, 0));
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
		//プレイヤーとエネミーの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyParts_.GetEventGameObject(), new Vector3(3.5f, enemyParts_.GetEventGameObject().transform.position.y, enemyParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectSet(playerParts_.GetEventGameObject(), new Vector3(-4.5f, playerParts_.GetEventGameObject().transform.position.y, playerParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);

		AllEventManager.GetInstance().AllUpdateEventExecute(2.0f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		//ウィンドウの表示
		AllEventManager.GetInstance().UpdateGameObjectSet(novelWindowParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		//チュートリアルの表示
		AllEventManager.GetInstance().UpdateGameObjectSet(tutorialParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//押下待ち
		AllEventManager.GetInstance().EventTriggerSet();
	}
	public void TutorialOpeningEventSetNext() {
		//チュートリアルの非表示
		AllEventManager.GetInstance().UpdateGameObjectSet(tutorialParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		//プレイヤーのマガジンの出現
		AllEventManager.GetInstance().EventSpriteRendererSet(
			playerMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
			);
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		//エネミーのマガジンの出現
		AllEventManager.GetInstance().EventSpriteRendererSet(
			enemyMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
			);
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		AllEventManager.GetInstance().AllUpdateEventExecute(0.2f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		for (int i = 0; i < 3; ++i) {
			//プレイヤーのマガジンの保有演出
			if (i < PlayerBattleData.GetInstance().GetHaveMonsterSize()) {
				AllEventManager.GetInstance().EventSpriteRendererSet(
					playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
					, null
					, new Color(playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
					);
			}

			//エネミーのマガジンの保有演出
			if (i < EnemyBattleData.GetInstance().GetHaveMonsterSize()) {
				AllEventManager.GetInstance().EventSpriteRendererSet(
					enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
					, null
					, new Color(enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 1)
					);
			}

			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute(0.25f);
		}

		{
			//文字列の設定
			EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
			string context = enemyTrainerData.GetJob() + "の　" + enemyTrainerData.GetName() + "が\nしょうぶを　しかけてきた！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);
		}

		//Blinkの開始
		AllEventManager.GetInstance().EventSpriteRendererSet(novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkStart);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		//Enterの押下待ち
		AllEventManager.GetInstance().EventTriggerSet();

		//Blinkの終了
		AllEventManager.GetInstance().EventSpriteRendererSet(novelWindowParts_.GetNovelBlinkIconParts().GetNovelBlinkIconEventSprite());
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.BlinkEnd);
		AllEventManager.GetInstance().AllUpdateEventExecute();

		{
			//文字列の設定
			string enemyFirstMonsterName = EnemyBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			EnemyTrainerData enemyTrainerData = EnemyTrainerData.GetInstance();
			string context = enemyTrainerData.GetJob() + "の　" + enemyTrainerData.GetName() + "は\n" + enemyFirstMonsterName + "を　くりだした！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);
		}

		//エネミーのマガジンの消滅
		AllEventManager.GetInstance().EventSpriteRendererSet(
			enemyMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
			);

		//エネミーのマガジンの消滅演出
		for (int i = 0; i < EnemyBattleData.GetInstance().GetHaveMonsterSize(); ++i) {
			AllEventManager.GetInstance().EventSpriteRendererSet(
				enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
				, null
				, new Color(enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, enemyMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
				);
		}
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//エネミーの退場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyParts_.GetEventGameObject(), new Vector3(3.5f + 9.5f, enemyParts_.GetEventGameObject().transform.position.y, enemyParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterSet()));

		//モンスターの登場演出
		{
			Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterSetEffect");
			List<Sprite> animeSprites = new List<Sprite>();
			for (int i = 0; i < sprites.Length; ++i) {
				animeSprites.Add(sprites[i]);
			}
			AllEventManager.GetInstance().EventSpriteRendererSet(enemyEffectParts_.GetEventSpriteRenderer(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		}

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		//エネミーモンスターの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyMonsterParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//エネミーモンスターのインフォメーションの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyStatusInfoParts_.GetEventGameObject(), new Vector3(-3.5f, enemyStatusInfoParts_.GetEventGameObject().transform.position.y, enemyStatusInfoParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.4f);

		//エネミーのDPゲージの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(enemyDreamPointInfoParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		{
			//文字列の設定
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context = "ゆけっ！　" + playerFirstMonsterName + "！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute(eventContextUpdateTime_);
		}

		//プレイヤーのマガジンの消滅
		AllEventManager.GetInstance().EventSpriteRendererSet(
			playerMagazineParts_.GetMagazineEventSpriteRenderer()
			, null
			, new Color(playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMagazineEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
			);

		//プレイヤーのマガジンの消滅演出
		for (int i = 0; i < PlayerBattleData.GetInstance().GetHaveMonsterSize(); ++i) {
			AllEventManager.GetInstance().EventSpriteRendererSet(
				playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer()
				, null
				, new Color(playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.r, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.g, playerMagazineParts_.GetMonsterSDsParts(i).GetMonsterSDEventSpriteRenderer().GetSpriteRenderer().color.b, 0)
				);
		}
		AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//プレイヤーの退場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerParts_.GetEventGameObject(), new Vector3(-4.5f - 9.5f, playerParts_.GetEventGameObject().transform.position.y, playerParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);

		//プレイヤーのアニメーション
		{
			List<Sprite> animeSprites = new List<Sprite>();
			animeSprites.Add(ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet1"));
			animeSprites.Add(ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet2"));
			AllEventManager.GetInstance().EventSpriteRendererSet(playerParts_.GetEventSprite(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		}

		//SE
		AllEventManager.GetInstance().SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathMonsterSet()));

		//モンスターの登場演出
		{
			Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterSetEffect");
			List<Sprite> animeSprites = new List<Sprite>();
			for (int i = 0; i < sprites.Length; ++i) {
				animeSprites.Add(sprites[i]);
			}
			AllEventManager.GetInstance().EventSpriteRendererSet(playerEffectParts_.GetEventSpriteRenderer(), animeSprites);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
		}

		AllEventManager.GetInstance().AllUpdateEventExecute(0.8f);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_ / 2);

		//プレイヤーモンスターの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerMonsterParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//プレイヤーモンスターのインフォメーションの入場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerStatusInfoParts_.GetEventGameObject(), new Vector3(-6.12f, playerStatusInfoParts_.GetEventGameObject().transform.position.y, playerStatusInfoParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.4f);

		//プレイヤーのDPゲージの登場
		AllEventManager.GetInstance().UpdateGameObjectSet(playerDreamPointInfoParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		//ウィンドウの非表示
		AllEventManager.GetInstance().UpdateGameObjectSet(novelWindowParts_.GetUpdateGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(false);

		//コマンドの選択肢とカーソルの出現
		AllEventManager.GetInstance().UpdateGameObjectSet(startCommandParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);

		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();
	}
}
