using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;

public class Client
{
    private Socket clientSocket;
    private Message msg;

	public Client(){
		msg = new Message ();
	}

    int ConnectSever(){
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(Configs.Ip), Configs.Port);
		Debug.Log ("Connecting...");
        try{
            clientSocket.Connect(ipEndPoint);
            return 1;
        }catch(Exception e){
			Debug.Log (e);
            return 0;
        }
    }

	~Client(){
		CloseClient ();
	}

    public void GetRemoteService(RequestCode requestCode,ActionCode actionCode,string data){
        int netState = ConnectSever();
        if (netState > 0)
        {
			Debug.Log ("Connecting Accepted!");
			SendRequest (requestCode, actionCode, data);
            StartListening();
        }
        else
        {
			NetWarning.Msg = "当前无法链接服务器，请检查网络！";
			NetWarning.StartShowWarning = true;
			return;
        }
    }

    void SendRequest(RequestCode requestCode,ActionCode actionCode, string data){
		byte[] msgSent = Message.PackData (requestCode, actionCode, data);
		Debug.Log ("Sending Message...");
        clientSocket.Send(msgSent);
    }

    void StartListening(){
        if (clientSocket == null || clientSocket.Connected == false) return;
		ViewManager.isLoading = true;
		clientSocket.BeginReceive (msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
    }
		
    void ReceiveCallBack(IAsyncResult ar){
        try{
			int count = clientSocket.EndReceive(ar);
            if(count==0) CloseClient();
            msg.ReadMessage(count,HandleMessage);
            clientSocket.BeginReceive (msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }catch(Exception e){
            Debug.Log("Can Not Handle Received Message!" + e);
            CloseClient();
        }
    }

    void HandleMessage(RequestCode requestCode,ActionCode actionCode, string data){
        if(requestCode == RequestCode.Register){
            int id = 0;
            try {
                id = int.Parse (data);
                DataManager._player.AccountId = id;
				Debug.Log("获得ID："+id);
            } catch (Exception e) {
				Debug.Log (e);
				NetWarning.Msg = "未能验明正身，将在稍后自动验证。";
				NetWarning.StartShowWarning = true;
            }
        }else if(requestCode == RequestCode.Game){
            if(actionCode == ActionCode.GetPersonalResult){
                try {
					string[] s= data.Split(',');
					if(DataManager._player.AccountId==0){
						DataManager._player.AccountId=int.Parse(s[0]);
					}

					int rank = int.Parse(s[1])+1;
					NetWarning.Msg = DataManager.GetRankStr(rank);;
					NetWarning.StartShowWarning = true;

                } catch (Exception e) {
                    Debug.Log ("无法获得排名!\n" + e);
                }
            }else if(actionCode == ActionCode.GetTotalRank){
                DataManager.SetTotalRank(data);
            }else{
                Debug.Log("没有找到actionCode:"+actionCode);
            }
        }else{
            Debug.Log("没有找到RequestCode:"+requestCode);
        }
        CloseClient();
    }

    void CloseClient(){
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
        ViewManager.isLoading = false;
    }
        
}


