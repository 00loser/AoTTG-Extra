using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IEnumerator = System.Collections.IEnumerator;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using UnityEngine;
using System;

internal class Emotes
{
    private const int _emoteSize = 2;
    private static readonly string _emotesPath = $@"{Application.dataPath}\Emotes";
    private static Dictionary<string, string> _emotes;
    private static DateTime cdEnd;

    //call this on game start
    internal static void Initialize()
    {
        string[] images = Directory.GetFiles(_emotesPath, "*.*")
            .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg")).ToArray(); //filter images only

        Debug.Log($"{images.Length} images");
        _emotes = new Dictionary<string, string>(images.Length);
        for (int i = 0; i < images.Length; i++)
            FengGameManagerMKII.instance.StartCoroutine(ImageToText(images[i]));
    }

    private static IEnumerator ImageToText(string path)
    {
        WWW www = new WWW($"file://{path}");
        yield return www;                                                              
        Texture2D image = new Texture2D(www.texture.width, www.texture.height, TextureFormat.ARGB32, false);                             
        www.LoadImageIntoTexture(image);
        image.Apply();
        //TextureScale.Scale(image, 20, 20); resize image to 20x20. texture loses a lot of quality with this method so u better resize images by urself
        www.Dispose();
        StringBuilder msg = new StringBuilder($"\n<size={_emoteSize}>");
        for (int y = 1; y < image.height + 1; y++)
        {
            for (int x = 0; x < image.width; x++)
            {
                Color pixelColor = image.GetPixel(x, image.height - y); //get current pixel Color
                string currentColor = Ext.rgbToHex(pixelColor); //convert rgb color to hex and set it to "currentColor"
                if (Ext.clamp_float(pixelColor.a) < 255) //detect transparent pixels
                    msg.Append(" "); //append empty space then
                else 
                    msg.Append($"<color={currentColor}>██</color>");
            }
            msg.AppendLine(); 
        }
        Object.Destroy(image);
        msg.Append("</size>");
        string name = path.Substring(path.LastIndexOf(@"\") + 1, (path.IndexOf(".") - 1) - path.LastIndexOf(@"\"));
        _emotes.Add(name, msg.ToString());
    }

    /*On InRoomChat:
  
     if (!this.inputLine.StartsWith("/"))
     {
         if (inputLine.StartsWith(":") && inputLine.EndsWith(":"))
         {
             Emotes.checkEmote(inputLine.Substring(1, inputLine.Length - 2));
             inputLine = string.Empty;
         }
         else
         {
             str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]).hexColor();
             if (str2 == string.Empty)
             {
                str2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.name]);
                if (PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam] != null)
                {
                   if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 1)
                   {
                      str2 = "<color=#00FFFF>" + str2 + "</color>";
                   }
                   else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties[PhotonPlayerProperty.RCteam]) == 2)
                   {
                      str2 = "<color=#FF00FF>" + str2 + "</color>";
                   }
                }
            }
            object[] parameters = new object[] { inputLine, str2 };
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
        }
    }*/

    internal static void checkEmote(string input)
    {
        if (_emotes.ContainsKey(input))
        {
            if (DateTime.Now >= cdEnd)
            {
                FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, new object[] { _emotes[input], string.Empty });
                cdEnd = DateTime.Now.AddSeconds(10); //add cooldown so ppl cant spam emotes
            }
            else
            {
                InRoomChat.messages.Add($"You are on cooldown. Try again in {cdEnd.Second - DateTime.Now.Second}s");
            }
        }
    }
}

internal class Ext
{
    internal static byte clamp_float(float n) =>
        (byte)(Mathf.Clamp01(n) * 255);

    internal static string rgbToHex(Color color) =>
        string.Format("#{0:X2}{1:X2}{2:X2}", clamp_float(color.r), clamp_float(color.g), clamp_float(color.b));
}

internal class TextureScale
{
    private static Color[] texColors;
    private static Color[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;

    internal static void Scale(Texture2D tex, int newWidth, int newHeight)
    {
        texColors = tex.GetPixels();
        newColors = new Color[newWidth * newHeight];
        ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
        ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
        w = tex.width;
        w2 = newWidth;

        BilinearScale(0, newHeight);

        tex.Resize(newWidth, newHeight);
        tex.SetPixels(newColors);
        tex.Apply();
    }

    private static void BilinearScale(int start, int end)
    {
        for (var y = start; y < end; y++)
        {
            int yFloor = (int)Mathf.Floor(y * ratioY);
            var y1 = yFloor * w;
            var y2 = (yFloor + 1) * w;
            var yw = y * w2;

            for (var x = 0; x < w2; x++)
            {
                int xFloor = (int)Mathf.Floor(x * ratioX);
                var xLerp = x * ratioX - xFloor;
                newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                       y * ratioY - yFloor);
            }
        }
    }

    private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
    {
        return new Color(c1.r + (c2.r - c1.r) * value,
                          c1.g + (c2.g - c1.g) * value,
                          c1.b + (c2.b - c1.b) * value,
                          c1.a + (c2.a - c1.a) * value);
    }
}
