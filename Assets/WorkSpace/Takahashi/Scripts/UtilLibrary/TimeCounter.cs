namespace t13 {

	public class TimeCounter {
		private float count_ = 0;

		public float count() { return count_; }

		public bool measure(float add_time, float regulation_time) {
			count_ += add_time;
			if (count_ < regulation_time) return false;
			count_ = 0;
			return true;
		}
		public void reset(float num = 0) {
			count_ = num;
		}
	}

}
