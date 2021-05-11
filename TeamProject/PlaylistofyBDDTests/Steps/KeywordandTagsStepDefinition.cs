using System;
using TechTalk.SpecFlow;

namespace PlaylistofyBDDTests.Steps
{
    [Binding]
    public class KeywordandTagsStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;
        public KeywordandTagsStepDefinition(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"a playlist that is being created by a registered and logged in user")]
        public void GivenAPlaylistThatIsBeingCreatedByARegisteredAndLoggedInUser()
        {
            _scenarioContext.Pending();
        }
        
        [Given(@"the Tags form is filled in")]
        public void GivenTheTagsFormIsFilledIn()
        {
            _scenarioContext.Pending();
        }

        [When(@"the playlist creation form is submitted")]
        public void WhenThePlaylistCreationFormIsSubmitted()
        {
            _scenarioContext.Pending();
        }

        [Then(@"that playlist should be associated with the tags entered")]
        public void ThenThatPlaylistShouldBeAssociatedWithTheTagsEntered()
        {
            _scenarioContext.Pending();
        }

        [Given(@"a playlist that is already in the database")]
        public void GivenAPlaylistThatIsAlreadyInTheDatabase()
        {
            _scenarioContext.Pending();
        }

        [Given(@"being viewed by the creator of the playlist on the usersPlaylist page")]
        public void GivenBeingViewedByTheCreatorOfThePlaylistOnTheUsersPlaylistPage()
        {
            _scenarioContext.Pending();
        }

        [When(@"the edit button is pressed for that playlist")]
        public void WhenTheEditButtonIsPressedForThatPlaylist()
        {
            _scenarioContext.Pending();
        }

        [Then(@"there will be an area on the edit page to enter new tags and delete existing tags for that playlist")]
        public void ThenThereWillBeAnAreaOnTheEditPageToEnterNewTagsAndDeleteExistingTagsForThatPlaylist()
        {
            _scenarioContext.Pending();
        }

    }
}
