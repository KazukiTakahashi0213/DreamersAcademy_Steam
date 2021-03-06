using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace t13 {

	public class Utility {
		const float PI = 3.14159265358979f;
		// ラジアンからデグリーへの変換
		static public float ToDegree(float radian) {
			return (radian / PI * 180.0f);
		}

		// デグリーからラジアンへの変換
		static public float ToRadian(float degree) {
			return (PI / 180.0f * degree);
		}

		//maxValueの値とnowValueの値の割合を出し、resultValueに掛けて、返り値で返す
		static public float ValueForPercentage(float maxValue, float nowValue, float resultValue) {
			return resultValue * (nowValue / maxValue);
		}

		//originalContextをcountとregulationの値で分割し、返り値で返す。
		static public string ContextUpdate(string originalContext, float count, float regulation) {
			if(regulation <= 0) return originalContext.Substring(0, originalContext.Length);

			float subLength = originalContext.Length * (count / regulation);

			if (subLength >= originalContext.Length) {
				return originalContext.Substring(0, originalContext.Length);
			}

			return originalContext.Substring(0, (int)subLength);
		}

		//半角の前に半角スペースを入れ、なんちゃて全角を作り、返り値で返す
		static public string HarfSizeForFullSize(string harfSizeStr) {
			string retString = "";

			for (int i = 0; i < harfSizeStr.Length; ++i) {
				retString += " " + harfSizeStr.Substring(i, 1);
			}

			return retString;
		}

		//tampStrの文字数がtampNumより小さければ、残りを全角スペースで埋めて、返り値で返す
		static public string StringFullSpaceBackTamp(string tampStr, int tampNum) {
			string retStr = tampStr;

			for (int i = 0; i < tampNum - tampStr.Length; ++i) {
				retStr += "　";
			}

			return retStr;
		}
		//tampStrの文字数がtampNumより小さければ、前から全角スペースで埋めて、返り値で返す
		static public string StringFullSpaceFrontTamp(string tampStr, int tampNum) {
			string retStr = tampStr;

			for (int i = 0; i < tampNum - tampStr.Length; ++i) {
				retStr = "　" + retStr;
			}

			return retStr;
		}
		//tampStrの文字数がtampNumより小さければ、残りを半角スペースで埋めて、返り値で返す
		static public string StringHarfSpaceBackTamp(string tampStr, int tampNum) {
			string retStr = tampStr;

			for (int i = 0; i < tampNum - tampStr.Length; ++i) {
				retStr += " ";
			}

			return retStr;
		}
		//tampStrの文字数がtampNumより小さければ、前から半角スペースで埋めて、返り値で返す
		static public string StringHarfSpaceFrontTamp(string tampStr, int tampNum) {
			string retStr = tampStr;

			for (int i = 0; i < tampNum - tampStr.Length; ++i) {
				retStr = " " + retStr;
			}

			return retStr;
		}

		//大きい順にソート
		static public void SimpleHiSort(int[] index) {
			for (int i = 0; i < index.Length - 1; ++i) {
				for (int j = i + 1; j < index.Length; ++j) {
					if (index[i] < index[j]) {
						int tmp = index[i];
						index[i] = index[j];
						index[j] = tmp;
					}
				}
			}
		}
		static public void SimpleHiSort(float[] index) {
			for (int i = 0; i < index.Length - 1; ++i) {
				for (int j = i + 1; j < index.Length; ++j) {
					if (index[i] < index[j]) {
						float tmp = index[i];
						index[i] = index[j];
						index[j] = tmp;
					}
				}
			}
		}

		//小さい順にソート
		static public void SimpleLowSort<T>(int[] index) {
			for (int i = 0; i < index.Length - 1; ++i) {
				for (int j = i + 1; j < index.Length; ++j) {
					if (index[i] > index[j]) {
						int tmp = index[i];
						index[i] = index[j];
						index[j] = tmp;
					}
				}
			}
		}
		static public void SimpleLowSort(float[] index) {
			for (int i = 0; i < index.Length - 1; ++i) {
				for (int j = i + 1; j < index.Length; ++j) {
					if (index[i] > index[j]) {
						float tmp = index[i];
						index[i] = index[j];
						index[j] = tmp;
					}
				}
			}
		}


		//大きい順に２次元配列をソート
		static public void SimpleHiSort2Index(int[] mainIndex, int[] subIndex) {
			for (int i = 0; i < mainIndex.Length - 1; ++i) {
				for (int j = i + 1; j < mainIndex.Length; ++j) {
					if (mainIndex[i] < mainIndex[j]) {
						{
							int tmp = mainIndex[i];
							mainIndex[i] = mainIndex[j];
							mainIndex[j] = tmp;
						}
						{
							int tmp = subIndex[i];
							subIndex[i] = subIndex[j];
							subIndex[j] = tmp;
						}
					}
				}
			}
		}

		//文字列を指定の文字で分割し、結果をListで返す
		static public List<string> ContextSlice(string sliceContext, string exceptChar) {
			int startNum = 0;
			int endNum = 0;
			List<string> retList = new List<string>();

			while (true) {
				endNum = sliceContext.IndexOf(exceptChar, startNum);
				if (endNum == -1) {
					retList.Add(sliceContext.Substring(startNum, sliceContext.Length - startNum));
					break;
				}

				retList.Add(sliceContext.Substring(startNum, endNum - startNum));

				startNum = endNum + 4;
			}

			return retList;
		}
	}

}
