using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Web.Http;
using MClub.RESTAPI.Models;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;

namespace MClub.RESTAPI.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        [ActionName("GetProfileWithLevelAndCoins")]
        [Authorize]
        public IEnumerable<Profile> GetProfileWithLevelAndCoins()
        {
            var identity = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;
            var email = identity.FindFirst(ClaimTypes.Email).Value;

            var profiles = new List<Profile>();

            using (var clientContext = new ClientContext(ConfigurationManager.AppSettings.Get("mClubSiteUrl")))
            {
                var passWord = new SecureString();
                foreach (char c in ConfigurationManager.AppSettings.Get("password").ToCharArray()) passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("userName"), passWord);
                var contributionTypesList = clientContext.Web.Lists.GetByTitle("Employee Profile");

                var userQuery = new CamlQuery() {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query></View>", "Title", email)
                };

                var userItems = contributionTypesList.GetItems(userQuery);

               // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
               clientContext.Load(userItems);
                clientContext.ExecuteQuery();

                foreach (var item in userItems)
                {
                    var profile = new Profile();
                    profile.Designation= Convert.ToString(item["Designation"]);
                    profile.DisplayName = Convert.ToString(item["Login_x0020_Name"]);
                    var profileUrl = new FieldUrlValue();
                    profileUrl = item["ProfileUrl"] as FieldUrlValue;
                    if (profileUrl != null)
                    {
                        profileUrl = item["ProfileUrl"] as FieldUrlValue;
                        profile.Image = profileUrl.Url;
                    }
                    var level = item["Level"] as FieldLookupValue;

                    if (level != null)
                    {
                        profile.Level = level.LookupValue;
                    }

                    profile.Location= Convert.ToString(item["Location"]);
                    profile.mCoins = Convert.ToString(item["mCoins_x0020_In_x0020_Hand"]);
                    profiles.Add(profile);
                }
            }

            return profiles.ToList();
        }

        [HttpGet]
        [ActionName("GetProfile")]
        public IEnumerable<Profile> GetProfile(string email)
        {
            var profiles = new List<Profile>();

            using (var clientContext = new ClientContext(ConfigurationManager.AppSettings.Get("mClubSiteUrl")))
            {
                var passWord = new SecureString();
                foreach (char c in ConfigurationManager.AppSettings.Get("password").ToCharArray()) passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("userName"), passWord);
                var contributionTypesList = clientContext.Web.Lists.GetByTitle("Employee Profile");

                var userQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query></View>", "Title", email)
                };

                var userItems = contributionTypesList.GetItems(userQuery);

                // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
                clientContext.Load(userItems);
                clientContext.ExecuteQuery();

                foreach (var item in userItems)
                {
                    var profile = new Profile();
                    profile.About = Convert.ToString(item["About"]);
                    profile.Contact = Convert.ToString(item["Contact"]);
                    profile.Designation = Convert.ToString(item["Designation"]);
                    profile.DisplayName = Convert.ToString(item["Login_x0020_Name"]);
                    profile.DownloadedTheApp = Convert.ToBoolean(item["Downloaded_x0020_App"]);
                    profile.Email = Convert.ToString(item["Title"]);
                    var profileUrl = new FieldUrlValue();
                    if (profileUrl != null)
                    {
                        profileUrl = item["ProfileUrl"] as FieldUrlValue;
                        profile.Image = profileUrl.Url;
                    }
                    var level = item["Level"] as FieldLookupValue;

                    if (level != null)
                    {
                        profile.Level = level.LookupValue;
                    }

                    profile.Location = Convert.ToString(item["Location"]);
                    profile.mCoins = Convert.ToString(item["mCoins_x0020_In_x0020_Hand"]);
                    profile.Team = Convert.ToString(item["Team"]);
                    profile.WebLogin = Convert.ToBoolean(item["Web_x0020_Login"]);

                    profiles.Add(profile);
                }
            }

            return profiles.ToList();
        }

        [HttpPost]
        [ActionName("SaveProfile")]
        public IEnumerable<Profile> SaveProfile(string email, bool downloadedApp)
        {

            //        //Retrieve list columns first
            //        var list = clientContext.Web.Lists.GetByTitle(listTitle);
            //        var listFields = list.Fields;
            //        clientContext.Load(listFields, fields => fields.Include(field => field.Title, field => field.InternalName));
            //clientContext.ExecuteQuery();

            //var itemCreateInfo = new ListItemCreationInformation();
            //        var listItem = list.AddItem(itemCreateInfo);
            //        var titleField = listFields.FirstOrDefault(f => f.Title == "Title"); //resolve Field Internal Name by Title
            //        listItem[titleField.InternalName] = "SharePoint";
            //listItem.Update();
            //clientContext.ExecuteQuery();
            var profiles = new List<Profile>();

            using (var clientContext = new ClientContext(ConfigurationManager.AppSettings.Get("mClubSiteUrl")))
            {
                var passWord = new SecureString();
                foreach (char c in ConfigurationManager.AppSettings.Get("password").ToCharArray()) passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("userName"), passWord);
                var contributionTypesList = clientContext.Web.Lists.GetByTitle("Employee Profile");

                var userQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query></View>", "Title", email)
                };

                var userItems = contributionTypesList.GetItems(userQuery);

                // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
                clientContext.Load(userItems);
                clientContext.ExecuteQuery();

                foreach (var item in userItems)
                {
                    var profile = new Profile();
                    profile.About = Convert.ToString(item["About"]);
                    profile.Contact = Convert.ToString(item["Contact"]);
                    profile.Designation = Convert.ToString(item["Designation"]);
                    profile.DisplayName = Convert.ToString(item["Login_x0020_Name"]);
                    profile.DownloadedTheApp = Convert.ToBoolean(item["Downloaded_x0020_App"]);
                    profile.Email = Convert.ToString(item["Title"]);
                    var profileUrl = new FieldUrlValue();
                    if (profileUrl != null)
                    {
                        profileUrl = item["ProfileUrl"] as FieldUrlValue;
                        profile.Image = profileUrl.Url;
                    }
                    var level = item["Level"] as FieldLookupValue;

                    if (level != null)
                    {
                        profile.Level = level.LookupValue;
                    }

                    profile.Location = Convert.ToString(item["Location"]);
                    profile.mCoins = Convert.ToString(item["mCoins_x0020_In_x0020_Hand"]);
                    profile.Team = Convert.ToString(item["Team"]);
                    profile.WebLogin = Convert.ToBoolean(item["Web_x0020_Login"]);

                    profiles.Add(profile);
                }
            }

            return profiles.ToList();
        }
       

    }
}
