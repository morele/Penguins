namespace TestGame.Interfaces
{
    public interface ICheckable
    {
        bool IsChecked(GameObject checkedObject);
        void OnChecked();
    }
}
