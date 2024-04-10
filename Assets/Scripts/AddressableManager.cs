using RenownedGames.Apex;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] private AssetLabelReference assetLabelReference;
    [SerializeField, ReorderableList] private List<AssetReferenceGameObject> assetReferenceGameObject = new();

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        assetReferenceGameObject.ForEach(assetReference => assetReference.InstantiateAsync());
    }

    private void LoadWithLabelReference()
    {
        Addressables.LoadAssetsAsync<GameObject>(assetLabelReference.labelString, null).Completed += OnLoadAssetsCompleted;
    }

    private void OnLoadAssetsCompleted(AsyncOperationHandle<IList<GameObject>> handle)
    {
        print("handle : " + handle);
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var asset in handle.Result)
            {
                print("asset : " + asset);
                Instantiate(asset);
            }
        }
    }
}
