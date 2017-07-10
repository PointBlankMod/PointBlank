using System.Collections.Generic;
using PointBlank.API;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;

namespace TestPlugin
{
    public class Test : Plugin
    {
        public override TranslationList Translations => new TranslationList()
        {
            { "test", "This is a test" }
        };

        public override ConfigurationList Configurations => new ConfigurationList()
        {
            { "test", "hello" },
            { "lolz", "smth" },
            { "testList", new List<object>()
            {
                "test string 1",
                69,
                "test string 2"
            } },
            { "adding", "just a test" }
        };

        public override string Version => "1.0.0.1";

        public override string VersionURL => "http://pastebin.com/raw/uYUr6pPN";

        public override void Load()
        {
            Logging.Log("Hello from test plugin load!");
            Logging.Log("Translation test: " + Test.Instance.Translations["test"]); // Call the translation
            Logging.Log("Configuration test: " + (string)Test.Instance.Configurations["test"]); // Call the test configuration
            Logging.Log("Configuration test 2: " + (string)((List<object>)Test.Instance.Configurations["testList"])[0]);
        }

        public override void Unload()
        {
            Logging.Log("Hello from test plugin unload!");
        }
    }
}
