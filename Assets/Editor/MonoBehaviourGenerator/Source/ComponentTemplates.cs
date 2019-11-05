using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace MonoBehaviourGenerator
{
    public delegate void ImplementationEventHandler(string sendMessage);
    [System.Serializable]
    public class ComponentTemplates
    {
        #region EditorWindow input fields

        public string prefix = "this";

        public ComponentTemplateProperties transform = new ComponentTemplateProperties(ComponentName.Transform);
        public ComponentTemplateProperties rigidbody = new ComponentTemplateProperties(ComponentName.Rigidbody);
        public ComponentTemplateProperties collider = new ComponentTemplateProperties(ComponentName.Collider);
        public ComponentTemplateProperties animator = new ComponentTemplateProperties(ComponentName.Animator);
        public ComponentTemplateProperties renderer = new ComponentTemplateProperties(ComponentName.Renderer);
        public ComponentTemplateProperties audioSource = new ComponentTemplateProperties(ComponentName.AudioSource);

        #endregion

        private ComponentTemplateProperties[] templateMapping = new ComponentTemplateProperties[6];
        public ComponentTemplates()
        {
            templateMapping[(int)ComponentName.Transform] = transform;
            templateMapping[(int)ComponentName.Rigidbody] = rigidbody;
            templateMapping[(int)ComponentName.Collider] = collider;
            templateMapping[(int)ComponentName.Animator] = animator;
            templateMapping[(int)ComponentName.Renderer] = renderer;
            templateMapping[(int)ComponentName.AudioSource] = audioSource;
        }

        public List<ComponentTemplateProperties> RetrieveChosen()
        {
            //Reset the prefix name if the supplied prefix has invalid characters
            if (!prefix.Any(char.IsLetter))
            {
                Debug.LogWarning($"The given prefix: {prefix} contained invalid characters so it was reverted back to: \"this\"");
                prefix = "this";
            }

            List<ComponentTemplateProperties> temp = new List<ComponentTemplateProperties>();

            for (int i = 0; i < templateMapping.Length; i++)
            {
                if (templateMapping[i].include)
                {
                    templateMapping[i].PrefixName = prefix;
                    temp.Add(templateMapping[i]);
                }
            }

            return temp;
        }
    }

}
