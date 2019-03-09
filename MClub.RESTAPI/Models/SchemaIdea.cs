using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MClub.RESTAPI.Models
{
    public class SchemaIdea
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Goal> Goal_Impact { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public string NextSteps { get; set; }
        public string AttachmentDescription { get; set; }
        public string Url { get; set; }
        public IEnumerable<ReviewSubStatus> ReviewSubStatus { get; set; }
        public string EmployeeRemarks { get; set; }
        public string ReviewerComments { get; set; }
        public IEnumerable<AdditionalInformationType> AdditionalInformationType { get; set; }
        public string AdditionalInformationRequest { get; set; }
        public string RejectionComments { get; set; }
        public IEnumerable<Location> CouncilLocation { get; set; } 

        public IEnumerable<ContributionStatus> ContributionStatus { get; set; }

        public IEnumerable<ContributionBucket> ContributionType { get; set; }

        public DateTime TentativeClosureDate { get; set; }

        public string IdeaAuthorEmail { get; set; }
    }
    public class Goal
    {
        public string Title { get; set; }
    }
    public class Category
    {
        public string Title { get; set; }
    }

    public class ReviewSubStatus
    {
        public string Title { get; set; }
    }
    public class AdditionalInformationType
    {
        public string Title { get; set; }
    }
    public class Location
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class ContributionStatus
    {
        public string Title { get; set; }
    }

    public class ContributionBucket
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}