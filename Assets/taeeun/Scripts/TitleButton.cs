using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public Image targetImage;
    public Sprite newImage;
    private Sprite originalImage;
    // Start is called before the first frame update
    void Start()
    {
        originalImage = targetImage.sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeSprite()
    {
        targetImage.sprite = newImage; // ��ư�� �������� �̹����� ��ȯ
        Invoke("BackToNormal", 0.3f); // 0.3�ʵ� �ٽ� ���ƿ�
    }
    private void BackToNormal()
    {
        targetImage.sprite = originalImage;
    }
    public void quit()
    {
        Application.Quit();
    }
    public void SceneChangeDelay()
    {
        Invoke("ChangeScene", 0.3f);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
