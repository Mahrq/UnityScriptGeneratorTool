using UnityEngine;
using UnityEditor;

namespace MonoBehaviourGenerator
{
    [System.Serializable]
    public class MethodTemplateProperties
    {
        public bool include;
        public AccessModifier accessModifier;
        private MethodName name;
        private Params methodParams;
        private string awakeImplementation = "";

        public MethodTemplateProperties(MethodName setName, Params setParams)
        {
            name = setName;
            methodParams = setParams;
        }
        /// <summary>
        /// Override version returns the completed method template.
        /// </summary>
        public override string ToString()
        {
            TextAsset templateFile = AssetDatabase.LoadAssetAtPath(
                    "Assets/Editor/MonoBehaviourGenerator/MethodTemplate.txt", typeof(TextAsset)) as TextAsset;
            if (templateFile)
            {
                string templateCopy = templateFile.text;
                string mAccessMod;
                switch (accessModifier)
                {
                    case AccessModifier.Private:
                        mAccessMod = "private";
                        break;
                    case AccessModifier.Protected:
                        mAccessMod = "protected";
                        break;
                    case AccessModifier.Virtual:
                        mAccessMod = "protected virtual";
                        break;
                    default:
                        mAccessMod = "private";
                        break;
                }
                string mName;
                switch (name)
                {
                    case MethodName.Awake:
                        mName = "Awake";
                        break;
                    case MethodName.Start:
                        mName = "Start";
                        break;
                    case MethodName.OnEnable:
                        mName = "OnEnable";
                        break;
                    case MethodName.OnDisable:
                        mName = "OnDisable";
                        break;
                    case MethodName.Update:
                        mName = "Update";
                        break;
                    case MethodName.FixedUpdate:
                        mName = "FixedUpdate";
                        break;
                    case MethodName.LateUpdate:
                        mName = "LateUpdate";
                        break;
                    case MethodName.OnCollisionEnter:
                        mName = "OnCollisionEnter";
                        break;
                    case MethodName.OnCollisionStay:
                        mName = "OnCollisionStay";
                        break;
                    case MethodName.OnCollisionExit:
                        mName = "OnCollisionExit";
                        break;
                    case MethodName.OnTriggerEnter:
                        mName = "OnTriggerEnter";
                        break;
                    case MethodName.OnTriggerStay:
                        mName = "OnTriggerStay";
                        break;
                    case MethodName.OnTriggerExit:
                        mName = "OnTriggerExit";
                        break;
                    default:
                        mName = "Start";
                        break;
                }
                string mParam;
                switch (methodParams)
                {
                    case Params.None:
                        mParam = "";
                        break;
                    case Params.Collision:
                        mParam = "Collision collision";
                        break;
                    case Params.Collider:
                        mParam = "Collider other";
                        break;
                    default:
                        mParam = "";
                        break;
                }
                templateCopy = templateCopy.Replace("METHOD_ACCESSMOD", mAccessMod);
                templateCopy = templateCopy.Replace("METHOD_NAME", mName);
                templateCopy = templateCopy.Replace("METHOD_PARAM", mParam);
                templateCopy = templateCopy.Replace("METHOD_IMPLEMENTATION", awakeImplementation);
                //Clear implementation for the next script.
                awakeImplementation = "";
                return templateCopy;
            }
            else
            {
                Debug.LogError("Can't find MethodTemplate.txt!\n" +
                    "Ensure that you havn't moved or deleted the file at Assets/Editor/MonoBehaviourGenerator/MethodTemplate.txt");
                return null;
            }
        }
        public void WriteImplementation(string recievedFromEvent)
        {
            if (name == MethodName.Awake)
            {
                //Automatically include the awake method if any events regarding implementation in awake is raised.
                if (!include)
                {
                    include = true;
                }
                awakeImplementation += $"{recievedFromEvent}\n\t\t";
            }
        }
    }
}
