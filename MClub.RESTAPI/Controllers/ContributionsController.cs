using MClub.RESTAPI.Models;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MClub.RESTAPI.Controllers
{
    public class ContributionsController : ApiController
    {
        [HttpGet]
        [ActionName("GetTypes")]
        public IEnumerable<ContributionTypes> GetTypes()
        {
            var contributionTypes = new List<ContributionTypes>();

            using (var clientContext = new ClientContext(ConfigurationManager.AppSettings.Get("mClubSiteUrl")))
            {
                var passWord = new SecureString();
                foreach (char c in ConfigurationManager.AppSettings.Get("password").ToCharArray()) passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("userName"), passWord);
                var contributionTypesList = clientContext.Web.Lists.GetByTitle("Contribution Configuration");
                var query = CamlQuery.CreateAllItemsQuery(100);
                var items = contributionTypesList.GetItems(query);

                // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
                clientContext.Load(items);
                clientContext.ExecuteQuery();
                foreach (var item in items)
                {
                    var contributionType = new ContributionTypes();
                    contributionType.Id = Convert.ToInt32(item["ID"]);
                    contributionType.Title = Convert.ToString(item["Title"]);

                    var contributionUrl = item["Image"] as FieldUrlValue;

                    if (contributionUrl != null)
                    {
                        contributionType.Image = contributionUrl.Url;
                    }

                    contributionType.SchemaUrl= Convert.ToString(item["SchemaUrl"]);

                    contributionTypes.Add(contributionType);

                }
            }

            return contributionTypes.ToList();
        }

        [HttpGet]
        [ActionName("GetUserContributionsCountForUser")]
        public IEnumerable<UserContributions> GetUserContributionsCountForUser(string email)
        {
            var userContributions = new List<UserContributions>();

            using (var clientContext = new ClientContext(ConfigurationManager.AppSettings.Get("mClubSiteUrl")))
            {
                var passWord = new SecureString();
                foreach (char c in ConfigurationManager.AppSettings.Get("password").ToCharArray()) passWord.AppendChar(c);

                clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("userName"), passWord);
                var ideaContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Idea");
                var internalProcessInnovationContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Internal Process Innovation");
                var participationInChallengesContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Participation in Challenges");
                var patentContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Patent");
                var poCContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - PoC");
                var projectInnovationContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Project Innovation");
                var speakingContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Speaking");
                var thoughtLeadershipContributionTypesList = clientContext.Web.Lists.GetByTitle("User Contributions - Thought Leadership");

                var ideaQuery = new CamlQuery()
                  {
                     ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                  };

                var ideaItems = ideaContributionTypesList.GetItems(ideaQuery);

                var internalQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var internalItems = internalProcessInnovationContributionTypesList.GetItems(internalQuery);

                var participationInChallengesQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var participationInChallengesItems = participationInChallengesContributionTypesList.GetItems(participationInChallengesQuery);

                var patentQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var patentItems = patentContributionTypesList.GetItems(patentQuery);

                var poCQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var poCItems = poCContributionTypesList.GetItems(poCQuery);

                var projectInnovationQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var projectInnovationItems = projectInnovationContributionTypesList.GetItems(projectInnovationQuery);

                var speakingQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var speakingItems = speakingContributionTypesList.GetItems(speakingQuery);

                var thoughtLeadershipQuery = new CamlQuery()
                {
                    ViewXml = string.Format("<View><Query><Where><Eq><FieldRef Name='{0}' /><Value Type='Text'>{1}</Value></Eq></Where></Query><OrderBy><FieldRef Name = 'Modified' Ascending = 'FALSE' /></OrderBy></View>", "Idea_x0020_Author_x0020_Email", email)
                };

                var thoughtLeadershipItems = thoughtLeadershipContributionTypesList.GetItems(thoughtLeadershipQuery);

                // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
                clientContext.Load(ideaItems);
                clientContext.Load(internalItems);
                clientContext.Load(participationInChallengesItems);
                clientContext.Load(patentItems);
                clientContext.Load(poCItems);
                clientContext.Load(projectInnovationItems);
                clientContext.Load(speakingItems);
                clientContext.Load(thoughtLeadershipItems);

                clientContext.ExecuteQuery();

                if (ideaItems.Count > 0)
                {
                   var ideaContribution = new UserContributions();
                    ideaContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    ideaContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        ideaContribution.ID = ideaType.LookupId;
                        ideaContribution.Title = ideaType.LookupValue;
                        ideaContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(ideaContribution);
                }

                if (internalItems.Count > 0)
                {
                    var internalContribution = new UserContributions();
                    internalContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    internalContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        internalContribution.ID = ideaType.LookupId;
                        internalContribution.Title = ideaType.LookupValue;
                        internalContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(internalContribution);
                }

                if (participationInChallengesItems.Count > 0)
                {
                    var participationContribution = new UserContributions();
                    participationContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    participationContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        participationContribution.ID = ideaType.LookupId;
                        participationContribution.Title = ideaType.LookupValue;
                        participationContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(participationContribution);
                }

                if (patentItems.Count > 0)
                {
                    var patentContribution = new UserContributions();
                    patentContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    patentContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        patentContribution.ID = ideaType.LookupId;
                        patentContribution.Title = ideaType.LookupValue;
                        patentContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(patentContribution);
                }

                if (poCItems.Count > 0)
                {
                    var poCContribution = new UserContributions();
                    poCContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    poCContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        poCContribution.ID = ideaType.LookupId;
                        poCContribution.Title = ideaType.LookupValue;
                        poCContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(poCContribution);
                }

                if (projectInnovationItems.Count > 0)
                {
                    var projectContribution = new UserContributions();
                    projectContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    projectContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        projectContribution.ID = ideaType.LookupId;
                        projectContribution.Title = ideaType.LookupValue;
                        projectContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(projectContribution);
                }

                if (speakingItems.Count > 0)
                {
                    var speakingContribution = new UserContributions();
                    speakingContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    speakingContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        speakingContribution.ID = ideaType.LookupId;
                        speakingContribution.Title = ideaType.LookupValue;
                        speakingContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(speakingContribution);

                }

                if (thoughtLeadershipItems.Count > 0)
                {
                    var thoughtLeadershipContribution = new UserContributions();
                    thoughtLeadershipContribution.Count = ideaItems.Count;
                    var ideaType = ideaItems[0]["Contribution_x0020_Bucket"] as FieldLookupValue;
                    var latestContribution = Convert.ToString(ideaItems[0]["Title"]);
                    thoughtLeadershipContribution.LatestContribution = latestContribution;

                    if (ideaType != null)
                    {
                        thoughtLeadershipContribution.ID = ideaType.LookupId;
                        thoughtLeadershipContribution.Title = ideaType.LookupValue;
                        thoughtLeadershipContribution.Image = GetImageOfTheContribution(ideaType.LookupId, clientContext);
                    }

                    userContributions.Add(thoughtLeadershipContribution);
                }
            }

            return userContributions.ToList();
        }

        private string GetImageOfTheContribution(int id, ClientContext clientContext)
        {
            var imageUrl = string.Empty;

            var passWord = new SecureString();
            foreach (char c in ConfigurationManager.AppSettings.Get("password").ToCharArray()) passWord.AppendChar(c);

            clientContext.Credentials = new SharePointOnlineCredentials(ConfigurationManager.AppSettings.Get("userName"), passWord);
            var contributionTypesList = clientContext.Web.Lists.GetByTitle("Contribution Configuration");

            var contributionItem = contributionTypesList.GetItemById(id);

            // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
            clientContext.Load(contributionItem);

            clientContext.ExecuteQuery();

            if (contributionItem != null)
            {
                var imageItem = contributionItem["Image"] as FieldUrlValue;
                imageUrl = imageItem.Url;
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = "https://marlabsinc.sharepoint.com/sites/mCoins/SiteCollectionImages/Contribution%20Types/Contribution_Idea.png";
            }
            return imageUrl;

        }
    }
}