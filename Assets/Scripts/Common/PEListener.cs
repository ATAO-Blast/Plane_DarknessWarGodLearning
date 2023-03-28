using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace DarknessWarGodLearning
{
    public class PEListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public UnityAction<PointerEventData> onPointerDown;
        public UnityAction<PointerEventData> onPointerUp;
        public UnityAction<PointerEventData> onPointerDrag;

        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            onPointerDrag?.Invoke(eventData);
        }
    }
}