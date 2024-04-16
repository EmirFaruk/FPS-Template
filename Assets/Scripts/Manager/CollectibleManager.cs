using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private static float totalPoint;

    public static void AddPoint(float point)
    {
        totalPoint += point;
        print("totalPoint : " + totalPoint);
    }
}


