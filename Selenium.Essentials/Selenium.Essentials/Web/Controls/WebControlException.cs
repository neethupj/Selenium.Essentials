﻿using OpenQA.Selenium;
using Selenium.Essentials.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Essentials.Web.Controls
{
    public class WebControlException : Exception
    {
        public IWebDriver WebDriver { get; protected set; }

        public BaseControl BaseControl { get; protected set; }

        public IWebElement RawElement { get; protected set; }


        public WebControlException(IWebDriver driver, string message, BaseControl uiControl)
            : base(message)
        {
            WebDriver = driver;
            BaseControl = uiControl;
        }

        public WebControlException(IWebDriver driver, string message, IWebElement uiControl)
            : base(message)
        {
            WebDriver = driver;
            RawElement = uiControl;
        }

        public WebControlException(IWebDriver driver, Exception innerException, BaseControl uiControl)
            : this(driver, innerException, message: null, uiControl: uiControl)
        {
        }

        public WebControlException(IWebDriver driver, Exception innerException, IWebElement uiControl)
            : this(driver, innerException, message: null, uiControl: uiControl)
        {
        }

        public WebControlException(IWebDriver driver, Exception innerException, string message = null, BaseControl uiControl = null)
            : base(message.HasValue() ? message : innerException.Message, innerException)
        {
            WebDriver = driver;
            BaseControl = uiControl;
        }

        public WebControlException(IWebDriver driver, Exception innerException, string message = null, IWebElement uiControl = null)
            : base(message.HasValue() ? message : innerException.Message, innerException)
        {
            WebDriver = driver;
            RawElement = uiControl;
        }

        protected static string CreateGenericDetailsMessage(IWebDriver driver, IWebElement uiControl, string message = null)
        {
            try
            {
                return $"{message}: UI element of type {uiControl?.GetType().Name} "
                    + $"on page: {driver?.Url}";
            }
            catch (Exception)
            {
                return message;
            }
        }

        protected static string CreateGenericDetailsMessage(IWebDriver driver, BaseControl uiControl, string message = null)
        {
            try
            {
                bool? hasDescription = uiControl?.Description.HasValue();

                return $"{message}: UI element of type {uiControl?.GetType().Name} "
                    + (hasDescription.HasValue && hasDescription.Value ? $"'{uiControl.Description}' " : string.Empty)
                    + $"on page: {driver?.Url}";
            }
            catch (Exception)
            {
                return message;
            }
        }
    }
}
