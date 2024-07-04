using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    Playing, GameOver
}

public class GamePlayManager : MonoBehaviour
{
    [Header("Game Settings")]
    public GameState State;
    [Range(0, 1)]
    public float delay;
    private bool moveMade;
    private readonly bool[] lineMoveComplete = new bool[4] { true, true, true, true };

    [Header("Game Over UI")]
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI GameOverScoreText;
    public GameObject GameOverPanel;

    private readonly Title[,] AllTitles = new Title[4, 4];
    private readonly List<Title[]> columns = new();
    private readonly List<Title[]> rows = new();
    private readonly List<Title> EmptyTitle = new();

    void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic();

        Title[] AllTitlesOneDim = FindObjectsOfType<Title>();
        foreach (Title t in AllTitlesOneDim)
        {
            t.Number = 0;
            AllTitles[t.indRow, t.indCol] = t;
            EmptyTitle.Add(t);
        }

        columns.Add(new Title[] { AllTitles[0, 0], AllTitles[1, 0], AllTitles[2, 0], AllTitles[3, 0] });
        columns.Add(new Title[] { AllTitles[0, 1], AllTitles[1, 1], AllTitles[2, 1], AllTitles[3, 1] });
        columns.Add(new Title[] { AllTitles[0, 2], AllTitles[1, 2], AllTitles[2, 2], AllTitles[3, 2] });
        columns.Add(new Title[] { AllTitles[0, 3], AllTitles[1, 3], AllTitles[2, 3], AllTitles[3, 3] });

        rows.Add(new Title[] { AllTitles[0, 0], AllTitles[0, 1], AllTitles[0, 2], AllTitles[0, 3] });
        rows.Add(new Title[] { AllTitles[1, 0], AllTitles[1, 1], AllTitles[1, 2], AllTitles[1, 3] });
        rows.Add(new Title[] { AllTitles[2, 0], AllTitles[2, 1], AllTitles[2, 2], AllTitles[2, 3] });
        rows.Add(new Title[] { AllTitles[3, 0], AllTitles[3, 1], AllTitles[3, 2], AllTitles[3, 3] });

        State = GameState.Playing;

        Generate();
        Generate();
    }

    bool CanMove()
    {
        if (EmptyTitle.Count > 0)
        {
            return true;
        }
        else
        {
            //Check Columns
            for (int i = 0; i < columns.Count; i++)
            {
                for (int j = 0; j < rows.Count - 1; j++)
                {
                    if (AllTitles[j, i].Number == AllTitles[j + 1, i].Number)
                    {
                        return true;
                    }
                }
            }

            //Check Rows
            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < columns.Count - 1; j++)
                {
                    if (AllTitles[i, j].Number == AllTitles[i, j + 1].Number)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool MakeOneMoveDownIndex(Title[] LineOfTitles)
    {
        for (int i = 0; i < LineOfTitles.Length - 1; i++)
        {
            //Move Block
            if (LineOfTitles[i].Number == 0 && LineOfTitles[i + 1].Number != 0)
            {
                LineOfTitles[i].Number = LineOfTitles[i + 1].Number;
                LineOfTitles[i + 1].Number = 0;
                return true;
            }

            //Merge Block
            if (LineOfTitles[i].Number != 0 && LineOfTitles[i].Number == LineOfTitles[i + 1].Number &&
                LineOfTitles[i].mergedThisTurn == false && LineOfTitles[i + 1].mergedThisTurn == false)
            {
                LineOfTitles[i].Number *= 2;
                LineOfTitles[i + 1].Number = 0;
                LineOfTitles[i].mergedThisTurn = true;
                LineOfTitles[i].PlayMergerAnimation();
                SoundManager.Instance.PlayMergeSound();
                ScoreManager.Instance.Score += LineOfTitles[i].Number;
                return true;
            }
        }
        return false;
    }

    bool MakeOneMoveUpIndex(Title[] LineOfTitles)
    {
        for (int i = LineOfTitles.Length - 1; i > 0; i--)
        {
            //Move Block
            if (LineOfTitles[i].Number == 0 && LineOfTitles[i - 1].Number != 0)
            {
                LineOfTitles[i].Number = LineOfTitles[i - 1].Number;
                LineOfTitles[i - 1].Number = 0;
                return true;
            }

            //Merge Block
            if (LineOfTitles[i].Number != 0 && LineOfTitles[i].Number == LineOfTitles[i - 1].Number &&
                LineOfTitles[i].mergedThisTurn == false && LineOfTitles[i - 1].mergedThisTurn == false)
            {
                LineOfTitles[i].Number *= 2;
                LineOfTitles[i - 1].Number = 0;
                LineOfTitles[i].mergedThisTurn = true;
                LineOfTitles[i].PlayMergerAnimation();
                SoundManager.Instance.PlayMergeSound();
                ScoreManager.Instance.Score += LineOfTitles[i].Number;
                return true;
            }
        }
        return false;
    }

    void Generate()
    {
        if (EmptyTitle.Count > 0)
        {
            int indexForNewNumber = Random.Range(0, EmptyTitle.Count);
            int randomNum = Random.Range(0, 10);

            if (randomNum == 0)
            {
                EmptyTitle[indexForNewNumber].Number = 4;
            }
            else
            {
                EmptyTitle[indexForNewNumber].Number = 2;
            }

            EmptyTitle[indexForNewNumber].PlayAppearAnimation();
            EmptyTitle.RemoveAt(indexForNewNumber);
        }
    }

    private void ResetMergedFlags()
    {
        foreach (Title t in AllTitles)
        {
            t.mergedThisTurn = false;
        }
    }

    private void UpdateEmptyTitle()
    {
        EmptyTitle.Clear();
        foreach (Title t in AllTitles)
        {
            if (t.Number == 0)
            {
                EmptyTitle.Add(t);
            }
        }
    }

    public void MoveMade()
    {
        if (moveMade)
        {
            SoundManager.Instance.PlayMoveSound();
            UpdateEmptyTitle();
            Generate();

            if (!CanMove())
            {
                GameOver();
            }
        }
    }

    public void Move(MoveDirection md)
    {
        moveMade = false;
        ResetMergedFlags();

        if (delay > 0)
        {
            StartCoroutine(MoveCoroutine(md));
        }
        else
        {
            for (int i = 0; i < rows.Count; i++)
            {
                switch (md)
                {
                    case MoveDirection.Down:
                        while (MakeOneMoveUpIndex(columns[i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Up:
                        while (MakeOneMoveDownIndex(columns[i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Left:
                        while (MakeOneMoveDownIndex(rows[i]))
                        {
                            moveMade = true;
                        }
                        break;
                    case MoveDirection.Right:
                        while (MakeOneMoveUpIndex(rows[i]))
                        {
                            moveMade = true;
                        }
                        break;
                }
            }

            MoveMade();
        }
    }

    IEnumerator MoveOneLineUpIndexCoroutine(Title[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveUpIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }

    IEnumerator MoveOneLineDownIndexCoroutine(Title[] line, int index)
    {
        lineMoveComplete[index] = false;
        while (MakeOneMoveDownIndex(line))
        {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }

    IEnumerator MoveCoroutine(MoveDirection md)
    {
        switch (md)
        {
            case MoveDirection.Down:
                for (int i = 0; i < columns.Count; i++)
                {
                    StartCoroutine(MoveOneLineUpIndexCoroutine(columns[i], i));
                }
                break;
            case MoveDirection.Left:
                for (int i = 0; i < rows.Count; i++)
                {
                    StartCoroutine(MoveOneLineDownIndexCoroutine(rows[i], i));
                }
                break;
            case MoveDirection.Right:
                for (int i = 0; i < rows.Count; i++)
                {
                    StartCoroutine(MoveOneLineUpIndexCoroutine(rows[i], i));
                }
                break;
            case MoveDirection.Up:
                for (int i = 0; i < columns.Count; i++)
                {
                    StartCoroutine(MoveOneLineDownIndexCoroutine(columns[i], i));
                }
                break;
        }

        while (!(lineMoveComplete[0] && lineMoveComplete[1] && lineMoveComplete[2] && lineMoveComplete[3]))
        {
            yield return null;
        }

        MoveMade();
        State = GameState.Playing;
        StopAllCoroutines();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GamePlay");
        SoundManager.Instance.PlayBackgroundMusic();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void GameOver()
    {
        SoundManager.Instance.PlayLoserSound();
        SoundManager.Instance.StopBackgroundMusic();
        State = GameState.GameOver;
        GameOverScoreText.text = ScoreManager.Instance.Score.ToString();
        GameOverPanel.SetActive(true);
    }
}