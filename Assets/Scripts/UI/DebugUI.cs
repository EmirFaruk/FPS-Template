using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RunnnerGame.UI.Debug
{
    public class DebugUI : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI timerText;

        [SerializeField]
        private KeyCode timerKey;


        private bool timerStart;
        private float timer;

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(timerKey))
            {
                timerStart = !timerStart;
            }


            if (timerStart)
            {
                timer += Time.deltaTime;
                timerText.text = timer.ToString();
            }
            else
            {
                timer = 0;
               // timerText.text = "00";
            }

        }
    }
}


