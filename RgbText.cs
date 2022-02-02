using System.Collections;
using UnityEngine;

public class RgbText : MonoBehaviour
{
	public static string CurrentColor;

	private string[] Colors;

	private void Start()
	{
		Colors = new string[]
		{
			"FF0000", "FF4200", "FFAA00", "FBFF00", "93FF00", "32FF00", "00FF32", "00FFAE", "00E4FF", "0087FF",
			"0004FF", "8300FF", "E000FF", "FF00B6", "FF0000"
		};
		StartCoroutine(StartAnimation());
	}
	
	public IEnumerator StartAnimation()
	{
		while (true)
		{
			for (int i = 0; i < Colors.Length; i++)
			{
				CurrentColor = Colors[i];
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}
