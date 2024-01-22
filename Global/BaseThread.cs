using System.Runtime.Versioning;
using System;
using System.Threading;

namespace Global;

public abstract class BaseThread
{
    private Thread m_thread;
    public BaseThread()
    {
        m_thread = new Thread(Run);
    }
    public void Join()
    {
        m_thread.Join();
    }
    public bool Join(int millisecondsTimeout)
    {
        return m_thread.Join(millisecondsTimeout);

    }
    public bool Join(TimeSpan timeout)
    {
        return m_thread.Join(timeout);
    }
    public void Start()
    {
        m_thread.Start();
    }
    protected abstract void Run();
}
