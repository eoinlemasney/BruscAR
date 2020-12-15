using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;



[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    //AR related variables
    [SerializeField]
    private GameObject[] placeablePrefabs;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 fixedPosition;

    //Scene management variables
    public bool isTapped = false;
    public RaycastHit hit;
    public Ray ray;

    // UI text variables
    public Text itemInfoText;
    public Text itemNameText;
    public Text extraInfoText;
    public RawImage scanImageIcon;

    // Object modification variables
    public float speedRotation = 2f;
    public float maxRotation = 30f;

    // Audio variables
    public AudioClip recyclingSound;
    public AudioClip bingSound;
    public AudioClip invalidBeep;
    public AudioSource audioS;

    //User session saved variables
    static int scannedCount;
    static int recycleItemsScannedCount;
    static int generalItemsScannedCount;
    static int compostItemsScannedCount;


    void Start()
    {
        // Get the users count of scanned items from previous sessions
        scannedCount = PlayerPrefs.GetInt("scanned_count");
        recycleItemsScannedCount = PlayerPrefs.GetInt("recycle_scanned_count");
        generalItemsScannedCount = PlayerPrefs.GetInt("general_scanned_count");
        compostItemsScannedCount = PlayerPrefs.GetInt("compost_scanned_count");
    }


    private void Awake()
    {   //Store a refrence to the AR tracked image manager
        // When the scene is loaded, instantiate each of the virtual prefabs and store them in
        // a dictionary with their name as the key.
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placeablePrefabs)
        {
            //roatate objects to face camera
            Vector3 xyz = new Vector3(0f, 180f, 0f);
            Quaternion correctRotation = Quaternion.Euler(xyz);
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, correctRotation);

            //stores the name of the prefab as its key, for cross reference with images later
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
            prefab.SetActive(false);

        }
    }

    private void OnEnable()
    {
        // When an image is added to the trackedImageManager, call the ImageChanged function
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }
    private void OnDisable()
    {
        // When an image is removed from the trackedImageManager, call the ImageChanged function
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // If the image being tracked is changed, update the scene with the new image
        // remove the previous AR objects
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }


    private void UpdateARImage(ARTrackedImage trackedImage)
    {
        //Get the name of the current tracked image
        // set the fixed position to this slightly above and behind this fixed image, this will be used to anchor the object
        //Identify the the items group e.g. Recylable, General or Compost. Update UI accordingly
        // Set the users scan count

        string raw_image_name = trackedImage.referenceImage.name;
        scanImageIcon.enabled = false;
        Vector3 position = trackedImage.transform.position + trackedImage.transform.up * 0.05f + trackedImage.transform.forward * -0.1f;
        fixedPosition = position;


        ReturnItemName(raw_image_name);
        string name = DetectImageGroup(raw_image_name);
        AssignGameObject(name, position);
        scannedCount += 1;
        PlayerPrefs.SetInt("scanned_count", scannedCount);
        PlayerPrefs.SetInt("general_scanned_count", generalItemsScannedCount);
        PlayerPrefs.SetInt("compost_scanned_count", compostItemsScannedCount);
        PlayerPrefs.SetInt("recycle_scanned_count", recycleItemsScannedCount);
        PlayerPrefs.Save();






    }

    public string DetectImageGroup(string raw_image_name)
    {
        //Identify the the items group e.g. Recylable, General or Compost. Update UI accordingly
        if (raw_image_name.Contains("Recycling"))
        {
            recycleItemsScannedCount += 1;
            return "Recycling";
        }
        else if (raw_image_name.Contains("General"))
        {
            generalItemsScannedCount += 1;
            return "General";
        }

        else if (raw_image_name.Contains("Compost"))
        {
            compostItemsScannedCount += 1;
            return "Compost";
        }
        else
        {
            return raw_image_name;
        }
    }

    void ReturnItemName(string raw_image_name)
    {
        //Identify the scanned items name based on the raw image name.

        if (raw_image_name.Contains("CokeCan"))
        {
            itemNameText.text = "Coke Can Detected.";
        }
        else if (raw_image_name.Contains("Innocent"))
        {
            itemNameText.text = "Innocent Bottle Detected.";

        }

        else if (raw_image_name.Contains("BarrysTea"))
        {
            itemNameText.text = "Barry's Tea Detected.";

        }

        else if (raw_image_name.Contains("TerrysChocolateGeneral"))
        {
            itemNameText.text = "Terry's Chocolate Orange Detected.";

        }
        else
        {
            itemNameText.text = "";
        }

    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        // Based on the items grou name, assign its AR object bin e.g. dreen, blue, or red and set it to active
        // Update the UI text accordingy.
        if (placeablePrefabs != null)
        {
            spawnedPrefabs[name].SetActive(true);
            spawnedPrefabs[name].transform.position = newPosition;
            spawnedPrefabs[name].transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            spawnedPrefabs[name].transform.localScale = scaleFactor;
            foreach (GameObject go in spawnedPrefabs.Values)
            {
                if (go.name != name)
                {
                    go.SetActive(false);
                }
                else
                {
                    UpdateItemText(name);

                }
            }

        }




    }

    void Update()
    {
        // On the update function, get the active scene objects
        // If they have not been tapped, rotate them and update UI with general content
        // else if they have been taped, make the object sway and update UI with more detailed conent
        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if (go.activeInHierarchy == true)
            {
                go.transform.position = fixedPosition;
                if (isTapped == false)
                {
                    RotateObject(go, 2f);

                    extraInfoText.text = "";
                    UpdateItemText(go.transform.name);

                }
                else
                {
                    SwayObject(go);
                    UpdateExtraInfo(go);
                    itemInfoText.text = "";


                }
            }

        }

        // Check for mouse down event on object
        // If tapped, update the isTapped boolean.
        // If tapped, play the objets corresponding sound
        if (Input.GetMouseButtonDown(0))
        {

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name.Contains("Recycling"))
                {

                    PlayParticles(hit.collider.gameObject);

                    if (isTapped == false)
                    {
                        audioS.PlayOneShot(recyclingSound);
                        isTapped = true;

                    }
                    else
                    {
                        isTapped = false;
                    }
                }

                else if (hit.collider.gameObject.name == "General")
                {
                    PlayParticles(hit.collider.gameObject);

                    if (isTapped == false)
                    {
                        audioS.PlayOneShot(invalidBeep);
                        isTapped = true;
                    }
                    else
                    {
                        isTapped = false;
                    }
                }

                else if (hit.collider.gameObject.name == "Compost")
                {
                    PlayParticles(hit.collider.gameObject);

                    if (isTapped == false)
                    {
                        audioS.PlayOneShot(bingSound);
                        isTapped = true;
                    }
                    else
                    {
                        isTapped = false;
                    }
                }




            }
        }

    }
    void SwayObject(GameObject activeObject)
    {
        //Sway the object 180 degrees on y axis

        activeObject.transform.rotation = Quaternion.Euler(0f, 180f, maxRotation * Mathf.Sin(Time.time * speedRotation));

    }

    void RotateObject(GameObject activeObject, float rotateSpeed)
    {
        // Rotate the object by the passed in speed

        activeObject.transform.Rotate(0, rotateSpeed, 0);
    }

    void UpdateExtraInfo(GameObject activeObject)
    {
        // Update the UI with extra info when the item is tapped.
        string name = activeObject.name;

        if (name == "Recycling")
        {
            extraInfoText.text = "Can should be Clean, Dry & Loose!";
        }
        else if (name == "General")
        {
            extraInfoText.text = "Unfortunately not all items are recycling. Check Tips&Tricks for more information.";
        }
        else if (name == "Compost")
        {
            extraInfoText.text = "Item is compostable.";
        }
    }

    void UpdateItemText(string name)
    {
        // Update the UI with general info wheen the item is not tapped
        if (name.Contains("Recycling"))
        {
            itemInfoText.text = "Item can be recycled. Tap Recycle Icon for more details.";

        }
        else if (name == "General")
        {
            itemInfoText.text = "Item cannot be recycled. Tap General Waste Icon for more details!";
        }
        else if (name == "Compost")
        {
            itemInfoText.text = "Item is compostable. Tap Compostable Icon for more details.";

            if (itemNameText.text == "Barry's Tea Detected.")
            {
                itemInfoText.text = "Tea bags are compostable but the box must be recycled.";
            }
        }

    }

    void PlayParticles(GameObject activeObject)
    {
        // Check if the child particle system of the object is playing. If so, stop it and if not, play it.
        if (!activeObject.transform.Find("Particle System").GetComponent<ParticleSystem>().isPlaying)
        {

            activeObject.transform.Find("Particle System").GetComponent<ParticleSystem>().Play();
        }
        else
        {
            activeObject.transform.Find("Particle System").GetComponent<ParticleSystem>().Stop();
        }

    }
}




