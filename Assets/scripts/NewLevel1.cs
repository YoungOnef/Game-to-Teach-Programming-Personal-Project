using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLevel1 : MonoBehaviour
{
    private InputHandler inputOutputManager = new InputHandler();

    // The TaskManager object
    private TaskManager taskManager = new TaskManager();

    // The ObjectManager object
    private ObjectManager objectManager = new ObjectManager();

    // The LuaManager object
    private LuaManager luaManager = new LuaManager();

    void Start()
    {
        GameObject newCube = GameObject.Find("cube");
        //renderer = cube:GetComponent("Renderer")
        // Get the renderer component of the cube
        cubeRenderer = cube.GetComponent<Renderer>();

        // Get the name of the current scene
        sceneName = SceneManager.GetActiveScene().name;
        sceneName += ".txt";



        // Initialize the userOutTextFunctionDispaly variable
        //userOutTextFunctionDispaly = GameObject.Find("FunctionDisplayText").GetComponent<TextMeshProUGUI>();
        //Teleport(10, 10, 10);

    }

}
