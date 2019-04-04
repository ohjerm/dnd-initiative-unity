using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private RectTransform Background;

    [SerializeField]
    private GameObject Instance;

    private List<Container> Containers = new List<Container>();
    public static string[] Names { get; private set; }
    public static int NumActors { get; private set; }

    private void Awake()
    {
        Screen.SetResolution(400, 220, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Names = File.ReadAllLines("Assets/Resources/names.txt");
        Debug.Log(Names.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            NextTurn();
        }

        NumActors = Containers.Count;
    }

    private void NextTurn()
    {
        int currentPosition = Containers.Count - 1;

        if (currentPosition == -1)
        {
            Debug.Log("Nothing in list");
            return; //nothing in list
        }

        bool firstTurn = true;

        for (int i = 0; i < Containers.Count; i++)
        {
            if (Containers[i].transform.Find("Turn").gameObject.activeSelf)
            {
                firstTurn = false;
                currentPosition = i;
                Containers[i].transform.Find("Turn").gameObject.SetActive(false);
            }
        }

        if(firstTurn)
        {
            //it's the first turn
        }
        else if (currentPosition == 0) //from the top
        {
            currentPosition = Containers.Count - 1;
        }
        else
        {
            currentPosition--;
        }

        Containers[currentPosition].transform.Find("Turn").gameObject.SetActive(true);
    }

    public void ClearAll()
    {
        foreach(Container c in Containers)
        {
            Destroy(c.gameObject);
        }

        Containers.Clear();
        Resize(0);
        RemoveTurnCounter();
    }

    public void ClearNpcs()
    {
        for(int i = Containers.Count - 1; i >= 0; i--)
        {
            if(Containers[i].IsNpc)
            {
                Destroy(Containers[i].gameObject);
                Containers.RemoveAt(i);
            }
        }

        Resize(Containers.Count);
        SortContainers();
        RemoveTurnCounter();
    }

    private void RemoveTurnCounter()
    {
        foreach(Container c in Containers)
        {
            c.transform.Find("Turn").gameObject.SetActive(false);
        }
    }

    public void OnAddNew()
    {
        GameObject g = Instantiate(Instance, Background);
        g.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        g.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
        g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        
        Container c = g.GetComponent<Container>();
        Containers.Add(c);

        Resize(Containers.Count);
        SortContainers();
    }

    internal void SortContainers()
    {
        Containers.Sort();
        for(int i = 0; i < Containers.Count; i++)
        {
            Containers[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 125 + i * 50);
        }
    }

    private void Resize(int num)
    {
        Screen.SetResolution(400, 220 + 50 * num, false);
        Background.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 220 + 50 * num);
    }
}
