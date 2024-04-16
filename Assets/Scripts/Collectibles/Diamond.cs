using DG.Tweening;
using System;
using UnityEngine;

public class Diamond : MonoBehaviour, ICollectible
{
    private Action action = () => { };

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float point = 1;


    void Start()
    {
        action = ApplyEffect;
    }

    void ApplyEffect()
    {
        CollectibleManager.AddPoint(point);

        // add points 
        // or stun enemy etc.
    }


    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        float endVal = transform.position.y + 4;

        print("COLLECT " + this.gameObject.name);

        transform.DOMoveY(endVal, 1).OnComplete(() => gameObject.SetActive(false));
        action.Invoke();
    }

    private void OnDisable()
    {
        Destroy(gameObject, 2);
    }
}

