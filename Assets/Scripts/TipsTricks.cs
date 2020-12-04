using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsTricks : MonoBehaviour
{
	public string FirstArticleURL, SecondArticleURL;

    public void LaunchFirstArticle()
    {
    	Application.OpenURL(FirstArticleURL);
    }

    public void LaunchSecondArticle()
    {
    	Application.OpenURL(SecondArticleURL);
    }
}
