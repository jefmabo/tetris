using Assets.Scripts;
using TMPro;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int Rows = 20; // ALTURA
    public static int Columns = 10; // LARGURA
    public int Score = 0;
    public TMP_Text TextScore;
    public int DifficultyPoint;
    public float Difficulty = 1;
    public GameObject GameOver;

    public static Transform[,] grid = new Transform[Columns, Rows];

    public bool InsideGrid(Vector2 position) => ((int)position.x >= 0 && (int)position.x < Columns && (int)position.y >= 0);

    public Vector2 Round(Vector2 roundedNumber) => new Vector2(Mathf.Round(roundedNumber.x), Mathf.Round(roundedNumber.y));

    public Side CheckSide(Vector2 position)
    {
        if (position.x <= 0)
            return Side.Left;
        else if (position.x >= 9)
            return Side.Right;
        else if (position.y <= 1)
            return Side.Bottom;

        return Side.None;
    }

    public void RefreshGrid(PieceController piece)
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[column, row] != null)
                {
                    if (grid[column, row].parent == piece.transform)
                    {
                        grid[column, row] = null;
                    }
                }
            }
        }

        foreach (Transform block in piece.transform)
        {
            Vector2 position = Round(block.position);

            if (position.y < Rows)
            {
                grid[(int)position.x, (int)position.y] = block;
            }
        }
    }

    public Transform TransformGridPosition(Vector2 position)
    {
        if (position.y > Rows - 1)
            return null;

        return grid[(int)position.x, (int)position.y];
    }

    public bool LineIsFull(int row)
    {
        for (int column = 0; column < Columns; column++)
            if (grid[column, row] == null)
                return false;

        return true;
    }

    public void RemoveBlock(int row)
    {
        for (int column = 0; column < Columns; column++)
        {
            Destroy(grid[column, row].gameObject);
            grid[column, row] = null;
        }
    }

    public void MoveLineDown(int row)
    {
        for (int column = 0; column < Columns; column++)
        {
            if (grid[column, row] != null)
            {
                grid[column, row - 1] = grid[column, row];
                grid[column, row] = null;

                grid[column, row - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllLinesDown(int row)
    {
        for (; row < Rows; row++)
            MoveLineDown(row);
    }

    public void SetScore(int score)
    {
        Score += score;
        TextScore.text = Score.ToString().PadLeft(7, '0');

        if (DifficultyPoint > 1000)
        {
            DifficultyPoint -= 1000;
            Difficulty += 0.5f;
        }
        else
        {
            DifficultyPoint += score;
        }
    }

    public void RemoveLine()
    {
        for (int row = 0; row < Rows; row++)
        {
            if (LineIsFull(row))
            {
                RemoveBlock(row);
                MoveAllLinesDown(row + 1);
                row--;
                SetScore(100);
            }
        }
    }

    public bool IsGameOver(PieceController blocks)
    {
        for (int column = 0; column < Columns; column++)
        {
            foreach (Transform block in blocks.transform)
            {
                Vector2 position = Round(block.position);
                if (position.y > Rows - 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void ShowGameOver()
    {
        GameOver.SetActive(true);
    }
}
