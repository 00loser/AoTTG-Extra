using System.Text;
using System.Collections.Generic;
public static class ModsProps
{
	private static readonly Dictionary<string, string> Props = new Dictionary<string, string>()
	{
		{ "GuardianMod", "[00ffff]{Guardian}" } // prop key, in game
	};
	public static string GetPlayerProps(PhotonPlayer player)
	{
		StringBuilder builder = new StringBuilder();
		int added = 0;
		int limit = 4;
		foreach (string key in Props.Keys)
			if (player.customProperties.ContainsKey("key") && added < limit)
				builder.Append(" " + Props[key]);
				added++;
		return builder.ToString().hexColor();
	} 
}
