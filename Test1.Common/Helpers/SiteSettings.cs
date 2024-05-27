using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Xml.Linq;

namespace Test1.Common.Helpers
{
    public class SiteSettings
    {
        private readonly IConfiguration _configuration;

        public SiteSettings(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            string userFileName = _configuration["Xml:UserFileName"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(userFileName))
            {
                throw new ArgumentException("UserFileName is not configured in appsettings.json");
            }

            FilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), userFileName);

            if (!File.Exists(FilePath))
            {
                new XDocument(new XElement("Users")).Save(FilePath);
            }
        }

        public string FilePath { get; set; }
    }
}