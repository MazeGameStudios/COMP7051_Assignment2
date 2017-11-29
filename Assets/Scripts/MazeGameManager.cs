using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MazeGameManager : MonoBehaviour {

	// EDITOR REFERENCES 
	public Text scoreDisplay;
	public GameObject wolfPrefab;

	// PUBLIC MEMBERS 
	public static MazeGameManager instance;
	public int score {
		get { 
			return mScore; 
		}
		set {
			mScore = value;
			scoreDisplay.text = "SCORE: " + mScore;
		}
	}
	[HideInInspector]
    public DayCycleController dayCycleController;
	[HideInInspector]
    public FogController fogController;
	[HideInInspector]
    public AudioController audioController;

	// PRIVATE MEMBERS
	private const string GAME_DATA_FILENAME = "gamedata.dat";
	private int mScore = 0;


	void Awake()    
    {
        Debug.Log("MazeGameManager Awake()");

        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        dayCycleController = GetComponent<DayCycleController>();
        fogController = GetComponent<FogController>();
        audioController = GetComponent<AudioController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Save"))
            SaveGamePrefs();
        else if (Input.GetButtonDown("Load"))
            LoadGamePrefs();
    }

    public void SaveGame()
    {
        // retrieve game data
        var player = GameObject.FindGameObjectWithTag("Player");
        var wolf = GameObject.FindGameObjectWithTag("Enemy");

        // save to struct 
        GameState data = new GameState();
		data.score = score;
        data.player = new PosRot(player.transform.position, player.transform.rotation);
        data.wolf = new PosRot(wolf.transform.position, wolf.transform.rotation);
        data.fogOn = RenderSettings.fog;
        data.isDay = dayCycleController.isDaytime;

        // persist to storage 
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/" + GAME_DATA_FILENAME, FileMode.OpenOrCreate);
        bf.Serialize(fs, data);
        fs.Close();
    }

    public void SaveGamePrefs()
    {
        print("sav");
        // retrieve game data
        var player = GameObject.FindGameObjectWithTag("Player");
        var wolf = GameObject.FindGameObjectWithTag("Enemy");

        // save to struct 
        GameState data = new GameState();
        data.score = score;
        data.player = new PosRot(player.transform.position, player.transform.rotation);
        data.wolf = new PosRot(wolf.transform.position, wolf.transform.rotation);
        data.fogOn = RenderSettings.fog;
        data.isDay = dayCycleController.isDaytime;

        // persist to storage 
        PlayerPrefs.SetString(GAME_DATA_FILENAME, JsonUtility.ToJson(data));
    }

    public void LoadGame()
    {
        // read from storage
        if (File.Exists(Application.persistentDataPath + "/" + GAME_DATA_FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/" + GAME_DATA_FILENAME, FileMode.Open, FileAccess.Read);
            GameState data = (GameState) bf.Deserialize(fs);
            fs.Close();

            // update game state 
			score = data.score;

            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = data.player.Position;
            player.transform.rotation = data.player.Rotation;

            var wolf = GameObject.FindGameObjectWithTag("Enemy");
            wolf.transform.position = data.wolf.Position;
            wolf.transform.rotation = data.wolf.Rotation;

            fogController.TurnFog(data.fogOn);
            dayCycleController.ChangeDayCycle(data.isDay);
        }
        else
        {
            Debug.Log("No game data found!");
        }
    }

    public void LoadGamePrefs()
    {
        print("load");
        // read from storage
        if (PlayerPrefs.HasKey(GAME_DATA_FILENAME))
        {

            string temp = PlayerPrefs.GetString(GAME_DATA_FILENAME);
            GameState data = JsonUtility.FromJson<GameState>(temp);

            // update game state 
            score = data.score;

            var player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = data.player.Position;
            player.transform.rotation = data.player.Rotation;

            var wolf = GameObject.FindGameObjectWithTag("Enemy");
            wolf.transform.position = data.wolf.Position;
            wolf.transform.rotation = data.wolf.Rotation;

            fogController.TurnFog(data.fogOn);
            dayCycleController.ChangeDayCycle(data.isDay);
        }
        else
        {
            Debug.Log("No game data found!");
        }
    }

    [System.Serializable]
    public struct GameState
    {
        public int score;
        public PosRot player;
        public PosRot wolf;
        public bool fogOn;
        public bool isDay;
    }

    // Note: Unfortunately Unity doesn't support Tuples, so we'll use this instead (which the player can also use!)
    [System.Serializable]
    public struct PosRot
    {
        // Note: Vectors and Quaternions are not serializable
        public Vector3 Position { get { return new Vector3(pX, pY, pZ); } }
        public Quaternion Rotation { get { return new Quaternion(qX, qY, qZ, qW); } }

        [SerializeField]
        private float pX, pY, pZ;
        [SerializeField]
        private float qX, qY, qZ, qW;

        public PosRot(Vector3 pos, Quaternion rot)
        {
            pX = pos.x; pY = pos.y; pZ = pos.z;
            qX = rot.x; qY = rot.y; qZ = rot.z; qW = rot.w;
        }
    }
}
