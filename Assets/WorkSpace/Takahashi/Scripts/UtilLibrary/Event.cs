using System.Collections.Generic;

namespace t13 {

	class Event<T> {
		public Event(T obj) {
			object_ = obj;
		}
		~Event() { }

		public bool update() {
			if (funcs_num_ < funcs_.Count) {
				if (funcs_[funcs_num_](object_)) {
					if (func_active_) funcs_num_ += 1;
					else {
						func_reset();
						return true;
					}

					if (funcs_num_ == funcs_.Count) {
						funcs_num_ = 0;
						return true;
					}
				}
			}

			return false;
		}

		public void func_add(EventFunc func) {
			funcs_.Add(func);
		}

		public void func_insert(EventFunc func, int insert_num) {
			funcs_.Insert(insert_num, func);
		}

		public bool event_finish() {
			func_active_ = false;

			return true;
		}

		public void func_reset() {
			funcs_.Clear();
			funcs_num_ = 0;
			func_active_ = true;
		}

		//get
		public int funcs_num() { return funcs_num_; }

		public delegate bool EventFunc(T obj);

		private T object_;
		private List<EventFunc> funcs_ = new List<EventFunc>();
		private int funcs_num_ = 0;

		private bool func_active_ = true;

	};

}
