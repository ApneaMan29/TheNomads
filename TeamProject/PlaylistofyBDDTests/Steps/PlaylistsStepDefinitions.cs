using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using PlaylistofyBDDTests.Hooks;
using TechTalk.SpecFlow;
using NUnit;

namespace PlaylistofyBDDTests.Steps
{
    [Binding]
    public class PlaylistSteps
    {
        private IWebDriver _driver;
        private string _hostBaseName = @"https://playlistofy.azurewebsites.net/";

        public PlaylistSteps(IWebDriver driver) => _driver = driver;

        [Given(@"a user is on the homepage")]
        public void GivenAUserIsOnTheHomepage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5001");
        }
        
        [Then(@"their playlist should be on the homepage")]
        public void ThenTheirPlaylistShouldBeOnTheHomepage()
        {
            var d = _driver.FindElement(By.Id("myPlaylists"));
            Assert.That(d, Is.Not.Null);
        }
    }
}
