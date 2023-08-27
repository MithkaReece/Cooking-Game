using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;


    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;

        //Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    //Load the actual scene from the loading scene
    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
