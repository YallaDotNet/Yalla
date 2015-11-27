namespace Yalla
{
    /// <summary>
    /// System configuration source.
    /// </summary>
    public partial class SystemConfigurationSource : IConfigurationSource
    {
        private static readonly SystemConfigurationSource _default = new SystemConfigurationSource();

        /// <summary>
        /// Gets the default instance of the <see cref="Yalla.SystemConfigurationSource"/> class.
        /// </summary>
        /// <value>The instance.</value>
        public static SystemConfigurationSource Default
        {
            get { return _default; }
        }

        private SystemConfigurationSource()
        {
        }

        /// <summary>
        /// Configures the logging system.
        /// </summary>
        public void Configure()
        {
            DoConfigure();
        }

        partial void DoConfigure();
    }
}
