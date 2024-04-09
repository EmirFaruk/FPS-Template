using RenownedGames.AITree;
using System;
using UnityEngine;
using RenownedGames.Apex;

[NodeContent("CompareFloatValues", "Custom/CompareFloatValues")]
public class CompareFloatValues : ObserverDecorator
{
    public enum Operator
    {
        IsLowerOrEqualTo,
        IsLowerThen,
        IsGreaterOrEqualTo,
        IsGreaterThen
    }


    [Title("Blackboard")]
    [SerializeField]
    [Label("Operator")]
    private Operator compareOperator;

    [SerializeField]
    [NonLocal]
    private FloatKey keyA;

    [SerializeField]
    [NonLocal]
    private FloatKey keyB;

    public override event Action OnValueChange;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        keyA.ValueChanged += OnValueChange;
        keyB.ValueChanged += OnValueChange;
    }

 

    /// <summary>
    /// Calculates the result of the condition.
    /// </summary>
    public override bool CalculateResult()
    {
        switch (compareOperator)
        {
            case Operator.IsLowerOrEqualTo:
                return keyA.GetValue() <= keyB.GetValue();
            case Operator.IsLowerThen:
                return keyA.GetValue() < keyB.GetValue();
            case Operator.IsGreaterOrEqualTo:
                return keyA.GetValue() >= keyB.GetValue();
            case Operator.IsGreaterThen:
                return keyA.GetValue() > keyB.GetValue();
            default:
                return false;
        }     
    }
}