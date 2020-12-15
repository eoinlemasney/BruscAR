using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserDetails : MonoBehaviour
{
    public Text userItemsScannedLabel;
    public Text recycleItemsItemsScannedLabel;
    public Text generalItemsScannedLabel;
    public Text compostItemsScannedLabel;

    int itemsScanned;
    int recycleItemsScannedCount;
    int generalItemsScannedCount;
    int compostItemsScannedCount;
    static int itemsScannedCount;

    void loadUserData()
    {
        string path = Application.dataPath + "/UserData/data.txt";
        //Read the text from data file. Only storing number of items scanned but this logic might change
        StreamReader reader = new StreamReader(path);
        itemsScanned = int.Parse(reader.ReadLine());
        reader.Close();
    }

    void Start()
    {
        // On start, load in past session saved conent
        // This content records the number of items that have been scanned.

        itemsScannedCount = PlayerPrefs.GetInt("scanned_count");
        recycleItemsScannedCount = PlayerPrefs.GetInt("recycle_scanned_count");
        generalItemsScannedCount = PlayerPrefs.GetInt("general_scanned_count");
        compostItemsScannedCount = PlayerPrefs.GetInt("compost_scanned_count");

        userItemsScannedLabel.text = "Total Items Scanned: " + itemsScannedCount;
        recycleItemsItemsScannedLabel.text = "Recyclable Items Scanned: " + recycleItemsScannedCount;
        generalItemsScannedLabel.text = "General Items Scanned: " + generalItemsScannedCount;
        compostItemsScannedLabel.text = "Compostable Items Scanned: " + compostItemsScannedCount;
    }
}
