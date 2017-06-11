using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;
using PointBlank.API.Unturned.Server;
using PointBlank.API.Unturned.Structure;
using SDG.Unturned;

namespace TestPlugin
{
    public class Test : Plugin
    {
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    { "test", "This is a test" }
                };
            }
        }

        public override ConfigurationList DefaultConfigurations
        {
            get
            {
                return new ConfigurationList()
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
            }
        }

        public override string Version { get { return "1.0.0.1"; } }

        public override string VersionURL { get { return "http://pastebin.com/raw/uYUr6pPN"; } }

        public override void Load()
        {
            Logging.Log("Hello from test plugin load!");
            Logging.Log("Translation test: " + Test.Translations["test"]); // Call the translation
            Logging.Log("Configuration test: " + (string)Test.Configurations["test"]); // Call the test configuration
            Logging.Log("Configuration test 2: " + (string)((List<object>)Test.Configurations["testList"])[0]);
        }

        public override void Unload()
        {
            Logging.Log("Hello from test plugin unload!");
        }
    }
}
