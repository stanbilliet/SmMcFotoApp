﻿using CommunityToolkit.Maui;
using MauiIcons.Material;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PicMe.App.ViewModels;
using PicMe.Core.Interfaces.Services;
using PicMe.Core.Services;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PicMe.Infrastructure.Repositories;
using PicMe.Core.Interfaces.Repositories;

namespace PicMe.App
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			var configuration = new ConfigurationBuilder()
				.AddUserSecrets<App>()
				.Build();

			//register repositories
			builder.Services.AddHttpClient<IOneRosterRepository, OneRosterRepository>();
			builder.Services.AddHttpClient<ISoapRepository, SoapRepository>();

			// Register services
			builder.Services.AddSingleton<ISettingsService, SettingsService>();
			builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
			builder.Services.AddSingleton<IJsonService, JsonService>();
			builder.Services.AddSingleton<IPinService, PinService>();
			builder.Services.AddSingleton<IPhotoService, PhotoService>();
			builder.Services.AddSingleton<IStorageService>(serviceProvider =>
			{
#if WINDOWS
				 var jsonService = serviceProvider.GetRequiredService<IJsonService>();
				 var soapRepository = serviceProvider.GetRequiredService<ISoapRepository>();
				 return new PicMe.App.Platforms.Windows.WindowsStorageService(jsonService,soapRepository);
#elif ANDROID
				var jsonService = serviceProvider.GetRequiredService<IJsonService>();
				var soapRepository = serviceProvider.GetRequiredService<ISoapRepository>();
				return new PicMe.Platforms.Android.AndroidStorageService(jsonService, soapRepository);

#else
    throw new PlatformNotSupportedException("The current platform is not supported.");
#endif
			});


			builder.Services.AddSingleton<IConfiguration>(configuration);
			builder.Services.AddSingleton<IStudentService, StudentService>();

			// Register viewmodels
			builder.Services.AddTransient<SettingsViewModel>();
			builder.Services.AddTransient<SyncViewModel>();
			builder.Services.AddTransient<SelectClassViewModel>();
            builder.Services.AddTransient<ClassListViewModel>();
			builder.Services.AddTransient<SelectedClassViewModel>();
			builder.Services.AddTransient<AppShellViewModel>();

			// Register pages
			builder.Services.AddTransient<SettingsPage>();
			builder.Services.AddTransient<ClassListPage>();
			builder.Services.AddTransient<SelectClassPage>();
			builder.Services.AddTransient<SyncPage>();
			builder.Services.AddTransient<SelectedClassPage>();
			builder.Services.AddTransient<AppShell>();
			builder.Services.AddTransient<MainPage>();


            builder
				.UseMaterialMauiIcons()
				.UseMauiApp<App>()
                .UseSkiaSharp()
                .UseMauiCommunityToolkit()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

#if DEBUG
            builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
