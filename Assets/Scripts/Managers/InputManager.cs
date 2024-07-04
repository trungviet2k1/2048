using UnityEngine;
public enum MoveDirection
{
    Right, Left, Up, Down
}
public class InputManager : MonoBehaviour
{
    private GamePlayManager gpm;

    private void Awake()
    {
        gpm = FindObjectOfType<GamePlayManager>();
    }

    void Update()
    {
        if (gpm.State == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                gpm.Move(MoveDirection.Right);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gpm.Move(MoveDirection.Left);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gpm.Move(MoveDirection.Up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gpm.Move(MoveDirection.Down);
            } 
        }
    }
}