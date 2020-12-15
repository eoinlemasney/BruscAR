using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserDetails : MonoBehaviour
{
	public Text userItemsScannedLabel;

	int itemsScanned;

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
    	//loadUserData();
    	//userItemsScannedLabel.text = "Items Scanned: " + itemsScanned;
    }
}
