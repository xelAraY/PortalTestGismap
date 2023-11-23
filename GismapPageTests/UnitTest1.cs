using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Drawing;
using SeleniumExtras.WaitHelpers;
using GismapPageTests.PageObjects;

namespace GismapPageTests
{
    public class Tests
    {
        private IWebDriver _webDriver;
        private PortalTestGismapPageObject _portalTestGismap;
        private string _webServiceUrl = "https://portaltest.gismap.by/next/";

        private readonly By _map = By.XPath("//*[@id=\"mapCenter_layer0\"]");
        private readonly By _scale = By.XPath("//*[@id=\"mapCenter_layer0\"]/div");
        private readonly By _zoomInFragment = By.XPath("//*[@id=\"mapCenter_layer0_tile_8_0_0\"]");
        private readonly By _zoomOutFragment = By.XPath("//*[@id=\"mapCenter_layer0_tile_9_0_0\"]");

        private readonly By _objectInfo = By.XPath("//*[@id=\"mapCenter_root\"]/div[3]");

        [SetUp]
        public void Setup()
        {
            _webDriver = new ChromeDriver();
            _webDriver.Navigate().GoToUrl(_webServiceUrl);
            _webDriver.Manage().Window.Maximize();

            _portalTestGismap = new PortalTestGismapPageObject(_webDriver);
            _portalTestGismap.OpenData();
        }

        [Test]
        public void MoveMapTest()
        {
            var mapElement = _webDriver.FindElement(_map);
            Point startLocation = mapElement.Location;

            _portalTestGismap.MoveMap(-300, 100);

            Point finalLocation = mapElement.Location;

            Assert.That(finalLocation, Is.Not.EqualTo(startLocation), "Map does not move");
        }

        [Test]
        public void ZoomInMapTest()
        {
            _portalTestGismap.ZoomMap(ZoomOptions.ZoomIn);

            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(_zoomInFragment));

            Assert.Pass("Map scaled succsessfully");
        }

        [Test]
        public void ZoomOutMapTest()
        {
            for (int i = 0; i < 3; i++)
            {
                _portalTestGismap.ZoomMap(ZoomOptions.ZoomIn);
            }
            _portalTestGismap.ZoomMap(ZoomOptions.ZoomOut);

            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(_zoomOutFragment));

            Assert.Pass("Map scaled succsessfully");
        }

        [Test]
        public void LioznenskijRnLayerTest()
        {
            _portalTestGismap.TurnOnLayer(Layers.LioznRn);
            _portalTestGismap.TurnOffLayer();

            Assert.Pass("Layer is working");
        }

        [TestCase(Layers.Land)]
        [TestCase(Layers.LandPlots)]
        [TestCase(Layers.ATD)]
        public void LandLayerTest(Layers layer)
        {
            for(int i = 0; i < 9; i++)
            {
                _portalTestGismap.ZoomMap(ZoomOptions.ZoomIn);
            }

            _portalTestGismap.TurnOnLayer(layer);
            _portalTestGismap.TurnOffLayer();

            Assert.Pass("Layer is working");
        }

        [TestCase(100, 0)]
        [TestCase(100, 20)]
        public void GetObjectInfoTest(int offsetX, int offsetY)
        {           
            _portalTestGismap.MoveMap(offsetX, offsetY);

            for (int i = 0; i < 9; i++)
            {
                _portalTestGismap.ZoomMap(ZoomOptions.ZoomIn);
            }
            Thread.Sleep(1000);
            _portalTestGismap.TurnOnLayer(Layers.Land);

            Actions actions = new Actions(_webDriver);
            actions.ContextClick().MoveByOffset(5, 100).Click().Perform();

            WebDriverWait wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(_objectInfo));

            Assert.Pass("Getting object info is working");
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();  
        }
    }
}