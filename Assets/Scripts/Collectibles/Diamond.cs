using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using RunnnerGame.Manager;

public class Diamond : MonoBehaviour, ICollectible
{
    private Action action;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float point;


    // Start is called before the first frame update
    void Start()
    {
        action = ApplyEffect;
    }

    void ApplyEffect()
    {
        CollectibleManager.Instance.AddPoint(point);

        // add points 
        // or stun enemy etc.
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void Collect()
    {
        float endVal = transform.position.y + 4;

        print("COLLECT " + this.gameObject.name);

        transform.DOMoveY(endVal, 1).OnComplete(()=> gameObject.SetActive(false));
        action?.Invoke();
    }

    private void OnDisable()
    {
        Destroy(gameObject, 2);
    }
}

