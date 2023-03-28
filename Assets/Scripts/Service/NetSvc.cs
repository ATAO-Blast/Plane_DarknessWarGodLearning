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
                    case ErrorCode.ServerDataError:
                        PECommon.Log("服务器数据异常", LogType.Error);
                        GameRoot.AddTips("Clinet Data InNormal");
                        break;
                    case ErrorCode.LackLevel:
                        GameRoot.AddTips("Player Level Needed!");
                        break;
                    case ErrorCode.LackCoin:
                        GameRoot.AddTips("Player Coin Needed!");
                        break;
                    case ErrorCode.LackCrystal:
                        GameRoot.AddTips("Player Crystal Needed!");
                        break;
                    case ErrorCode.ClientDataError:
                        GameRoot.AddTips("Client Data Error");
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
                case CMD.RspGuide:
                    MainCitySys.Instance.RspGuide(msg);
                    break;
                case CMD.RspStrong:
                    MainCitySys.Instance.RspStrong(msg);
                    break;
                case CMD.PshChat:
                    MainCitySys.Instance.PushChat(msg);
                    break;
                case CMD.RspBuy:
                    MainCitySys.Instance.RspBuy(msg);
                    break;
                case CMD.PshPower:
                    MainCitySys.Instance.PshPower(msg);
                    break;
                case CMD.RspTaskReward:
                    MainCitySys.Instance.RspTaskReward(msg);
                    break;
                case CMD.PshTaskPrgs:
                    MainCitySys.Instance.PshTaskPrgs(msg);
                    break;
            }
            
        }
    }
}