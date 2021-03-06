﻿using System;
using System.Collections.Generic;

namespace Prism.Logging
{
    public static class ILoggerExtensions
    {
        private const string Category = nameof(Category);

        public static void Debug(this ILogger logger, string message, IDictionary<string, string> properties = null)
        {
            if (properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            properties[Category] = nameof(Debug);

            logger.Log(message, properties);
        }

        public static void Debug(this ILogger logger, string message, params (string key, string value)[] properties) =>
            Debug(logger, message, GetProperties(properties));

        public static void Info(this ILogger logger, string message, IDictionary<string, string> properties = null)
        {
            if (properties is null)
            {
                properties = new Dictionary<string, string>();
            }

            properties[Category] = nameof(Info);

            logger.Log(message, properties);
        }

        public static void Info(this ILogger logger, string message, params (string key, string value)[] properties) =>
            Info(logger, message, GetProperties(properties));

        public static void Log(this ILogger logger, string message) =>
            logger.Log(message, new Dictionary<string, string>());

        public static void Log(this ILogger logger, string message, params (string key, string value)[] properties) =>
            logger.Log(message, GetProperties(properties));

        public static void Log(this ILogger logger, Exception ex, IDictionary<string, string> properties)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties.Add("Type", ex.GetType().FullName);
            properties.Add("StackTrace", ex.StackTrace);

            logger.Log(ex.Message, properties);
        }

        public static void Log(this ILogger logger, Exception ex, params (string key, string value)[] properties) =>
            Log(logger, ex, GetProperties(properties));

        public static void Report(this ICrashesService logger, Exception ex) =>
            logger.Report(ex, new Dictionary<string, string>());

        public static void Report(this ICrashesService logger, Exception ex, params (string key, string value)[] properties) =>
            logger.Report(ex, GetProperties(properties));

        public static void TrackEvent(this IAnalyticsService logger, string name) =>
            logger.TrackEvent(name, new Dictionary<string, string>());

        public static void TrackEvent(this IAnalyticsService logger, string name, params (string key, string value)[] properties) =>
            logger.TrackEvent(name, GetProperties(properties));

        public static void Warn(this ILogger logger, string message, IDictionary<string, string> properties = null)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            properties[Category] = nameof(Warn);

            logger.Log(message, properties);
        }

        public static void Warn(this ILogger logger, string message, params (string key, string message)[] properties) =>
            Warn(logger, message, GetProperties(properties));

        private static IDictionary<string, string> GetProperties((string key, string message)[] properties)
        {
            var dict = new Dictionary<string, string>();
            foreach (var (key, message) in properties)
            {
                dict.Add(key, message);
            }
            return dict;
        }
    }
}
