using System;

namespace t13 {

	public enum TimeFluctProcess {
		None
		, Liner
		, Bounce
		, Back
		, Circ
		, Cubic
		, Elastic
		, Expo
		, Quad
		, Quart
		, Quint
		, Sine
		, Max
	}

	public class TimeFluctProcessState {
		public TimeFluctProcessState(TimeFluctProcess setState) {
			state_ = setState;
		}

		public TimeFluctProcess state_;

		//None
		static private float NoneIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			return 0;
		}
		static private float NoneOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			return 0;
		}
		static private float NoneInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			return 0;
		}

		//Liner
		static private float LinerIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * count / regulation_time + timeFluct.GetStartPos_Archive();
		}
		static private float LinerOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * count / regulation_time + timeFluct.GetStartPos_Archive();
		}
		static private float LinerInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * count / regulation_time + timeFluct.GetStartPos_Archive();
		}

		//Bounce
		static private float BounceIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			TimeFluct tempTimeFluct = new TimeFluct();
			tempTimeFluct.GetProcessState().state_ = TimeFluctProcess.Bounce;
			float result = tempTimeFluct.OutFluct(regulation_time - count, startPos, endPos, regulation_time);

			return timeFluct.GetRange() - result + timeFluct.GetStartPos_Archive();
		}
		static private float BounceOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if ((count /= regulation_time) < (1 / 2.75f)) {
				return timeFluct.GetRange() * (7.5625f * count * count) + timeFluct.GetStartPos_Archive();
			}
			else if (count < (2 / 2.75f)) {
				float postFix = count -= (1.5f / 2.75f);
				return timeFluct.GetRange() * (7.5625f * (postFix) * count + .75f) + timeFluct.GetStartPos_Archive();
			}
			else if (count < (2.5 / 2.75)) {
				float postFix = count -= (2.25f / 2.75f);
				return timeFluct.GetRange() * (7.5625f * (postFix) * count + .9375f) + timeFluct.GetStartPos_Archive();
			}
			else {
				float postFix = count -= (2.625f / 2.75f);
				return timeFluct.GetRange() * (7.5625f * (postFix) * count + .984375f) + timeFluct.GetStartPos_Archive();
			}
		}
		static private float BounceInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if (count < regulation_time / 2) {
				TimeFluct tempTimeFluct = new TimeFluct();
				tempTimeFluct.GetProcessState().state_ = TimeFluctProcess.Bounce;
				float result = tempTimeFluct.InFluct(count * 2, startPos, endPos, regulation_time);

				return result * .5f + timeFluct.GetStartPos_Archive();
			}
			else {
				TimeFluct tempTimeFluct = new TimeFluct();
				tempTimeFluct.GetProcessState().state_ = TimeFluctProcess.Bounce;
				float result = tempTimeFluct.OutFluct(count * 2 - regulation_time, startPos, endPos, regulation_time);

				return result * .5f + timeFluct.GetRange() * .5f + timeFluct.GetStartPos_Archive();
			}
		}

		//Back
		static private float BackIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			float s = 1.70158f;
			float postFix = count /= regulation_time;
			return timeFluct.GetRange() * (postFix) * count * ((s + 1) * count - s) + timeFluct.GetStartPos_Archive();
		}
		static private float BackOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			float s = 1.70158f;
			return timeFluct.GetRange() * ((count = count / regulation_time - 1) * count * ((s + 1) * count + s) + 1) + timeFluct.GetStartPos_Archive();
		}
		static private float BackInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			float s = 1.70158f;
			if ((count /= regulation_time / 2) < 1) {
				return timeFluct.GetRange() / 2 * (count * count * (((s *= (1.525f)) + 1) * count - s)) + timeFluct.GetStartPos_Archive();
			}
			else {
				float postFix = count -= 2;
				return timeFluct.GetRange() / 2 * ((postFix) * count * (((s *= (1.525f)) + 1) * count + s) + 2) + timeFluct.GetStartPos_Archive();
			}
		}

		//Circ
		static private float CircIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return -timeFluct.GetRange() * ((float)Math.Sqrt(1 - (count /= regulation_time) * count) - 1) + timeFluct.GetStartPos_Archive();
		}
		static private float CircOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * (float)Math.Sqrt(1 - (count = count / regulation_time - 1) * count) + timeFluct.GetStartPos_Archive();
		}
		static private float CircInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if ((count /= regulation_time / 2) < 1) return -timeFluct.GetRange() / 2 * (float)(Math.Sqrt(1 - count * count) - 1) + timeFluct.GetStartPos_Archive();
			else return timeFluct.GetRange() / 2 * (float)(Math.Sqrt(1 - count * (count -= 2)) + 1) + timeFluct.GetStartPos_Archive();
		}

		//Cubic
		static private float CubicIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * (count /= regulation_time) * count * count + timeFluct.GetStartPos_Archive();
		}
		static private float CubicOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * ((count = count / regulation_time - 1) * count * count + 1) + timeFluct.GetStartPos_Archive();
		}
		static private float CubicInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if ((count /= regulation_time / 2) < 1) return timeFluct.GetRange() / 2 * count * count * count + timeFluct.GetStartPos_Archive();
			else return timeFluct.GetRange() / 2 * ((count -= 2) * count * count + 2) + timeFluct.GetStartPos_Archive();
		}

		//Elastic
		static private float ElasticIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if (count == 0) return timeFluct.GetStartPos_Archive();
			if ((count /= regulation_time) == 1) return timeFluct.GetStartPos_Archive() + timeFluct.GetRange();

			float p = regulation_time * .3f;
			float a = timeFluct.GetRange();
			float s = p / 4;
			float postFix = a * (float)Math.Pow(2, 10 * (count -= 1));
			return -(postFix * (float)Math.Sin((count * regulation_time - s) * (2 * TimeFluct.PI) / p)) + timeFluct.GetStartPos_Archive();
		}
		static private float ElasticOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if (count == 0) return timeFluct.GetStartPos_Archive();
			if ((count /= regulation_time) == 1) return timeFluct.GetStartPos_Archive() + timeFluct.GetRange();

			float p = regulation_time * .3f;
			float a = timeFluct.GetRange();
			float s = p / 4;
			return (a * (float)Math.Pow(2, -10 * count) * (float)Math.Sin((count * regulation_time - s) * (2 * TimeFluct.PI) / p) + timeFluct.GetRange() + timeFluct.GetStartPos_Archive());
		}
		static private float ElasticInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if (count == 0) return timeFluct.GetStartPos_Archive();
			if ((count /= regulation_time / 2) == 2) return timeFluct.GetStartPos_Archive() + timeFluct.GetRange();

			float p = regulation_time * (.3f * 1.5f);
			float a = timeFluct.GetRange();
			float s = p / 4;

			if (count < 1) {
				float postFix = a * (float)Math.Pow(2, 10 * (count -= 1));
				return -.5f * (postFix * (float)Math.Sin((count * regulation_time - s) * (2 * TimeFluct.PI) / p)) + timeFluct.GetStartPos_Archive();
			}
			else {
				float postFix = a * (float)Math.Pow(2, -10 * (count -= 1));
				return postFix * (float)Math.Sin((count * regulation_time - s) * (2 * TimeFluct.PI) / p) * .5f + timeFluct.GetRange() + timeFluct.GetStartPos_Archive();
			}
		}

		//Expo
		static private float ExpoIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return (count == 0) ? timeFluct.GetStartPos_Archive() : timeFluct.GetRange() * (float)Math.Pow(2, 10 * (count / regulation_time - 1)) + timeFluct.GetStartPos_Archive();
		}
		static private float ExpoOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return (count == regulation_time) ? timeFluct.GetStartPos_Archive() + timeFluct.GetRange() : timeFluct.GetRange() * (-(float)Math.Pow(2, -10 * count / regulation_time) + 1) + timeFluct.GetStartPos_Archive();
		}
		static private float ExpoInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if (count == 0) return timeFluct.GetStartPos_Archive();
			if (count == regulation_time) return timeFluct.GetStartPos_Archive() + timeFluct.GetRange();
			if ((count /= regulation_time / 2) < 1) return timeFluct.GetRange() / 2 * (float)Math.Pow(2, 10 * (count - 1)) + timeFluct.GetStartPos_Archive();
			return timeFluct.GetRange() / 2 * (-(float)Math.Pow(2, -10 * --count) + 2) + timeFluct.GetStartPos_Archive();
		}

		//Quad
		static private float QuadIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * (count /= regulation_time) * count + timeFluct.GetStartPos_Archive();
		}
		static private float QuadOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return -timeFluct.GetRange() * (count /= regulation_time) * (count - 2) + timeFluct.GetStartPos_Archive();
		}
		static private float QuadInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if ((count /= regulation_time / 2) < 1) return ((timeFluct.GetRange() / 2) * (count * count)) + timeFluct.GetStartPos_Archive();
			return -timeFluct.GetRange() / 2 * (((count - 2) * (--count)) - 1) + timeFluct.GetStartPos_Archive();
		}

		//Quart
		static private float QuartIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * (count /= regulation_time) * count * count * count + timeFluct.GetStartPos_Archive();
		}
		static private float QuartOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return -timeFluct.GetRange() * ((count = count / regulation_time - 1) * count * count * count - 1) + timeFluct.GetStartPos_Archive();
		}
		static private float QuartInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if ((count /= regulation_time / 2) < 1) return timeFluct.GetRange() / 2 * count * count * count * count + timeFluct.GetStartPos_Archive();
			return -timeFluct.GetRange() / 2 * ((count -= 2) * count * count * count - 2) + timeFluct.GetStartPos_Archive();
		}

		//Quint
		static private float QuintIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * (count /= regulation_time) * count * count * count * count + timeFluct.GetStartPos_Archive();
		}
		static private float QuintOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * ((count = count / regulation_time - 1) * count * count * count * count + 1) + timeFluct.GetStartPos_Archive();
		}
		static private float QuintInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			if ((count /= regulation_time / 2) < 1) return timeFluct.GetRange() / 2 * count * count * count * count * count + timeFluct.GetStartPos_Archive();
			return timeFluct.GetRange() / 2 * ((count -= 2) * count * count * count * count + 2) + timeFluct.GetStartPos_Archive();
		}

		//Sine
		static private float SineIn(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return -timeFluct.GetRange() * (float)Math.Cos(count / regulation_time * (TimeFluct.PI / 2)) + timeFluct.GetRange() + timeFluct.GetStartPos_Archive();
		}
		static private float SineOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return timeFluct.GetRange() * (float)Math.Sin(count / regulation_time * (TimeFluct.PI / 2)) + timeFluct.GetStartPos_Archive();
		}
		static private float SineInOut(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) {
			//計算
			return -timeFluct.GetRange() / 2 * ((float)Math.Cos(TimeFluct.PI * count / regulation_time) - 1) + timeFluct.GetStartPos_Archive();
		}

		private delegate float FluctFunc(TimeFluctProcessState mine, TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time);

		private FluctFunc[] inFlucts_ = new FluctFunc[(int)TimeFluctProcess.Max] {
			NoneIn
			, LinerIn
			, BounceIn
			, BackIn
			, CircIn
			, CubicIn
			, ElasticIn
			, ExpoIn
			, QuadIn
			, QuartIn
			, QuintIn
			, SineIn
		};
		public float InFluct(TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) { return inFlucts_[(int)state_](this, timeFluct, count, startPos, endPos, regulation_time); }

		private FluctFunc[] outFlucts_ = new FluctFunc[(int)TimeFluctProcess.Max] {
			NoneOut
			, LinerOut
			, BounceOut
			, BackOut
			, CircOut
			, CubicOut
			, ElasticOut
			, ExpoOut
			, QuadOut
			, QuartOut
			, QuintOut
			, SineOut
		};
		public float OutFluct(TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) { return outFlucts_[(int)state_](this, timeFluct, count, startPos, endPos, regulation_time); }

		private FluctFunc[] inOutFlucts_ = new FluctFunc[(int)TimeFluctProcess.Max] {
			NoneInOut
			, LinerInOut
			, BounceInOut
			, BackInOut
			, CircInOut
			, CubicInOut
			, ElasticInOut
			, ExpoInOut
			, QuadInOut
			, QuartInOut
			, QuintInOut
			, SineInOut
		};
		public float InOutFluct(TimeFluct timeFluct, float count, float startPos, float endPos, float regulation_time) { return inOutFlucts_[(int)state_](this, timeFluct, count, startPos, endPos, regulation_time); }
	}

}
