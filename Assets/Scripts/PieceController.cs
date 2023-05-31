using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Rotation { CantRotate, CanRotate90, CanRotate360 }
    public Rotation Rotate;
    public float Fall;

    public float Speed;
    public float Timer;

    private GameController controller;
    private Spawner spawner;

    void Start()
    {
        controller = FindObjectOfType<GameController>();
        spawner = FindAnyObjectByType<Spawner>();
        Timer = Speed;
        controller.CalcHiScore();
    }

    void Update()
    {
        RefreshTimer();
        CheckMoveDown();
        CheckRotate();
        CheckMoveRight();
        CheckMoveLeft();
    }

    private void CheckMoveLeft()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeftAndRight(-1, 0, 0);
    }

    private void CheckMoveRight()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            MoveLeftAndRight(1, 0, 0);
    }

    private void CheckRotate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotatePiece();
            if (ValidPosition())
            {
                controller.RefreshGrid(this);
            }
            else
            {
                Side side = controller.CheckSide(transform.position);
                if (side == Side.Left)
                    transform.position += new Vector3(1, 0, 0);
                else if (side == Side.Right)
                    if (tag == "I")
                        transform.position += new Vector3(-2, 0, 0);
                    else
                        transform.position += new Vector3(-1, 0, 0);
                else if (side == Side.Bottom)
                    transform.position += new Vector3(0, 1, 0);
            }
        }
    }

    private void CheckMoveDown()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveBySpeedAndTime(0, -1, 0);

            if (ValidPosition())
            {
                controller.RefreshGrid(this);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                controller.RemoveLine();
                enabled = false;
                controller.SetScore(10);
                spawner.ShowNextPiece();
            }
        }

        MoveDownAutomatically();
    }

    private void MoveDownAutomatically()
    {
        if (Time.time - Fall >= (1 / controller.Difficulty) && !Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -1, 0);

            if (ValidPosition())
            {
                controller.RefreshGrid(this);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                controller.RemoveLine();
                enabled = false;
                controller.SetScore(10);

                if(controller.IsGameOver(this))
                {
                    controller.ShowGameOver();
                }

                spawner.ShowNextPiece();
            }

            Fall = Time.time;
        }
    }

    private void RefreshTimer()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            Timer = Speed;
        }
    }

    private void MoveLeftAndRight(float x, float y, float z)
    {
        MoveBySpeedAndTime(x, y, z);

        if (ValidPosition())
        {
            controller.RefreshGrid(this);
        }
        else
        {
            transform.position += new Vector3(x * -1, y * -1, z * -1);
        }
    }

    private void MoveBySpeedAndTime(float x, float y,float z)
    {
        Timer += Time.deltaTime;
        if (Timer > Speed)
        {
            transform.position += new Vector3(x, y, z);
            Timer = 0;
        }
    }

    void RotatePiece()
    {
        switch (Rotate)
        {
            case Rotation.CanRotate90:
                if (transform.rotation.z < 0)
                    transform.Rotate(0, 0, 90);
                else
                    transform.Rotate(0, 0, -90);
                break;
            case Rotation.CanRotate360:
                transform.Rotate(0, 0, -90);
                break;
        }
    }

    bool ValidPosition()
    {
        foreach (Transform child in transform)
        {
            Vector2 piecePosition = controller.Round(child.position);
            if (controller.InsideGrid(piecePosition) == false)
                return false;

            if(controller.TransformGridPosition(piecePosition) != null && controller.TransformGridPosition(piecePosition).parent != transform) 
                return false;
        }

        return true;
    }

}
