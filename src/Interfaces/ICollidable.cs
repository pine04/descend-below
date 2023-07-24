namespace DescendBelow {
    // Defines a method that specifies an object's behavior when colliding with another object.
    public interface ICollidable {
        Collider Collider { get; }
        void Collide(Collider target);
    }
}