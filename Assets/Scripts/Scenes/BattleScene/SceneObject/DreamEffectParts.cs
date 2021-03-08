using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamEffectParts : MonoBehaviour {
	[SerializeField] private EventSpriteRenderer screenEventSprite_ = null;
	[SerializeField] private EventSpriteRenderer monsterEventSprite_ = null;
	[SerializeField] private UpdateGameObject monsterUpdateObject_ = null;
	[SerializeField] private UpdateGameObject updateGameObject_ = null;
	[SerializeField] private EventSpriteRenderer effectEventSprite_ = null;

	public void DreamSyncronizeEventSet(IMonsterData monsterData, Vector3 monsterStartPos, Vector3 monsterEndPos) {
		AllEventManager eventManager = AllEventManager.GetInstance();
		AllSceneManager sceneManager = AllSceneManager.GetInstance();

		//表示
		eventManager.UpdateGameObjectSet(updateGameObject_);
		eventManager.UpdateGameObjectsActiveSetExecute(true);

		//モンスターの画像の設定
		{
			List<Sprite> sprites = new List<Sprite>();
			sprites.Add(monsterData.tribesData_.frontTex_);
			eventManager.EventSpriteRendererSet(monsterEventSprite_, sprites);
			eventManager.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
			eventManager.AllUpdateEventExecute();
		}

		//モンスターの初期位置への移動
		eventManager.UpdateGameObjectSet(monsterUpdateObject_, monsterStartPos);
		eventManager.UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		eventManager.AllUpdateEventExecute();

		//ウェイト
		eventManager.EventWaitSet(0.5f);

		//モンスターを中心に移動
		eventManager.UpdateGameObjectSet(monsterUpdateObject_, new Vector3(0, monsterStartPos.y, monsterStartPos.z));
		eventManager.UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		eventManager.AllUpdateEventExecute(1.0f, t13.TimeFluctProcess.Sine);

		//ウェイト
		eventManager.EventWaitSet(0.8f);

		//進化後のモンスターの画像の設定
		{
			List<Sprite> sprites = new List<Sprite>();
			sprites.Add(monsterData.tribesData_.frontDreamTex_);
			eventManager.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.SpriteSet);
			eventManager.EventSpriteRendererSet(monsterEventSprite_, sprites);
			eventManager.AllUpdateEventExecute();
		}

		//進化時SE
		eventManager.SEAudioPlayOneShotEventSet(ResourcesSoundsLoader.GetInstance().GetSounds(SoundsPathSupervisor.GetInstance().GetPathDreamEffect()));

		//進化エフェクト
		{
			Sprite[] sprites = ResourcesGraphicsLoader.GetInstance().GetGraphicsAll("BattleScene/MonsterDreamEffect");
			List<Sprite> spriteList = new List<Sprite>();
			for(int i = 0;i < sprites.Length; ++i) {
				spriteList.Add(sprites[i]);
			}
			eventManager.EventSpriteRendererSet(effectEventSprite_, spriteList);
			eventManager.EventSpriteRenderersUpdateExecuteSet(EventSpriteRendererEventManagerExecute.Anime);
			eventManager.AllUpdateEventExecute(0.5f);
		}

		//ウェイト
		eventManager.EventWaitSet(0.8f);

		//モンスターの退場
		eventManager.UpdateGameObjectSet(monsterUpdateObject_, monsterEndPos);
		eventManager.UpdateGameObjectUpdateExecuteSet(UpdateGameObjectEventManagerExecute.PosMove);
		eventManager.AllUpdateEventExecute(1.0f, t13.TimeFluctProcess.Quart);

		//ウェイト
		eventManager.EventWaitSet(0.5f);

		//非表示
		eventManager.UpdateGameObjectSet(updateGameObject_);
		eventManager.UpdateGameObjectsActiveSetExecute(false);
	}
}
