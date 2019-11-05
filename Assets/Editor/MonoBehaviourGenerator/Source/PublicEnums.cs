
namespace MonoBehaviourGenerator
{
    public enum AccessModifier
    {
        Private,
        Protected,
        //Appends virtual with protected if selected.
        Virtual,
    }
    public enum MethodName
    {
        Awake,
        Start,
        OnEnable,
        OnDisable,
        Update,
        FixedUpdate,
        LateUpdate,
        OnCollisionEnter,
        OnCollisionStay,
        OnCollisionExit,
        OnTriggerEnter,
        OnTriggerStay,
        OnTriggerExit
    }
    public enum ComponentName
    {
        Transform,
        Rigidbody,
        Collider,
        Animator,
        Renderer,
        AudioSource,
    }
    public enum Params
    {
        None,
        Collision,
        Collider
    }
    public enum ReferenceImplementation
    {
        None,
        Property,
        OnAwake,
        Both
    }
}
