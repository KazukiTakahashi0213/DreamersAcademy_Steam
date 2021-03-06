using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpdateImageProcess {
	None
	, ChangeColor
	, FillAmountUpdate
	, Max
}

public class UpdateImageProcessState {
	public UpdateImageProcessState(UpdateImageProcess setState) {
		state_ = setState;
	}

	public UpdateImageProcess state_;

	//None
	static private UpdateImageProcess NoneUpdate(UpdateImageProcessState mine, UpdateImage updateImage) {
		return mine.state_;
	}

	//ChangeColor
	static private UpdateImageProcess ChangeColorUpdate(UpdateImageProcessState mine, UpdateImage updateImage) {
		if(updateImage.GetTimeCounter().measure(Time.deltaTime, updateImage.GetTimeRegulation())) {
			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateRed(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(0)
				, updateImage.GetChangeEndColor().r
				, updateImage.GetTimeRegulation()
				, updateImage.GetTimeRegulation()
				);

			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateGreen(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(1)
				, updateImage.GetChangeEndColor().g
				, updateImage.GetTimeRegulation()
				, updateImage.GetTimeRegulation()
				);

			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateBlue(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(2)
				, updateImage.GetChangeEndColor().b
				, updateImage.GetTimeRegulation()
				, updateImage.GetTimeRegulation()
				);

			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(3)
				, updateImage.GetChangeEndColor().a
				, updateImage.GetTimeRegulation()
				, updateImage.GetTimeRegulation()
				);

			return UpdateImageProcess.None;
		}
		else {
			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateRed(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(0)
				, updateImage.GetChangeEndColor().r
				, updateImage.GetTimeCounter().count()
				, updateImage.GetTimeRegulation()
				);

			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateGreen(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(1)
				, updateImage.GetChangeEndColor().g
				, updateImage.GetTimeCounter().count()
				, updateImage.GetTimeRegulation()
				);

			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateBlue(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(2)
				, updateImage.GetChangeEndColor().b
				, updateImage.GetTimeCounter().count()
				, updateImage.GetTimeRegulation()
				);

			updateImage.GetImage().color = t13.UnityUtil.Color32InFluctUpdateAlpha(
				updateImage.GetImage().color
				, updateImage.GetTimeFlucts(3)
				, updateImage.GetChangeEndColor().a
				, updateImage.GetTimeCounter().count()
				, updateImage.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	//FillAmountUpdate
	static private UpdateImageProcess FillAmountUpdateUpdate(UpdateImageProcessState mine, UpdateImage updateImage) {
		if(updateImage.GetTimeCounter().measure(Time.deltaTime, updateImage.GetTimeRegulation())) {
			updateImage.GetImage().fillAmount = updateImage.GetTimeFlucts(0).InFluct(
				updateImage.GetTimeRegulation()
				, updateImage.GetImage().fillAmount
				, updateImage.GetEndFillAmount()
				, updateImage.GetTimeRegulation()
				);

			return UpdateImageProcess.None;
		}
		else {
			updateImage.GetImage().fillAmount = updateImage.GetTimeFlucts(0).InFluct(
				updateImage.GetTimeCounter().count()
				, updateImage.GetImage().fillAmount
				, updateImage.GetEndFillAmount()
				, updateImage.GetTimeRegulation()
				);
		}

		return mine.state_;
	}

	private delegate UpdateImageProcess UpdateFunc(UpdateImageProcessState mine, UpdateImage updateImage);

	private UpdateFunc[] updateFuncs_ = new UpdateFunc[(int)UpdateImageProcess.Max] {
		NoneUpdate
		, ChangeColorUpdate
		, FillAmountUpdateUpdate
	};
	public UpdateImageProcess Update(UpdateImage updateImage) { return updateFuncs_[(int)state_](this, updateImage); }

}
