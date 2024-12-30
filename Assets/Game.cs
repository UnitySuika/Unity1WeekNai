using HatenoWorks.Novel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private MainGame mainGame;
    private void Start()
    {
        mainGame.StartNovel();
    }
}
