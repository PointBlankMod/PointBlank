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
                    { "test", "hello" }
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

            ServerEvents.OnStructureCreated += new ServerEvents.StructureStatusHandler(TestEvent1);
            ServerEvents.OnStructureRemoved += new ServerEvents.StructureStatusHandler(TestEvent2);
        }

        public override void Unload()
        {
            Logging.Log("Hello from test plugin unload!");
        }

        private void TestEvent1(UnturnedStructure test)
        {
            Logging.Log("Structure Created!");
        }

        private void TestEvent2(UnturnedStructure test)
        {
            Logging.Log("Structure Removed!");
        }
    }
}
