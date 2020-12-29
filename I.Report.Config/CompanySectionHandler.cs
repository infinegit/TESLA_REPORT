using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I.Report.Config
{
    public class CompanySectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            System.Collections.IDictionary configs;
            DictionarySectionHandler baseHandler = new DictionarySectionHandler();
            configs = (System.Collections.IDictionary)baseHandler.Create(parent, configContext, section);
            return configs;
        }
    }
}
