using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessWarGodLearning
{
    public class CreateCharWnd : WindowRoot
    {
        public TMP_InputField iptName;
        public Button rdNameBtn;
        public Button clickEnterBtn;
        protected override void InitWnd()
        {
            base.InitWnd();
            iptName.text = resSvc.GetRDNameData(false);
            rdNameBtn.onClick.AddListener(() =>
            {
                audioSvc.PlayUIAudio(Constants.UILoginBtn);
                string rdName = resSvc.GetRDNameData(false);
                iptName.text = rdName;
            });

            clickEnterBtn.onClick.AddListener(() =>
            {
                audioSvc.PlayUIAudio(Constants.UILoginBtn);
                if (iptName.text != "")
                {
                    //发送名字数据到服务器，登录主城
                    GameMsg msg = new GameMsg()
                    {
                        cmd = (int)CMD.ReqRename,
                        reqRename = new ReqRename()
                        {
                            name = iptName.text,
                        }
                    };

                    netSvc.SendMsg(msg);
                }else
                {
                    GameRoot.AddTips("Not Allowed Name");
                }
            });
        }
        private void OnDisable()
        {
            rdNameBtn.onClick.RemoveAllListeners();
            clickEnterBtn.onClick.RemoveAllListeners();
        }
    }
}