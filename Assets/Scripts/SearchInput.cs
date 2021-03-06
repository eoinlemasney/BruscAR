﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Serialization;

using Newtonsoft.Json;

[System.Serializable]
public class RecycleObject
{
    public string name;
    public string image_name;
    public string recyclable;
    public string instructions; 
}

[System.Serializable]
public class RootObject
{
    public RecycleObject[] recycleItems;
}

public class SearchInput : MonoBehaviour
{
    public InputField SearchBar;
    public RootObject recycle_data = new RootObject();

    private Sprite imageToLoad;
    public GameObject itemIndicator;
    public GameObject itemImage;
    public GameObject itemRecyclable;
    public GameObject itemInstructions;
    public Image ItemFoundImage;
    public Text ItemFoundLabel;
    public Text IsRecyclableLabel;
    public Text InstructionsLabel;


    // Load Data from JSON File for searchable data base
    void loadRecycleData()
    {
        Debug.Log("Reading Data");
        Debug.Log("Loading Database as Resource");
        var jsonFile = Resources.Load<TextAsset>("recycle_database");
        recycle_data = JsonUtility.FromJson<RootObject>("{\"recycleItems\":" + jsonFile + "}");
        Debug.Log("Data Loaded");
    }

    public void printUserText()
    {
        Debug.Log(SearchBar.text);
    }

    // Print data
    public void printData()
    {
        foreach(RecycleObject item in recycle_data.recycleItems){
            string data = "Name: " + item.name + ". Is it recyclable: " + item.recyclable;
            print(data);
        }
    }
    // Search data for user input and print back
    public void searchData()
    {
        if (SearchBar.text != "")
        {
            string SearchKey = SearchBar.text.ToLower();
            foreach(RecycleObject item in recycle_data.recycleItems)
            {
               if (item.name == SearchKey)
               {
                    print("FOUND ITEM");
                    itemFound(item);
                    return;
               } 
            }
            print("ITEM NOT FOUND");
            itemNotFound();
        }
        else
        {
            print("User needs to give input");
        }
    }

    void loadImage(RecycleObject item)
    {
        Debug.Log(Application.dataPath + item.image_name);
        imageToLoad = Resources.Load<Sprite>(item.image_name);
        Debug.Log(imageToLoad);
        itemImage.GetComponent<Image>().sprite = imageToLoad;
    }

    public void itemFound(RecycleObject item)
    {
        loadImage(item);
        itemIndicator.SetActive(true);
        itemImage.SetActive(true);
        itemRecyclable.SetActive(true);
        itemInstructions.SetActive(true);
        ItemFoundLabel.text = "Item Found";
        IsRecyclableLabel.text = "Can it be Recycled: " + item.recyclable;
        InstructionsLabel.text = "Extra Instructions: " + item.instructions;
    }

    public void itemNotFound()
    {
        itemIndicator.SetActive(true);
        itemImage.SetActive(false);
        itemRecyclable.SetActive(false);
        itemInstructions.SetActive(false);
        ItemFoundLabel.text = "Item Not Found";
    }

    void Start()
    {
        Debug.Log("Loading Recycle Data");
        loadRecycleData();
    }
}
