using Microsoft.Extensions.Primitives;

namespace Bb.Sql
{

    public interface ISqlServerWatcher : IDisposable
    {
        IChangeToken Watch();
    }


}