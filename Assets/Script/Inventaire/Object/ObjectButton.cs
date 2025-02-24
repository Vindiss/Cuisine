using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private KitchenObject kitchenObject;
    
    private TextMeshProUGUI _description;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowDescription();
    }

    private void ShowDescription()
    {
        if (kitchenObject != null)
        {
            _description.text = kitchenObject.description;
        }
        else
        {
            _description.text = "Pas de description pour c'est objet.";
        }
    }
}
