﻿using System;
using System.Diagnostics;

namespace Yalla
{
    partial class LogManager
    {
        /// <summary>
        /// Retrieves or creates the logger for the current type.
        /// </summary>
        public static ILog GetCurrentClassLogger()
        {
#if SILVERLIGHT
            var frame = new StackFrame(1);
#else
            var frame = new StackFrame(1, false);
#endif
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            return GetLogger(type);
        }
    }
}