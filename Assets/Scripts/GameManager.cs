using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{    

    [SerializeField] private GameObject woodPlankPrefab;
    //[SerializeField] private GameObject woodPlankStaticPrefab;
    //[SerializeField] private GameObject woodPlankDynamicPrefab;
    //private GameObject[] woodPlanks;
    private GameObject[] woodPlanksTower;
    private GameObject[] woodPlanksDomino;
    private GameObject woodPlank;
    //private GameObject woodPlankStatic;
    //private GameObject woodPlankDynamic;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject hoverBoardPrefab;
    private GameObject hoverBoard;
    private MeshRenderer[] boardRenderers;
    private MeshRenderer boardRenderer;
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text controlsInfoText;
    [SerializeField] private Text infoText;
    [SerializeField] private Image energyBarFill;
    [SerializeField] private Image aimImage;

    [SerializeField] private GameObject energyBar;
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject photoModeMenu;

    [SerializeField] private GameObject barrelsPrefab;
    private GameObject barrels;    

    [SerializeField] private GameObject bGuyCubesPrefab;
    private GameObject bGuyCubes;

    [SerializeField] private GameObject hazardPrefab;
    private GameObject hazardInst;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundFXVolumeSlider;
    [SerializeField] private Slider ambienceVolumeSlider;

    [SerializeField] private int planksCount;
    [SerializeField] private float amp;
    [SerializeField] private float freq;
    [SerializeField] private float fac;


    //[SerializeField] private GameObject cubeObstacleStatic;

    //[SerializeField] private Text barrelsInfoText;
    //[SerializeField] private RawImage bGuyImage;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0";        
        
        woodPlank = null;
        //planksCount = 48;        
        //woodPlanks = new GameObject[planksCount];
        woodPlanksTower = new GameObject[48];
        woodPlanksDomino = new GameObject[36];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;        

        MainManager.Instance.energy = 100f;
        MainManager.Instance.playerObject = playerPrefab;

        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);

        musicVolumeSlider.value = MainManager.Instance.musicVolume;
        soundFXVolumeSlider.value = MainManager.Instance.soundFXVolume;
        ambienceVolumeSlider.value = MainManager.Instance.ambienceVolume;


        hoverBoard = Instantiate(hoverBoardPrefab, new Vector3(-1f, 23f, 0f),
            hoverBoardPrefab.transform.rotation);

        boardRenderers = hoverBoard.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < boardRenderers.Length; i++)
        {
            boardRenderers[i].gameObject.SetActive(false);
        }

        boardRenderers[0].gameObject.SetActive(true);

        SpawnHazards();        

    }


    // Update is called once per frame
    void Update()
    {

        if (MainManager.Instance.isGameActive)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                GoToJetCam();
            }

            UpdateEnergyBar();

            if (!MainManager.Instance.isPlayerInJet)
            {
                UpdateScoreText();                

                //if (hazardPrefab == null)
                //{
                //    SpawnHazards();
                //}                

                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    ReSpawnRubbish();
                }

                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    PlankTowerTest();
                }

                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    PlankDominoTest();
                }

                Physics.autoSimulation = true;                
                SwitchBoards();

                if (Input.GetKeyDown(KeyCode.I))
                {
                    string br = System.Environment.NewLine;
                    //controlsInfoText.text = "[A] [W] [S] [D] Move\n[LMB] Fire Projectile";
                    controlsInfoText.text = "[A] [W] [S] [D] Move" + br + "[LMB] Fire Projectile" + br 
                        + "[RMB] Attract Object" + br + "[E] Launch Object" + br + "[CTRL] Dash" + br + "[F] Float Up";

                    if (controlsInfoText.color.a < 1)
                    {
                        controlsInfoText.color += new Color(0f, 0f, 0f, 1f);
                    }

                    if (infoText.color.a > 0)
                    {
                        infoText.color -= new Color(0f, 0f, 0f, 0.5f);
                    }
                }

                if (controlsInfoText.color.a > 0)
                {
                    controlsInfoText.color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime * 2f);
                }
                else
                {
                    if (infoText.color.a < 0.5)
                    {
                        infoText.color += new Color(0f, 0f, 0f, 0.5f);
                    }
                }
            }
            else
            {
                string br = System.Environment.NewLine;                
                controlsInfoText.text = "[A] [D] [<-] [->] Steer" + br + "[M] Move" + br
                    + "[B] Boost";

                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (controlsInfoText.color.a < 1)
                    {
                        controlsInfoText.color += new Color(0f, 0f, 0f, 1f);                        
                    }

                    if (infoText.color.a > 0)
                    {
                        infoText.color -= new Color(0f, 0f, 0f, 0.5f);
                    }
                }

                if (controlsInfoText.color.a > 0)
                {
                    controlsInfoText.color -= new Color(0f, 0f, 0f, 0.1f * Time.deltaTime * 2f);
                }
                else
                {
                    if (infoText.color.a < 0.5)
                    {
                        infoText.color += new Color(0f, 0f, 0f, 0.5f);
                    }
                }
            }
            
        }
        else
        {
            Physics.autoSimulation = false;            
        }

        
        TogglePauseMenuCanvas();        
        KeyToPauseMenu();

        MainManager.Instance.musicVolume = musicVolumeSlider.value;
        MainManager.Instance.soundFXVolume = soundFXVolumeSlider.value;
        MainManager.Instance.ambienceVolume = ambienceVolumeSlider.value;
    }


    private void SpawnHazards()
    {
        for (int i = 0; i < 50; i++)
        {
            float posX = Random.Range(-240f, 240f);
            float posY = Random.Range(2.5f, 50f);
            float posZ = Random.Range(-240f, 240f);
            Vector3 hazSpawnPos = new Vector3(posX, posY, posZ);
            hazardInst = Instantiate(hazardPrefab, hazSpawnPos, hazardPrefab.transform.rotation);
        }
    }


    private void UpdateScoreText()
    {
        scoreText.text = "" + MainManager.Instance.score;
    }

    private void UpdateEnergyBar()
    {
        energyBarFill.transform.localScale = new Vector3(MainManager.Instance.energy / 100f, 1f, 1f);
    }


    private void GoToJetCam()
    {
        if (!MainManager.Instance.isPlayerInJet)// && MainManager.Instance.playerNearJetVehicle)
        {
            MainManager.Instance.isPlayerInJet = true;
            Physics.gravity = new Vector3(0f, 0f, 0f);
            aimImage.gameObject.SetActive(false);
        }
        else
        {
            MainManager.Instance.isPlayerInJet = false;
            Physics.gravity = new Vector3(0f, -9.8f, 0f) * 3f;
            aimImage.gameObject.SetActive(true);
            //transform.position = enterJetPosition;                
        }
    }

    /*
    IEnumerator BarrelsTextIntroTimer()
    {
        yield return new WaitForSeconds(5f);

        bGuyImage.gameObject.SetActive(false);

        StartCoroutine(BarrelsTextFadeTimer());
    }

    IEnumerator BarrelsTextFadeTimer()
    {
        barrelsInfoText.color = new Color(barrelsInfoText.color.r, barrelsInfoText.color.g,
            barrelsInfoText.color.b, 1f);
        barrelsInfoText.text = "Press [B] To Neatly Spawn A Bunch Of Barrels";

        yield return new WaitForSeconds(10f);

        barrelsInfoText.gameObject.SetActive(false);
    }*/

    public void TogglePauseMenuCanvas()
    {
        if (Input.GetKeyDown(KeyCode.P) && !MainManager.Instance.isInputFieldFocused)
        {
            if (MainManager.Instance.isGameActive)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                
                pauseMenuCanvas.SetActive(true);
                energyBar.SetActive(false);
                aimImage.gameObject.SetActive(false);
                scoreText.gameObject.SetActive(false);
                infoText.gameObject.SetActive(false);
                controlsInfoText.gameObject.SetActive(false);
                MainManager.Instance.isGameActive = false;
            }
            else
            {                
                pauseMenuCanvas.SetActive(false);
                energyBar.SetActive(true);

                if (!MainManager.Instance.isPlayerInJet)
                {
                    aimImage.gameObject.SetActive(true);
                }                

                scoreText.gameObject.SetActive(true);
                infoText.gameObject.SetActive(true);
                controlsInfoText.gameObject.SetActive(true);
                MainManager.Instance.isGameActive = true;
                
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }        
    }


    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        energyBar.SetActive(true);

        if (!MainManager.Instance.isPlayerInJet)
        {
            aimImage.gameObject.SetActive(true);
        }

        scoreText.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);
        controlsInfoText.gameObject.SetActive(true);
        MainManager.Instance.isGameActive = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public void QuitToMainMenu()
    {
        //MainManager.Instance.SaveDataToFile();
        SceneManager.LoadScene(0);
    }


    public void ExitGame()
    {
        MainManager.Instance.SaveDataToFile();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }


    private void KeyToPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.M) && !MainManager.Instance.isInputFieldFocused)
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
            photoModeMenu.SetActive(false);
        }
    }


    public void ButtonToPauseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        photoModeMenu.SetActive(false);
    }


    public void ButtonToOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }


    public void ButtonToPhotoModeMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        photoModeMenu.SetActive(true);
    }


    private void SwitchBoards()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            for (int i = 0; i < boardRenderers.Length; i++)
            {
                boardRenderers[i].gameObject.SetActive(false);
            }

            boardRenderers[0].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < boardRenderers.Length; i++)
            {
                boardRenderers[i].gameObject.SetActive(false);
            }

            boardRenderers[1].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            for (int i = 0; i < boardRenderers.Length; i++)
            {
                boardRenderers[i].gameObject.SetActive(false);
            }

            boardRenderers[2].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            for (int i = 0; i < boardRenderers.Length; i++)
            {
                boardRenderers[i].gameObject.SetActive(false);
            }

            boardRenderers[3].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            for (int i = 0; i < boardRenderers.Length; i++)
            {
                boardRenderers[i].gameObject.SetActive(false);
            }

            boardRenderers[4].gameObject.SetActive(true);
        }
    }


    private void ReSpawnRubbish()
    {
        if (barrels == null)
        {
            barrels = Instantiate(barrelsPrefab, Vector3.zero, barrelsPrefab.transform.rotation);
        }
        else
        {
            Destroy(barrels.gameObject);
        }

        if (bGuyCubes == null)
        {
            bGuyCubes = Instantiate(bGuyCubesPrefab, Vector3.zero, bGuyCubesPrefab.transform.rotation);
        }
        else
        {
            Destroy(bGuyCubes.gameObject);
        }
    }


    private void PlankTowerTest()
    {
        planksCount = 48;

        if (woodPlank == null)
        {
            for (int i = 0; i < planksCount; i++)
            {
                fac = 1f;
                amp = 50f;
                freq = (-i * (2 * Mathf.PI / planksCount)) * fac;
                float rotY = (-i * (-360f / planksCount)) * fac;
                Vector3 pos = new Vector3(-20f + Mathf.Sin(freq) * amp, 0.5f + i, -100f - Mathf.Cos(freq) * amp);
                

                woodPlank = Instantiate(woodPlankPrefab, pos, woodPlankPrefab.transform.rotation);
                woodPlank.transform.eulerAngles = new Vector3(0f, 90f + rotY, 0f);

                woodPlanksTower.SetValue(woodPlank, i);

                woodPlanksTower[i].GetComponent<Rigidbody>().isKinematic = true;
            }

        }
        else
        {
            for (int i = 0; i < planksCount; i++)
            {
                Destroy(woodPlanksTower[i].gameObject);
            }

        }
        
    }


    private void PlankDominoTest()
    {
        planksCount = 36;

        

        if (woodPlank == null)
        {

            for (int i = 0; i < planksCount; i++)
            {
                Vector3 pos = new Vector3(-20f, 5f, -100f);

                int count = 12;

                fac = -0.545f;
                amp = 10f;

                freq = (i * (2 * Mathf.PI / count));
                float rotY = (i * (-360f / count));

                float posX = Mathf.Sin(freq * fac) * amp;
                float posZ = -Mathf.Cos(freq * fac) * amp;

                if (i <= 11)
                {
                    pos = new Vector3(-20f + posX, 5f, -100f + posZ);
                }
                if (i > 11)
                {
                    count = 24;
                    pos = new Vector3(-47.5f + (i * 2.5f), 5f, -110f);
                    rotY = 0f;
                }
                if (i > 23)
                {
                    count = 36;
                    pos = new Vector3(-47.5f + ((i * 2.5f) - 30), 5f, -90f);
                    rotY = 0f;

                }

                woodPlank = Instantiate(woodPlankPrefab, pos, woodPlankPrefab.transform.rotation);

                woodPlank.transform.eulerAngles = new Vector3(90f, 90f + (rotY * fac), 0f);

                woodPlanksDomino.SetValue(woodPlank, i);

                woodPlanksDomino[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            for (int i = 0; i < planksCount; i++)
            {
                Destroy(woodPlanksDomino[i].gameObject);
            }

        }
    }



}
