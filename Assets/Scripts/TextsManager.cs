using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextsManager : MonoBehaviour
{

    [SerializeField] private GameObject welcomeText;
    [SerializeField] private GameObject secretText;
    [SerializeField] private GameObject jetText;
    [SerializeField] private GameObject carText;
    [SerializeField] private GameObject limitsText;

    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MainManager.Instance.isGameActive)
        {
            ToggleWelcomeText();
            ToggleSecretText();
            ToggleJetJoyRideText();
            ToggleCarJoyRideText();
            ToggleLevelLimtsText();

            FadeText();
                        
        }        
        
    }


    private void FadeText()
    {
        if (welcomeText.GetComponent<Text>().color.a > 0f || 
            secretText.GetComponent<Text>().color.a > 0f || 
            jetText.GetComponent<Text>().color.a > 0f ||
            carText.GetComponent<Text>().color.a > 0f || 
            limitsText.GetComponent<Text>().color.a > 0f)
        {
            welcomeText.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime);
            secretText.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime);
            jetText.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime);
            carText.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime);
            limitsText.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime);
        }
    }

    
    IEnumerator TextFadeTimer(GameObject textObject)
    {
        textObject.GetComponent<Text>().color = new Color(
            textObject.GetComponent<Text>().color.r,
            textObject.GetComponent<Text>().color.g,
            textObject.GetComponent<Text>().color.b, 1f); ;
        

        yield return new WaitForSeconds(10f);

        textObject.SetActive(false);
    }
    


    private void ToggleWelcomeText()
    {
        if (MainManager.Instance.playerFirstWelcome)
        {
            welcomeText.SetActive(true);
            StartCoroutine(TextFadeTimer(welcomeText));
            MainManager.Instance.playerFirstWelcome = false;
        }        
    }


    private void ToggleSecretText()
    {
        if (MainManager.Instance.playerFoundSecret)
        {
            secretText.SetActive(true);
            StartCoroutine(TextFadeTimer(secretText));
            MainManager.Instance.playerFoundSecret = false;
        }
        
    }


    private void ToggleJetJoyRideText()
    {
        if (MainManager.Instance.playerNearJetVehicle)
        {
            jetText.SetActive(true);
            StartCoroutine(TextFadeTimer(jetText));
            MainManager.Instance.playerNearJetVehicle = false;
        }
        
    }


    private void ToggleCarJoyRideText()
    {
        if (MainManager.Instance.playerNearCarVehicle)
        {
            carText.SetActive(true);
            StartCoroutine(TextFadeTimer(carText));
            MainManager.Instance.playerNearCarVehicle = false;
        }
        
    }


    private void ToggleLevelLimtsText()
    {
        if (MainManager.Instance.playerNearLimits)
        {
            limitsText.SetActive(true);
            StartCoroutine(TextFadeTimer(limitsText));
            MainManager.Instance.playerNearLimits = false;
        }
        
    }


}
