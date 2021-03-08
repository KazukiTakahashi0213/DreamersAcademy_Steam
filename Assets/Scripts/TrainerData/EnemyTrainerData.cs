using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrainerData {
	public void SetTrainerData(string job, string name, Sprite sprite) {
		job_ = job;
		name_ = name;
		sprite_ = sprite;
	}

	public string GetJob() { return job_; }
	public string GetName() { return name_; }
	public Sprite GetSprite() { return sprite_; }

	private string job_ = " ";
	private string name_ = " ";
	private Sprite sprite_ = null;

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
