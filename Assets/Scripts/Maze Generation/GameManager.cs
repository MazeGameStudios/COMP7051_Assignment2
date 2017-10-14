using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Transform spawnLocation;     // attempted spawn location

    public Maze mazePrefab;
    private Maze mazeInstance; //the gameobject created at runtime, procedurally generated
    public bool animateMaze = false;

    public GameObject platformPrefab;
    public float platformBorderSize = 1f;


    private void Awake()
    {
        BeginGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
    }

    private void BeginGame()
    {
        // Create an instance of the maze (but don't generate yet)
        var mazeObject = Instantiate(mazePrefab);
        mazeInstance = mazeObject.GetComponent<Maze>();

        // Calculate maze position
        var platformTopY = FindFloorLevelForMaze();

        // Move maze
        var actualSpawnPosition = new Vector3(spawnLocation.position.x, platformTopY, spawnLocation.position.z);
        mazeObject.transform.position = actualSpawnPosition;
        mazeObject.transform.rotation = spawnLocation.rotation;
        // Generate maze
        if (animateMaze) StartCoroutine(mazeInstance.GenerateWithDelay());
        else mazeInstance.Generate();

        // Spawn the platform
        var mazeSize = mazeInstance.GetExtents();
        var platform = Instantiate(platformPrefab, actualSpawnPosition + (Vector3.down * 25f) + (Vector3.down * 0.01f), spawnLocation.rotation);
        platform.transform.localScale = new Vector3(mazeSize.x + platformBorderSize, 50f, mazeSize.z + platformBorderSize);   //hard coded height
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }

    // Prevent the maze from clipping into the terrain
    private float FindFloorLevelForMaze()
    {
        RaycastHit hit;
        if (Physics.BoxCast(
            spawnLocation.position + Vector3.up * 100f,                             // center (start high to ensure not clipping into terrain)
            mazeInstance.GetExtents() * 0.5f,                                       // box half extents
            Vector3.down,                                                           // direction
            out hit,                                                                // hit info
            spawnLocation.rotation,                                                 // orientation
            200f))                                                                  // max checking distance
        {
            return hit.point.y;
        }
        else
        {
            return 0;
        }
    }
}

/*
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Transform spawnLocation;     // attempted spawn location

    public Maze mazePrefab;
    private Maze mazeInstance; //the gameobject created at runtime, procedurally generated
    public bool animateMaze = false;

    public GameObject platformPrefab;
    public float platformBorderRadius = 1f;


    private void Awake()
    {
        BeginGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RestartGame();
        }
    }

    private void BeginGame()
    {
        // Create an instance of the maze (but don't generate yet)
        var mazeObject = Instantiate(mazePrefab);
        mazeInstance = mazeObject.GetComponent<Maze>();

        // Calculate maze position
        var platformTopY = FindFloorLevelForMaze();
        var platformBotY = FindBottomOfTerrain();
        var platformHeight = platformTopY - platformBotY;

        // Move and generate the maze
        var actualSpawnPosition = new Vector3(spawnLocation.position.x, platformTopY, spawnLocation.position.z);
        mazeObject.transform.position = actualSpawnPosition;
        mazeObject.transform.rotation = spawnLocation.rotation;
        if (animateMaze) StartCoroutine(mazeInstance.GenerateWithDelay());
        else mazeInstance.Generate();

        // Spawn the platform
        var platform = Instantiate(platformPrefab, actualSpawnPosition + (Vector3.down * platformHeight * 0.25f) + (Vector3.down * 0.01f), spawnLocation.rotation);
        platform.transform.localScale = new Vector3(mazeInstance.size.x + platformBorderRadius, platformHeight / 2f, mazeInstance.size.z + platformBorderRadius);
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }

    // Prevent the maze from clipping into the terrain
    private float FindFloorLevelForMaze()
    {
        Debug.Log(spawnLocation);
        RaycastHit hit;
        if (Physics.BoxCast(
            spawnLocation.position,                                                 // center
            new Vector3(mazeInstance.size.x / 2, 1f, mazeInstance.size.z / 2),      // box half extents
            Vector3.down,                                                           // direction
            out hit,                                                                // hit info
            spawnLocation.rotation,                                                 // orientation
            100))                                                                   // max checking distance
        {
            return hit.point.y;
        }
        else
        {
            return 0;
        }
    }

    private float FindBottomOfTerrain()
    {
        RaycastHit hit;
        if (Physics.BoxCast(
            spawnLocation.position - (Vector3.down * 100f),                     // center
            new Vector3(mazeInstance.size.x / 2, 1f, mazeInstance.size.z / 2),  // box half extents
            Vector3.up,                                                         // direction
            out hit,                                                            // hit info
            spawnLocation.rotation,                                             // orientation
            100f))                                                              // max checking distance
        {
            return hit.point.y;
        }
        else
        {
            return 0;
        }
    }


}
*/