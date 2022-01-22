if (inputLine == "/clear")
{
    if (PhotonNetwork.isMasterClient)
    {
        for (int i = 0; i <= 14; i++)
        {
            string text = (i == 14) ? "Chat cleaned":string.Empty;
            FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, new object[]{ text, string.Empty });
        }
    }
    else
    {
        //FengGameManagerMKII.instance.chatRoom.addLINE("Error: You aren't masterclient");
        addLINE("Error: You aren't masterclient");
    }
}
