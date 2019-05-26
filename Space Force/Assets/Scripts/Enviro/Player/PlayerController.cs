using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject active_item1;
    public GameObject active_item2;
    public Text pauseText, quitText;

    
    public Cam view;
    private EquipmentManager equipment_mgr;
    private Body body;
    private bool gamePaused;

    // Start is called before the first frame update
    void Start()
    {
        equipment_mgr = gameObject.GetComponentInChildren<EquipmentManager>();
        body = gameObject.GetComponentInChildren<Body>();
        view = gameObject.GetComponentInChildren<Cam>();
        gamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("escape") && !gamePaused) // make cursor visible
        {
            Application.Quit();
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetButtonDown("Fire1") && !gamePaused) //  fire primary gun
        {
            Debug.Log("Fire1 Button key pressed");
            equipment_mgr.UsePrimary();
        }

        if (Input.GetButtonDown("Fire2") && !gamePaused) // fire secondary gun
        {
            Debug.Log("Fire2 Button key pressed");
            equipment_mgr.UseSecondary();
        }

        if (Input.GetButtonDown("Jump") && !gamePaused) // jump
        {
            Debug.Log("Jump Button key pressed");
            body.Jump();
        }

        if (Input.GetKeyDown(KeyCode.P)) // pause game
        {
            Debug.Log("Pause Button key pressed");
            Pause();
        }

        // If the user hits escape or quit they should go back to menu
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Q))
        {
            Debug.Log("User quit game. Going back to menu");
            quitText.enabled = true;
            Invoke("GoBackToMenu", 3);
        }


    }

    // pause game by pausing time scale value to zero (affects everthing relying on Time.deltaTime)
    void Pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            gamePaused = true;
            pauseText.enabled = true;

        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            gamePaused = false;
            pauseText.enabled = false;

        }
    }

    void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
