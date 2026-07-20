using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SinuousProductions
{
    public class UndoButton : MonoBehaviour
    {
        public SelectionSlotUI slot;

        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (slot != null && slot.GetCard() != null)
                    slot.TriggerUndo();
            });
        }
    }
}