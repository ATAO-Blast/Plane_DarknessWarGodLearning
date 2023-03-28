using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DarknessWarGodLearning
{
    public class DynamicWnd : WindowRoot
    {
        public Animation tipsAni;
        public TextMeshProUGUI textTips;

        private bool isTipsShow = false;
        private Queue<string> tipsQue = new Queue<string>();
        protected override void InitWnd()
        {
            base.InitWnd();
            SetActive(textTips, false);
        }
        public void AddTipsQue(string text)
        {
            lock(tipsQue)
            {
                tipsQue.Enqueue(text);
            }
        }
        private void Update()
        {
            if (tipsQue.Count > 0 && isTipsShow == false)
            {
                string tips = tipsQue.Dequeue();
                SetTips(tips);
                isTipsShow = true;
            }
        }
        private void SetTips(string text)
        {
            SetActive(textTips);
            SetText(textTips, text);

            AnimationClip clip = tipsAni.GetClip("TipsShowAnime");
            tipsAni.Play();

            StartCoroutine(AniPlayDone(clip.length, () =>
            {
                SetActive(textTips, false);
                isTipsShow = false;
            }));
        }
        IEnumerator AniPlayDone(float sec,Action cb)
        {
            yield return new WaitForSeconds(sec);
            cb?.Invoke();
        }
    }
}