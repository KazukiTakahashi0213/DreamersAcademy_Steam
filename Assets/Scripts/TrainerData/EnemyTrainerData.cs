using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrainerData {
	public void SetTrainerData(string job, string name, Sprite sprite) {
		job_ = job;
		name_ = name;
		sprite_ = sprite;
	}

	//get
	public string job() { return job_; }
	public string name() { return name_; }
	public Sprite GetSprite() { return sprite_; }

	private string job_ = " ";
	private string name_ = " ";
	private Sprite sprite_ = null;

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
