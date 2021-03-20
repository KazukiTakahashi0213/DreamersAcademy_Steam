using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrainerData {
	public void MonsterAdd(IMonsterData addMonster) {
		if (haveMonsterSize_ == MONSTER_MAX_SIZE) return;

		monsterDatas_[haveMonsterSize_] = addMonster;
		haveMonsterSize_ += 1;
	}
	public void MonsterSwap(int changeNumber, int baseNumber) {
		IMonsterData temp = monsterDatas_[baseNumber];

		monsterDatas_[baseNumber] = monsterDatas_[changeNumber];
		monsterDatas_[changeNumber] = temp;
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

	//手持ちの技のデータ
	private const int SKILL_MAX_SIZE = 8;
	private int haveSkillSize_ = 0;
	private List<SkillData> skillDatas_ = new List<SkillData>();
	public int GetHaveSkillSize() { return haveSkillSize_; }
	public SkillData GetSkillDatas(int value) { return skillDatas_[value]; }
	public int GetSkillDatasCount() { return skillDatas_.Count; }

	public void SkillAdd(SkillData skillData) {
		//既に所持していたら、追加しない
		for(int i = 0;i < haveSkillSize_; ++i) {
			if(skillDatas_[i].skillNumber_ == skillData.skillNumber_) {
				return;
			}
		}

		if(haveSkillSize_ < SKILL_MAX_SIZE) {
			skillDatas_[haveSkillSize_] = skillData;
		}
		else {
			skillDatas_.Add(skillData);
		}

		haveSkillSize_ += 1;
	}

	//バトル制御
	public bool battleEnd_ = false;
	public bool battleResult_ = false;

	//コンティニュー制御
	public bool prepareContinue_ = false;

	//マップの階層制御
	public int nowMapFloor_ = 0;

	//クリア回数
	public int clearTimes_ = 0;

	//クリアしている階層
	public int clearMapFloor_ = 0;

	//チュートリアルをクリアしているか
	public bool clearTutorial_ = false;

	//シングルトン
	private PlayerTrainerData() {
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
		skillDatas_.Add(new SkillData(SkillDataNumber.None));
	}

	static private PlayerTrainerData instance_ = null;
	static public PlayerTrainerData GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new PlayerTrainerData();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
