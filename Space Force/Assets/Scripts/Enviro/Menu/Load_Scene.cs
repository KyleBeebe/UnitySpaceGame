
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_Scene : MonoBehaviour
{
    public void LoadByIndex(int scene_index)
    {
        SceneManager.LoadScene(scene_index);
    }
}
