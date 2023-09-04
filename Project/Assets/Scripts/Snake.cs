using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private enum State
    {
        Alive,
        Dead
    }

    public Image gameOverWindow;
    private State state;
    bool canChangeDir = true;
    private Direction gridMoveDirection;
    public float gridMoveTimer;
    public float gridMoveTimerMax;
    private Vector2Int gridposition;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    TMP_Text message;
    SpriteRenderer sr;
    Color myColor;
    bool isWrapping = false;


    public void Setup(LevelGrid levelGrid, Color color)
    {
        this.myColor = color;
        this.levelGrid = levelGrid;
        sr.color = myColor;
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        gridposition = new Vector2Int(10, 10);

        gridMoveTimerMax = .1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        message = gameOverWindow.transform.Find("You Died").GetComponent<TMP_Text>();

        snakeBodyPartList = new List<SnakeBodyPart>();

        snakeBodySize = 0;
        state = State.Alive;
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                gameOverWindow.gameObject.SetActive(true);
                var high = PlayerPrefs.GetInt("highscore");
                if (GameHandler.Score > high)
                {
                    message.text = "NEW HIGHSCORE!";
                    Debug.Log(message.text);
                }
                else if (GameHandler.Score < high)
                {
                    message.text = "You Died!";
                }
                break;
        }

        for (int i = snakeBodyPartList.Count; i > 0; i--)
            snakeBodyPartList[snakeBodyPartList.Count - 1].SetBodyPartColor(myColor, sr);

    }

    private void HandleInput()
    {
        if (canChangeDir != false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (gridMoveDirection != Direction.Down || snakeBodySize == 0)
                {
                    gridMoveDirection = Direction.Up;
                    canChangeDir = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (gridMoveDirection != Direction.Up || snakeBodySize == 0)
                {
                    gridMoveDirection = Direction.Down;
                    canChangeDir = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (gridMoveDirection != Direction.Right || snakeBodySize == 0)
                {
                    gridMoveDirection = Direction.Left;
                    canChangeDir = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (gridMoveDirection != Direction.Left || snakeBodySize == 0)
                {
                    gridMoveDirection = Direction.Right;
                    canChangeDir = false;
                }
            }
        }

    }

    public void SnakeDied()
    {
        state = State.Dead;
    }

    Vector2Int GetMoveDir()
    {
        Vector2Int gridMovePositionVector;
        switch (gridMoveDirection)
        {
            default:
            case Direction.Right: gridMovePositionVector = new Vector2Int(-1, 0); break;
            case Direction.Left: gridMovePositionVector = new Vector2Int(1, 0); break;
            case Direction.Up: gridMovePositionVector = new Vector2Int(0, 1); break;
            case Direction.Down: gridMovePositionVector = new Vector2Int(0, -1); break;
        }
        return gridMovePositionVector;
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer > gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            canChangeDir = true;

            Vector2Int oldGridPos = gridposition;
            if (!GameHandler.wallMode)
            {
                gridposition = levelGrid.ValidateGridPosition(gridposition + GetMoveDir(), isWrapping);
                isWrapping = levelGrid.GetSnakeIsWrapping();
            }
            else
            {
                levelGrid.TryGetOnWall(gridposition);
            }

            SnakeMovePosition previousSnakeMovePosition = null;

            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, oldGridPos, gridMoveDirection, snakeBodyPartList, levelGrid);

            snakeMovePositionList.Insert(0, snakeMovePosition);

            //if (isWrapping == false)
            //    gridposition += GetMoveDir();

            //for (int i = snakeBodyPartList.Count - 1; i >= 0; i--)
            //{
            //    snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePosition, isWrapping);
            //}

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartPosition = snakeBodyPart.GetGridPosition();

                if (gridposition == snakeBodyPartPosition)
                {
                    Score.TrySetNewHighScore(GameHandler.Score);
                    state = State.Dead;
                }
            }

            bool snakeAteFood = levelGrid.SnakeTryEatFood(gridposition);

            if (snakeAteFood)
            {
                snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (snakeBodyPartList.Count >= snakeBodySize + 1)
            {
                snakeBodyPartList.RemoveAt(snakeMovePositionList.Count - 1);
            }


            if (isWrapping)
            {
                transform.position = new Vector3(gridposition.x, gridposition.y, 0);
            }
            else
            {
                Vector3 gridPos = new Vector3(gridposition.x, gridposition.y);
                //transform.position = Vector3.Lerp(transform.position, gridPos, .5f);
                transform.position = gridPos;
            }
            UpdateSnakeBodyParts();

            if (snakeBodyPartList.Count == 0)
            {
                isWrapping = false;
                levelGrid.SetSnakeIsWrapping(false);
            }

            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(GetMoveDir()) - 90);
        }

    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart());
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = snakeBodyPartList.Count - 1; i >= 0; i--)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i], isWrapping);
        }
    }

    public Vector2Int GetGridPosition()
    {
        return gridposition;
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    //i dont even use this function how dumbbma,/./.
    public List<Vector2Int> GetFullSnakePositionList()
    {

        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridposition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GridPosition);
        }
        return gridPositionList;
    }

    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart()
        {
            GameObject snakeBodyGameObject = new GameObject("GameBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.I.snakeBodySprite;
            transform = snakeBodyGameObject.transform;
        }

        public void SetBodyPartColor(Color newColor, SpriteRenderer snakesr)
        {
            transform.GetComponent<SpriteRenderer>().color = newColor;
            snakesr.color = newColor;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition, bool isWrapping)
        {
            this.snakeMovePosition = snakeMovePosition;

            if (isWrapping)
            {
                transform.position = new Vector3(snakeMovePosition.GridPosition.x, snakeMovePosition.GridPosition.y, 0);

                //The iswrapping is getting updated before the body parts can get across the screen
                if (this == snakeMovePosition.SnakeBodyPartList[0])
                {
                    snakeMovePosition.LevelGrid.SetSnakeIsWrapping(false);
                }
            }

            else if (!isWrapping)
            {
                Vector3 gridPos = new Vector3(snakeMovePosition.GridPosition.x, snakeMovePosition.GridPosition.y);
                transform.position = new Vector3(gridPos.x, gridPos.y, 0);

                //transform.position = Vector3.Lerp(transform.position, gridPos, .5f);
            }

            float angle;
            switch (snakeMovePosition.Direction)
            {
                default:
                case Direction.Up:
                    angle = 0;
                    switch (snakeMovePosition.PreviousDirection)
                    {
                        default: angle = 90; break;
                        case Direction.Left: angle = 0 + 45; break;
                        case Direction.Right: angle = 0 - 45; break;
                    }
                    break;
                case Direction.Down:
                    angle = 180;
                    switch (snakeMovePosition.PreviousDirection)
                    {
                        default: angle = 90; break;
                        case Direction.Left: angle = 180 + 45; break;
                        case Direction.Right: angle = 180 - 45; break;
                    }
                    break;
                case Direction.Left:
                    angle = -90;
                    switch (snakeMovePosition.PreviousDirection)
                    {
                        default: angle = -90; break;
                        case Direction.Down: angle = -45; break;
                        case Direction.Up: angle = 45; break;
                    }
                    break;
                case Direction.Right:
                    angle = 90;
                    switch (snakeMovePosition.PreviousDirection)
                    {
                        default: angle = 90; break;
                        case Direction.Down: angle = 45; break;
                        case Direction.Up: angle = -45; break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GridPosition;
        }
    }

    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;
        private List<SnakeBodyPart> bodyPartList;
        private LevelGrid lg;

        public Vector2Int GridPosition => gridPosition;
        public Direction Direction => direction;
        public LevelGrid LevelGrid => lg;
        public List<SnakeBodyPart> SnakeBodyPartList => bodyPartList;

        public Direction PreviousDirection
        {
            get
            {
                if (previousSnakeMovePosition == null)
                {
                    return Direction.Right;
                }
                else
                {
                    return previousSnakeMovePosition.direction;
                }
            }
        }

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction, List<SnakeBodyPart> bodyPartList, LevelGrid levelGrid)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
            this.bodyPartList = bodyPartList;
            this.lg = levelGrid;
        }
    }
}
