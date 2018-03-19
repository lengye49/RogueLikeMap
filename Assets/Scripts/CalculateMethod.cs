using UnityEngine;
using System.Collections.Generic;

public class CalculateMethod{
	/// <summary>
	/// 获取Range范围内的随机值
	/// </summary>
	/// <returns>The random value.</returns>
	/// <param name="range">Range格式 1|8 .</param>
	public static int GetRandomValue(string range){
		string[] s = range.Split ('|');
		int min = int.Parse (s [0]);
		int max = int.Parse (s [1]);
		return Random.Range (min, max + 1);
	}

	/// <summary>
	/// 获取整数随机值.
	/// </summary>
	/// <returns>The random value.</returns>
	/// <param name="min">Min（包含）.</param>
	/// <param name="max">Max（不包含）</param>
	public static int GetRandomValue(int min,int max){
		return Random.Range (min, max);
	}

	/// <summary>
	/// 根据权重随机选1,取不到则为-1,需要处理-1的情况
	/// </summary>
	/// <returns>值,权重.</returns>
	public static int GetRandomValue(Dictionary<int,int> d){
		int totalWeight = 0;
		foreach (int key in d.Keys) {
			totalWeight += d [key];
		}
		int r = Random.Range (0, totalWeight);

		totalWeight = 0;
		foreach(int key in d.Keys){
			totalWeight += d [key];
			if (r < totalWeight)
				return key;
		}
		return -1;
	}
}

