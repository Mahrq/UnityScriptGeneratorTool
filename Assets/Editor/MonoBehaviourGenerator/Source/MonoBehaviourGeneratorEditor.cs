using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
/// <summary>
/// Author:         Mark Mendoza
/// 
/// Description:    Editor window tool for creating MonoBehaviour scripts that include methods, components
///                 and reference implementation specified by the user.
/// 
/// Notes:          
/// </summary>
namespace MonoBehaviourGenerator
{
    public class MonoBehaviourGeneratorEditor : EditorWindow
    {
        ScriptableObject target;
        SerializedObject sObj;
        SerializedProperty methodTemplates;
        SerializedProperty componentTemplates;

        [Tooltip("Name of the MonoBehaviour script that will be made.")]
        private string scriptName = "NewBehaviour";
        [Tooltip("Name of the folder in the Assets folder.")]
        private string folderName = "Source";
        [SerializeField]
        [Tooltip("Set which Unity methods will be generated in the script.")]
        private MethodTemplates includeMethods = new MethodTemplates();
        [SerializeField]
        [Tooltip("Set which Unity components will be generated in the script.")]
        private ComponentTemplates includeComponents = new ComponentTemplates();

        private Vector2 scrollPos;

        [MenuItem("Generator/MonoBehavior Generator")]
        static void StartWindow()
        {
            MonoBehaviourGeneratorEditor window = GetWindow<MonoBehaviourGeneratorEditor>();
            window.Show();
        }
        private void OnEnable()
        {
            target = this;
            sObj = new SerializedObject(target);
            methodTemplates = sObj.FindProperty("includeMethods");
            componentTemplates = sObj.FindProperty("includeComponents");

            includeMethods.SubscribeAwakeToEvent();
        }
        private void OnDisable()
        {
            includeMethods.RemoveAwakeFromEvent();
        }
        private void OnGUI()
        {
            //Dynamic scroller appeaars depending on window size
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);

            GUILayout.BeginVertical();
            //Title
            GUILayout.Space(8);
            GUI.skin.label.fontSize = 16;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("MonoBehaviour Generator");
            GUILayout.Space(32);
            //Script Name
            scriptName = EditorGUILayout.TextField("Name", scriptName);
            GUILayout.Space(8);
            //Folder Destination
            folderName = EditorGUILayout.TextField("Destination Folder", folderName);
            GUILayout.Space(8);
            //Method Selection
            EditorGUILayout.PropertyField(methodTemplates, true);
            GUILayout.Space(8);
            //Component Selection
            EditorGUILayout.PropertyField(componentTemplates, true);
            sObj.ApplyModifiedProperties();
            GUILayout.Space(8);
            //Button
            if (GUILayout.Button("Generate Script"))
            {
                GenerateScript();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void GenerateScript()
        {
            //Retrieve the template file.
            TextAsset templateFile = AssetDatabase.LoadAssetAtPath(
                "Assets/Editor/MonoBehaviourGenerator/ScriptTemplate.txt", typeof(TextAsset)
                ) as TextAsset;
            //Check if the template file exists.
            if (!templateFile)
            {
                Debug.LogError("Can't find ScriptTemplate.txt!\n" +
                    "Ensure that you havn't moved or deleted the file at Assets/Editor/MonoBehaviourGenerator/ScriptTemplate.txt");
                return;
            }
            string directory = $"{Application.dataPath}/{folderName}";
            //If the directory doesn't exist then create one.
            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            string savePath = directory += $"/{scriptName}.cs";
            //Open StreamWriter and create the file.
            if (!System.IO.File.Exists(savePath))
            {
                //Create a copy of template with intention to replace certain keywords in the file
                string templateCopy = templateFile.text;

                #region Field and Property Generator

                string fieldCopy = "";
                string propCopy = "";
                string propRetriever;
                List<ComponentTemplateProperties> fChosenTemplates = includeComponents.RetrieveChosen();
                for (int i = 0; i < fChosenTemplates.Count; i++)
                {
                    fieldCopy += $"{fChosenTemplates[i].ComponentGenerator(out propRetriever)}\n";
                    propCopy += $"{propRetriever}\n\n";
                }

                #endregion

                #region Method Generator

                string methodcopy = "";
                List<MethodTemplateProperties> mChosenTemplates = includeMethods.RetrieveChosen();
                for (int i = 0; i < mChosenTemplates.Count; i++)
                {
                    methodcopy += $"{mChosenTemplates[i].ToString()}\n\n";
                }

                #endregion
                templateCopy = templateCopy.Replace("SCRIPT_NAME", scriptName);
                templateCopy = templateCopy.Replace("COMPONENT_TEMPLATE", fieldCopy);
                templateCopy = templateCopy.Replace("METHOD_TEMPLATE", methodcopy);
                templateCopy = templateCopy.Replace("COMPONENT_PROPERTY_TEMPLATE", propCopy);

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(savePath))
                {
                    sw.Write(templateCopy);
                }
            }
            else
            {
                Debug.LogError($"There is already a file with that name at {savePath}");
            }
            AssetDatabase.Refresh();
        }
    }
}



