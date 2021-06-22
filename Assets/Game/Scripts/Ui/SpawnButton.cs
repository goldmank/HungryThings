using Game.Scripts.Infra.Events;
using Game.Scripts.Level;
using IO.Infra.Scripts.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Ui
{
    public class SpawnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private EaterType _eaterType;

        private bool _pressed;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
            Debug.Log("spawn button up");
            SimpleEventManager.Get().TriggerEvent(Events.Game.EaterHolderReleased);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_pressed)
            {
                return;
            }
            
            Debug.Log("return eater to button");
            SimpleEventManager.Get().TriggerEvent(Events.Game.EaterHolderDiscard);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_pressed)
            {
                return;
            }
            
            Debug.Log("spawn eater holder");
            SimpleEventManager.Get().TriggerEvent(Events.Game.EaterHolderSpawn, new EventParams.Builder().Add(Events.Game.ParamEaterType, (int)_eaterType).build());
        }
    }
}