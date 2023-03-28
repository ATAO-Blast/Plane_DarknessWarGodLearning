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
        /// �����¼����
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

            //������ʹ�ÿ�������ʾ�����½�ɫ
            if (msg.rspLogin.playerData.name == "")
            {
                //�򿪽�ɫ��������
                createCharWnd.SetWndState();
            }
            else
            {
                //��������
                MainCitySys.Instance.EnterMainCity();
            }
            //�رյ�¼����
            loginWnd.SetWndState(false);
        }
        public void RspRename(GameMsg msg)
        {
            var name = msg.rspRename.name;
            GameRoot.Instance.SetPalyerName(name);
            //��������
            MainCitySys.Instance.EnterMainCity();

            //�رմ�������
            createCharWnd.SetWndState(false);
        }
    }
}