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

    public Text itemInfoText;
    public Text itemNameText;
    public Text extraInfoText;

    public RawImage scanImageIcon;

    private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);

    private Vector3 fixedPosition;

    public float speedRotation = 2f;
    public float maxRotation = 30f;

    public RaycastHit hit;
    public Ray ray;

    public AudioClip recyclingSound;
    public AudioClip bingSound;
    public AudioClip invalidBeep;

    public AudioSource audioS;

    //initiate floating text
    public GameObject floatingTextPrefab;
    public GameObject ParticleSystem;

    public bool isTapped = false;
    static int scannedCount;

    public string currentImage;

    public string image_name;

    private void Awake()
    {
        Debug.Log("Awake");
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placeablePrefabs)
        {
            //zero so that it starts hidden, default rortation
            Vector3 xyz = new Vector3(0f, 180f, 0f);
            Quaternion correctRotation = Quaternion.Euler(xyz);
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, correctRotation);

            //make sure name is correct for searching for it laters
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
            prefab.SetActive(false);
    
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
        string raw_image_name = trackedImage.referenceImage.name;
        scanImageIcon.enabled = false;
        Vector3 position = trackedImage.transform.position + trackedImage.transform.up * 0.05f + trackedImage.transform.forward * 0.2f;
        fixedPosition = position;


        ReturnItemName(raw_image_name);
        string name = DetectImageGroup(raw_image_name);
        AssignGameObject(name, position);
        scannedCount += 1;
        PlayerPrefs.SetInt("scanned_count", scannedCount);
        PlayerPrefs.Save();






    }

    public string DetectImageGroup(string raw_image_name)
    {
        if (raw_image_name.Contains("Recycling"))
        {
            return "Recycling";
        }
        else if (raw_image_name.Contains("General"))
        {
            return "General";
        }

        else if (raw_image_name.Contains("Compost"))
        {
            return "Compost";
        } else
        {
            return raw_image_name;
        }
    }

    void ReturnItemName(string raw_image_name)
    {
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

                    } else
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
                    } else
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

            activeObject.transform.rotation = Quaternion.Euler(0f, 180f, maxRotation * Mathf.Sin(Time.time * speedRotation));

        }

        void RotateObject(GameObject activeObject, float rotateSpeed)
        {

            activeObject.transform.Rotate(0, rotateSpeed, 0);
        }

        void UpdateExtraInfo(GameObject activeObject)
        {
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




