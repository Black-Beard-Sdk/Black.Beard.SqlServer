using Microsoft.Extensions.Primitives;

namespace Bb.SqlServerStructures
{

    public interface ISqlServerWatcher : IDisposable
    {
        IChangeToken Watch();

    }

}