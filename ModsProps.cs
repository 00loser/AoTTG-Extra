using System.Text;
using System.Collections.Generic;
public static class ModsProps
{
	private static readonly Dictionary<string, string> Props = new Dictionary<string, string>(1) //dictionary length
	{
		{ "GuardianMod", "[00ffff]{Guardian}" } // prop key, in game display
	};
	private const int limit = 4;
	
	public static string GetPlayerProps(PhotonPlayer player)
	{
		StringBuilder builder = new StringBuilder();
		int added = 0;
		foreach (string key in Props.Keys)
		{
			if (player.customProperties.ContainsKey(key) && added < limit)
			{
				builder.Append($" {Props[key]}");
				added++;
			}
		}
		return builder.ToString().hexColor();
		/*
		public static string hexColor(this string text)
		{
			if (text.Contains("]"))
				text = text.Replace("]", ">");
			bool flag = false;
			while (text.Contains("[") && !flag)
			{
				int num = text.IndexOf("[");
				if (text.Length >= num + 7)
				{
					string text2 = text.Substring(num + 1, 6);
					text = text.Remove(num, 7).Insert(num, "<color=#" + text2);
					int startIndex = text.Length;
					if (text.Contains("["))
						startIndex = text.IndexOf("[");
					text = text.Insert(startIndex, "</color>");
				}
				else
					flag = true;
		}
		if (flag)
			return string.Empty;
		return text;
		}

		*/
	} 
}
