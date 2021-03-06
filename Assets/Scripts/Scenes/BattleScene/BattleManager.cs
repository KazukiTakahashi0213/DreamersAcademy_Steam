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

		//初期Stateを設定
		nowProcessState_ = new OpeningProcess();
		nowCommandState_ = new CommandAttack();
		nowAttackCommandState_ = new AttackCommand0();

		//エネミー、プレイヤーの位置の初期化
		playerParts_.transform.localPosition = new Vector3(13.0f, -1.4f, 5);
		enemyParts_.transform.localPosition = new Vector3(-13.5f, 3.0f, 5);

		//エネミー、プレイヤーのステータスインフォの初期化
		playerStatusInfoParts_.transform.localPosition = new Vector3(13.5f, -1.25f, 5);
		enemyStatusInfoParts_.transform.localPosition = new Vector3(-13.5f, 3.4f, 5);
		playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().fillAmount = 0;
		enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().fillAmount = 0;
		playerStatusInfoParts_.ProcessIdleEnd();
		playerMonsterParts_.ProcessIdleEnd();

		//エネミー、プレイヤーのモンスターの非表示
		playerMonsterParts_.gameObject.SetActive(false);
		enemyMonsterParts_.gameObject.SetActive(false);

		//睡眠スクリーンの初期化
		sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color = new Color(sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.r, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.g, sleepScreenParts_.GetEventScreenSprite().GetSpriteRenderer().color.b, 0);

		//コマンドパーツの初期化
		novelWindowParts_.GetCommandParts().gameObject.SetActive(false);
		novelWindowParts_.GetAttackCommandParts().gameObject.SetActive(false);
		novelWindowParts_.GetNovelWindowText().text = "";
		novelWindowParts_.GetCommandParts().GetCommandWindowTexts(1).color = new Color32(50, 50, 50, 255);

		//カーソルの初期化
		cursorParts_.gameObject.SetActive(false);

		//プレイヤー、エネミーの画像の設定
		playerParts_.GetEventSprite().GetSpriteRenderer().sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet0");
		enemyParts_.GetMonsterSprite().sprite = EnemyTrainerData.GetInstance().GetSprite();


		//エネミーモンスターの読み込み
		{
			//エネミーの先頭のモンスターの取得
			IMonsterData md = EnemyBattleData.GetInstance().GetMonsterDatas(0);

			//画像の設定
			enemyMonsterParts_.GetMonsterSprite().sprite = md.tribesData_.frontTex_;

			//ステータスインフォに反映
			enemyStatusInfoParts_.MonsterStatusInfoSet(md);
		}

		//プレイヤーモンスターの読み込み
		{
			//エネミーの先頭のモンスターの取得
			IMonsterData md = PlayerBattleData.GetInstance().GetMonsterDatas(0);

			//画像の設定
			playerMonsterParts_.GetMonsterSprite().sprite = md.tribesData_.backTex_;

			//ステータスインフォに反映
			playerStatusInfoParts_.MonsterStatusInfoSet(md);

			//技をTextに反映
			for (int i = 0; i < 4; ++i) {
				novelWindowParts_.GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().text = "　" + t13.Utility.StringFullSpaceBackTamp(md.GetSkillDatas(i).skillName_, 7);
			}
			//文字の色の変更
			for(int i = 0;i < 4; ++i) {
				if (EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) > 1.0f) {
					novelWindowParts_.GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(207, 52, 112, 255);
				}
				else if (EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) < 1.0f
					&& EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) > 0) {
					novelWindowParts_.GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(52, 130, 207, 255);
				}
				else if (EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarChecker(md.GetSkillDatas(i).elementType_) < 0.1f) {
					novelWindowParts_.GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(195, 195, 195, 255);
				}
				else {
					novelWindowParts_.GetAttackCommandParts().GetSkillParts().GetSkillEventTexts(i).GetText().color = new Color32(50, 50, 50, 255);
				}
			}
		}

		//イベントのセット
		OpeningEventSet();
	}
	//MainLoop
	public void SceneUpdate() {
		nowProcessState_ = nowProcessState_.Update(this);
	}

	public void SceneEnd() {
		PlayerBattleData playerData = PlayerBattleData.GetInstance();

		for(int i = 0;i < playerData.GetHaveMonsterSize(); ++i) {
			playerData.GetMonsterDatas(i).BattleDataReset();
		}
	}

	public GameObject GetGameObject() { return gameObject; }

	private const float CONFUSION_TIME = 0.2f;
	private t13.TimeCounter confusionCounter_ = new t13.TimeCounter();

	private const int POISON_DAMAGE = 5;
	private bool poisonMonsterDown_ = false;

	[SerializeField] private CursorParts cursorParts_ = null;
	[SerializeField] private NovelWindowParts novelWindowParts_ = null;
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

	public HpGaugeParts GetEnemyMonsterHpGauge() { return enemyStatusInfoParts_.GetFrameParts().GetHpGaugeParts(); }
	public HpGaugeParts GetPlayerMonsterHpGauge() { return playerStatusInfoParts_.GetFrameParts().GetHpGaugeParts(); }
	public MonsterParts GetEnemyMonsterParts() { return enemyMonsterParts_; }
	public MonsterParts GetPlayerMonsterParts() { return playerMonsterParts_; }
	public StatusInfoParts GetPlayerStatusInfoParts() { return playerStatusInfoParts_; }
	public StatusInfoParts GetEnemyStatusInfoParts() { return enemyStatusInfoParts_; }
	public EffectParts GetPlayerEffectParts() { return playerEffectParts_; }
	public EffectParts GetEnemyEffectParts() { return enemyEffectParts_; }
	public NovelWindowParts GetNovelWindowParts() { return novelWindowParts_; }
	public CursorParts GetCursorParts() { return cursorParts_; }
	public AudioParts GetPlayerAudioParts() { return playerAudioParts_; }
	public AudioParts GetEnemyAudioParts() { return enemyAudioParts_; }
	public PlayerParts GetPlayerParts() { return playerParts_; }
	public EnemyParts GetEnemyParts() { return enemyParts_; }
	public ScreenParts GetSleepScreenParts() { return sleepScreenParts_; }
	public MagazineParts GetPlayerMagazineParts() { return playerMagazineParts_; }
	public MagazineParts GetEnemyMagazineParts() { return enemyMagazineParts_; }
	public EventSpriteRenderer GetDreamEffectScreenEventSprite() { return dreamEffectScreenEventSprite_; }
	public DreamEffectParts GetDreamEffectParts() { return dreamEffectParts_; }

	public void AttackCommandSkillInfoTextSet(int number) {
		IMonsterData md = PlayerBattleData.GetInstance().GetMonsterDatas(0);

		string playPointContext = t13.Utility.HarfSizeForFullSize(md.GetSkillDatas(number).nowPlayPoint_.ToString()) + "／" + t13.Utility.HarfSizeForFullSize(md.GetSkillDatas(number).playPoint_.ToString());

		novelWindowParts_.GetAttackCommandParts().GetSkillInfoParts().GetCommandWindowText().text =
			"PP　　　　" + playPointContext + "\n"
			+ "わざタイプ／" + md.GetSkillDatas(number).elementType_.GetName();
	}

	public void ChangeUiAttackCommand() {
		novelWindowParts_.GetCommandParts().gameObject.SetActive(false);
		novelWindowParts_.GetNovelWindowText().gameObject.SetActive(false);

		novelWindowParts_.GetAttackCommandParts().gameObject.SetActive(true);

		t13.UnityUtil.ObjectPosMove(cursorParts_.gameObject, new Vector3(-8.4f, -3.25f, -4));

		AttackCommandSkillInfoTextSet(0);
		playerSelectSkillNumber_ = 0;

		nowAttackCommandState_ = new AttackCommand0();
	}
	public void ChangeUiCommand() {
		novelWindowParts_.GetAttackCommandParts().gameObject.SetActive(false);

		novelWindowParts_.GetCommandParts().gameObject.SetActive(true);
		novelWindowParts_.GetNovelWindowText().gameObject.SetActive(true);

		t13.UnityUtil.ObjectPosMove(cursorParts_.gameObject, new Vector3(1.7f, -3.25f, -4));

		nowCommandState_ = new CommandAttack();

		//dpが100以上だったら
		if (PlayerBattleData.GetInstance().dreamPoint_ >= 100) {
			novelWindowParts_.GetNovelWindowText().text = 
				"ゆめたちが　\n"
				+ "きょうめいしている・・・";
		}
		else {
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context_ = playerFirstMonsterName + "は　どうする？";
			novelWindowParts_.GetNovelWindowText().text = context_;
		}
	}
	public void InactiveUiAttackCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		novelWindowParts_.GetAttackCommandParts().gameObject.SetActive(false);
		cursorParts_.gameObject.SetActive(false);

		novelWindowParts_.GetNovelWindowText().gameObject.SetActive(true);
	}
	public void InactiveUiCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		novelWindowParts_.GetCommandParts().gameObject.SetActive(false);
		cursorParts_.gameObject.SetActive(false);
	}
	public void ActiveUiCommand() {
		novelWindowParts_.GetCommandParts().gameObject.SetActive(true);
		cursorParts_.gameObject.SetActive(true);

		t13.UnityUtil.ObjectPosMove(cursorParts_.gameObject, new Vector3(1.7f, -3.25f, -4));

		nowCommandState_ = new CommandAttack();

		//dpが100以上だったら
		if (PlayerBattleData.GetInstance().dreamPoint_ >= 100) {
			novelWindowParts_.GetNovelWindowText().text =
				"ゆめたちが　\n"
				+ "きょうめいしている・・・";
		}
		else {
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context_ = playerFirstMonsterName + "は　どうする？";
			novelWindowParts_.GetNovelWindowText().text = context_;
		}
	}

	public void PoisonDamageProcess(TrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts, MonsterParts monsterParts) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//どく状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Poison
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Poison) {
			//ダメージ
			trainerBattleData.GetMonsterDatas(0).nowHitPoint_ -= POISON_DAMAGE;

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

	public bool BurnsDamageProcess(TrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts, MonsterParts monsterParts) {
		AllSceneManager allSceneMgr = AllSceneManager.GetInstance();

		//やけど状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Burns
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Burns) {
			//やけどダメージのカウント
			if (trainerBattleData.GetMonsterDatas(0).battleData_.BurnsCounter()) {
				//ダメージ
				trainerBattleData.GetMonsterDatas(0).nowHitPoint_ -= 1;
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
				if (playerSelectSkillNumber_ == 0) nowAttackCommandState_ = nowAttackCommandState_.RightSelect(this);
				else if (playerSelectSkillNumber_ == 1) nowAttackCommandState_ = nowAttackCommandState_.DownSelect(this);
				else if (playerSelectSkillNumber_ == 2) nowAttackCommandState_ = nowAttackCommandState_.UpSelect(this);
				else if (playerSelectSkillNumber_ == 3) nowAttackCommandState_ = nowAttackCommandState_.LeftSelect(this);
			}
		}
	}
	public void ConfusionUseStart(TrainerBattleData trainerBattleData) {
		//こんらん状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Confusion
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Confusion) {
			//ターン数のセット
			trainerBattleData.GetMonsterDatas(0).battleData_.ConfusionTurnSeedCreate();
		}
	}
	public void ConfusionProcessUse(TrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts) {
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
					novelWindowParts_.GetEventText()
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
					novelWindowParts_.GetEventText()
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

	public void SleepUseStart(TrainerBattleData trainerBattleData) {
		//ねむり状態なら
		if (trainerBattleData.GetMonsterDatas(0).battleData_.firstAbnormalState_.state_ == AbnormalType.Sleep
			|| trainerBattleData.GetMonsterDatas(0).battleData_.secondAbnormalState_.state_ == AbnormalType.Sleep) {
			//ターン数のセット
			trainerBattleData.GetMonsterDatas(0).battleData_.SleepTurnSeedCreate();
		}
	}
	public void SleepProcessUse(TrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts) {
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
					novelWindowParts_.GetEventText()
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
					novelWindowParts_.GetEventText()
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
	public ICommandState nowCommandState_ { get; set; }
	private NovelWindowPartsActiveState novelWindowPartsActiveState_ = new NovelWindowPartsActiveState(NovelWindowPartsActive.Active);
	private BattleSceneInputSoundProvider inputSoundProvider_ = new BattleSceneInputSoundProvider();
	public IProcessState nowProcessState() { return nowProcessState_; }
	public NovelWindowPartsActiveState GetNovelWindowPartsActiveState() { return novelWindowPartsActiveState_; }
	public BattleSceneInputSoundProvider GetInputSoundProvider() { return inputSoundProvider_; }

	public void CommandUpCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(0, 0.8f, 0));
	}
	public void CommandDownCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(0, -0.8f, 0));
	}
	public void CommandRightCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(4.1f, 0, 0));
	}
	public void CommandLeftCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(-4.1f, 0, 0));
	}

	public IAttackCommandState nowAttackCommandState_ { get; set; }
	public void AttackCommandUpCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(0, 0.8f, 0));
		playerSelectSkillNumber_ -= 2;
	}
	public void AttackCommandDownCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(0, -0.8f, 0));
		playerSelectSkillNumber_ += 2;
	}
	public void AttackCommandRightCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(5.6f, 0, 0));
		playerSelectSkillNumber_ += 1;
	}
	public void AttackCommandLeftCursorMove() {
		t13.UnityUtil.ObjectPosAdd(cursorParts_.gameObject, new Vector3(-5.6f, 0, 0));
		playerSelectSkillNumber_ -= 1;
	}
	public int playerSelectSkillNumber_ { get; set; }
	public int enemySelectSkillNumber_ { get; set; }

	private const float eventContextUpdateTime_ = 0.4f;
	private const float eventWaitTime_ = 0.8f;
	public float GetEventContextUpdateTime() { return eventContextUpdateTime_; }
	public float GetEventWaitTime() { return eventWaitTime_; }

	public void PlayerEnemyStatusInfoPartsDPEffect() {
		for (int i = 0; i < 2; ++i) {
			//プレイヤーのステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				playerStatusInfoParts_.GetDPGaugeMeterUpdateImage()
				, new Color(playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.r, playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.g, playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.b, 0.3f)
				);

			//エネミーのステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage()
				, new Color(enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.r, enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.g, enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.b, 0.3f)
				);

			AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.1f);

			//プレイヤーのステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				playerStatusInfoParts_.GetDPGaugeMeterUpdateImage()
				, new Color(playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.r, playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.g, playerStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.b, 1)
				);

			//エネミーのステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage()
				, new Color(enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.r, enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.g, enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage().GetImage().color.b, 1)
				);

			AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.1f);
		}

		//プレイヤーのステータスインフォのDPの演出
		float playerEndFillAmount = t13.Utility.ValueForPercentage(100, PlayerBattleData.GetInstance().dreamPoint_, 1);
		AllEventManager.GetInstance().UpdateImageSet(
			playerStatusInfoParts_.GetDPGaugeMeterUpdateImage()
			, new Color32()
			, playerEndFillAmount
			);
		//エネミーのステータスインフォのDPの演出
		float enemyEndFillAmount = t13.Utility.ValueForPercentage(100, EnemyBattleData.GetInstance().dreamPoint_, 1);
		AllEventManager.GetInstance().UpdateImageSet(
			enemyStatusInfoParts_.GetDPGaugeMeterUpdateImage()
			, new Color32()
			, enemyEndFillAmount
			);

		AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.FillAmountUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(1.0f);
	}

	public void StatusInfoPartsDPEffectEventSet(TrainerBattleData trainerBattleData, StatusInfoParts statusInfoParts) {
		for (int i = 0; i < 2; ++i) {
			//ステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				statusInfoParts.GetDPGaugeMeterUpdateImage()
				, new Color(statusInfoParts.GetDPGaugeMeterUpdateImage().GetImage().color.r, statusInfoParts.GetDPGaugeMeterUpdateImage().GetImage().color.g, statusInfoParts.GetDPGaugeMeterUpdateImage().GetImage().color.b, 0.3f)
				);
			AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.1f);

			//ステータスインフォのDPの点滅演出
			AllEventManager.GetInstance().UpdateImageSet(
				statusInfoParts.GetDPGaugeMeterUpdateImage()
				, new Color(statusInfoParts.GetDPGaugeMeterUpdateImage().GetImage().color.r, statusInfoParts.GetDPGaugeMeterUpdateImage().GetImage().color.g, statusInfoParts.GetDPGaugeMeterUpdateImage().GetImage().color.b, 1)
				);
			AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.ChangeColor);
			AllEventManager.GetInstance().AllUpdateEventExecute();

			//ウェイト
			AllEventManager.GetInstance().EventWaitSet(0.1f);
		}

		float endFillAmount = t13.Utility.ValueForPercentage(100, trainerBattleData.dreamPoint_, 1);
		AllEventManager.GetInstance().UpdateImageSet(
			statusInfoParts.GetDPGaugeMeterUpdateImage()
			, new Color32()
			, endFillAmount
			);

		AllEventManager.GetInstance().UpdateImagesUpdateExecuteSet(UpdateImageEventManagerExecute.FillAmountUpdate);
		AllEventManager.GetInstance().AllUpdateEventExecute(1.0f);
	}

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
			string context = enemyTrainerData.job() + "の　" + enemyTrainerData.name() + "が\nしょうぶを　しかけてきた！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetEventText(), context);
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
			string context = enemyTrainerData.job() + "の　" + enemyTrainerData.name() + "は\n" + enemyFirstMonsterName + "を　くりだした！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetEventText(), context);
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

		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);

		{
			//文字列の設定
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context = "ゆけっ！　" + playerFirstMonsterName + "！";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetEventText(), context);
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
		AllEventManager.GetInstance().UpdateGameObjectSet(playerStatusInfoParts_.GetEventGameObject(), new Vector3(4.0f, playerStatusInfoParts_.GetEventGameObject().transform.position.y, playerStatusInfoParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.4f);
		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);
		{
			//文字列の設定
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context = playerFirstMonsterName + "は　どうする？";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();
		}
		//コマンドの選択肢とカーソルの出現
		AllEventManager.GetInstance().UpdateGameObjectSet(novelWindowParts_.GetCommandParts().GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectSet(cursorParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);
		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();
	}
}
