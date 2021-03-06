using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandWindowParts : MonoBehaviour {
	[SerializeField] private SpriteRenderer commandWindowSprite_ = null;
	[SerializeField] private Text commandWindowText_ = null;
	[SerializeField] private UpdateGameObject eventGameObject_ = null;

	public SpriteRenderer GetCommandWindowSprite() { return commandWindowSprite_; }
	public Text GetCommandWindowText() { return commandWindowText_; }
	public UpdateGameObject GetEventGameObject() { return eventGameObject_; }
}
