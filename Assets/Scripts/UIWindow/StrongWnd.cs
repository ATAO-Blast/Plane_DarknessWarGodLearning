using PEProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

namespace DarknessWarGodLearning
{
    public class StrongWnd : WindowRoot
    {
        [Header("UI Define")]

        public Image imgCurPos,propArr1, propArr2, propArr3;
        public TextMeshProUGUI txtStarLv,propHp1,propHurt1, propDef1,propHp2, propHurt2, propDef2;
        public TextMeshProUGUI txtNeedLv,txtCostCoin,txtCostCrystal, txtCoin;
        public Transform starTransGroup,costInfoTrans;

        [Header("Data Area")]
        public Transform posBtnTrans;
        public Button btnClose,btnStrong;

        private Image[] imgs = new Image[6];
        private int curPosIndex;
        private PlayerData pd;
        private StrongCfg nextSd;
        protected override void InitWnd()
        {
            base.InitWnd();
            pd = GameRoot.Instance.PlayerData;
            btnClose.onClick.AddListener(ClickCloseBtn);
            btnStrong.onClick.AddListener(ClickStrongBtn);
            RegClickEvts();
            ClickPosItem(0);
        }
        /// <summary>
        /// 给imgs数组赋值，并且给OnPinterClick添加listener
        /// </summary>
        private void RegClickEvts()
        {
            for (int i = 0; i < posBtnTrans.childCount; i++)
            {
                Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
                OnPointerClick(img.gameObject, (object args) =>
                {
                    ClickPosItem((int)args);
                    audioSvc.PlayUIAudio(Constants.UIClickBtn);
                },i);
                imgs[i] = img;
            }
        }
        private void ClickPosItem(int index)
        {
            curPosIndex = index;
            for(int i = 0;i < imgs.Length;i++)
            {
                Transform trans = imgs[i].transform;
                if(i == curPosIndex)
                {
                    SetSprite(imgs[i],PathDefine.ItemArrorBG);
                    trans.localPosition = new Vector3(11.5f, trans.localPosition.y, 0);
                    trans.GetComponent<RectTransform>().sizeDelta = new Vector2(373, 149);
                }
                else
                {
                    SetSprite(imgs[i], PathDefine.ItemPlatBG);
                    trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                    trans.GetComponent<RectTransform>().sizeDelta = new Vector2(335, 125);
                }
            }
            RefreshItem();
        }
        private void RefreshItem()
        {
            //金币
            SetText(txtCoin, pd.coin);
            switch(curPosIndex)
            {
                case 0:
                    SetSprite(imgCurPos,PathDefine.ItemToukui); 
                    break;
                case 1:
                    SetSprite(imgCurPos, PathDefine.ItemBody);
                    break;
                case 2:
                    SetSprite(imgCurPos, PathDefine.ItemYaobu);
                    break;
                case 3:
                    SetSprite(imgCurPos, PathDefine.ItemHand);
                    break;
                case 4:
                    SetSprite(imgCurPos, PathDefine.ItemLeg);
                    break;
                case 5:
                    SetSprite(imgCurPos, PathDefine.ItemFoot);
                    break;
            }
            int curStarLv = pd.strongArr[curPosIndex];
            SetText(txtStarLv, curStarLv+"星级");
            for (int i = 0; i < starTransGroup.childCount; i++)
            {
                Image starImage = starTransGroup.GetChild(i).GetComponent<Image>();
                if (i<curStarLv)
                {
                    SetSprite(starImage, PathDefine.ItemStarFul);
                }
                else
                {
                    SetSprite(starImage, PathDefine.ItemStarEpt);
                }
            }
            int nextStarLv = curStarLv + 1;
            int sumAddHp = resSvc.GetPropAddValPreLv(curPosIndex, nextStarLv, 0);
            int sumAddHurt = resSvc.GetPropAddValPreLv(curPosIndex, nextStarLv, 1);
            int sumAddDef = resSvc.GetPropAddValPreLv(curPosIndex, nextStarLv, 2);

            SetText(propHp1, "生命 +" + sumAddHp);
            SetText(propHurt1, "伤害 +" + sumAddHurt);
            SetText(propDef1, "防御 +" + sumAddDef);


            nextSd = resSvc.GetStrongCfg(curPosIndex,nextStarLv);

            if (nextSd != null)
            {
                SetActive(propHp2);
                SetActive(propHurt2);
                SetActive(propDef2);
                SetActive(propArr1);
                SetActive(propArr2);
                SetActive(propArr3);
                SetActive(costInfoTrans);

                int nextAddHp = nextSd.addhp;
                int nextAddHurt = nextSd.addhurt;
                int nextAddDef = nextSd.adddef;

                SetText(propHp2, "强化后 +" + nextAddHp);
                SetText(propHurt2,"+"+ nextAddHurt);
                SetText(propDef2,"+"+ nextAddDef);

                SetText(txtNeedLv, "需要等级："+nextSd.minlv);
                SetText(txtCostCoin,"需要消耗：    "+ nextSd.coin);

                SetText(txtCostCrystal, nextSd.crystal+ "/"+ pd.crystal);
            }
            else
            {
                SetActive(propHp2, false);
                SetActive(propHurt2, false);
                SetActive(propDef2, false);
                SetActive(propArr1,false);
                SetActive(propArr2, false);
                SetActive(propArr3, false);
                SetActive(costInfoTrans, false);
            }

        }
        /// <summary>
        /// 交给MainCitySys.RspStrong来刷新此UI
        /// </summary>
        public void UpdateUI()
        {
            audioSvc.PlayUIAudio(Constants.FBItemEnter);
            ClickPosItem(curPosIndex);
        }
        private void ClickCloseBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetWndState(false);
        }
        private void ClickStrongBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);

            if (pd.strongArr[curPosIndex] < 10)
            {
                if(pd.lv < nextSd.minlv)
                {
                    GameRoot.AddTips(Constants.Color("Player Lv Needed",TxtColor.Blue));
                    return;
                }
                if (pd.coin < nextSd.coin)
                {
                    GameRoot.AddTips(Constants.Color("more Coin Needed",TxtColor.Blue));
                    return;
                }
                if (pd.crystal < nextSd.crystal)
                {
                    GameRoot.AddTips(Constants.Color("more Crystal Needed",TxtColor.Blue));
                    return;
                }
                GameMsg msg = new GameMsg()
                {
                    cmd = (int)CMD.ReqStrong,
                    reqStrong = new ReqStrong()
                    {
                        pos = curPosIndex
                    }
                };
                netSvc.SendMsg(msg);
            }
            else
            {
                GameRoot.AddTips(Constants.Color("Starlv is full", TxtColor.Yellow));
            }
        }
        private void OnDisable()
        {
            btnClose.onClick.RemoveAllListeners();
            btnStrong.onClick.RemoveAllListeners();
        }
    }
}