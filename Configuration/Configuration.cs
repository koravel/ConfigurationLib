using Microsoft.Extensions.Configuration;

namespace ConfigurationLib
{
    public class Configuration
    {
        private IConfiguration _configuration;
        private IConfigurationBuilder _defaultConfigurationBuilder;

        public Configuration()
        {
            _defaultConfigurationBuilder = new ConfigurationBuilder();
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public Configuration(Action<IConfigurationBuilder> action)
        {
            _defaultConfigurationBuilder = new ConfigurationBuilder();
            action.Invoke(_defaultConfigurationBuilder);
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public Configuration(IEnumerable<IConfigurationSource> configurationSources)
        {
            _defaultConfigurationBuilder = new ConfigurationBuilder();

            foreach (var source in configurationSources)
            {
                _defaultConfigurationBuilder.Sources.Add(source);
            }
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public Configuration(Action<IConfigurationBuilder> action, IEnumerable<IConfigurationSource> configurationSources) {
            _defaultConfigurationBuilder = new ConfigurationBuilder();

            action.Invoke(_defaultConfigurationBuilder);
            foreach (var source in configurationSources)
            {
                _defaultConfigurationBuilder.Sources.Add(source);
            }
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public void AddConfigurationSource(IConfigurationSource configurationSource) {
            _defaultConfigurationBuilder = new ConfigurationBuilder();

            _defaultConfigurationBuilder.AddConfiguration(_configuration);
            _defaultConfigurationBuilder.Sources.Add(configurationSource);
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public void AddConfigurationSources(IEnumerable<IConfigurationSource> configurationSources)
        {
            _defaultConfigurationBuilder = new ConfigurationBuilder();

            _defaultConfigurationBuilder.AddConfiguration(_configuration);
            foreach (var source in configurationSources)
            {
                _defaultConfigurationBuilder.Sources.Add(source);
            }
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public void ResetConfiguration() {
            _configuration = _defaultConfigurationBuilder.Build();
        }

        public T GetValueOrThrow<T>(string key) where T : IParsable<T>
        {
            if (!T.TryParse(_configuration.GetSection(key).Value, null, out T result)) throw new Exception();
            return result;
        }
    }
}
