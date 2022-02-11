using Microsoft.Extensions.Primitives;


namespace Bb.Sql
{

    internal class SqlServerPeriodicalWatcher : ISqlServerWatcher, IDisposable
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshInterval"></param>
        public SqlServerPeriodicalWatcher(TimeSpan refreshInterval)
        {
            _timer = new Timer(Change, null, TimeSpan.Zero, refreshInterval);
        }


        public SqlServerPeriodicalWatcher Subscribe(Action action)
        {
            this._changeTokenRegistration = ChangeToken.OnChange(() => this.Watch(), action);
            return this;
        }


        private void Change(object? state)
        {
            _cancellationTokenSource?.Cancel();
        }

        public IChangeToken Watch()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var _changeToken = new CancellationChangeToken(_cancellationTokenSource.Token);
            return _changeToken;

        }


        private readonly Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;
        private IDisposable _changeTokenRegistration;
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                    _cancellationTokenSource?.Dispose();
                    _changeTokenRegistration?.Dispose();
                }

                // TODO: libérer les ressources non managées (objets non managés) et substituer le finaliseur
                // TODO: affecter aux grands champs une valeur null
                disposedValue = true;
            }
        }

        // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour libérer les ressources non managées
        // ~SqlServerPeriodicalWatcher()
        // {
        //     // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ne changez pas ce code. Placez le code de nettoyage dans la méthode 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}