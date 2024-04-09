using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RunnnerGame.Manager
{
    public class CollectibleManager : MonoBehaviour
    {
        private static CollectibleManager instance;
        public static CollectibleManager Instance => instance;

        private float totalPoint;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        public void AddPoint(float point)
        {
            totalPoint += point;
            print(totalPoint);
        }
    }
}


