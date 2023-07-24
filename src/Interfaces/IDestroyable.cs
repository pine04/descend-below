namespace DescendBelow {
    // Defines a read-only boolean property to determine when an object can be destroyed and a method to specify its behavior when destroyed.
    public interface IDestroyable {
        bool CanDestroy { get; }
        void Destroy();
    }
}