using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Container : MonoBehaviour, IComparable, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject nameObject;

    public int Initiative { get; private set; }
    public string Name { get; private set; }
    public bool IsNpc { get; private set; }
    public bool Dead { get; private set; }
    public int Health { get; private set; }

    private bool isHovered = false;
    private List<string> names = new List<string>();

    private void Awake()
    {
        Initiative = 0;
        Name = "";
        IsNpc = true;
        Dead = false;
    }

    public void PotentialNames(string currentText)
    {
        ScrollRect sr = transform.Find("Scroll View").GetComponent<ScrollRect>();
        names.Clear();

        Debug.Log(Controller.Names.Length);

        foreach(string s in Controller.Names)
        {
            if (s.ToLower().Contains(currentText.ToLower()))
            {
                names.Add(s);
            }
        }

        if (currentText == "" || names.Count == 0)
        {
            sr.gameObject.SetActive(false);
            return;
        }
        else
        {
            sr.gameObject.SetActive(true);
        }

        RectTransform content = sr.transform.Find("Viewport/Content").GetComponent<RectTransform>();

        foreach(Transform n in content)
        {
            Destroy(n.gameObject);
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, 10 + 30 * names.Count);

        for (int i = 0; i < names.Count; i++)
        {
            GameObject g = Instantiate(nameObject, content);
            g.GetComponent<Text>().text = names[i];
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25 - 30 * i);
        }

    }

    public void SelectName(string n)
    {
        InputField t = transform.Find("Name").GetComponent<InputField>();
        t.text = n;

        names.Clear();

        ScrollRect sr = transform.Find("Scroll View").GetComponent<ScrollRect>();
        sr.gameObject.SetActive(false);

        SetName(n);
    }


    public void SetName(string newName)
    {
        Name = newName;

        CheckActivityInside();
    }

    private void CheckActivityInside()
    {
        if(!isHovered)
        {
            ScrollRect sr = transform.Find("Scroll View").GetComponent<ScrollRect>();
            sr.gameObject.SetActive(false);
        }
        else
        {
            transform.Find("Name").GetComponent<InputField>().Select();
            transform.Find("Name").GetComponent<InputField>().ActivateInputField();
        }
    }

    public void SetHealth(string hp)
    {
        int.TryParse(hp, out int health);
        Health = health;
    }

    public void SetInitiative(string newInitiative)
    {
        int.TryParse(transform.Find("Initiative").GetComponent<InputField>().text, out int input);
        Initiative = input;
        FindObjectOfType<Controller>().SortContainers();
    }

    public override string ToString()
    {
        return $"{Name}:{Initiative}";
    }

    public override bool Equals(object other)
    {
        return other != null && other is Container && this.Initiative == ((Container)other).Initiative;
    }

    public override int GetHashCode()
    {
        return this.Initiative.GetHashCode();
    }

    public int CompareTo(object obj)
    {
        Container other = obj as Container;
        return this.Initiative.CompareTo(other.Initiative); //certain that CompareTo will return -1|0|1
    }

    public void SetNpc(bool isNpc)
    {
        IsNpc = isNpc;

        SetBackgroundColor();
    }

    public void Died(bool dead)
    {
        Dead = dead;

        transform.Find("Health").GetComponent<InputField>().text = dead ? "0" : "" + Health;

        SetBackgroundColor();
    }

    private void SetBackgroundColor()
    {
        if (!Dead)
        {
            GetComponent<Image>().color = IsNpc ? Color.clear : new Color(0, 0.2f, 0.6f, 0.25f);
        }
        else
        {
            GetComponent<Image>().color = new Color(0.8f, 0.1f, 0.1f, 1);
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
