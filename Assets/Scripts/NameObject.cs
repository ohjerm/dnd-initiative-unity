using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NameObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.parent.parent.parent.GetComponent<Container>().SelectName(GetComponent<Text>().text);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.Find("Image").GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.Find("Image").GetComponent<Image>().color = new Color(1, 1, 1, 0f);
    }
}
