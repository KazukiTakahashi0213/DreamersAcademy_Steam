using System.Collections;
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
	public string skillNname_;

	public float effectValue_;
	public int optionEffectTriggerRateValue_;
	public int hitRateValue_;
	public int upDpValue_;

	public int playPoint_;

	public int elementType_;
	public int[] effectType_;

	public int triggerPriority_;
	public int criticalParameterRank_;

	public string effectName_;

	public ResourcesSkillAddParameterRank[] addPlayerParameterRanks_;
	public ResourcesSkillAddParameterRank[] addEnemyParameterRanks_;

	public ResourcesSkillAddAbnormal[] addPlayerAbnormals_;
	public ResourcesSkillAddAbnormal[] addEnemyAbnormals_;

	public string effectInfo_;
}

public class ResourcesSkillDatasLoader {
	List<ResourcesSkillData> resourcesSkillDatas = null;

	public ResourcesSkillData GetSkillDatas(int number) {
		if (resourcesSkillDatas != null) return resourcesSkillDatas[number];

		resourcesSkillDatas = new List<ResourcesSkillData>();

		TextAsset[] textAssets = Resources.LoadAll<TextAsset>("SkillDatas");

		for (int i = 0; i < textAssets.Length; ++i) {
			ResourcesSkillData data = JsonUtility.FromJson<ResourcesSkillData>(textAssets[i].ToString());
			resourcesSkillDatas.Add(data);
		}

		return resourcesSkillDatas[number];
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
