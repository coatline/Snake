using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    private GameObject foodGameObject;
    private int height;
    private int width;
    private Snake snake;
    public bool isWrapping;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;
        SpawnFood();
    }

    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakePositionList().IndexOf(foodGridPosition) != -1);
        foodGameObject = new GameObject("Apple", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.I.appleSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }
    public bool SnakeTryEatFood(Vector2Int snakeGridPos)
    {
        if (snakeGridPos == foodGridPosition)
        {
            GameAssets.I.PlayAppleCrunch();
            GameObject.Destroy(foodGameObject);
            GameHandler.AddScore(1);
            SpawnFood();
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition, bool isWraping)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width;
            isWrapping = true;
        }
        if (gridPosition.x > width)
        {
            isWrapping = true;
            gridPosition.x = 0;
        }
        if (gridPosition.y < 0)
        {
            isWrapping = true;
            gridPosition.y = height;
        }
        if (gridPosition.y > height)
        {
            isWrapping = true;
            gridPosition.y = 0;
        }
            return gridPosition;
    }

    public bool GetSnakeIsWrapping()
    {
        return isWrapping;
    }

    public void SetSnakeIsWrapping(bool trueorfalse)
    {
        isWrapping = trueorfalse;
    }

    public void TryGetOnWall(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            snake.SnakeDied();
        }
        if (gridPosition.x > width)
        {
            snake.SnakeDied();
        }
        if (gridPosition.y < 0)
        {
            snake.SnakeDied();
        }
        if (gridPosition.y > height)
        {
            snake.SnakeDied();
        }
    }
}
