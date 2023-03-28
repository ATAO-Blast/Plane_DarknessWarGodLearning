using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DarknessWarGodLearning
{
    public class LoadingWnd : WindowRoot
    {
        public TextMeshProUGUI textTips;
        public Image imgFG;
        public Image imgPoint;
        public TextMeshProUGUI textPrg;

        float bgWidth;
        protected override void InitWnd()
        {
            base.InitWnd();
            bgWidth = imgFG.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
            SetText(textTips, "This is a game tips");
            SetText(textPrg,"0%");
            imgFG.fillAmount = 0;
            imgPoint.transform.localPosition = new Vector3(-500, 0, 0);
        }
        public void SetProgerss(float prg)
        {
            SetText(textPrg, (int)(prg * 100) + "%");
            imgFG.fillAmount = prg;
            float posX = prg * bgWidth - 545;
            imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        }
    }
}