using UnityEngine;
using PENet;
using PEProtocol;
using System.Collections.Generic;

namespace DarknessWarGodLearning
{
    public class NetSvc : MonoBehaviour
    {
        public static NetSvc Instance { get; private set; }
        private static readonly string obj = "lock";

        PESocket<ClientSession,GameMsg> client = null;

        private Queue<GameMsg> msgQue = new Queue<GameMsg>();
        
        public void InitSvc()
        {
            Instance = this;
            client = new PESocket<ClientSession, GameMsg>();

            client.SetLog(true, (msg, lv) =>
            {
                switch (lv)
                {
                    case 0:
                        msg = "Log:" + msg;
                        Debug.Log(msg);
                        break;
                    case 1:
                        msg = "Warn:" + msg;
                        Debug.LogWarning(msg);
                        break;
                    case 2:
                        msg = "Error:" + msg;
                        Debug.LogError(msg);
                        break;
                    case 3:
                        msg = "Info:" + msg;
                        Debug.Log(msg);
                        break;
                }
            });
            client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
            PECommon.Log("Init NetSvc");
        }
        public void SendMsg(GameMsg msg)
        {
            if(client.session!=null)
            {
                client.session.SendMsg(msg);
            }
            else
            {
                GameRoot.AddTips("Server disconnected!!");
                InitSvc();
            }
        }
        public void AddNetPkg(GameMsg msg)
        {
            lock (obj)
            {
                msgQue.Enqueue(msg);
            }
        }
        private void Update()
        {
            if(msgQue.Count > 0)
            {
                lock(obj)
                {
                    GameMsg msg = msgQue.Dequeue();
                    ProcessMsg(msg);
                }
            }
        }
        /// <summary>
        /// 分发服务器消息
        /// </summary>
        /// <param name="msg">服务器传来的消息</param>
        private void ProcessMsg(GameMsg msg)
        {
            if (msg.err != (int)ErrorCode.None)
            {
                switch ((ErrorCode)msg.err)
                {
                    case ErrorCode.AcctIsOnline:
                        GameRoot.AddTips("Your Account is already online");
                        break;
                    case ErrorCode.WrongPass:
                        GameRoot.AddTips("Your Password is wrong");
                        break;
                    case ErrorCode.UpdateDBError:
                        PECommon.Log("数据库更新异常",LogType.Error);
                        GameRoot.AddTips("Network instablilty");//不可以对客户端发送服务器内部错误
                        break;
                }
            }
            
            switch ((CMD)msg.cmd)
            {
                case CMD.RspLogin:
                    LoginSys.Instance.RspLogin(msg);
                    break;
                case CMD.RspRename:
                    LoginSys.Instance.RspRename(msg);
                    break;
            }
            
        }
    }
}