using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class LoginSys : SystemRoot
    {
        public static LoginSys Instance { get; private set; }
        public LoginWnd loginWnd;
        public CreateCharWnd createCharWnd;
        public override void InitSys()
        {
            base.InitSys();
            Instance = this;
        }
        /// <summary>
        /// 进入登录场景
        /// </summary>
        public void EnterLogin()
        {

            resSvc.AsyncLoadScene(Constants.SceneLogin,OpenLoginWnd);
        }
        private void OpenLoginWnd()
        {
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
        }
        public void RspLogin(GameMsg msg)
        {
            GameRoot.AddTips("Login Successed");
            GameRoot.Instance.SetPlayerData(msg.rspLogin);

            //服务器使用空数据提示创建新角色
            if (msg.rspLogin.playerData.name == "")
            {
                //打开角色创建界面
                createCharWnd.SetWndState();
            }
            else
            {
                //进入主城
                MainCitySys.Instance.EnterMainCity();
            }
            //关闭登录界面
            loginWnd.SetWndState(false);
        }
        public void RspRename(GameMsg msg)
        {
            var name = msg.rspRename.name;
            GameRoot.Instance.SetPalyerName(name);
            //进入主城
            MainCitySys.Instance.EnterMainCity();

            //关闭创建界面
            createCharWnd.SetWndState(false);
        }
    }
}