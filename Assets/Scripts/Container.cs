using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour, IComparable
{
    public int Initiative { get; private set; }
    public string Name { get; private set; }
    public bool IsNpc { get; private set; }
    public bool Dead { get; private set; }
    public int Health { get; private set; }

    private void Awake()
    {
        Initiative = 0;
        Name = "";
        IsNpc = true;
        Dead = false;
    }


    public void SetName(string newName)
    {
        Name = newName;
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
}
