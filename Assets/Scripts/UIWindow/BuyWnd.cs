using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PEProtocol;

namespace DarknessWarGodLearning
{
    public class BuyWnd : WindowRoot
    {
        [SerializeField] private Button btnClose, btnSure;
        [SerializeField] private TextMeshProUGUI txtInfo;
        /// <summary>
        /// 0:����������1��������
        /// </summary>
        private int buyType;
        protected override void InitWnd()
        {
            base.InitWnd();
            btnClose.onClick.AddListener(ClickCloseBtn);
            btnSure.onClick.AddListener(ClickSureBtn);
            btnSure.interactable = true;
            RefreshUI();
        }
        public void SetBuyType(int buyType)
        {
            this.buyType = buyType;
        }
        private void RefreshUI()
        {
            switch(buyType)
            {
                case 0:
                    //����
                    SetText(txtInfo, "�Ƿ񻨷�" + Constants.Color("10��ʯ",TxtColor.Red) + "����" + Constants.Color("100����",TxtColor.Green)+"?");
                    break;
                case 1:
                    //���
                    SetText(txtInfo, "�Ƿ񻨷�" + Constants.Color("10��ʯ",TxtColor.Red) + "����" + Constants.Color("1000���",TxtColor.Green)+"?");
                    break;
            }
        }
        private void ClickCloseBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetWndState(false);
        }
        private void ClickSureBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            //Send Msg
            GameMsg msg = new GameMsg()
            {
                cmd = (int)CMD.ReqBuy,
                reqBuy = new ReqBuy()
                {
                    buytype = buyType,
                    cost = 10
                }
            };
            netSvc.SendMsg(msg);
            btnSure.interactable = false;
        }
        private void OnDisable()
        {
            btnClose.onClick.RemoveAllListeners();
            btnSure.onClick.RemoveAllListeners();
        }
    }
}