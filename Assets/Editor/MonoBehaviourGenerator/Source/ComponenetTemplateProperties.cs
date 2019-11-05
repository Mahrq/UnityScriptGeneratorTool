using UnityEngine;
using UnityEditor;

namespace MonoBehaviourGenerator
{
    [System.Serializable]
    public class ComponentTemplateProperties
    {
        #region Properties

        public bool include;
        public bool serialize;
        public AccessModifier accessModifier;
        public ReferenceImplementation implementation;
        private ComponentName name;
        private string prefixName;
        public string PrefixName
        {
            set
            {
                prefixName = value;
            }
        }

        #endregion

        public ComponentTemplateProperties(ComponentName setName)
        {
            name = setName;
            implementation = ReferenceImplementation.OnAwake;
        }

        public static event ImplementationEventHandler OnImplementationReady;

        public string ComponentGenerator(out string propertyTemplateCopy)
        {
            TextAsset fieldTemplate = AssetDatabase.LoadAssetAtPath(
                "Assets/Editor/MonoBehaviourGenerator/FieldTemplate.txt", typeof(TextAsset)) as TextAsset;
            if (fieldTemplate)
            {
                bool implementInAwake = false;
                string fTemplateCopy = fieldTemplate.text;
                string fAccessMod;
                switch (accessModifier)
                {
                    case AccessModifier.Private:
                        fAccessMod = "private";
                        break;
                    case AccessModifier.Protected:
                        fAccessMod = "protected";
                        break;
                        //Reusing the same enum. Fields cannot be virtual anyway so making it private.
                    case AccessModifier.Virtual:
                        fAccessMod = "private";
                        break;
                    default:
                        fAccessMod = "private";
                        break;
                }
                string fType;
                switch (name)
                {
                    case ComponentName.Transform:
                        fType = "Transform";
                        break;
                    case ComponentName.Rigidbody:
                        fType = "Rigidbody";
                        break;
                    case ComponentName.Collider:
                        fType = "Collider";
                        break;
                    case ComponentName.Animator:
                        fType = "Animator";
                        break;
                    case ComponentName.Renderer:
                        fType = "Renderer";
                        break;
                    case ComponentName.AudioSource:
                        fType = "AudioSource";
                        break;
                    default:
                        fType = "Transform";
                        break;
                }
                string fPrefix = prefixName;
                fPrefix = fPrefix.Replace(fPrefix[0].ToString(), char.ToLower(fPrefix[0]).ToString());
                switch (implementation)
                {
                    case ReferenceImplementation.None:
                        propertyTemplateCopy = "";
                        break;
                    case ReferenceImplementation.Property:
                        propertyTemplateCopy = PropertyGenerator(fType, fPrefix);
                        break;
                    case ReferenceImplementation.OnAwake:
                        propertyTemplateCopy = "";
                        implementInAwake = true;
                        break;
                    case ReferenceImplementation.Both:
                        propertyTemplateCopy = PropertyGenerator(fType, fPrefix);
                        implementInAwake = true;
                        break;
                    default:
                        propertyTemplateCopy = "";
                        break;
                }
                string fSerialize = serialize ? "[SerializeField]" : "";

                if (implementInAwake)
                {
                    ImplementationGenerator(fType, fPrefix);
                }

                fTemplateCopy = fTemplateCopy.Replace("ATTRIBUTE", fSerialize);
                fTemplateCopy = fTemplateCopy.Replace("FIELD_ACCESSMOD", fAccessMod);
                fTemplateCopy = fTemplateCopy.Replace("FIELD_TYPE", fType);
                fTemplateCopy = fTemplateCopy.Replace("PREFIX_", fPrefix);
                return fTemplateCopy;

            }
            else
            {
                Debug.LogError("Can't find FieldTemplate.txt!\n" +
                "Ensure that you havn't moved or deleted the file at Assets/Editor/MonoBehaviourGenerator/FieldTemplate.txt");
                propertyTemplateCopy = "";
                return null;
            }
        }

        private string PropertyGenerator(string type, string fieldPrefix)
        {
            TextAsset propertyTemplate = AssetDatabase.LoadAssetAtPath(
                "Assets/Editor/MonoBehaviourGenerator/FieldPropertyTemplate.txt", typeof(TextAsset)) as TextAsset;
            if (propertyTemplate)
            {
                string propertyCopy = propertyTemplate.text;
                string propPrefix = fieldPrefix;
                //Change first character to upper case
                propPrefix = propPrefix.Replace(propPrefix[0].ToString(), char.ToUpper(propPrefix[0]).ToString());
                propertyCopy = propertyCopy.Replace("FIELD_TYPE", type);
                propertyCopy = propertyCopy.Replace("PREFIX_", propPrefix);
                propertyCopy = propertyCopy.Replace("pREFIX_", fieldPrefix);
                return propertyCopy;

            }
            else
            {
                Debug.LogError("Can't find FieldPropertyTemplate.txt!\n" +
                "Ensure that you havn't moved or deleted the file at Assets/Editor/MonoBehaviourGenerator/FieldPropertyTemplate.txt");
                return null;
            }
        }

        private void ImplementationGenerator(string type, string fieldPrefix)
        {
            //Fill out the template then send it as an event where the method writer will listen.
            TextAsset impTemplate = AssetDatabase.LoadAssetAtPath(
            "Assets/Editor/MonoBehaviourGenerator/ImplementationTemplate.txt", typeof(TextAsset)) as TextAsset;
            if (impTemplate)
            {
                string impCopy = impTemplate.text;
                impCopy = impCopy.Replace("PREFIX_", fieldPrefix);
                impCopy = impCopy.Replace("FIELD_TYPE", type);
                if (OnImplementationReady != null)
                {
                    OnImplementationReady(impCopy);
                }
            }
            else
            {
                Debug.LogError("Can't find FieldTemplate.txt!\n" +
                "Ensure that you havn't moved or deleted the file at Assets/Editor/MonoBehaviourGenerator/ImplementationTemplate.txt");
            }
        }
    }
}
