using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DarknessWarGodLearning
{
    public class WindowRoot : MonoBehaviour
    {
        protected ResSvc resSvc = null;
        protected AudioSvc audioSvc = null;
        protected NetSvc netSvc = null;
        protected TimerSvc timerSvc = null;
        public void SetWndState(bool isActive = true)
        {
            if(gameObject.activeSelf != isActive) { SetActive(gameObject,isActive); }
            if(isActive ) { InitWnd(); }
            else { ClearWnd(); }
        }
        public bool GetWndState()
        {
            return gameObject.activeSelf;
        }
        /// <summary>
        /// 初始化来直接获取各种服务
        /// </summary>
        protected virtual void InitWnd()
        {
            resSvc = ResSvc.Instance;
            audioSvc = AudioSvc.Instance;
            netSvc = NetSvc.Instance;
            timerSvc = TimerSvc.Instance;
        }
        protected virtual void ClearWnd()
        {
            resSvc = null;
            audioSvc = null;
            netSvc = null;
            timerSvc = null;
        }
        #region SetTMPText
        protected void SetText(TextMeshProUGUI textMeshProUGUI, string content = "")
        {
            textMeshProUGUI.text = content;
        }
        protected void SetText(TextMeshProUGUI textMeshProUGUI, int num = 0)
        {
            SetText(textMeshProUGUI,num.ToString());
        }
        protected void SetText(Transform trans, string content = "")
        {
            SetText(trans.GetComponent<TextMeshProUGUI>(), content);
        }
        protected void SetText(Transform trans, int num = 0)
        {
            SetText(trans.GetComponent<TextMeshProUGUI>(), num);
        }
        #endregion

        protected T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if(t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }
        #region SetGoActiveState
        protected void SetActive(GameObject go,bool state = true)
        {
            go.SetActive(state);
        }
        protected void SetActive(Transform trans, bool state = true)
        {
            trans.gameObject.SetActive(state);
        }
        protected void SetActive(RectTransform rectTrans, bool state = true)
        {
            rectTrans.gameObject.SetActive(state);
        }
        protected void SetActive(Image img, bool state = true)
        {
            img.gameObject.SetActive(state);
        }
        protected void SetActive(TextMeshProUGUI textMeshProUGUI, bool state = true)
        {
            textMeshProUGUI.gameObject.SetActive(state);
        }
        #endregion
        #region Click Evts
        /// <summary>
        /// 给Image等组件添加OnPointerClick回调，注意回调的参数也是需要Listener指定的
        /// </summary>
        protected void OnPointerClick(GameObject go, UnityAction<object> evt,object args)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerClick = evt;
            listener.args = args;
        }
        protected void OnPointerDown(GameObject go,UnityAction<PointerEventData> evt)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerDown = evt;
        }
        protected void OnPointerUp(GameObject go, UnityAction<PointerEventData> evt)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerUp = evt;
        }
        protected void OnPointerDrag(GameObject go, UnityAction<PointerEventData> evt)
        {
            PEListener listener = GetOrAddComponent<PEListener>(go);
            listener.onPointerDrag = evt;
        }
        #endregion

        #region SetSprite
        protected void SetSprite(Image img,string path)
        {
            Sprite sprite = resSvc.LoadSprite(path, true);
            img.sprite = sprite;
        }
        #endregion
        protected Transform GetTrans(string name,Transform trans = null)
        {
            if (trans != null)
            {
                return trans.Find(name);
            }
            else
            {
                return transform.Find(name);
            }
        }
        protected Transform GetTrans(string name, GameObject go = null)
        {
            if (go != null)
            {
                return go.transform.Find(name);
            }
            else
            {
                return transform.Find(name);
            }
        }
    }
}