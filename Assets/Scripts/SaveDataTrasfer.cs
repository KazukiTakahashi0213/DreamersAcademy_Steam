using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameSaveData {
	public int[] haveMonsterNumber_ = new int[6];
	public int[,] haveMonsterSkillNumber_ = new int[6, 4];
	public int[] haveSkillNumber_ = new int[256];

	public int nowMapFloor_;
	public int clearTimes_;
	public int clearMapFloor_;
	public bool clearTutorial_;
}

public class SaveDataTrasfer {
	public void DataSave() {
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		var SavePath = Application.persistentDataPath + "/DreamersAcademy.save";

		Debug.Log("path: " + SavePath);

		// iOSでは下記設定を行わないとエラーになる
		#if UNITY_IPHONE
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif

		// 保存
		GameSaveData save = new GameSaveData();

		//モンスターのデータ
		for(int i = 0;i < playerData.GetHaveMonsterSize(); ++i) {
			//モンスターのデータの保存
			save.haveMonsterNumber_[i] = playerData.GetMonsterDatas(i).tribesData_.monsterNumber_;

			//モンスターのデータの技の保存
			save.haveMonsterSkillNumber_[i, 0] = playerData.GetMonsterDatas(i).GetSkillDatas(0).skillNumber_;
			save.haveMonsterSkillNumber_[i, 1] = playerData.GetMonsterDatas(i).GetSkillDatas(1).skillNumber_;
			save.haveMonsterSkillNumber_[i, 2] = playerData.GetMonsterDatas(i).GetSkillDatas(2).skillNumber_;
			save.haveMonsterSkillNumber_[i, 3] = playerData.GetMonsterDatas(i).GetSkillDatas(3).skillNumber_;
		}

		//技のデータ
		for(int i = 0;i < playerData.GetHaveSkillSize(); ++i) {
			save.haveSkillNumber_[i] = playerData.GetSkillDatas(i).skillNumber_;
		}

		//マップのデータ
		save.nowMapFloor_ = playerData.nowMapFloor_;
		save.clearTimes_ = playerData.clearTimes_;
		save.clearMapFloor_ = playerData.clearMapFloor_;

		//チュートリアルのデータ
		save.clearTutorial_ = playerData.clearTutorial_;

		using (FileStream fs = new FileStream(SavePath, FileMode.Create, FileAccess.Write)) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, save);
		}
	}
	public void ContinueDataSave() {
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		var SavePath = Application.persistentDataPath + "/DreamersAcademy.save";

		Debug.Log("path: " + SavePath);

		// iOSでは下記設定を行わないとエラーになる
		#if UNITY_IPHONE
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif

		// 保存
		GameSaveData save = new GameSaveData();

		//モンスターのデータ
		for (int i = 0; i < 3; ++i) {
			//モンスターのデータの保存
			save.haveMonsterNumber_[i] = playerData.GetMonsterDatas(i).tribesData_.monsterNumber_;

			//モンスターのデータの技の保存
			save.haveMonsterSkillNumber_[i, 0] = playerData.GetMonsterDatas(i).GetSkillDatas(0).skillNumber_;
			save.haveMonsterSkillNumber_[i, 1] = playerData.GetMonsterDatas(i).GetSkillDatas(1).skillNumber_;
			save.haveMonsterSkillNumber_[i, 2] = playerData.GetMonsterDatas(i).GetSkillDatas(2).skillNumber_;
			save.haveMonsterSkillNumber_[i, 3] = playerData.GetMonsterDatas(i).GetSkillDatas(3).skillNumber_;
		}

		//技のデータ
		for (int i = 0; i < playerData.GetHaveSkillSize(); ++i) {
			save.haveSkillNumber_[i] = playerData.GetSkillDatas(i).skillNumber_;
		}

		//マップのデータ
		save.nowMapFloor_ = 0;
		save.clearTimes_ = playerData.clearTimes_ + 1;
		save.clearMapFloor_ = 0;

		//チュートリアルのデータ
		save.clearTutorial_ = playerData.clearTutorial_;

		using (FileStream fs = new FileStream(SavePath, FileMode.Create, FileAccess.Write)) {
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, save);
		}
	}
	public bool DataLoad() {
		PlayerTrainerData playerData = PlayerTrainerData.GetInstance();

		var SavePath = Application.persistentDataPath + "/DreamersAcademy.save";

		Debug.Log("path: " + SavePath);

		// iOSでは下記設定を行わないとエラーになる
		#if UNITY_IPHONE
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
		#endif

		// 読み込み
		GameSaveData load = null;
		try {
			using (FileStream fs = new FileStream(SavePath, FileMode.Open, FileAccess.Read)) {
				try {

				}
				catch (FileNotFoundException ioEx) {
					return false;
				}
				BinaryFormatter bf = new BinaryFormatter();
				load = bf.Deserialize(fs) as GameSaveData;
			}
		}
		catch (FileNotFoundException ioEx) {
			return false;
		}

		//モンスターのデータ
		for (int i = 0; i < load.haveMonsterNumber_.Length; ++i) {
			if (load.haveMonsterNumber_[i] == 0) break;

			//モンスターのデータの読み込み
			playerData.MonsterAdd(new MonsterData(new MonsterTribesData((MonsterTribesDataNumber)load.haveMonsterNumber_[i]), 0, 50));

			//モンスターのデータの技の保存
			playerData.GetMonsterDatas(i).SkillAdd(new SkillData((SkillDataNumber)load.haveMonsterSkillNumber_[i, 0]));
			playerData.GetMonsterDatas(i).SkillAdd(new SkillData((SkillDataNumber)load.haveMonsterSkillNumber_[i, 1]));
			playerData.GetMonsterDatas(i).SkillAdd(new SkillData((SkillDataNumber)load.haveMonsterSkillNumber_[i, 2]));
			playerData.GetMonsterDatas(i).SkillAdd(new SkillData((SkillDataNumber)load.haveMonsterSkillNumber_[i, 3]));
		}

		//技のデータ
		for (int i = 0; i < load.haveSkillNumber_.Length; ++i) {
			if (load.haveSkillNumber_[i] == 0) break;

			playerData.SkillAdd(new SkillData((SkillDataNumber)load.haveSkillNumber_[i]));
		}

		//マップのデータ
		playerData.nowMapFloor_ = load.nowMapFloor_;
		playerData.clearTimes_ = load.clearTimes_;
		playerData.clearMapFloor_ = load.clearMapFloor_;

		//チュートリアルのデータ
		playerData.clearTutorial_ = load.clearTutorial_;

		return true;
	}

	//シングルトン
	private SaveDataTrasfer() { }
	static private SaveDataTrasfer instance_ = null;
	static public SaveDataTrasfer GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new SaveDataTrasfer();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
