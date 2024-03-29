﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesSkillAddParameterRank {
	public int addParameterRank_;
	public int value_;
}

[System.Serializable]
public class ResourcesSkillAddAbnormal {
	public int addAbnormal_;
}

[System.Serializable]
public class ResourcesSkillData {
	public int skillNumber_;
	public string skillName_;

	public float effectValue_;
	public int optionEffectTriggerRateValue_;
	public int successRateValue_;
	public int upDpValue_;

	public int playPoint_;

	public int elementType_;
	public int effectValueType_;

	public int triggerPriority_;
	public int criticalParameterRank_;

	public string effectName_;

	public ResourcesSkillAddParameterRank[] addSelfParameterRanks_;
	public ResourcesSkillAddParameterRank[] addOtherParameterRanks_;

	public ResourcesSkillAddAbnormal[] addSelfAbnormals_;
	public ResourcesSkillAddAbnormal[] addOtherAbnormals_;

	public string effectInfo_;
}

public class ResourcesSkillDatasLoader {
	List<ResourcesSkillData> resourcesSkillDatas = null;
	List<string> resourcesSkillDataNames_ = new List<string>();

	public ResourcesSkillData GetSkillDatas(int number) {
		if (resourcesSkillDatas != null) return resourcesSkillDatas[number];

		resourcesSkillDatas = new List<ResourcesSkillData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("SkillDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesSkillData data = JsonUtility.FromJson<ResourcesSkillData>(textAssets[i].ToString());
			data.skillNumber_ = i;
			resourcesSkillDatas.Add(data);
			resourcesSkillDataNames_.Add(data.skillName_);
		}

		return resourcesSkillDatas[number];
	}
	public ResourcesSkillData GetSkillDatas(string dataName) {
		if (resourcesSkillDatas != null) {
			for (int i = 0; i < resourcesSkillDataNames_.Count; ++i) {
				if (resourcesSkillDataNames_[i] == dataName) {
					return resourcesSkillDatas[i];
				}
			}

			return null;
		}

		resourcesSkillDatas = new List<ResourcesSkillData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("SkillDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesSkillData data = JsonUtility.FromJson<ResourcesSkillData>(textAssets[i].ToString());
			data.skillNumber_ = i;
			resourcesSkillDatas.Add(data);
			resourcesSkillDataNames_.Add(data.skillName_);
		}

		for(int i = 0;i < resourcesSkillDataNames_.Count; ++i) {
			if(resourcesSkillDataNames_[i] == dataName) {
				return resourcesSkillDatas[i];
			}
		}

		return null;
	}

	//シングルトン
	private ResourcesSkillDatasLoader() { }

	static private ResourcesSkillDatasLoader instance_ = null;
	static public ResourcesSkillDatasLoader GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new ResourcesSkillDatasLoader();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }
}
