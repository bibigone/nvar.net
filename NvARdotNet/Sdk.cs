using System;

namespace NvARdotNet
{
    public static class Sdk
    {
        public static Version Version
        {
            get
            {
                var status = Native.PoseApi.GetVersion(out var version);
                NvarException.ThrowIfNotSuccess(status, Native.PoseApi.PREFIX + nameof(Native.PoseApi.GetVersion));
                var build = (version >> 8) & 0xFF;
                var minor = (version >> 16) & 0xFF;
                var major = (version >> 24) & 0xFF;
                return new((int)major, (int)minor, (int)build);
            }
        }

        public static void AddPath(string path)
        {
            var pathValue = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrWhiteSpace(pathValue))
                pathValue = string.Empty;
            else if (pathValue[0] != ';')
                pathValue = ";" + pathValue;
            if ((pathValue + ";").IndexOf(";" + path + ";", StringComparison.InvariantCultureIgnoreCase) >= 0)
                return;
            pathValue = path + pathValue;
            Environment.SetEnvironmentVariable("PATH", pathValue);
        }

        /// <summary>
        /// Default value for <see cref="Feature.ModelDir"/>.
        /// Initially is initialized from environment variable <c>NVAR_MODEL_DIR</c>
        /// but can be explicitly overridden by user.
        /// </summary>
        public static string? ModelDir
        {
            get => modelDir;
            set => modelDir = value;
        }
        private static volatile string? modelDir = Environment.GetEnvironmentVariable("NVAR_MODEL_DIR");
    }
}
