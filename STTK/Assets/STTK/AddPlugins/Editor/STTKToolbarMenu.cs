using UnityEditor;
using UnityEngine;

public class STTKToolbarMenu : MonoBehaviour
{
    [MenuItem("STTK/Add Components/Add MenuFramework Components")]
    static void CreateMenu()
    {
        GameObject IOManager = Resources.Load("IOManager") as GameObject;
        Instantiate(IOManager, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject MenuSystem = Resources.Load("MenuSystem") as GameObject;
        Instantiate(MenuSystem, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject SideMenu = Resources.Load("SideMenus") as GameObject;
        Instantiate(SideMenu, new Vector3(0, 0, 0), Quaternion.identity);
    }
}