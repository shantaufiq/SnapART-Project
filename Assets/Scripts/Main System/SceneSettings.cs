using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettings : MonoBehaviour
{
    private int buttonPressCount = 0;
    private float firstButtonPressTime = 0;

    public void OnButtonPressed()
    {
        if (buttonPressCount == 0)
        {
            firstButtonPressTime = Time.time;
            buttonPressCount++;
        }
        else
        {
            buttonPressCount++;

            if (buttonPressCount == 3 && Time.time - firstButtonPressTime < 4)
            {
                ChangeScene();
            }

            else if (buttonPressCount == 3 || Time.time - firstButtonPressTime >= 4)
            {
                buttonPressCount = 0;
            }
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
