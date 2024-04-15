using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DataStorageSystem : MonoBehaviour
{
    public TMP_InputField eventNameInputField;
    public TMP_InputField folderLocationInputField;

    void Start()
    {
        LoadDataIntoInputFields();
    }

    public void SaveData()
    {
        string eventName = eventNameInputField.text;
        string folderLocation = folderLocationInputField.text;

        PlayerPrefs.SetString("EventName", eventName);
        PlayerPrefs.SetString("FolderLocation", folderLocation);

        Debug.Log("Data saved: EventName = " + eventName + ", FolderLocation = " + folderLocation);

        SceneManager.LoadScene(0);
    }

    private void LoadDataIntoInputFields()
    {
        eventNameInputField.text = GetEventName();
        folderLocationInputField.text = GetFolderLocation();
    }

    public static string GetEventName()
    {
        return PlayerPrefs.GetString("EventName", "DefaultEvent");
    }

    public static string GetFolderLocation()
    {
        return PlayerPrefs.GetString("FolderLocation", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
    }
}
