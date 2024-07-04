using UnityEngine;

public class TouchInputManager : MonoBehaviour
{
    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwipe = false;
    private readonly float minSwipeDict = 50.0f;
    private readonly float maxSwipeTime = 1.5f;

    private GamePlayManager gpm;

    void Awake()
    {
        gpm = FindObjectOfType<GamePlayManager>();
    }

    void Update()
    {
        if (gpm.State == GameState.Playing && Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;
                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;
                    case TouchPhase.Ended:
                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDict = (touch.position - fingerStartPos).magnitude;

                        if (isSwipe && gestureTime < maxSwipeTime && gestureDict > minSwipeDict)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            _ = Vector2.zero;
                            Vector2 swipeType;
                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if (swipeType.x != 0.0f)
                            {
                                if (swipeType.x > 0.0f)
                                {
                                    //Move Right
                                    gpm.Move(MoveDirection.Right);
                                }
                                else
                                {
                                    //Move Left
                                    gpm.Move(MoveDirection.Left);
                                }
                            }

                            if (swipeType.y != 0.0f)
                            {
                                if (swipeType.y > 0.0f)
                                {
                                    //Move Up
                                    gpm.Move(MoveDirection.Up);
                                }
                                else
                                {
                                    //Move Down
                                    gpm.Move(MoveDirection.Down);
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}