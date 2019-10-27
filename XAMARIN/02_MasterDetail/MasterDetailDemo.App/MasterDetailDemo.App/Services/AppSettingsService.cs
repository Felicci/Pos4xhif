﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using Xamarin.Forms;

// Registriert das Service, sodass die Instanz in der App mit DependencyService.Get<AppSettingsService>()
// abgerufen werden kann. Es wird nur 1 Instanz erstellt, falls keine Option für 
// DependencyFetchTarget.GlobalInstance in DependencyService angegeben wird.
// Vgl. https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/introduction
[assembly: Dependency(typeof(MasterDetailDemo.App.Services.AppSettingsService))]
namespace MasterDetailDemo.App.Services
{
    /// <summary>
    /// Liest Konfigurationseinstellungen aus der Datei appsettings.json. Diese datei muss als
    /// eingebettete Resource in Visual Studio deklariert werden.
    /// https://www.andrewhoefling.com/Blog/Post/xamarin-app-configuration-control-your-app-settings
    /// </summary>
    class AppSettingsService
    {
        // Lädt erst, wenn erstmalig eine Einstellung angefordert wird. Dann wird nicht mehr
        // von der Datei gelesen.
        private Lazy<JsonElement> _settings;
        public string ConfigFile => "appsettings.json";
        public AppSettingsService()
        {
            // Wird erstmalig eine Einstellung angefragt, wird die Datei ausgelesen.
            _settings = new Lazy<JsonElement>(() => ReadAppSettings());
        }

        /// <summary>
        /// Liefert den Wert eines Properties in der appsettings.json.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <exception cref="ServiceException">Propertyname wurde nicht gefunden.</exception>
        /// <exception cref="ServiceException">Zugriff aud appsettings.json nicht möglich.</exception>
        /// <returns>Wert des Properties.</returns>
        public string Get(string propertyName)
        {
            JsonElement settings = _settings.Value;
            try
            {
                return settings.GetProperty(propertyName).GetString();
            }
            catch (Exception e)
            {
                throw new ServiceException($"Cannot read property {propertyName}.", e);
            }
        }
        /// <summary>
        /// Liefert den Wert eines Properties in der appsettings.json oder den angegebenen 
        /// Defaultwert, falls das Property nicht gefunden wird oder gelesen werden kann.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <exception cref="ServiceException">Zugriff aud appsettings.json nicht möglich.</exception>
        /// <returns>Wert des Properties oder defaultValue, wenn nicht ermittelbar.</returns>
        public string GetOrDefault(string propertyName, string defaultValue)
        {
            try
            {
                return Get(propertyName);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Liest die Datei appsettings.json und gibt das JSON Dokument geparst zurück.
        /// </summary>
        /// <returns>JsonElement mit dem Inhalt von appsettings.json</returns>
        private JsonElement ReadAppSettings()
        {
            try
            {
                Assembly assembly = GetType().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{ConfigFile}"))
                {
                    return JsonDocument.Parse(stream).RootElement;
                }
            }
            catch (Exception e)
            {
                throw new ServiceException("Cannot read settings.", e);
            }
        }
    }
}
