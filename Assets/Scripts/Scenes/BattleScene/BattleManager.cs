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
		InactiveUiCommand();
		InactiveUiAttackCommand();

		//ゆめの文字の色の変更
		commandCommandParts_.GetCommandWindowTexts(1).color = new Color32(50, 50, 50, 255);

		//プレイヤー、エネミーの画像の設定
		playerParts_.GetEventSprite().GetSpriteRenderer().sprite = ResourcesGraphicsLoader.GetInstance().GetGraphics("Player/PlayerMonsterSet0");
		enemyParts_.GetMonsterSprite().sprite = EnemyTrainerData.GetInstance().GetSprite();


		//エネミーモンスターの読み込み
		{
			//エネミーのモンスターの読み込み
			for(int i = 0;i < EnemyTrainerData.GetInstance().GetHaveMonsterSize(); ++i) {
				EnemyBattleData.GetInstance().MonsterAdd(EnemyTrainerData.GetInstance().GetMonsterDatas(i));
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
			for (int i = 0; i < PlayerTrainerData.GetInstance().GetHaveMonsterSize(); ++i) {
				PlayerBattleData.GetInstance().MonsterAdd(PlayerTrainerData.GetInstance().GetMonsterDatas(i));
			}

			//プレイヤーの先頭のモンスターの取得
			IMonsterData md = PlayerBattleData.GetInstance().GetMonsterDatas(0);

			//画像の設定
			playerMonsterParts_.GetMonsterSprite().sprite = md.tribesData_.backTex_;

			//ステータスインフォに反映
			playerStatusInfoParts_.MonsterStatusInfoSet(md);

			//技をTextに反映
			for (int i = 0; i < attackCommandParts_.GetCommandParts().GetCommandWindowTextsCount(); ++i) {
				attackCommandParts_.GetCommandParts().GetCommandWindowTexts(i).text = "　" + t13.Utility.StringFullSpaceBackTamp(md.GetSkillDatas(i).skillName_, 7);
			}

			//文字の色の変更
			for (int i = 0; i < attackCommandParts_.GetCommandParts().GetCommandWindowTextsCount(); ++i) {
				int simillarResult = EnemyBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarCheckerForValue(md.GetSkillDatas(i).elementType_);

				if (simillarResult == 0) attackCommandParts_.GetCommandParts().GetCommandWindowTexts(i).color = new Color32(195, 195, 195, 255);
				else if (simillarResult == 1) attackCommandParts_.GetCommandParts().GetCommandWindowTexts(i).color = new Color32(52, 130, 207, 255);
				else if (simillarResult == 2) attackCommandParts_.GetCommandParts().GetCommandWindowTexts(i).color = new Color32(50, 50, 50, 255);
				else if (simillarResult == 3) attackCommandParts_.GetCommandParts().GetCommandWindowTexts(i).color = new Color32(207, 52, 112, 255);
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

	[SerializeField] private NovelWindowParts novelWindowParts_ = null;
	[SerializeField] private CommandParts commandCommandParts_ = null;
	[SerializeField] private AttackCommandParts attackCommandParts_ = null;
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

	public MonsterParts GetEnemyMonsterParts() { return enemyMonsterParts_; }
	public MonsterParts GetPlayerMonsterParts() { return playerMonsterParts_; }
	public StatusInfoParts GetPlayerStatusInfoParts() { return playerStatusInfoParts_; }
	public StatusInfoParts GetEnemyStatusInfoParts() { return enemyStatusInfoParts_; }
	public EffectParts GetPlayerEffectParts() { return playerEffectParts_; }
	public EffectParts GetEnemyEffectParts() { return enemyEffectParts_; }
	public NovelWindowParts GetNovelWindowParts() { return novelWindowParts_; }
	public CommandParts GetCommandCommandParts() { return commandCommandParts_; }
	public AttackCommandParts GetAttackCommandParts() { return attackCommandParts_; }
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

		attackCommandParts_.GetSkillInfoParts().GetCommandWindowText().text =
			"PP　　　　" + playPointContext + "\n"
			+ "わざタイプ／" + md.GetSkillDatas(number).elementType_.GetName();
	}

	public void ActiveUiCommand() {
		commandCommandParts_.gameObject.SetActive(true);

		commandCommandParts_.SelectReset(new Vector3(-3.35f, 0.43f, -4));

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
	public void ActiveUiAttackCommand() {
		attackCommandParts_.gameObject.SetActive(true);

		attackCommandParts_.GetCommandParts().SelectReset(new Vector3(1.66f, -2.17f, -4.0f));

		AttackCommandSkillInfoTextSet(0);

		playerSelectSkillNumber_ = 0;
	}
	public void InactiveUiCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		commandCommandParts_.gameObject.SetActive(false);
	}
	public void InactiveUiAttackCommand() {
		novelWindowParts_.GetNovelWindowText().text = "　";

		attackCommandParts_.gameObject.SetActive(false);
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
				if (attackCommandParts_.GetCommandParts().SelectNumber() == 0) {
					attackCommandParts_.GetCommandParts().CommandSelectRight(new Vector3(5.56f, 0, 0));

					//SE
					inputSoundProvider_.RightSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
				else if (attackCommandParts_.GetCommandParts().SelectNumber() == 1) {
					attackCommandParts_.GetCommandParts().CommandSelectDown(new Vector3(0, -0.83f, 0));

					//SE
					inputSoundProvider_.DownSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
				else if (attackCommandParts_.GetCommandParts().SelectNumber() == 2) {
					attackCommandParts_.GetCommandParts().CommandSelectUp(new Vector3(0, 0.83f, 0));

					//SE
					inputSoundProvider_.UpSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
				else if (attackCommandParts_.GetCommandParts().SelectNumber() == 3) {
					attackCommandParts_.GetCommandParts().CommandSelectLeft(new Vector3(-5.56f, 0, 0));

					//SE
					inputSoundProvider_.LeftSelect();

					//どくのダメージ処理
					PoisonDamageProcess(PlayerBattleData.GetInstance(), playerStatusInfoParts_, playerMonsterParts_);
				}
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
		AllEventManager.GetInstance().UpdateGameObjectSet(playerStatusInfoParts_.GetEventGameObject(), new Vector3(4.0f, playerStatusInfoParts_.GetEventGameObject().transform.position.y, playerStatusInfoParts_.GetEventGameObject().transform.position.z));
		AllEventManager.GetInstance().UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		AllEventManager.GetInstance().AllUpdateEventExecute(0.4f);
		//ウェイト
		AllEventManager.GetInstance().EventWaitSet(eventWaitTime_);
		{
			//文字列の設定
			string playerFirstMonsterName = PlayerBattleData.GetInstance().GetMonsterDatas(0).tribesData_.monsterName_;
			string context = playerFirstMonsterName + "は　どうする？";

			AllEventManager.GetInstance().EventTextSet(novelWindowParts_.GetNovelWindowEventText(), context);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);
			AllEventManager.GetInstance().AllUpdateEventExecute();
		}
		//コマンドの選択肢とカーソルの出現
		AllEventManager.GetInstance().UpdateGameObjectSet(commandCommandParts_.GetEventGameObject());
		AllEventManager.GetInstance().UpdateGameObjectsActiveSetExecute(true);
		//イベントの最後
		AllEventManager.GetInstance().EventFinishSet();
	}
}
