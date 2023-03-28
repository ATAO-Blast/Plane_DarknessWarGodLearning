using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessWarGodLearning
{
    public class LoginWnd : WindowRoot
    {
        public Button btnNotice;
        public Button btnEnter;
        public TMP_InputField iptAcct;
        public TMP_InputField iptPass;
        protected override void InitWnd()
        {
            base.InitWnd();
            if(PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
            {
                iptAcct.text = PlayerPrefs.GetString("Acct");
                iptPass.text = PlayerPrefs.GetString("Pass");
            }
            else
            {
                iptAcct.text = string.Empty;
                iptPass.text = string.Empty;
            }
            btnEnter.onClick.AddListener(ClickEnterBtn);
            btnNotice.onClick.AddListener(ClickNotiveBtn);
        }
        /// <summary>
        /// 点击进入游戏
        /// </summary>
        private void ClickEnterBtn()
        {
            audioSvc.PlayUIAudio(Constants.UILoginBtn);

            string _acct = iptAcct.text;
            string _pass = iptPass.text;
            if( _acct!="" && _pass!="") 
            {
                PlayerPrefs.SetString("Acct", _acct);
                PlayerPrefs.SetString("Pass", _pass);

                //TO DO 发送网络消息，请求登录
                GameMsg msg = new GameMsg()
                {
                    cmd = (int)CMD.ReqLogin,
                    reqLogin = new ReqLogin()
                    {
                        acct = _acct,
                        pass = _pass
                    }
                };
                netSvc.SendMsg(msg);
                //模拟登录
                //LoginSys.Instance.RspLogin();

            }
            else
            {
                GameRoot.AddTips("Not allowed Input");
            }
        }

        private void ClickNotiveBtn()
        {
            audioSvc.PlayUIAudio(Constants.UILoginBtn);
            GameRoot.AddTips("This module hasn't finished yet");
        }
        private void OnDisable()
        {
            btnEnter.onClick.RemoveListener(ClickEnterBtn);
            btnNotice.onClick.RemoveListener(ClickNotiveBtn);
        }
    }
}