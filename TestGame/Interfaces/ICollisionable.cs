namespace TestGame.Interfaces
{
    public interface ICollisionable
    {
        bool IsCollisionDetect(GameObject collisionObject);
        void OnCollisionDetect(GameObject collisionObject);
    }
}
