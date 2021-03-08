using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommandSelectProcess : IProcessState {
	private bool eventEnd_ = false;

	public IProcessState BackProcess() {
		return this;
	}

	public IProcessState NextProcess() {
		return new CommandEventSetProcess();
	}

	public IProcessState Update(BattleManager mgr) {
		if (AllEventManager.GetInstance().EventUpdate()) {
			eventEnd_ = true;
		}

		//敵の思考時間の処理
		EnemyBattleData.GetInstance().ThinkingTimeCounter();

		//思考時間が終わっていたら
		if (EnemyBattleData.GetInstance().ThinkingTimeEnd() 
			&& eventEnd_) {
			//タイプ相性の測定
			int[] typeSimillarResult = new int[3] { 0, 0, 0 };
			int[] monsterNumbers = new int[3] { 0, 1, 2 };

			for (int i = 0; i < EnemyBattleData.GetInstance().GetMonsterDatasLength(); ++i) {
				//戦えたら、None以外だったら
				if (EnemyBattleData.GetInstance().GetMonsterDatas(i).battleActive_
					&& EnemyBattleData.GetInstance().GetMonsterDatas(i).tribesData_.monsterNumber_ != 0) {
					{
						int simillarResult = PlayerBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarCheckerForValue(EnemyBattleData.GetInstance().GetMonsterDatas(i).tribesData_.firstElement_);

						typeSimillarResult[i] += simillarResult;
					}
					{
						int simillarResult = PlayerBattleData.GetInstance().GetMonsterDatas(0).ElementSimillarCheckerForValue(EnemyBattleData.GetInstance().GetMonsterDatas(i).tribesData_.secondElement_);

						typeSimillarResult[i] += simillarResult;
					}
				}
			}

			//先頭のモンスターの相性が悪かったら
			if (typeSimillarResult[0] < 4) {
				t13.Utility.SimpleHiSort2Index(typeSimillarResult, monsterNumbers);

				EnemyBattleData.GetInstance().changeMonsterNumber_ = monsterNumbers[0];

				if (EnemyBattleData.GetInstance().changeMonsterNumber_ > 0) {
					EnemyBattleData.GetInstance().changeMonsterActive_ = true;
				}
			}

			//気まぐれで変化
			//2/10の確立
			if (AllSceneManager.GetInstance().GetRandom().Next(0, 10) < 2) {
				EnemyBattleData.GetInstance().changeMonsterNumber_ = AllSceneManager.GetInstance().GetRandom().Next(1, EnemyBattleData.GetInstance().GetHaveMonsterSize());

				if (EnemyBattleData.GetInstance().GetMonsterDatas(EnemyBattleData.GetInstance().changeMonsterNumber_).battleActive_
					&& EnemyBattleData.GetInstance().GetMonsterDatas(EnemyBattleData.GetInstance().changeMonsterNumber_).tribesData_.monsterNumber_ != (int)MonsterTribesDataNumber.None) {
					EnemyBattleData.GetInstance().changeMonsterActive_ = true;
				}
				else {
					EnemyBattleData.GetInstance().changeMonsterActive_ = false;
				}
			}

			//交換していなかったら
			if (EnemyBattleData.GetInstance().changeMonsterActive_ == false) {

				//現在、場に出ているモンスターのデータの取得
				IMonsterData enemyMD = EnemyBattleData.GetInstance().GetMonsterDatas(EnemyBattleData.GetInstance().changeMonsterNumber_);
				IMonsterData playerMD = PlayerBattleData.GetInstance().GetMonsterDatas(0);

				const int EFFECT_ATTACK_SIZE = 4;
				int nowEffectAttackSize = 0;
				int[] skillDamages = new int[EFFECT_ATTACK_SIZE] {
					0,0,0,0,
				};
				int[] skillNumbers = new int[EFFECT_ATTACK_SIZE] {
					0,0,0,0,
				};

				//攻撃技の威力計算
				for (int i = 0; i < enemyMD.GetSkillSize(); ++i) {
					//サポート技だったら処理を飛ばす
					if (enemyMD.GetSkillDatas(i).effectType_.state_ == EffectType.Support) continue;

					skillDamages[nowEffectAttackSize] = MonsterData.TestDamageCalculate(enemyMD, playerMD, enemyMD.GetSkillDatas(i));
					skillNumbers[nowEffectAttackSize] = i;
					nowEffectAttackSize += 1;
				}

				//ダメージ量を大きい順にソート
				for (int i = 0; i < nowEffectAttackSize - 1; ++i) {
					for (int j = i + 1; j < nowEffectAttackSize; ++j) {
						if (skillDamages[i] < skillDamages[j]) {
							{
								int tmp = skillDamages[i];
								skillDamages[i] = skillDamages[j];
								skillDamages[j] = tmp;
							}
							{
								int tmp = skillNumbers[i];
								skillNumbers[i] = skillNumbers[j];
								skillNumbers[j] = tmp;
							}
						}
					}
				}

				//ppがあれば、一番の火力の高い技を選択
				for (int i = 0; i < skillNumbers.Length; ++i) {
					if (enemyMD.GetSkillDatas(skillNumbers[i]).nowPlayPoint_ > 0) {
						mgr.enemySelectSkillNumber_ = skillNumbers[i];
						i = skillNumbers.Length;
					}
				}

				//こんらん状態であれば
				if (enemyMD.battleData_.HaveAbnormalType(AbnormalType.Confusion)) {
					//2/10の確立
					if (AllSceneManager.GetInstance().GetRandom().Next(0, 10) < 3) {
						mgr.enemySelectSkillNumber_ = AllSceneManager.GetInstance().GetRandom().Next(0, skillNumbers.Length);
					}
				}

				//気まぐれで変化
				//3/10の確立
				if (AllSceneManager.GetInstance().GetRandom().Next(0, 10) < 3) {
					mgr.enemySelectSkillNumber_ = AllSceneManager.GetInstance().GetRandom().Next(0, skillNumbers.Length);
				}

				//dpが100以上だったら
				if (EnemyBattleData.GetInstance().dreamPoint_ >= 100) {
					//パワーアップしていなかったら
					if (!enemyMD.battleData_.HaveAbnormalType(AbnormalType.Hero)) {
						//パワーアップするか否かのフラグの設定
						EnemyBattleData.GetInstance().dreamSyncronize_ = true;
					}
				}

				//ppの消費
				ISkillData enemySkillData = enemyMD.GetSkillDatas(mgr.enemySelectSkillNumber_);
				enemySkillData.nowPlayPoint_ -= 1;

				//dpが100以下だったら
				if (EnemyBattleData.GetInstance().dreamPoint_ <= 100) {
					//dpの変動
					EnemyBattleData.GetInstance().dreamPoint_ += enemySkillData.upDpValue_;
				}
			}

			eventEnd_ = false;

			return mgr.nowProcessState().NextProcess();
		}

		//やけどのダメージ処理
		if (mgr.BurnsDamageProcess(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts(), mgr.GetEnemyMonsterParts())) {
			return new CommandEventExecuteProcess();
		}

		if (EnemyBattleData.GetInstance().PoinsonCounter()) {
			//どくのダメージ処理
			mgr.PoisonDamageProcess(EnemyBattleData.GetInstance(), mgr.GetEnemyStatusInfoParts(), mgr.GetEnemyMonsterParts());
			if (mgr.PoisonDamageDown()) return new CommandEventExecuteProcess();
		}

		return this;
	}
}
