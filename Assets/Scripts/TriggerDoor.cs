using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TriggerDoor : MonoBehaviour
{
    void Start()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "myassetBundle"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("MyObject");
        Instantiate(prefab);

        myLoadedAssetBundle.Unload(false);
    }


    void OnTriggerEnter(Collider doorInformation)
    {
        if (doorInformation.gameObject.name == "Player")
        {
            Debug.Log("Collision Detected");
            SceneManager.LoadScene("TestLevel02", LoadSceneMode.Single); 
        }
    }
}
