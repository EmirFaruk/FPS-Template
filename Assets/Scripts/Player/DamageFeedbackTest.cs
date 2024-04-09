using System;
using UnityEngine;

public class DamageFeedbackTest : MonoBehaviour
{

    [SerializeField] Transform damageUI;
    [SerializeField] private Transform Enemy;

    private bool countdownStart;
    private bool countdownEnd;
    float countdowm = 2;


    private void Awake()
    {
        //    Player = this.transform;
    }

    private void Start()
    {
        // damageUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //ShowUI(Enemy);
            ShowDamageIndicator(Enemy);
        }

        //  if (Input.GetKeyDown(KeyCode.Z))
        //  {
        //      ReLocateEnemy();
        //      countdowm = 10;
        //  }
        //
        //  ShowUI();
        //  // countdowm -= Time.deltaTime;
        //  // if (countdowm >= 0)
        //  //     damageUI.gameObject.SetActive(true);
        //  // else damageUI.gameObject.SetActive(false);
        //
        //  DrawLine();
        // if (countdownStart)
        // {
        //     countdowm -= Time.deltaTime;
        //     if (countdowm <= 0)
        //     {
        //         countdownStart = false;
        //         damageUI.gameObject.SetActive(false);
        //     }
        // }
        //

    }



    public void ShowUI(Transform enemy)
    {
        countdownStart = true;
        damageUI.gameObject.SetActive(true);

        Vector3 direction = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z)
                                - new Vector3(transform.position.x, 0, transform.position.z);
        direction.y = 0;

        //  float angle = Vector3.Angle(green.transform.forward, purple.transform.forward);
        float angle = Vector3.Angle(transform.forward, direction);

        //    Debug.Log("clean angle : " + angle);

        if (direction.z >= transform.position.z && direction.x >= transform.position.x)
        {
            //  feedbackText.text = "ÖNÜNDE VE SAÐDA";
            angle = Math.Abs(angle - 90);
            //  Debug.Log("Area I : " + angle);
        }
        if (direction.z >= transform.position.z && direction.x < transform.position.x)
        {
            //    feedbackText.text = "ÖNÜNDE VE SOLDA";
            angle = Math.Abs(angle + 90);
            //   Debug.Log("Area II : " + angle);
        }
        if (direction.z < transform.position.z && direction.x < transform.position.x)
        {
            //  feedbackText.text = "ARKANDA VE SOLDA";
            angle = Math.Abs(angle + 90);
            //  Debug.Log("Area III : " + angle);
        }
        if (direction.z < transform.position.z && direction.x >= transform.position.x)
        {
            //  feedbackText.text = "ARKANDA VE SAÐDA";
            angle = (90 - angle) + 360;
            //  Debug.Log("Area IV : " + angle);
        }

        // feedbackText.text = angle.ToString();

        damageUI.transform.rotation = Quaternion.AngleAxis(angle - 90, new Vector3(0, 0, 1));
    }

    public void ShowDamageIndicator(Transform enemy)
    {
        print("Damage Indicator");

        Vector3 direction = transform.position - enemy.transform.position;
        Quaternion enemyRot = Quaternion.LookRotation(direction);
        enemyRot.z = -enemyRot.y;
        enemyRot.x = enemyRot.y = 0;

        Vector3 northDirection = new Vector3(0, 0, transform.eulerAngles.y - 180);

        damageUI.transform.rotation = enemyRot * Quaternion.Euler(northDirection);
    }

}

/* OLD
 *  /*   void ReLocateEnemy()
 //   {
 //       countdowm = 3;
 //       enemy.transform.position = new Vector3(Random.Range(-11, 10), 0, Random.Range(-11, 10));
 //   }
 //
 //   void DrawLine()
 //   {
 //       Debug.DrawRay(Player.position, Player.forward * 5, Color.green);
 //       Debug.DrawRay(enemy.position, enemy.forward * 5, Color.magenta);
 //
 //       Vector3 direction = enemy.transform.position - Player.transform.position;
 //       Debug.DrawRay(Player.position, direction, Color.black);
 //
 //   }
 //
    void ShowUI()
    {
        Vector3 direction = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z)
                                - new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
        direction.y = 0;

        //  float angle = Vector3.Angle(green.transform.forward, purple.transform.forward);
        float angle = Vector3.Angle(Player.forward, direction);

        Debug.Log("clean angle : " + angle);

        if (direction.z >= Player.position.z && direction.x >= Player.position.x)
        {
            //  feedbackText.text = "ÖNÜNDE VE SAÐDA";
            angle = Math.Abs(angle - 90);
            Debug.Log("Area I : " + angle);
        }
        if (direction.z >= Player.position.z && direction.x < Player.position.x)
        {
            //    feedbackText.text = "ÖNÜNDE VE SOLDA";
            angle = Math.Abs(angle + 90);
            Debug.Log("Area II : " + angle);
        }
        if (direction.z < Player.position.z && direction.x < Player.position.x)
        {
            //  feedbackText.text = "ARKANDA VE SOLDA";
            angle = Math.Abs(angle + 90);
            Debug.Log("Area III : " + angle);
        }
        if (direction.z < Player.position.z && direction.x >= Player.position.x)
        {
            //  feedbackText.text = "ARKANDA VE SAÐDA";
            angle = (90 - angle) + 360;
            Debug.Log("Area IV : " + angle);
        }

        // feedbackText.text = angle.ToString();

        damageUI.transform.rotation = Quaternion.AngleAxis(angle - 90, new Vector3(0, 0, 1));
    }

 
  void Method1()
    {
        Vector3 direction = enemy.transform.position - Player.transform.position;

        Player.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    void Method2()
    {
        Vector3 direction = enemy.transform.position - Player.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        Quaternion current = Player.localRotation;

        Player.transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime);
    }
 */