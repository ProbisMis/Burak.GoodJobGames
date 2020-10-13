namespace GoodJobGames.Utilities.ValidationHelper.ValidatorResolver
{
    public interface IValidatorResolver
    {
        T Resolve<T>() where T : class;
        T Resolve<T>(bool throwIfTypeNotFound) where T : class;
    }
}
