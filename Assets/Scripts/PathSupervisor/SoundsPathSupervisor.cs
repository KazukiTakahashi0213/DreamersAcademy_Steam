using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPathSupervisor {
	//シングルトン
	private SoundsPathSupervisor() { }

	static private SoundsPathSupervisor instance_ = null;
	static public SoundsPathSupervisor GetInstance() {
		if (instance_ != null) return instance_;

		instance_ = new SoundsPathSupervisor();
		return instance_;
	}
	static public void ReleaseInstance() { instance_ = null; }

	//SE
	public string GetPathCursor1() { return "SE/Cursor1"; }
	public string GetPathDreamEffect() { return "SE/DreamEffect"; }
	public string GetPathMonsterDown() { return "SE/MonsterDown"; }
	public string GetPathMonsterSet() { return "SE/MonsterSet"; }
	public string GetPathNovelNext() { return "SE/NovelNext"; }
	public string GetPathSelect1() { return "SE/Select1"; }
	public string GetPathSelect2() { return "SE/Select2"; }
	public string GetPathSelect3() { return "SE/Select3"; }

	//BGM
	public string GetPathDreamers_Battle() { return "BGM/BattleScene/Dreamers_Battle"; }
	public string GetPathDreamers_Dead() { return "BGM/BattleScene/Dreamers_Dead"; }
	public string GetPathDreamers_Lose() { return "BGM/BattleScene/Dreamers_Lose"; }
	public string GetPathDreamers_Rock() { return "BGM/BattleScene/Dreamers_Rock"; }
	public string GetPathDreamers_Win() { return "BGM/BattleScene/Dreamers_Win"; }
	public string GetPathDreamers_End() { return "BGM/EndingScene/Dreamers_End"; }
	public string GetPathDreamers_Map() { return "BGM/MapScene/Dreamers_Map"; }
	public string GetPathDreamers_Title() { return "BGM/TitleScene/Dreamers_Title"; }
}
