using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BugMenuSceneProcess {
	None
	, SkillSelect
	, Max
}

public class BBugMenuSceneProcessStateProvider {
	public BugMenuSceneProcess state_ = BugMenuSceneProcess.None;

	protected List<BBugMenuSceneProcessState> states_ = new List<BBugMenuSceneProcessState>();

	public BugMenuSceneProcess Update(BugMenuManager bugMenuManager) { return states_[(int)state_].Update(bugMenuManager); }

	public virtual void init(BugMenuManager bugMenuManager) { }
}
