using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Match3
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                    if (_instance == null)
                    {
                        throw new Exception("no GameManager!");
                    }
                }
                return _instance;
            }
        }

        public SpriteManager spriteManager;
        public Hud hud;
        public bool debugMode;
        public BoardDebugView boardDebugView;
        public GameObject debugUI;

        void Awake()
        {
            boardDebugView.gameObject.SetActive(debugMode);
            debugUI.SetActive(debugMode);
        }
    }
}