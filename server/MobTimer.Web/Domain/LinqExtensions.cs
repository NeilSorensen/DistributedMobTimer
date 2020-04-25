using System;
using System.Collections.Generic;
using System.Linq;

namespace MobTimer.Web.Domain 
{
    public static class LinqExtensions {
        private static Random randomizer = new Random();
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => randomizer.Next());
        }
    }
}