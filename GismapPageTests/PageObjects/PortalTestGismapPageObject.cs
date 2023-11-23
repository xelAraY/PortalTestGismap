using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GismapPageTests.PageObjects
{
    public class PortalTestGismapPageObject
    {
        private IWebDriver _webDriver;
        private WebDriverWait _wait;

        private readonly By _map = By.XPath("//*[@id=\"mapCenter_layer0\"]");
        private readonly By _openData = By.XPath("//*[@id=\"dijit_form_Button_1_label\"]");
        private readonly By _zoomIn = By.XPath("//*[@id=\"mapCenter_zoom_slider\"]/div[1]");
        private readonly By _zoomOut = By.XPath("//*[@id=\"mapCenter_zoom_slider\"]/div[2]");
        private readonly By _layers = By.XPath("//*[@id=\"layerControl_parent_titleBarNode\"]/div");

        private readonly By _activateLioznenskijRnLayer = By.XPath("//*[@id=\"uniqName_26_1\"]/table/tbody/tr/td[2]/i");
        private readonly By _lioznRnLayer = By.XPath("//*[@id=\"mapCenter_Lioznenskij_rn_2018\"]");

        private readonly By _activateLandLayer = By.XPath("//*[@id=\"uniqName_26_2\"]/table/tbody/tr/td[2]/i");
        private readonly By _landLayer = By.XPath("//*[@id=\"mapCenter_Land_Minsk_public\"]");

        private readonly By _activateLandPlotsLayer = By.XPath("//*[@id=\"uniqName_26_3\"]/table/tbody/tr/td[2]/i");
        private readonly By _landPlotsLayer = By.XPath("//*[@id=\"mapCenter_Uchastki_Minsk_public\"]");

        private readonly By _activateATDLayer = By.XPath("//*[@id=\"uniqName_26_4\"]/table/tbody/tr/td[2]/i");
        private readonly By _ATDLayer = By.XPath("//*[@id=\"mapCenter_ATE_Minsk_public\"]");

        private By? _activateLayer = null;
        private By? _layerComponent = null;

        public PortalTestGismapPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
        }

        public void MoveMap(int x, int y)
        {
            var mapElement = _webDriver.FindElement(_map);
            Point startLocation = mapElement.Location;
            Actions actions = new Actions(_webDriver);
            actions.MoveToElement(mapElement).ClickAndHold().MoveByOffset(x, y).Release().Perform();

        }

        public void ZoomMap(ZoomOptions zoomOption)
        {
            IWebElement zoomButton;
            if (zoomOption == ZoomOptions.ZoomIn)
            {
                zoomButton = _webDriver.FindElement(_zoomIn);
            }
            else
            {
                zoomButton = _webDriver.FindElement(_zoomOut);
            }
            
            zoomButton.Click();
        }

        public void OpenData()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(_openData));
            var openDataButton = _webDriver.FindElement(_openData);
            openDataButton.Click();
            _wait.Until(ExpectedConditions.ElementIsVisible(_map));
        }

        public void OpenLayersMenu()
        {
            var layersMenu = _webDriver.FindElement(_layers);
            layersMenu.Click();
            //_wait.Until(ExpectedConditions.ElementIsVisible(layerButton));
        }

        public void TurnOnLayer(Layers layer)
        {
            OpenLayersMenu();

            switch (layer)
            {
                case Layers.LioznRn:                   
                    _activateLayer = _activateLioznenskijRnLayer;
                    _layerComponent = _lioznRnLayer;
                    
                    break;
                case Layers.Land:
                    _activateLayer = _activateLandLayer;
                    _layerComponent = _landLayer;
                    break;
                case Layers.LandPlots:
                    _activateLayer = _activateLandPlotsLayer;
                    _layerComponent = _landPlotsLayer;
                    break;
                case Layers.ATD:
                    _activateLayer = _activateATDLayer;
                    _layerComponent = _ATDLayer;
                    break;
            }
            _wait.Until(ExpectedConditions.ElementIsVisible(_activateLayer));

            var activateLayerButton = _webDriver.FindElement(_activateLayer);
            activateLayerButton.Click();
            _wait.Until(ExpectedConditions.ElementIsVisible(_layerComponent));
        }

        public void TurnOffLayer()
        {
            var activateLayerButton = _webDriver.FindElement(_activateLayer);
            activateLayerButton.Click();
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_layerComponent));
        }
    }
}
