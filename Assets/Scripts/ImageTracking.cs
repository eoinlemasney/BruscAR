using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;



[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    public Text scoreLabel;

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs)
        {
            //zero so that it starts hidden, defaukt rortaion
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            //make sure name is correct for searching for it later
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }
    }

private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
   private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedPrefabs[name];
        prefab.SetActive(true);

        foreach(GameObject go in spawnedPrefabs.Values)
        {
            if(go.name != name)
            {
                go.SetActive(false);
            }
        }
        if (name == "Recycling")
        {
            scoreLabel.text = "Item can be recycled. Tap Recycle Icon for more details.";
        }
        else if (name == "General")
        {
            scoreLabel.text = "Item cannot be recycled. Tap General Waste Icon for more details.";
        }
        else if (name == "Compost")
        {
            scoreLabel.text = "Item is compostable. Tap Compostable Icon for more details.";
        }


    }
}
