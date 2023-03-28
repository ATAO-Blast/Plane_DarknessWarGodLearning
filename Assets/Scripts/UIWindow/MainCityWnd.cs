using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PEProtocol;

namespace DarknessWarGodLearning
{
    public class MainCityWnd : WindowRoot
    {
        public Button btnUpFight, btnBuyPower, btnHead, btnGuide, btnCharge, btnVip, btnTask, btnArena, btnMKCoin, btnStrong, btnMenu,btnChat;
        public TextMeshProUGUI txtFight, txtPower, txtLv, txtVip,txtName,txtExpPrg;
        public Image imgPowerPrg,imgTouch,imgDirBg,imgDirPoint;
        public Transform expPrgTrans;
        public Animation menuButtonAnim;

        private bool menuButtonClosed = false;
        private Vector2 startPos = Vector2.zero;
        private Vector2 defaultPos = Vector2.zero;
        private float pointDis;

        private AutoGuideCfg curTaskData;
        protected override void InitWnd()
        {
            base.InitWnd();
            pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
            defaultPos = imgDirBg.transform.position;
            SetActive(imgDirPoint, false);
            btnMenu.onClick.AddListener(PlayMenuButtonAnim);
            btnHead.onClick.AddListener(ClickHeadBtn);
            btnGuide.onClick.AddListener(ClickGuideBtn);
            btnStrong.onClick.AddListener(ClickStrongBtn);
            btnChat.onClick.AddListener(ClickChatBtn);
            btnBuyPower.onClick.AddListener(ClickBuyPowerBtn);
            btnMKCoin.onClick.AddListener(ClickMKCoinBtn);
            btnTask.onClick.AddListener(ClickTaskBtn);
            btnArena.onClick.AddListener(ClickArenaBtn);
            RegisterTouchEvents();
            RefreshUI();
        }
        public void RefreshUI()
        {
            PlayerData playerData = GameRoot.Instance.PlayerData;
            SetText(txtFight,PECommon.GetFightByProps(playerData));
            SetText(txtPower,"Power"+playerData.power+"/"+PECommon.GetPowerLimit(playerData.lv));
            imgPowerPrg.fillAmount = playerData.power * 1.0f / PECommon.GetPowerLimit(playerData.lv);
            SetText(txtLv,playerData.lv);
            SetText(txtName, playerData.name);

            //经验值进度条
            #region expPrg
            int expPrgVal = (int)((playerData.exp * 1.0f / PECommon.GetExpUpValByLv(playerData.lv)) * 100);
            SetText(txtExpPrg, expPrgVal + "%");
            int index = expPrgVal / 10;

            GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
            float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
            float canvasWidth = Screen.width * globalRate;
            float cellWidth = (canvasWidth - 180) / 10;

            grid.cellSize = new Vector2(cellWidth, 21);

            for (int i = 0; i < expPrgTrans.childCount; i++)
            {
                Image image = expPrgTrans.GetChild(i).GetComponent<Image>();
                if (i < index)
                {
                    image.fillAmount = 1;
                }
                else if (i == index)
                {
                    image.fillAmount = expPrgVal % 10 * 1.0f / 10;
                }
                else
                {
                    image.fillAmount = 0;
                }
            }
            #endregion

            //设置自动任务图标
            curTaskData = resSvc.GetAutoGuideCfg(playerData.guideid);
            if(curTaskData != null)
            {
                SetGuideButton(curTaskData.npcID);
            }
            else
            {
                SetGuideButton(-1);
            }
        }
        private void SetGuideButton(int npcID)
        {
            string spPath = "";
            Image img = btnGuide.GetComponent<Image>();
            switch(npcID)
            {

                case Constants.NPCWiseMan:
                    spPath = PathDefine.WiseManHead;
                    break;
                case Constants.NPCGeneral:
                    spPath = PathDefine.GeneralHead;
                    break;
                case Constants.NPCArtisan:
                    spPath = PathDefine.ArtisanHead;
                    break;
                case Constants.NPCTrader:
                    spPath = PathDefine.TraderHead;
                    break;
                default:
                    spPath = PathDefine.TaskHead;
                    break;
            }
            SetSprite(img, spPath);
        }
        #region ButtonEvents
        private void PlayMenuButtonAnim()
        {
            audioSvc.PlayUIAudio(Constants.UIExtenBtn);
            AnimationClip clip = null;
            if(!menuButtonClosed)
            {
                clip = menuButtonAnim.GetClip("CloseMCMenu");
            }
            else
            {
                clip = menuButtonAnim.GetClip("OpenMCMenu");
            }
            menuButtonAnim.Play(clip.name);
            menuButtonClosed = !menuButtonClosed;
        }
        private void ClickHeadBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIOpenPage);
            MainCitySys.Instance.OpenInfoWnd();
        }
        private void ClickStrongBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIOpenPage);
            MainCitySys.Instance.OpenStrongWnd();
        }
        private void ClickGuideBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            if(curTaskData !=  null)
            {
                MainCitySys.Instance.RunTask(curTaskData);
            }
            else
            {
                GameRoot.AddTips("More Guide Missions are under developing......");
            }
        }
        private void ClickChatBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            MainCitySys.Instance.OpenChatWnd();
        }
        private void ClickBuyPowerBtn()
        {
            audioSvc.PlayUIAudio (Constants.UIClickBtn);
            MainCitySys.Instance.OpenBuyWnd(0);
        }
        private void ClickMKCoinBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            MainCitySys.Instance.OpenBuyWnd(1);
        }
        private void ClickTaskBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            MainCitySys.Instance.OpenTaskWnd();
        }
        private void ClickArenaBtn()
        {
            audioSvc.PlayUIAudio(Constants.UIClickBtn);
            MainCitySys.Instance.EnterFuben();
        }
        public void RegisterTouchEvents()
        {
            OnPointerDown(imgTouch.gameObject, evt =>
            {
                startPos = evt.position;
                SetActive(imgDirPoint);
                imgDirBg.transform.position = evt.position;
            });
            OnPointerUp(imgTouch.gameObject, evt =>
            {
                imgDirBg.transform.position = defaultPos;
                SetActive(imgDirPoint, false);
                imgDirPoint.transform.position = Vector2.zero;

                MainCitySys.Instance.SetMoveDir(Vector2.zero);
            });
            OnPointerDrag(imgTouch.gameObject, evt =>
            {
                Vector2 dir = evt.position - startPos;
                float length = dir.magnitude;
                if (length > pointDis)
                {
                    Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                    imgDirPoint.transform.position = startPos + clampDir;
                }
                else imgDirPoint.transform.position = evt.position;

                MainCitySys.Instance.SetMoveDir(dir.normalized);
            });
        }
        #endregion
        private void OnDisable()
        {
            btnMenu.onClick.RemoveAllListeners();
            btnHead.onClick.RemoveAllListeners();
            btnGuide.onClick.RemoveAllListeners();
            btnStrong.onClick.RemoveAllListeners();
            btnChat.onClick.RemoveAllListeners();
            btnBuyPower.onClick.RemoveAllListeners();
            btnMKCoin.onClick.RemoveAllListeners();
            btnTask.onClick.RemoveAllListeners();
            btnArena.onClick.RemoveAllListeners();
        }
    }
}