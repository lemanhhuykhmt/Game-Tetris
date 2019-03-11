using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    float lastFall = 0;
    private float interval = 0.5f;
    private bool isEnd = false;
    private GameObject curGroup;
    public int score;
    public int level;
    public Text txtScore;
    public Text txtLevel;

    public GameObject pnlEndGame;
    public Text txtEndScore;
    public Button btnRestart;

    // Use this for initialization
    void Start()
    {
        score = 0;
        level = 1;
        curGroup = FindObjectOfType<Spawer>().Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnd)
        {
            return;
        }
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            curGroup.transform.position += new Vector3(-1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                curGroup.transform.position += new Vector3(1, 0, 0);
        }

        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            curGroup.transform.position += new Vector3(1, 0, 0);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                curGroup.transform.position += new Vector3(-1, 0, 0);
        }
        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            curGroup.transform.Rotate(0, 0, 90);

            // See if valid
            if (isValidGridPos())
                // It's valid. Update grid.
                updateGrid();
            else
                // It's not valid. revert.
                curGroup.transform.Rotate(0, 0, -90);
        }

        // Move Downwards and Fall
        else if (Input.GetKeyDown(KeyCode.DownArrow) ||
                 Time.time - lastFall >= interval)
        {
            // Modify position
            curGroup.transform.position += new Vector3(0, -1, 0);

            // See if valid
            if (isValidGridPos())
            {
                // It's valid. Update grid.
                updateGrid();
            }
            else
            {
                // It's not valid. revert.
                curGroup.transform.position += new Vector3(0, 1, 0);
                // Check lose
                if(curGroup.transform.position.y == FindObjectOfType<Spawer>().transform.position.y)
                {
                    EndGame();
                    return;
                }
                // Clear filled horizontal lines
                int rowDel = Grid.deleteFullRows();
                // Update Score and Level
                score += rowDel * 10;
                if (score % 100 == 0 && rowDel > 0)
                {
                    level++;
                    interval -= 0.3f;
                    if (interval < 0) interval = 0.1f;
                }
                txtLevel.text = "Level: " + level;
                txtScore.text = "Score: " + score;
                // Spawn next Group
                curGroup = FindObjectOfType<Spawer>().Spawn();
            }

            lastFall = Time.time;
        }
    }
    bool isValidGridPos()
    {
        foreach (Transform child in curGroup.transform)
        {
            Vector2 v = Grid.roundVec2(child.position);


            // Not inside Border?
            if (!Grid.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != curGroup.transform)
                return false;
        }
        return true;
    }
    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == curGroup.transform)
                        Grid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in curGroup.transform)
        {
            Vector2 v = Grid.roundVec2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    private void EndGame()
    {
        isEnd = true;
        pnlEndGame.SetActive(true);
        txtEndScore.text = "Your Score: " + score;
        Time.timeScale = 0;
    }
    public void ReStart()
    {
        isEnd = false;
        pnlEndGame.SetActive(false);
        Time.timeScale = 1;
        interval = 1;
        resetAll();
        score = 0;
        level = 1;
        Destroy(curGroup);
        Destroy(FindObjectOfType<Spawer>().nextGroup);
        FindObjectOfType<Spawer>().nextGroup = null;
        curGroup = FindObjectOfType<Spawer>().Spawn();
    }
    private void resetAll()
    {
        for (int y = 0; y < Grid.h; ++y)
            for (int x = 0; x < Grid.w; ++x)
                if(Grid.grid[x, y] != null)
                {
                    Destroy(Grid.grid[x, y].gameObject);
                    Grid.grid[x, y] = null;
                }
        //updateGrid();
    }
}
