using System;
using System.Linq;

namespace ORTS.Core.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<T> SubSample<T>(this IObservable<T> source, int interval)
        {
            var i=-1;
            return source.Where(o =>
                {
                    i = (i + 1) % interval;
                    return i == 0;
                });
                   
        }
    }
}
