using System;
using System.Linq;
using Microsoft.SharePoint;
using System.IO;
using System.Net;
using System.ComponentModel;
using Tarento.Common.Utilities;
using Tarento.Common.Utilities.Logging;
using Tarento.Common.Utilities.Exceptions;

namespace SharepointExtensionProject
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                using (SPSite site = new SPSite("http://weshare.sedwdev.local"))
                {
                    using (SPWeb web = site.RootWeb)
                    {

                        SPList list = null;
                
                        
                        SPField field = null;
                        if (web.ListExists("Test", out list))
                        {
                            if (list.FieldExists("Titssle", out field))
                            {
                                //do something with the field
                            }
                            else
                            {

                                LogHelper.SendTrace("This is for test", 100, "Webpart");

                                //No field exist and log it in error log
                            }
                        }
                        else
                        {
                            //No List exist in the site
                        }
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}

