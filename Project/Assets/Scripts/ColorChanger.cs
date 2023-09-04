using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{

    public Color[] colors;
    public SpriteRenderer snakeModelHead;
    public SpriteRenderer snakeModelBody;
    int colorInt;

    void Awake()
    {
        GameHandler.snakeColor = colors[colorInt];
        ChangeModelColor();
    }

    public void ChangeColor(int direction)
    {
        if (direction == -1 && colorInt == 0)
        {
            colorInt = colors.Length - 1;
        }
        else if (direction == 1 && colorInt == colors.Length - 1)
        {
            colorInt = 0;
        }
        else
        {
            colorInt += direction;
        }

        GameHandler.snakeColor = colors[colorInt];
        ChangeModelColor();
    }

    void ChangeModelColor()
    {

        snakeModelHead.color = colors[colorInt];
        snakeModelBody.color = colors[colorInt];
    }
}
