using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
public bool _gameOver = false;

    public void GameOver()
    {
        _gameOver=true;
    }
}
