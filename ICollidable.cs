namespace DescendBelow {
    public interface ICollidable {
        Collider Collider { get; }
        void Collide(Collider target);
    }
}