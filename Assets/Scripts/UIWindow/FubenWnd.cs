using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

namespace DarknessWarGodLearning
{
    public class FubenWnd : WindowRoot
    {
        [SerializeField] Button btnClose;
        [SerializeField] Transform pointerTrans;
        [SerializeField] Button[] btnFubens = new Button[6];

        private PlayerData pd;
        protected override void InitWnd()
        {
            base.InitWnd();
            btnClose.onClick.AddListener(ClickCloseBtn);
            pd = GameRoot.Instance.PlayerData;

            RefreshUI();
        }
        public void RefreshUI()
        {
            int fbid = pd.fuben;
            for (int i = 0; i < btnFubens.Length; i++)
            {
                if (i < fbid % 10000)
                {
                    SetActive(btnFubens[i].gameObject);
                    if (i == fbid % 10000 - 1)
                    {
                        pointerTrans.SetParent(btnFubens[i].transform);
                        pointerTrans.localPosition = new Vector3(105, 150, 0);
                    }
                }
                else
                {
                    SetActive(btnFubens[i].gameObject, false);
                }
            }
        }
        public void ClickFubenBtn(int fbid)
        {
            Debug.Log(fbid);
        }
        private void ClickCloseBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            SetWndState(false);
        }
        private void OnDisable()
        {
            btnClose.onClick.RemoveAllListeners();
            for (int i = 0; i < btnFubens.Length; i++)
            {
                btnFubens[i].onClick.RemoveAllListeners();
            }
        }
    }
}