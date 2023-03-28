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
        public Button btnUpFight, btnBuyPower, btnHead, btnGuide, btnCharge, btnVip, btnTask, btnArena, btnMKCoin, btnStrong, btnMenu;
        public TextMeshProUGUI txtFight, txtPower, txtLv, txtVip,txtName,txtExpPrg;
        public Image imgPowerPrg,imgTouch,imgDirBg,imgDirPoint;
        public Transform expPrgTrans;
        public Animation menuButtonAnim;

        private bool menuButtonClosed = false;
        private Vector2 startPos = Vector2.zero;
        private Vector2 defaultPos = Vector2.zero;
        private float pointDis;
        protected override void InitWnd()
        {
            base.InitWnd();
            pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOPDis;
            defaultPos = imgDirBg.transform.position;
            SetActive(imgDirPoint, false);
            btnMenu.onClick.AddListener(PlayMenuButtonAnim);
            btnHead.onClick.AddListener(ClickHeadBtn);
            RegisterTouchEvents();
            RefreshUI();
        }
        private void RefreshUI()
        {
            PlayerData playerData = GameRoot.Instance.PlayerData;
            SetText(txtFight,PECommon.GetFightByProps(playerData));
            SetText(txtPower,"Power"+playerData.power+"/"+PECommon.GetPowerLimit(playerData.lv));
            imgPowerPrg.fillAmount = playerData.power * 1.0f / PECommon.GetPowerLimit(playerData.lv);
            SetText(txtLv,playerData.lv);
            SetText(txtName, playerData.name);

            //expPrg
            int expPrgVal = (int)((playerData.exp * 1.0f / PECommon.GetExpUpValByLv(playerData.lv)) * 100);
            SetText(txtExpPrg,expPrgVal + "%");
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
                else if(i == index)
                {
                    image.fillAmount = expPrgVal % 10 * 1.0f / 10;
                }
                else
                {
                    image.fillAmount = 0;
                }
            }
            
        }
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
        private void OnDisable()
        {
            btnMenu.onClick.RemoveAllListeners();
            btnHead.onClick.RemoveAllListeners();
        }
    }
}