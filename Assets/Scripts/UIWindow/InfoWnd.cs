using UnityEngine.UI;
using TMPro;
using PEProtocol;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class InfoWnd : WindowRoot
    {
        [Header("InfoWnd")]
        public RawImage charShow;
        public TextMeshProUGUI txtInfo,txtExpprg, txtPowerprg, txtJob, txtFight, txtHp, txtHurt, txtDef;
        public Button btnDetail,btnClose;
        public Image imgPowerprg, imgExpprg;

        [Header("Detail")]
        public Transform transDetail;
        public Button btnCloseDetail;
        public TextMeshProUGUI txtDetailHp, txtDetailHurt, txtDetailDef, txtDetailAp, txtDetailApdef, txtDetailDodge, txtDetailPierce, txtDetailCritical;
        protected override void InitWnd()
        {
            base.InitWnd();
            RefreshUI();
            btnClose.onClick.AddListener(ClickCloseBtn);
            btnDetail.onClick.AddListener (ClickDetailBtn);
            btnCloseDetail.onClick.AddListener(ClickDetailCloseBtn);
            RegTouchEvts();
        }
        private Vector2 startPos;
        private void RegTouchEvts()
        {
            OnPointerDown(charShow.gameObject, (evt) =>
            {
                startPos = evt.position;
                MainCitySys.Instance.SetStartRotate();
            });

            OnPointerDrag(charShow.gameObject, (evt) =>
            {
                float rotate = -(evt.position.x - startPos.x)*0.4f;
                MainCitySys.Instance.SetPalyerRotate(rotate);
            });
        }
        
        private void RefreshUI()
        {
            PlayerData pd = GameRoot.Instance.PlayerData;
            SetText(txtInfo, pd.name + " " + "LV." + pd.lv);
            SetText(txtExpprg,pd.exp + "/"+PECommon.GetExpUpValByLv(pd.lv));
            SetText(txtPowerprg,pd.power + "/"+PECommon.GetPowerLimit(PECommon.GetPowerLimit(pd.lv)));
            SetText(txtJob , "Job Assassin");
            SetText(txtHp, "Hp  " + pd.hp);
            SetText(txtFight, "Fgt  " + PECommon.GetFightByProps(pd));
            SetText(txtHurt, "Hrt  " + (pd.ad+pd.ap));
            SetText(txtDef, "Def  " + (pd.addef+pd.apdef));

            float powerPrg =pd.power*1.0f/PECommon.GetPowerLimit(pd.lv);
            imgPowerprg.fillAmount = powerPrg;

            float expPrg = pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv);
            imgExpprg.fillAmount = expPrg;

            //Detail Todo
            SetActive(transDetail, false);
            SetText(txtDetailHp, pd.hp);
            SetText(txtDetailAp, pd.ap);
            SetText(txtDetailDef, pd.addef);
            SetText(txtDetailApdef, pd.apdef);
            SetText(txtDetailDodge, pd.dodge + "%");
            SetText(txtDetailPierce, pd.pierce+"%");
            SetText(txtDetailCritical, pd.critical+"%");
        }
        private void ClickCloseBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            MainCitySys.Instance.CloseInfoWnd();
            SetWndState(false);
        }
        private void ClickDetailBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetActive(transDetail);
            
        }
        private void ClickDetailCloseBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetActive(transDetail, false);
        }
        private void OnDisable()
        {
            btnClose.onClick.RemoveAllListeners();
            btnDetail.onClick.RemoveAllListeners();
            btnCloseDetail.onClick.RemoveAllListeners();
        }
    }
}