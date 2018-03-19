using System;
using System.Text;
using System.Linq;
using UnityEngine;
public class Message
{
    private byte[] data = new byte[3072];
	private int startIndex=0;

    public Message() {
        data = new byte[3072];
    }

    public byte[] Data{
        get{ return data;}
		set{ data = value;}
    }
	public int StartIndex{
		get{return startIndex;}
		set{startIndex=value;}
	}
	public int RemainSize{
		get{return data.Length-startIndex;}
	}


    public void ReadMessage(int newDataCount,Action<RequestCode,ActionCode,string> HandleMessage){
        startIndex += newDataCount;
//        if (newDataCount < 8) {
//			Debug.Log("信息不完整！");
//          return;
//    }
        if (startIndex <= 4)
            return;
        int msgLength = BitConverter.ToInt32(data, 0);//获取数据长度
        if (startIndex - 4 < msgLength)
            return;//如果数据长度少于应有长度，则等待下次执行

        RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
        ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);
        string s = Encoding.UTF8.GetString(data, 12, msgLength - 8);
        Debug.Log("Request:" + requestCode + ",Action:" + actionCode + ",Message:" + s);
        HandleMessage(requestCode, actionCode, s);

        //处理剩余数据
        Array.Copy(data, msgLength + 4, data, 0, startIndex - 4 - msgLength);
        startIndex -= (msgLength + 4);
    }

    public static byte[] PackData(RequestCode requestCode,ActionCode actionCode, string data){
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        requestCodeBytes = requestCodeBytes.Concat(actionCodeBytes).ToArray().Concat(dataBytes).ToArray();           
        return requestCodeBytes;
    }
}


