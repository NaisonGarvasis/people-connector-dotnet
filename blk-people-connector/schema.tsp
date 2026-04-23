import "@wictorwilen/cocogen";
using coco;

// People connectors may require preview features for beta-only labels. Use --use-preview-features when cocogen reports a Graph beta requirement.
@coco.profileSource({
  webUrl: "https://contoso.com/people",
  displayName: "BLK People Connector",
  priority: "first"
})
@coco.connection({
  contentCategory: "people",
  name: "BLK People Connector",
  connectionId: "BLKPeopleConnector",
  connectionDescription: "People connector that enriches employee profiles with education, certifications, job history, skills, and biographical information. It enables improved people discovery and insights across Microsoft 365 and Copilot experience"
})
@coco.item()
model BLKPersonProfile {
  
  // Email
  @coco.id
  @coco.label("personAccount")
  @coco.source("primaryWorkEmail","userPrincipalName")
  account: string;

  // Skills
  @coco.label("personSkills")
  @coco.source("skillData.skills", "displayName")
  skills: string[];
  
  /// Certifications
  @coco.label("personCertifications")
  @coco.source("certificationData[*].certification", "displayName")
  @coco.source("certificationData[*].issuer", "issuingCompany")
  @coco.source("certificationData[*].specialties", "description")
  @coco.source("certificationData[*].certificationNumber", "certificationId")
  certifications: string[];

  /// Education 
  @coco.label("personEducationalActivities")
  // Program
  @coco.source("educationData[*].degree", "program.displayName")
  @coco.source("educationData[*].fieldofStudy", "program.fieldsOfStudy")
  @coco.source("educationData[*].gpa", "program.grade")
  @coco.source("educationData[*].yearDegreeReceived", "completionMonthYear")
  // Institution
  @coco.source("educationData[*].school", "institution.displayName")
  @coco.source("educationData[*].schoolStateProvince", "institution.location.state")
  @coco.source("educationData[*].schoolCountry", "institution.location.countryOrRegion")
  educationalActivities : string[];

  /// Biography
  @coco.label("personNote")
  @coco.source("biography", "detail")
  note: string;

  /// Job History 
  @coco.search({ searchable: true, retrievable: true })
  @coco.source("jobHistoryData[*].organization", "detail.company.displayName")
  @coco.source("jobHistoryData[*].jobTitle", "detail.jobTitle")
  @coco.source("jobHistoryData[*].businessTitle", "detail.secondaryJobTitle")
  @coco.source("jobHistoryData[*].position", "detail.description")
  @coco.source("jobHistoryData[*].location", "detail.company.address.street")
  jobHistory: string[];

}
