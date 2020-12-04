using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelLauncher : MonoBehaviour
{

	public GameObject MenuOrigPosition;
	public GameObject MenuActivePosition;
	public GameObject MenuPanel;

	private bool ActivatePanel, DeactivatePanel;

    // Start is called before the first frame update
    void Start()
    {
        MenuPanel.transform.position = MenuOrigPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActivatePanel)
        {
        	MenuPanel.transform.position = MenuActivePosition.transform.position;
        }
        if (DeactivatePanel)
        {
        	MenuPanel.transform.position = MenuOrigPosition.transform.position;
        }
    }

    public void MovePanel()
    {
    	DeactivatePanel = false;
    	ActivatePanel = true;
    }

    public void MovePanelBack()
    {
    	DeactivatePanel = true;
    	ActivatePanel = false;
    }
}
