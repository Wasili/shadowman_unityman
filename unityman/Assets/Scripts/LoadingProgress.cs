using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingProgress : MonoBehaviour {
    public GameObject canvasHolder;

    public void LoadScene(string scene)
    {
        canvasHolder.SetActive(true);
        StartCoroutine("AscynSceneLoader", scene);
    }

    public IEnumerator AscynSceneLoader(string scene)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            GetComponent<Image>().fillAmount = progress;

            if (operation.progress == 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
