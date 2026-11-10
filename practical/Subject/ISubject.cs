namespace Practical;

public interface ISubject<in T>
{
    IDisposable Subscribe(T observer);
}