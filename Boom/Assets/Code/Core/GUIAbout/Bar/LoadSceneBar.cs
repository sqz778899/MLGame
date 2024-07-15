using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneBar : MonoBehaviour
{
    public Image m_bar;
    public TextMeshProUGUI m_txt_Loading;
    
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(MSceneManager.Instance.CurrentSceneIndex);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                m_bar.fillAmount = displayProgress * 0.01f;
                m_txt_Loading.text = displayProgress + "%";
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            m_bar.fillAmount = displayProgress * 0.01f;
            m_txt_Loading.text = displayProgress + "%";
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
    }
}