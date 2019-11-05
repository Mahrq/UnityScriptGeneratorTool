using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MonoBehaviourGenerator
{
    [System.Serializable]
    public class MethodTemplates
    {
        #region EditorWindow input fields

        public MethodTemplateProperties awake = new MethodTemplateProperties(MethodName.Awake, Params.None);
        public MethodTemplateProperties start = new MethodTemplateProperties(MethodName.Start, Params.None);
        public MethodTemplateProperties onEnable = new MethodTemplateProperties(MethodName.OnEnable, Params.None);
        public MethodTemplateProperties onDisable = new MethodTemplateProperties(MethodName.OnDisable, Params.None);
        public MethodTemplateProperties update = new MethodTemplateProperties(MethodName.Update, Params.None);
        public MethodTemplateProperties fixedUpdate = new MethodTemplateProperties(MethodName.FixedUpdate, Params.None);
        public MethodTemplateProperties lateUpdate = new MethodTemplateProperties(MethodName.LateUpdate, Params.None);
        public MethodTemplateProperties onCollisionEnter = new MethodTemplateProperties(MethodName.OnCollisionEnter, Params.Collision);
        public MethodTemplateProperties onCollisionStay = new MethodTemplateProperties(MethodName.OnCollisionStay, Params.Collision);
        public MethodTemplateProperties onCollisionExit = new MethodTemplateProperties(MethodName.OnCollisionExit, Params.Collision);
        public MethodTemplateProperties onTriggerEnter = new MethodTemplateProperties(MethodName.OnTriggerEnter, Params.Collider);
        public MethodTemplateProperties onTriggerStay = new MethodTemplateProperties(MethodName.OnTriggerStay, Params.Collider);
        public MethodTemplateProperties onTriggerExit = new MethodTemplateProperties(MethodName.OnTriggerExit, Params.Collider);

        #endregion
        private MethodTemplateProperties[] templateMapping = new MethodTemplateProperties[13];

        public MethodTemplates()
        {
            templateMapping[(int)MethodName.Awake] = awake;
            templateMapping[(int)MethodName.Start] = start;
            templateMapping[(int)MethodName.OnEnable] = onEnable;
            templateMapping[(int)MethodName.OnDisable] = onDisable;
            templateMapping[(int)MethodName.Update] = update;
            templateMapping[(int)MethodName.FixedUpdate] = fixedUpdate;
            templateMapping[(int)MethodName.LateUpdate] = lateUpdate;
            templateMapping[(int)MethodName.OnCollisionEnter] = onCollisionEnter;
            templateMapping[(int)MethodName.OnCollisionStay] = onCollisionStay;
            templateMapping[(int)MethodName.OnCollisionExit] = onCollisionExit;
            templateMapping[(int)MethodName.OnTriggerEnter] = onTriggerEnter;
            templateMapping[(int)MethodName.OnTriggerStay] = onTriggerStay;
            templateMapping[(int)MethodName.OnTriggerExit] = onTriggerExit;
        }

        /// <summary>
        /// Finds the the instances of properties that have include checked which will help with
        /// generating the method template.
        /// </summary>
        public List<MethodTemplateProperties> RetrieveChosen()
        {
            List<MethodTemplateProperties> temp = new List<MethodTemplateProperties>();

            //Iterate through the array to check which box is ticked
            for (int i = 0; i < templateMapping.Length; i++)
            {
                //If true then add the corresponding template to the list
                if (templateMapping[i].include)
                {
                    temp.Add(templateMapping[i]);
                }
            }
            return temp;
        }
        //Subscribe Awake to the event to let it decide whether or not to fill the implementation template.
        public void SubscribeAwakeToEvent()
        {
            ComponentTemplateProperties.OnImplementationReady += awake.WriteImplementation;
        }
        public void RemoveAwakeFromEvent()
        {
            ComponentTemplateProperties.OnImplementationReady -= awake.WriteImplementation;
        }
    }
}
