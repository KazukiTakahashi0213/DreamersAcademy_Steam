using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateImageEventManager {
	private UpdateImageEventManagerExecuteState executeState_ = new UpdateImageEventManagerExecuteState(UpdateImageEventManagerExecute.None);

	private int updateImagesExecuteCounter_ = 0;
	private List<UpdateImage> updateImages_ = new List<UpdateImage>();
	private List<Color32> changeColorEnds_ = new List<Color32>();
	private List<float> endFillAmounts_ = new List<float>();
	private List<List<UpdateImage>> executeUpdateImages_ = new List<List<UpdateImage>>();
	private List<List<Color32>> executeChangeColorEnds_ = new List<List<Color32>>();
	private List<List<float>> executeEndFillAmounts_ = new List<List<float>>();
	private List<UpdateImageEventManagerExecute> updateImageEventManagerExecutes_ = new List<UpdateImageEventManagerExecute>();

	public UpdateImageEventManagerExecuteState GetExecuteState() { return executeState_; }

	public List<UpdateImage> GetExecuteUpdateImages() { return executeUpdateImages_[updateImagesExecuteCounter_]; }
	public List<Color32> GetExecuteChangeColorEnds() { return executeChangeColorEnds_[updateImagesExecuteCounter_]; }
	public List<float> GetExecuteEndFillAmounts() { return executeEndFillAmounts_[updateImagesExecuteCounter_]; }

	public void UpdateImageSet(UpdateImage updateImage, Color32 color, float endFillAmount) {
		updateImages_.Add(updateImage);
		changeColorEnds_.Add(color);
		endFillAmounts_.Add(endFillAmount);
	}
	public void UpdateImageExecuteSet(UpdateImageEventManagerExecute setExecute = UpdateImageEventManagerExecute.None) {
		List<UpdateImage> addUpdateImages = new List<UpdateImage>();
		List<Color32> addColor32s = new List<Color32>();
		List<float> addFillAmounts = new List<float>();

		for (int i = 0; i < updateImages_.Count; ++i) {
			addUpdateImages.Add(updateImages_[i]);
			addColor32s.Add(changeColorEnds_[i]);
			addFillAmounts.Add(endFillAmounts_[i]);
		}

		executeUpdateImages_.Add(addUpdateImages);
		executeChangeColorEnds_.Add(addColor32s);
		executeEndFillAmounts_.Add(addFillAmounts);
		updateImageEventManagerExecutes_.Add(setExecute);

		updateImages_.Clear();
		changeColorEnds_.Clear();
		endFillAmounts_.Clear();
	}

	public void UpdateImagesUpdateExecute(float timeRegulation, t13.TimeFluctProcess timeFluctProcess) {
		executeState_.state_ = updateImageEventManagerExecutes_[updateImagesExecuteCounter_];

		executeState_.Execute(this, timeRegulation, timeFluctProcess);

		updateImagesExecuteCounter_ += 1;
	}

	public void UpdateImagesClear() {
		updateImages_.Clear();
		changeColorEnds_.Clear();
		endFillAmounts_.Clear();
		executeUpdateImages_.Clear();
		executeChangeColorEnds_.Clear();
		executeEndFillAmounts_.Clear();
		updateImageEventManagerExecutes_.Clear();

		updateImagesExecuteCounter_ = 0;
	}
}
