using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessWarGodLearning
{
    public class GuideWnd : WindowRoot
    {
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtTalk;
        public Image imgIcon;
        public Button btnNextLine;

        private PlayerData pd;
        private AutoGuideCfg curtTaskData;
        public string[] dialogArr;
        private int curDialogIndex;
        protected override void InitWnd()
        {
            base.InitWnd();
            pd = GameRoot.Instance.PlayerData;
            curtTaskData = MainCitySys.Instance.GetCurtTaskData();
            dialogArr = curtTaskData.dilogArr.Split('#');
            curDialogIndex = 1;
            SetTalk();
            btnNextLine.onClick.AddListener(NextLine);
        }
        
        private void SetTalk()
        {
            string[] talkLine = dialogArr[curDialogIndex].Split('|');
            if (talkLine[0] == "0")
            {
                //Palyer
                SetSprite(imgIcon,PathDefine.SelfIcon);
                SetText(txtName, pd.name);
            }
            else
            {
                //curNPC
                switch(curtTaskData.npcID)
                {
                    case Constants.NPCWiseMan:
                        SetSprite(imgIcon,PathDefine.WiseManIcon);
                        SetText(txtName, "Wise Man");
                        break;
                    case Constants.NPCGeneral:
                        SetSprite(imgIcon, PathDefine.GeneralIcon);
                        SetText(txtName, "General");
                        break;
                    case Constants.NPCArtisan:
                        SetSprite(imgIcon, PathDefine.ArtisanIcon);
                        SetText(txtName, "Artisan");
                        break;
                    case Constants.NPCTrader:
                        SetSprite(imgIcon, PathDefine.TraderIcon);
                        SetText(txtName, "Trader");
                        break;
                    default:
                        SetSprite(imgIcon, PathDefine.GuideIcon);
                        SetText(txtName, "Task Guide");
                        break;
                }
            }
            /*imgIcon.SetNativeSize(); // 如果img组件没有开启固定宽高比，需要此方法*/
            SetText(txtTalk, talkLine[1].Replace("$name",pd.name));
        }
        private void NextLine()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            var length = dialogArr.Length;
            curDialogIndex += 1;
            if (curDialogIndex >= length)
            {
                //向服务器发送任务引导完成信息
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.ReqGuide,
                    reqGuide = new ReqGuide
                    {
                        guideid = curtTaskData.ID
                    }
                };
                netSvc.SendMsg(msg);
                SetWndState(false);
            }
            else
            {
                SetTalk();
            }
        }
        private void OnDisable()
        {
            btnNextLine.onClick.RemoveAllListeners();
        }
    }
}