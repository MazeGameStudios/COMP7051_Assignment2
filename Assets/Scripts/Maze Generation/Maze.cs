using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Maze : MonoBehaviour {

    public MazeCell cellPrefab;
    public GameObject openingPrefab;

    private MazeCell[,] cells;
    private MazeCell entrance, exit;
    private MazeCellEdge entranceCandidate, exitCandidate;
    private int exitCandiateLength = 0;

    public IntVector2 size;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public Material[] wallMaterials;

    public float generationStepDelay;

    public GameObject wolfPrefab;
    public int mobCount = 2;
    private int currentMobsSpawned = 0;

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }


    public IEnumerator GenerateWithDelay()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            yield return delay;
            DoNextGenerationStep(activeCells);
        }
    }

    public void Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            DoNextGenerationStep(activeCells);
        }
        GenerateEntranceExit();
    }


    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        var entrance = CreateCell(RandomEdgeCoordinates, 0);
        activeCells.Add(entrance);
        entrance.name = "entrance";
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates, currentCell.passageLength + 1);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else
            {
                CreateWall(currentCell, neighbor, direction);

                SpawnMob(neighbor.transform);

                // No longer remove the cell here.
            }
        }
        else
        {
            var wall = CreateWall(currentCell, null, direction);
            // No longer remove the cell here.

            // determine if this wall can be used as an exit
            if (currentCell.passageLength > exitCandiateLength)
            {
                exitCandidate = wall;
                exitCandiateLength = currentCell.passageLength;
            }

            // determine if this wall can be used as an entrance
            if (currentCell.passageLength == 0)
            {
                entranceCandidate = wall;
            }
        }

    }

    void SpawnMob(Transform mobSpawnLocation)
    {
        if (currentMobsSpawned <= mobCount) //spawn wolves
        {
            float chance = Random.Range(0, 9);

            if (chance > 3)
            {
                print("spawning wolf " + currentMobsSpawned);
                Instantiate(wolfPrefab, mobSpawnLocation.position, Quaternion.identity);
                currentMobsSpawned++;
            }

        }
    }

    private void GenerateEntranceExit()
    {
        // Set the entrance/exit
        entrance = entranceCandidate.cell;
        exit = exitCandidate.cell;

        // Create opening objects to detect player 
        Instantiate(openingPrefab, entrance.transform);
        Instantiate(openingPrefab, exit.transform);

        // Remove walls
        Destroy(entranceCandidate.gameObject);
        Destroy(exitCandidate.gameObject);
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private MazeCellEdge CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        wall.GetComponentInChildren<Renderer>().material = wallMaterials[(int)direction];

        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
            wall.GetComponentInChildren<Renderer>().material = wallMaterials[(int)direction.GetOpposite()];
        }

        return wall;
    }

    private MazeCell CreateCell(IntVector2 coordinates, int length)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.passageLength = length;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public IntVector2 RandomEdgeCoordinates
    {
        get
        {
            if (Random.value > 0.5)
                return new IntVector2(Random.value > 0.5 ? 0 : size.x - 1, Random.Range(0, size.z));
            else
                return new IntVector2(Random.Range(0, size.x), Random.value > 0.5 ? 0 : size.z - 1);
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    public bool IsEdgeCoordinate(IntVector2 coordinate)
    {
        return (coordinate.x == 0 || coordinate.x == size.x - 1) && (coordinate.z == 0 || coordinate.z == size.z - 1);
    }

    public Vector3 GetExtents()
    {
        var extents = new Vector3(size.x, 1, size.z);
        extents.Scale(transform.localScale);
        return extents;
    }
}

[System.Serializable]
public struct IntVector2
{
    public int x;
    public int z;

    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b) //addition operator
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }
}

public enum MazeDirection
{
    North,
    East,
    South,
    West
}

public static class MazeDirections
{

    public const int Count = 4;

    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };
    private static MazeDirection[] opposites = {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return opposites[(int)direction];
    }

    private static IntVector2[] vectors = {
        new IntVector2(0, 1),
        new IntVector2(1, 0),
        new IntVector2(0, -1),
        new IntVector2(-1, 0)
    };

    public static IntVector2 ToIntVector2(this MazeDirection direction) //this keyword makes this behave as an instance method of MazeDirection
    {
        return vectors[(int)direction];
    }


    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return rotations[(int)direction];
    }
}
