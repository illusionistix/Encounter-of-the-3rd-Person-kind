using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;


    [SerializeField] private AudioSource mainMusicSource;
    [SerializeField] private AudioSource ambienceSource;

    private GameObject m_playerObject = null;
    private GameObject m_preferredBoard = null;
    private Transform m_playerLegRTransform = null;
    private string m_screenshotsPath = null;

    private bool m_isGameStarting = true;
    private bool m_isGameActive = true;
    private bool m_isPlayerOnGround = true;
    private bool m_isPlayerInJet = false;
    private bool m_isPlayerOnHoverboard = false;
    private bool m_offHoverboard = true;
    private bool m_isPlayerInWater = false;
    private bool m_isInputFieldFocused = false;
    private bool m_playerFirstWelcome = false;
    private bool m_playerNearJetVehicle = false;
    private bool m_playerNearCarVehicle = false;
    private bool m_playerNearLimits = false;
    private bool m_playerFoundSecret = false;
    
    private float m_musicVolume = 0f;
    private float m_soundFXVolume = 0.5f;
    private float m_ambienceVolume = 0.5f;
    private float m_mouseSensitivity = 2f;    
    private float m_energy = 100f;
    private int m_score = 0;


    public GameObject playerObject { get { return m_playerObject; } set { m_playerObject = value; } }    
    public Transform playerLegRTransform { get { return m_playerLegRTransform; } set { m_playerLegRTransform = value; } }    
    public string screenshotsPath { get { return m_screenshotsPath; } set { m_screenshotsPath = value; } }    
    public bool isGameStarting { get { return m_isGameStarting; } set { m_isGameStarting = value; } }    
    public bool isGameActive { get { return m_isGameActive; } set { m_isGameActive = value; } }    
    public bool isPlayerOnGround { get { return m_isPlayerOnGround; } set { m_isPlayerOnGround = value; } }    
    public bool isPlayerInJet { get { return m_isPlayerInJet; } set { m_isPlayerInJet = value; } }    
    public bool isPlayerOnHoverboard { get { return m_isPlayerOnHoverboard; } set { m_isPlayerOnHoverboard = value; } }    
    public bool offHoverboard { get { return m_offHoverboard; } set { m_offHoverboard = value; } }    
    public bool isPlayerInWater { get { return m_isPlayerInWater; } set { m_isPlayerInWater = value; } }    
    public bool isInputFieldFocused { get { return m_isInputFieldFocused; } set { m_isInputFieldFocused = value; } }    
    public bool playerFirstWelcome { get { return m_playerFirstWelcome; } set { m_playerFirstWelcome = value; } }    
    public bool playerNearJetVehicle { get { return m_playerNearJetVehicle; } set { m_playerNearJetVehicle = value; } }    
    public bool playerNearCarVehicle { get { return m_playerNearCarVehicle; } set { m_playerNearCarVehicle = value; } }    
    public bool playerNearLimits { get { return m_playerNearLimits; } set { m_playerNearLimits = value; } }    
    public bool playerFoundSecret { get { return m_playerFoundSecret; } set { m_playerFoundSecret = value; } }    
    public GameObject preferredBoard { get { return m_preferredBoard; } set { m_preferredBoard = value; } }    
    public float musicVolume { get { return m_musicVolume; } set { m_musicVolume = value; } }    
    public float soundFXVolume { get { return m_soundFXVolume; } set { m_soundFXVolume = value; } }    
    public float ambienceVolume { get { return m_ambienceVolume; } set { m_ambienceVolume = value; } }    
    public float mouseSensitivity { get { return m_mouseSensitivity; } set { m_mouseSensitivity = value; } }    
    public int score { get { return m_score; } set { m_score = value; } }    
    public float energy
    {
        get { return m_energy; }

        set
        {
            if (value < 0)
            {
                m_energy = 0;
            }
            else if (value > 100)
            {
                m_energy = 100f;
            }
            else
            {
                m_energy = value;
            }
        }
    }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        mainMusicSource.volume = musicVolume;
        ambienceSource.volume = ambienceVolume;        
    }


    private void OnDisable()
    {
        SaveDataToFile();
    }

    [System.Serializable]
    class SaveData
    {
        public string screenshotsPath;
    }

    public void SaveDataToFile()
    {
        SaveData data = new SaveData();
        data.screenshotsPath = screenshotsPath;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


    public void LoadDataFromFile()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            screenshotsPath = data.screenshotsPath;
        }
    }

}
