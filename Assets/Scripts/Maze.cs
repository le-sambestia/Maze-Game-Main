using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Maze : MonoBehaviour
{
    public int Rows = 2;
    public int Columns = 2;
    public GameObject Wall;
    public GameObject Floor;
    public InputField HeightField;
    public InputField WidthField;

    private MazeCell[,] grid;
    private int currentRow = 0;
    private int currentColumn = 0;
    private bool scanComplete = false;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        //destroy all children of transform
        foreach (Transform transform in transform)
        {
            Destroy(transform.gameObject);
        }
        //Dimension 2*2
        CreateGrid();

        currentRow = 0;
        currentColumn = 0;
        scanComplete = false;


        HuntAndKill();
    }

    void CreateGrid()
    {
        float size = Wall.transform.localScale.x;
        grid = new MazeCell[Rows, Columns];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                GameObject floor = Instantiate(Floor, new Vector3(j * size, 0, -i * size), Quaternion.identity);
                floor.name = "Floor_" + i + "_" + j;

                GameObject upWall = Instantiate(Wall, new Vector3(j * size, 1.75f, -i * size + 1.25f), Quaternion.identity);
                upWall.name = "UpWall_" + i + "_" + j;

                GameObject downWall = Instantiate(Wall, new Vector3(j * size, 1.75f, -i * size - 1.25f), Quaternion.identity);
                downWall.name = "DownWall_" + i + "_" + j;

                GameObject leftWall = Instantiate(Wall, new Vector3(j * size - 1.25f, 1.75f, -i * size), Quaternion.Euler(0, 90, 0));
                leftWall.name = "LeftWall_" + i + "_" + j;

                GameObject rightWall = Instantiate(Wall, new Vector3(j * size + 1.25f, 1.75f, -i * size), Quaternion.Euler(0, 90, 0));
                rightWall.name = "RighttWall_" + i + "_" + j;

                grid[i, j] = new MazeCell();
                grid[i, j].UpWall = upWall;
                grid[i, j].DownWall = downWall;
                grid[i, j].LeftWall = leftWall;
                grid[i, j].RightWall = rightWall;

                floor.transform.parent = transform;
                upWall.transform.parent = transform;
                downWall.transform.parent = transform;
                leftWall.transform.parent = transform;
                rightWall.transform.parent = transform;

                if (i == 0 && j == 0)
                {
                    Destroy(leftWall);
                }

                if (i == Rows - 1 && j == Columns - 1)
                {
                    Destroy(rightWall);
                }
            }
        }
    }

    void HuntAndKill()
    {
        grid[currentRow, currentColumn].Visited = true;
        while (!scanComplete)
        {
            Walk();
            Hunt();
        }
    }

    bool IsCellUnvisitedAndWithinBoundries(int row, int column)
    {
        if (row >= 0 && row < Rows && column >= 0 && column < Columns && !grid[row, column].Visited)
        {
            return true;
        }
        return false;
    }

    void Walk()
    {
        while (AreThereUnvisitedNeighbors())
        {
            int direction = Random.Range(0, 4);

            //check up
            if (direction == 0)
            {
                Debug.Log("Check Up");
                if (IsCellUnvisitedAndWithinBoundries(currentRow - 1, currentColumn))
                {
                    if (grid[currentRow, currentColumn].UpWall)
                    {
                        Destroy(grid[currentRow, currentColumn].UpWall);
                    }

                    currentRow--;
                    grid[currentRow, currentColumn].Visited = true;

                    if (grid[currentRow, currentColumn].DownWall)
                    {
                        Destroy(grid[currentRow, currentColumn].DownWall);
                    }
                }
            }

            //check down
            else if (direction == 1)
            {
                Debug.Log("Check Down");
                if (IsCellUnvisitedAndWithinBoundries(currentRow + 1, currentColumn))
                {
                    if (grid[currentRow, currentColumn].DownWall)
                    {
                        Destroy(grid[currentRow, currentColumn].DownWall);
                    }

                    currentRow++;
                    grid[currentRow, currentColumn].Visited = true;

                    if (grid[currentRow, currentColumn].UpWall)
                    {
                        Destroy(grid[currentRow, currentColumn].UpWall);
                    }
                }
            }

            //check left
            else if (direction == 2)
            {
                Debug.Log("Check Left");
                if (IsCellUnvisitedAndWithinBoundries(currentRow, currentColumn - 1))
                {
                    if (grid[currentRow, currentColumn].LeftWall)
                    {
                        Destroy(grid[currentRow, currentColumn].LeftWall);
                    }

                    currentColumn--;
                    grid[currentRow, currentColumn].Visited = true;

                    if (grid[currentRow, currentColumn].RightWall)
                    {
                        Destroy(grid[currentRow, currentColumn].RightWall);
                    }
                }
            }

            //chcek right
            else if (direction == 3)
            {
                Debug.Log("Check Right");
                if (IsCellUnvisitedAndWithinBoundries(currentRow, currentColumn + 1))
                {
                    if (grid[currentRow, currentColumn].RightWall)
                    {
                        Destroy(grid[currentRow, currentColumn].RightWall);
                    }

                    currentColumn++;
                    grid[currentRow, currentColumn].Visited = true;

                    if (grid[currentRow, currentColumn].LeftWall)
                    {
                        Destroy(grid[currentRow, currentColumn].LeftWall);
                    }
                }
            }
        }
    }

    void Hunt()
    {
        scanComplete = true;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (!grid[i, j].Visited && AreThereVisitedNeighbors(i, j))
                {
                    scanComplete = false;
                    currentRow = i;
                    currentColumn = j;
                    grid[currentRow, currentColumn].Visited = true;
                    DestroyAdjacentWall();
                    return;
                }
            }
        }
    }
    void DestroyAdjacentWall()
    {
        bool destroyed = false;

        while (!destroyed)
        {
            int direction = Random.Range(0, 4);
            //check up
            if (direction == 0)
            {
                if (currentRow > 0 && grid[currentRow - 1, currentColumn].Visited)
                {

                    if (grid[currentRow, currentColumn].UpWall)
                    {
                        Destroy(grid[currentRow, currentColumn].UpWall);
                    }

                    if (grid[currentRow - 1, currentColumn].DownWall)
                    {
                        Destroy(grid[currentRow - 1, currentColumn].DownWall);
                    }

                    destroyed = true;
                }
            }
            //check down
            else if (direction == 1)
            {
                if (currentRow > Rows - 1 && grid[currentRow + 1, currentColumn].Visited)
                {
                    if (grid[currentRow, currentColumn].DownWall)
                    {
                        Destroy(grid[currentRow, currentColumn].DownWall);
                    }
                    if (grid[currentRow + 1, currentColumn].UpWall)
                    {
                        Destroy(grid[currentRow + 1, currentColumn].UpWall);
                    }
                    destroyed = true;
                }
            }
            //check left
            else if (direction == 2)
            {
                if (currentColumn > 0 && grid[currentRow, currentColumn - 1].Visited)
                {

                    if (grid[currentRow, currentColumn].LeftWall)
                    {
                        Destroy(grid[currentRow, currentColumn].LeftWall);
                    }

                    if (grid[currentRow, currentColumn - 1].RightWall)
                    {
                        Destroy(grid[currentRow, currentColumn - 1].RightWall);
                    }
                    destroyed = true;
                }
            }
            //check right
            else if (direction == 3)
            {
                if (currentColumn < Columns - 1 && grid[currentRow, currentColumn + 1].Visited)
                {
                    if (grid[currentRow, currentColumn].RightWall)
                    {
                        Destroy(grid[currentRow, currentColumn].RightWall);
                    }

                    if (grid[currentRow, currentColumn + 1].LeftWall)
                    {
                        Destroy(grid[currentRow, currentColumn + 1].LeftWall);
                    }
                    destroyed = true;
                }
            }
        }
    }
    bool AreThereUnvisitedNeighbors()
    {
        //check up
        if (IsCellUnvisitedAndWithinBoundries(currentRow - 1, currentColumn))
        {
            return true;
        }

        //check down
        if (IsCellUnvisitedAndWithinBoundries(currentRow + 1, currentColumn))
        {
            return true;
        }

        //check left
        if (IsCellUnvisitedAndWithinBoundries(currentRow, currentColumn + 1))
        {
            return true;
        }

        //check right
        if (IsCellUnvisitedAndWithinBoundries(currentRow, currentColumn - 1))
        {
            return true;
        }
        return false;
    }

    public bool AreThereVisitedNeighbors(int row, int column)
    {
        //check up
        if (row > 0 && grid[row - 1, column].Visited)
        {
            return true;
        }

        //check down
        if (row < Rows - 1 && grid[row + 1, column].Visited)
        {
            return true;
        }

        //check left
        if (column > 0 && grid[row, column - 1].Visited)
        {
            return true;
        }

        //check right
        if (column < Columns - 1 && grid[row, column - 1].Visited)
        {
            return true;
        }

        return false;
    }

    public void Regenerate()
    {
        int rows = 2;
        int columns = 2;

        if (int.TryParse(HeightField.text, out rows))
        {
            Rows = Mathf.Max(2, rows);
        }

        if (int.TryParse(WidthField.text, out columns))
        {
            Columns = Mathf.Max(2, columns);
        }

        GenerateGrid();
    }
}
