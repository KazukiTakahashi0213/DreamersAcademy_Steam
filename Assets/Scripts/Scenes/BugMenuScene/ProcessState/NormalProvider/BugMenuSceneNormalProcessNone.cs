using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMenuSceneNormalProcessNone : BBugMenuSceneProcessState {
	public override BugMenuSceneProcess Update(BugMenuManager bugMenuManager) {
		return BugMenuSceneProcess.None;
	}
}
