namespace Practical.Subject;

public class Subject<T> : ISubject<T>
{
    private readonly ICollection<T> _observers = [];
    
    public IDisposable Subscribe(T observer)
    {
        _observers.Add(observer);
        return new Unsubscriber(() => _observers.Remove(observer));   
    }
    
    public void Notify(Action<T> action)
    {
        foreach (var observer in _observers) 
            action(observer);
    }

    private sealed class Unsubscriber(Action unsubscribe) : IDisposable
    {
        public void Dispose() => unsubscribe();
    }
}