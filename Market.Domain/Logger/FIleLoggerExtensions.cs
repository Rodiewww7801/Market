using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Logger
{
    public static class FIleLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string path)
        {
            factory.AddProvider(new FileLoggerProvider(path));
            return factory;
        }
    }
}
