namespace Efficient_Updating_System
{
    public interface ILateUpdate : IAbstractUpdate
    {
        void EfficientLateUpdate(float time);
    }
}