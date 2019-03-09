using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MClub.RESTAPI.Models;

namespace MClub.RESTAPI.Controllers
{
    public class SchemaController : ApiController
    {
        [HttpGet]
        [ActionName("GetSchemaforIdea")]
        public IEnumerable<SchemaIdea> GetSchemaforIdea()
        {
            var schemaforIdea = new SchemaIdea();

            var goals = new List<Goal>();
            
            goals.Add(new Models.Goal { Title = "Goal1" });
            goals.Add(new Models.Goal { Title = "Goal2" });
            schemaforIdea.Goal_Impact = goals;

            var categories = new List<Category>();
            categories.Add(new Models.Category { Title = "Category1" });
            categories.Add(new Models.Category { Title = "Category2" });
            schemaforIdea.Category = categories;

            var additionaInformationTypes = new List<AdditionalInformationType>();
            additionaInformationTypes.Add(new Models.AdditionalInformationType { Title = "Additional Info1" });
            additionaInformationTypes.Add(new Models.AdditionalInformationType { Title = "Additional Info2" });
            schemaforIdea.AdditionalInformationType = additionaInformationTypes;

            var reviewSubStatus = new List<ReviewSubStatus>();
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Not Started" });
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Review Initiated" });
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Additional Information Requested" });
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Additional Details Submitted" });
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Discussion / Demo Requested" });
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Discussion / Demo Completed" });
            reviewSubStatus.Add(new Models.ReviewSubStatus { Title = "Review Completed" });
            schemaforIdea.ReviewSubStatus = reviewSubStatus;

            var locations = new List<Location>();
            locations.Add(new Location { Id = 1, Title = "Kochi" });
            schemaforIdea.CouncilLocation = locations;

            var contributionStatus = new List<ContributionStatus>();
            contributionStatus.Add(new Models.ContributionStatus { Title = "Draft" });
            contributionStatus.Add(new Models.ContributionStatus { Title = "Submitted for Review" });
            contributionStatus.Add(new Models.ContributionStatus { Title = "Recalled" });
            contributionStatus.Add(new Models.ContributionStatus { Title = "Review In - progress" });
            contributionStatus.Add(new Models.ContributionStatus { Title = "Contribution Approved" });
            contributionStatus.Add(new Models.ContributionStatus { Title = "Contribution Rejected" });
            schemaforIdea.ContributionStatus = contributionStatus;

            var contributionTypes = new List<ContributionBucket>();
            contributionTypes.Add(new ContributionBucket { Id = 2, Title = "Idea" });
            contributionTypes.Add(new ContributionBucket { Id = 3, Title = "Patent" });
            contributionTypes.Add(new ContributionBucket { Id = 4, Title = "PoC" });
            contributionTypes.Add(new ContributionBucket { Id = 5, Title = "Participation in Challenges" });
            contributionTypes.Add(new ContributionBucket { Id = 6, Title = "Thought Leadership" });
            contributionTypes.Add(new ContributionBucket { Id = 7, Title = "Project Innovation" });
            contributionTypes.Add(new ContributionBucket { Id = 8, Title = "Speaking" });
            contributionTypes.Add(new ContributionBucket { Id = 9, Title = "Internal Process Innovation" });

            schemaforIdea.ContributionType = contributionTypes;

            var listSchema = new List<SchemaIdea>();

            listSchema.Add(schemaforIdea);

            return listSchema.ToList();
        }
    }
}
