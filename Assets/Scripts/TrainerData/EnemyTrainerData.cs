using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrainerData {
	public void SetTrainerData(ResourcesEnemyTrainerData data) {
		number_ = data.trainerNumber_;
		name_ = data.trainerName_;
		job_ = data.jobName_;

		sprite_ = ResourcesGraphicsLoader.GetInstance().GetGraphics("Enemy/" + data.texName_);

		attackRate_ = data.attackRate_;
		tradeRate_ = data.tradeRate_;

		for(int i = 0;i < data.monsterDatas_.Length; ++i) {
			MonsterData monsterData = new MonsterData(new MonsterTribesData(data.monsterDatas_[i].monsterName_), 0, 50);

			//技の習得
			for (int j = 0; j < data.monsterDatas_[i].skillNames_.Length; ++j) {
				monsterData.SkillAdd(new SkillData(data.monsterDatas_[i].skillNames_[j]));
			}

			//モンスターの追加
			MonsterAdd(monsterData);
		}
	}

	public int GetNumber() { return number_; }
	public string GetName() { return name_; }
	public string GetJob() { return job_; }
	public Sprite GetSprite() { return sprite_; }

	private int number_ = 0;
	private string name_ = " ";
	private string job_ = " ";
	private Sprite sprite_ = null;

	public int GetAttackRate() { return attackRate_; }
	public int GetTradeRate() { return tradeRate_; }

	private int attackRate_ = 50;
	private int tradeRate_ = 50;

	public void MonsterAdd(IMonsterData addMonster) {
		if (haveMonsterSize_ == MONSTER_MAX_SIZE) return;

		monsterDatas_[haveMonsterSize_] = addMonster;
		haveMonsterSize_ += 1;
	}

	public IMonsterData GetMonsterDatas(int num) { return monsterDatas_[num]; }
	public int GetMonsterDatasLength() { return monsterDatas_.Length; }
	public int GetHaveMonsterSize() { return haveMonsterSize_; }

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

	//シングルトン
	private EnemyTrainerData() { }

	static private EnemyTrainerData instance_ = null;
	static public EnemyTrainerData GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new EnemyTrainerData();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
