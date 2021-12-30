using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class Antis : MonoBehaviour
{
    private static List<PhotonPlayer> IgnoredSenders = new List<PhotonPlayer>();
    
    private static Dictionary<string, int> RPCs = new Dictionary<string, int>();

    private static readonly List<string> IgnoredRPCsToSpam = new List<string>() 
    {   
        //Here are missing RPCs, if the antis report spam for no reason, add the name of the RPC here.

        "Chat",
        "netSetAbnormalType",
        "netCrossFade",
        "setPhase",
        "netPlayAnimation",
        "net3DMGSMOKE",
        "setHairPRC",
        "setMyTarget",
        "netSetLevel",
        "netPauseAnimation",
        "changeTitanPt",
        "rockPlayAnimation",
        "setlfLookTarget",
        "myMasterIs",
        "loadskinRPC",
        "changeHumanPt"
    };

    /*
    
    In start of NetworkingPeer.ExecuteRPC():

    if (Antis.CheckForRPCSpam(rpcData, sender))
		return;

    */

    public static bool CheckForRPCSpam(ExitGames.Client.Photon.Hashtable rpcData, PhotonPlayer sender)
    {
        if (rpcData == null && sender == null)
            return false;
        if (IgnoredSenders.Contains(sender))
            return true;

        //Getting RPC name
        string RPCName = string.Empty;
        if (rpcData.ContainsKey((byte)5))
            RPCName = PhotonNetwork.PhotonServerSettings.RpcList[(int)((byte)rpcData[(byte)5])];
        else
            RPCName = rpcData[(byte)3] as string;

        //Adding info
        string info = $"{sender.ID}|{RPCName}";
        if (RPCs.ContainsKey(info))
            RPCs[info]++;
        else
            RPCs.Add(info, 1);

        //Checking spam
        if (RPCs[info] > 25 && !IgnoredRPCsToSpam.Contains(RPCName))
        {
            Logger.ANTIS(RPCName + " Spam from: " + sender.ID.ToString()); //U can call your own logger
            IgnoredSenders.Add(sender);
            return true;
        }
        return false;
    }

    private void Start() => StartCoroutine(ResetInfo());

    private System.Collections.IEnumerator ResetInfo()
    {
        while (true)
        {
            RPCs.Clear();
            IgnoredSenders.Clear();
            yield return new WaitForSeconds(1f);
        }
    }
}
