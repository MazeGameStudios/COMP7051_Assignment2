using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Maze mazePrefab;

    private Maze mazeInstance; //the gameobject created at runtime, procedurally generated

    public bool animateMaze = false;

    private void Start()
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
        mazeInstance = Instantiate(mazePrefab) as Maze;
        if (animateMaze) StartCoroutine(mazeInstance.GenerateWithDelay());
        else mazeInstance.Generate();
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
