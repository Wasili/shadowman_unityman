using UnityEngine;
using System.Collections;

public class HandleFinish : MonoBehaviour {
    public LoadingProgress loadingBar;
    public string sceneToLoad = "Level1";

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.gameObject.tag == "Player")
        {
            loadingBar.LoadScene(sceneToLoad);
        }
    }
}
