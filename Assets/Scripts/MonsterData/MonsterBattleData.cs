using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBattleData : IMonsterBattleData {
	public MonsterBattleData() {
		firstAbnormalState_ = new AbnormalTypeState(AbnormalType.None);
		secondAbnormalState_ = new AbnormalTypeState(AbnormalType.None);
	}
	public AbnormalTypeState firstAbnormalState_ { get; set; }
	public AbnormalTypeState secondAbnormalState_ { get; set; }

	private const int RankLimit_ = 6;

	private int attackParameterRank_ = 0;
	private int defenseParameterRank_ = 0;
	private int speedParameterRank_ = 0;

	private int avoidRateParameterRank_ = 0;
	private int hitRateParameterRank_ = 0;

	private const float BURNS_LIMIT = 0.5f;
	private float burnsCount_ = 0;

	private const int SLEEP_TURN_LIMIT = 7;
	private int sleepTurn_ = 0;

	private const int CONFUSION_TURN_LIMIT = 5;
	private int confusionTurn_ = 0;

	public void RankReset() {
		attackParameterRank_ = 0;
		defenseParameterRank_ = 0;
		speedParameterRank_ = 0;

		avoidRateParameterRank_ = 0;
		hitRateParameterRank_ = 0;
	}

	public void AttackParameterRankAdd(int value) {
		if (attackParameterRank_ + value > RankLimit_) {
			attackParameterRank_ = RankLimit_;
			return;
		}
		attackParameterRank_ += value;
	}
	public void DefenseParameterRankAdd(int value) {
		if (defenseParameterRank_ + value > RankLimit_) {
			defenseParameterRank_ = RankLimit_;
			return;
		}
		defenseParameterRank_ += value;
	}
	public void SpeedParameterRankAdd(int value) {
		if (speedParameterRank_ + value > RankLimit_) {
			speedParameterRank_ = RankLimit_;
			return;
		}
		speedParameterRank_ += value;
	}

	public void AvoidRateParameterRankAdd(int value) {
		if (avoidRateParameterRank_ + value > RankLimit_) {
			avoidRateParameterRank_ = RankLimit_;
			return;
		}
		avoidRateParameterRank_ += value;
	}
	public void HitRateParameterRankAdd(int value) {
		if (hitRateParameterRank_ + value > RankLimit_) {
			hitRateParameterRank_ = RankLimit_;
			return;
		}
		hitRateParameterRank_ += value;
	}

	public float RealAttackParameterRank() {
		//分子,分母
		float numerator = 2, denominator = 2;

		if (attackParameterRank_ < 0) denominator -= attackParameterRank_;
		else numerator += attackParameterRank_;

		return numerator / denominator;
	}
	public float RealDefenseParameterRank() {
		//分子,分母
		float numerator = 2, denominator = 2;

		if (defenseParameterRank_ < 0) denominator -= defenseParameterRank_;
		else numerator += defenseParameterRank_;

		return numerator / denominator;
	}
	public float RealSpeedParameterRank() {
		//分子,分母
		float numerator = 2, denominator = 2;

		if (speedParameterRank_ < 0) denominator -= speedParameterRank_;
		else numerator += speedParameterRank_;

		return numerator / denominator;
	}

	public int GetAvoidRateParameterRank() { return avoidRateParameterRank_; }
	public int GetHitRateParameterRank() { return hitRateParameterRank_; }

	public void AbnormalSetStatusInfoParts(StatusInfoParts statusInfoParts) {
		//状態異常の１つ目の表示処理
		if (firstAbnormalState_.state_ != AbnormalType.None) {
			//文字の変更
			statusInfoParts.GetFirstAbnormalStateInfoParts().GetInfoEventText().GetText().text = firstAbnormalState_.GetName();

			//表示
			//文字の色の変更
			statusInfoParts.GetFirstAbnormalStateInfoParts().GetInfoEventText().GetText().color = new Color(1, 1, 1, 1);

			//色の変更
			statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color = firstAbnormalState_.GetColor();
		}
		else {
			//非表示
			//文字の色の変更
			statusInfoParts.GetFirstAbnormalStateInfoParts().GetInfoEventText().GetText().color = new Color(1, 1, 1, 0);

			//色の変更
			statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color = new Color(statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.r, statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.g, statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.b, 0);
		}

		//状態異常の２つ目の表示処理
		if (secondAbnormalState_.state_ != AbnormalType.None) {
			//文字の変更
			statusInfoParts.GetSecondAbnormalStateInfoParts().GetInfoEventText().GetText().text = secondAbnormalState_.GetName();

			//表示
			//文字の色の変更
			statusInfoParts.GetSecondAbnormalStateInfoParts().GetInfoEventText().GetText().color = new Color(1, 1, 1, 1);

			//色の変更
			statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color = secondAbnormalState_.GetColor();
		}
		else {
			//非表示
			//文字の色の変更
			statusInfoParts.GetSecondAbnormalStateInfoParts().GetInfoEventText().GetText().color = new Color(1, 1, 1, 0);

			//色の変更
			statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color = new Color(statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.r, statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.g, statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.b, 0);
		}
	}

	public void AbnormalSetStatusInfoPartsEventSet(StatusInfoParts statusInfoParts) {
		//状態異常の１つ目の表示処理
		if (firstAbnormalState_.state_ != AbnormalType.None) {
			//文字の変更
			AllEventManager.GetInstance().EventTextSet(
				statusInfoParts.GetFirstAbnormalStateInfoParts().GetInfoEventText()
				, firstAbnormalState_.GetName()
				);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);

			AllEventManager.GetInstance().AllUpdateEventExecute();

			//表示
			//文字の色の変更
			AllEventManager.GetInstance().EventTextSet(
				statusInfoParts.GetFirstAbnormalStateInfoParts().GetInfoEventText()
				, ""
				, new Color(1, 1, 1, 1)
				);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.ChangeColor);

			//色の変更
			AllEventManager.GetInstance().EventSpriteRendererSet(
				statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite()
				, null
				, firstAbnormalState_.GetColor()
				);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

			AllEventManager.GetInstance().AllUpdateEventExecute();
		}
		else {
			//非表示
			//文字の色の変更
			AllEventManager.GetInstance().EventTextSet(
				statusInfoParts.GetFirstAbnormalStateInfoParts().GetInfoEventText()
				, ""
				, new Color(1, 1, 1, 0)
				);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.ChangeColor);

			//色の変更
			AllEventManager.GetInstance().EventSpriteRendererSet(
				statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite()
				, null
				, new Color(statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.r, statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.g, statusInfoParts.GetFirstAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.b, 0)
				);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

			AllEventManager.GetInstance().AllUpdateEventExecute();
		}

		//状態異常の２つ目の表示処理
		if (secondAbnormalState_.state_ != AbnormalType.None) {
			//文字の変更
			AllEventManager.GetInstance().EventTextSet(
				statusInfoParts.GetSecondAbnormalStateInfoParts().GetInfoEventText()
				, secondAbnormalState_.GetName()
				);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.CharaUpdate);

			AllEventManager.GetInstance().AllUpdateEventExecute();

			//表示
			//文字の色の変更
			AllEventManager.GetInstance().EventTextSet(
				statusInfoParts.GetSecondAbnormalStateInfoParts().GetInfoEventText()
				, ""
				, new Color(1, 1, 1, 1)
				);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.ChangeColor);

			//色の変更
			AllEventManager.GetInstance().EventSpriteRendererSet(
				statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite()
				, null
				, secondAbnormalState_.GetColor()
				);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

			AllEventManager.GetInstance().AllUpdateEventExecute();
		}
		else {
			//非表示
			//文字の色の変更
			AllEventManager.GetInstance().EventTextSet(
				statusInfoParts.GetSecondAbnormalStateInfoParts().GetInfoEventText()
				, ""
				, new Color(1, 1, 1, 0)
				);
			AllEventManager.GetInstance().EventTextsUpdateExecuteSet(EventTextEventManagerExecute.ChangeColor);

			//色の変更
			AllEventManager.GetInstance().EventSpriteRendererSet(
				statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite()
				, null
				, new Color(statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.r, statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.g, statusInfoParts.GetSecondAbnormalStateInfoParts().GetBaseEventSprite().GetSpriteRenderer().color.b, 0)
				);
			AllEventManager.GetInstance().EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.ChangeColor);

			AllEventManager.GetInstance().AllUpdateEventExecute();
		}
	}

	public bool BurnsCounter() {
		burnsCount_ += Time.deltaTime;

		if(burnsCount_ > BURNS_LIMIT) {
			burnsCount_ = 0;

			return true;
		}

		return false;
	}

	public void RefreshAbnormalType(AbnormalType refreshAbnormalType) {
		if(firstAbnormalState_.state_ == refreshAbnormalType) {
			firstAbnormalState_.state_ = secondAbnormalState_.state_;
			secondAbnormalState_.state_ = AbnormalType.None;
		}
		else if(secondAbnormalState_.state_ == refreshAbnormalType) {
			secondAbnormalState_.state_ = AbnormalType.None;
		}
	}
	public bool HaveAbnormalType(AbnormalType haveAbnormalType) {
		if (firstAbnormalState_.state_ == haveAbnormalType) {
			return true;
		}
		else if (secondAbnormalState_.state_ == haveAbnormalType) {
			return true;
		}

		return false;
	}

	public void SleepTurnSeedCreate() {
		//既にセットされていなかったら
		if (sleepTurn_ <= 0) {
			sleepTurn_ = AllSceneManager.GetInstance().GetRandom().Next(2, SLEEP_TURN_LIMIT + 1);
		}
	}
	public bool UseSleepTurn() {
		sleepTurn_ -= 1;

		return sleepTurn_ <= 0;
	}

	public void ConfusionTurnSeedCreate() {
		//既にセットされていなかったら
		if (confusionTurn_ <= 0) {
			confusionTurn_ = AllSceneManager.GetInstance().GetRandom().Next(2, CONFUSION_TURN_LIMIT + 1);
		}
	}
	public bool UseConfusionTurn() {
		confusionTurn_ -= 1;

		return confusionTurn_ <= 0;
	}
}
