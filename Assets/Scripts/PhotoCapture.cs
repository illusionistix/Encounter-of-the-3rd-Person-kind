using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PhotoCapture : MonoBehaviour
{
    private AudioSource clickAudio;

    [SerializeField] private Text noPathWarningText;
    [SerializeField] private InputField savePathInputField;    
    [SerializeField] private Text placeholderText;
    
    [SerializeField] private RawImage scPreviewImage;
    private Texture2D scTexture;

    [SerializeField] private CanvasGroup pauseMenuCanvas;
    CanvasGroup m_pauseMenuCanvas = null;

    [SerializeField] private CanvasGroup brandImageCanvas;
    CanvasGroup m_brandImageCanvas = null;

    private bool isCapturingScreenshot;
    private bool isScreenshotCaptured;

    private const string scFileName = "Screenshot";
    //private string scFolderPath;

    private static int counter = 0;
    private string savePath ()=> MainManager.Instance.screenshotsPath + "/" + scFileName + counter + ".png";


    // Start is called before the first frame update
    void Awake()
    {
        if (Directory.Exists(MainManager.Instance.screenshotsPath) &&
            Directory.GetFiles(MainManager.Instance.screenshotsPath).Length > 0)
        {
            counter = Directory.GetFiles(MainManager.Instance.screenshotsPath).Length;
            placeholderText.text = MainManager.Instance.screenshotsPath;
            LoadAndApplyTexture();
        }
        
        noPathWarningText.gameObject.SetActive(false);
        clickAudio = GetComponent<AudioSource>();
        m_pauseMenuCanvas = pauseMenuCanvas;
        m_brandImageCanvas = brandImageCanvas;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (savePathInputField.text != "" && MainManager.Instance.screenshotsPath == "")
            {
                MainManager.Instance.screenshotsPath = savePathInputField.text;
                LoadAndApplyTexture();
            }
            
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            TogglePreviewImage();
        }


        if (MainManager.Instance.screenshotsPath != "")
        {
            placeholderText.text = MainManager.Instance.screenshotsPath;
        }
        else
        {
            placeholderText.text = "Enter Folder Path...";
        }

        if (savePathInputField.isFocused)
        {
            MainManager.Instance.isInputFieldFocused = true;
            noPathWarningText.gameObject.SetActive(false);
        }
        else
        {
            MainManager.Instance.isInputFieldFocused = false;
        }
        

        scPreviewImage.enabled = scPreviewImage.texture != null;

        
        if (isCapturingScreenshot)
        {
            m_pauseMenuCanvas.alpha = 0;
            m_brandImageCanvas.alpha = 1;

            clickAudio.Play();

            if (Directory.Exists(MainManager.Instance.screenshotsPath) &&
            Directory.GetFiles(MainManager.Instance.screenshotsPath).Length > 0)
            {
                counter = Directory.GetFiles(MainManager.Instance.screenshotsPath).Length;                
            }

            counter += 1;
            
            ScreenCapture.CaptureScreenshot(savePath());

            isCapturingScreenshot = false;
            isScreenshotCaptured = true;

            return;
        }


        if (isScreenshotCaptured)
        {
            isScreenshotCaptured = false;
            StartCoroutine(RefreshPreviewTimer());
        }

        

    }


    public void TakePicture()
    {

        if (savePathInputField.text == "" && MainManager.Instance.screenshotsPath == "")
        {
            noPathWarningText.gameObject.SetActive(true);
            noPathWarningText.text = "- Please enter a Folder Path -" + System.Environment.NewLine 
                + "Example: C:/Users/Public/Pictures";
        }

        if (savePathInputField.text != "" && MainManager.Instance.screenshotsPath == "")
        {
            noPathWarningText.gameObject.SetActive(false);
            MainManager.Instance.screenshotsPath = savePathInputField.text;

            if (!Directory.Exists(MainManager.Instance.screenshotsPath))
            {
                Directory.CreateDirectory(MainManager.Instance.screenshotsPath);
                placeholderText.text = MainManager.Instance.screenshotsPath;

                isCapturingScreenshot = true;
            }
            else
            {
                placeholderText.text = MainManager.Instance.screenshotsPath;

                isCapturingScreenshot = true;
            }
        }


        if (savePathInputField.text == "" && MainManager.Instance.screenshotsPath != "")
        {
            if (Directory.Exists(MainManager.Instance.screenshotsPath))
            {
                noPathWarningText.gameObject.SetActive(false);
                placeholderText.text = MainManager.Instance.screenshotsPath;
                isCapturingScreenshot = true;
            }
            else
            {
                noPathWarningText.gameObject.SetActive(true);
            }

        }

        if (savePathInputField.text != "" && MainManager.Instance.screenshotsPath != "")
        {
            noPathWarningText.gameObject.SetActive(false);
            MainManager.Instance.screenshotsPath = savePathInputField.text;

            if (!Directory.Exists(MainManager.Instance.screenshotsPath))
            {
                Directory.CreateDirectory(MainManager.Instance.screenshotsPath);
                placeholderText.text = MainManager.Instance.screenshotsPath;

                isCapturingScreenshot = true;
            }
            else
            {
                placeholderText.text = MainManager.Instance.screenshotsPath;

                isCapturingScreenshot = true;
            }

        }

    }

    private void LoadAndApplyTexture()
    {
        if (File.Exists(savePath()))
        {
            var bytes = File.ReadAllBytes(savePath());

            scTexture = new Texture2D(2, 2);
            scTexture.LoadImage(bytes);
            scTexture.Apply();
            scPreviewImage.texture = scTexture;
        }
    }


    IEnumerator RefreshPreviewTimer()
    {
        m_pauseMenuCanvas.alpha = 1;
        m_brandImageCanvas.alpha = 0;

        yield return new WaitForSeconds(0.5f);

        LoadAndApplyTexture();
    }


    private void TogglePreviewImage()
    {
        if (scPreviewImage.texture != null && !savePathInputField.isFocused && 
            !MainManager.Instance.isGameActive)
        {
            if (scPreviewImage.gameObject.activeSelf)
            {
                scPreviewImage.gameObject.SetActive(false);
            }
            else
            {
                scPreviewImage.gameObject.SetActive(true);
            }
        }
    }


}
