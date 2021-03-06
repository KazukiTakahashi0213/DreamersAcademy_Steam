using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovelWindowParts : MonoBehaviour {
	[SerializeField] SpriteRenderer novelWindowSprite_ = null;
	[SerializeField] Text novelWindowText_ = null;
	[SerializeField] CommandParts commandParts_ = null;
	[SerializeField] AttackCommandParts attackCommandParts_ = null;
	[SerializeField] EventText eventText_ = null;
	[SerializeField] NovelBlinkIconParts novelBlinkIconParts_ = null;
	[SerializeField] UpdateGameObject updateGameObject_ = null;

	public SpriteRenderer GetNovelWindowSprite() { return novelWindowSprite_; }
	public Text GetNovelWindowText() { return novelWindowText_; }
	public CommandParts GetCommandParts() { return commandParts_; }
	public AttackCommandParts GetAttackCommandParts() { return attackCommandParts_; }
	public EventText GetEventText() { return eventText_; }
	public NovelBlinkIconParts GetNovelBlinkIconParts() { return novelBlinkIconParts_; }
	public UpdateGameObject GetUpdateGameObject() { return updateGameObject_; }
}
