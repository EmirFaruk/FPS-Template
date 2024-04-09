using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public abstract class Interactable : MonoBehaviour
{
    #region VARIABLES
    [Inject] protected ZenjectGetter ZenjectGetter;
    protected HUD HUD;
    protected Renderer interactableRenderer;
    protected List<Renderer> interactableRenderers = new();
    protected List<Material> materialHandler = new();
    protected List<Material>[] materialHandlers;

    private Material outlineMaterial;

    private bool hasRenderer;

    private string popUpBase;
    private string popUpHue;
    private string popUpEnd;
    private Color hueColor;
    private Vector3 popUpPosition;
    #endregion

    #region ABSTRACT METHODS
    public abstract void OnInteract();

    public virtual void OnFocus()
    {
        UpdateOutline(true);
        ShowPopUp();
    }

    public virtual void OnLoseFocus()
    {
        UpdateOutline(false);

        HUD.PopUp.SetEnability(false);
    }
    #endregion

    #region UNITY EVENT FUNCTIONS

    public virtual void Awake()
    {
        HUD = ZenjectGetter.HUD;
    }

    public virtual void OnEnable()
    {
        SetLayer();
        InitializeOutlineMaterial();
        InitializeRenderer();
    }

    #endregion

    #region METHODS

    #region Initialization
    private void SetLayer()
    {
        //interaction layer;
        this.gameObject.layer = 7;
    }

    private void InitializeOutlineMaterial()
    {
        outlineMaterial = new Material(HUD.OutlineShader);
    }

    private void InitializeRenderer()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            hasRenderer = true;
            interactableRenderer = renderer;
            materialHandler = interactableRenderer.materials.ToList();
        }
        else
        {
            hasRenderer = false;
            InitializeChildRenderers();
        }
    }

    private void InitializeChildRenderers()
    {
        foreach (Transform item in transform)
        {
            if (item.TryGetComponent(out Renderer childRenderer))
                interactableRenderers.Add(childRenderer);
        }

        materialHandlers = new List<Material>[interactableRenderers.Count];

        for (int i = 0; i < interactableRenderers.Count; i++)
            materialHandlers[i] = interactableRenderers[i].materials.ToList();
    }

    public virtual void Init(Vector3 popUpPosition, string popUpBase, string popUphue, string popUpEnd, Color hueColor)
    {
        this.popUpPosition = popUpPosition;
        this.popUpBase = popUpBase;
        this.popUpHue = popUphue;
        this.popUpEnd = popUpEnd;
        this.hueColor = hueColor;
    }
    #endregion    

    protected void UpdateOutline(bool isShowing)
    {
        if (hasRenderer)
            UpdateMaterials(interactableRenderer, materialHandler, isShowing);

        else
            for (int i = 0; i < interactableRenderers.Count; i++)
                UpdateMaterials(interactableRenderers[i], materialHandlers[i], isShowing);
    }

    void UpdateMaterials(Renderer currentRenderer, List<Material> currentMaterialHandlers, bool isShowing)
    {
        if (isShowing)
            currentMaterialHandlers.Add(outlineMaterial);

        else
            currentMaterialHandlers.Remove(currentMaterialHandlers.Last());

        currentRenderer.materials = currentMaterialHandlers.ToArray();
    }

    public virtual void ShowPopUp(Vector3 position = default, string messageBase = "Interact ", string hue = "this ", string end = "", Color hueColor = default)
    {
        HUD.PopUp.SetEnability(true);
        HUD.PopUp.SetPreviewWorldPosition(position == default ? this.popUpPosition : position);
        //HUD.ShowPopUp(position == default ? this.popUpPosition : position, messageBase, hue, end, hueColor);
    }
    #endregion
}

#region Backups
/*
    if (TryGetComponent(out Renderer renderer))
        {
            hasRenderer = true;
            interactableRenderer = renderer;
            materialHandler = interactableRenderer.materials.ToList();
        }
        else
        {
            hasRenderer = false;

            foreach (Transform item in transform)
            {
                if (item.TryGetComponent(out Renderer childRenderer))
                    interactableRenderers.Add(childRenderer);
            }

            materialHandlers = new List<Material>[interactableRenderers.Count];

            for (int i = 0; i < interactableRenderers.Count; i++)
                materialHandlers[i] = interactableRenderers[i].materials.ToList();
        }*/

/*
 
 void ShowOutline()
    {
        if (hasRenderer)
        {
            materialHandler.Add(outlineMaterial);
            interactableRenderer.materials = materialHandler.ToArray();
        }
        else
        {
            for (int i = 0; i < materialHandlers.Length; i++)
            {
                materialHandlers[i].Add(outlineMaterial);
                interactableRenderers[i].materials = materialHandlers[i].ToArray();
            }
        }
    }

    void RemoveOutline()
    {
        if (hasRenderer)
        {
            materialHandler.Remove(materialHandler.Last());
            interactableRenderer.materials = materialHandler.ToArray();
        }
        else
        {
            for (int i = 0; i < materialHandlers.Length; i++)
            {
                materialHandlers[i].Remove(materialHandlers[i].Last());
                interactableRenderers[i].materials = materialHandlers[i].ToArray();
            }
        }
    }
 
 */
#endregion