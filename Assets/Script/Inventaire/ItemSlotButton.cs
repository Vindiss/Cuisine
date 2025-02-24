using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotButton : MonoBehaviour, IPointerDownHandler
{
    public KitchenObject kitchenObject = null;

    public void UpdateImage(Sprite sprite)
    {
        GetComponent<Image>().sprite = sprite;
    }

    public void RemoveImage()
    {
        GetComponent<Image>().sprite = null;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        //UpdateImage(kitchenObject.Image);
    }
}
