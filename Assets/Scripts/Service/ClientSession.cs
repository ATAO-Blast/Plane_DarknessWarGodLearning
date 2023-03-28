using PENet;
using PEProtocol;

namespace DarknessWarGodLearning
{
    public class ClientSession : PESession<GameMsg>
    {
        protected override void OnConnected()
        {
            GameRoot.AddTips("Server Connected");
            PECommon.Log("Server Connected");
        }
        protected override void OnReciveMsg(GameMsg msg)
        {
            PECommon.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
            NetSvc.Instance.AddNetPkg(msg);
        }
        protected override void OnDisConnected()
        {
            GameRoot.AddTips("Server disconnected");
            PECommon.Log("Server DisConnected");
        }
    }
}