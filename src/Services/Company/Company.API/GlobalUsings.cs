﻿global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using AWC.Shared.Kernel.Utilities;
global using Awc.BuildingBlocks.EventBus;
global using Awc.BuildingBlocks.EventBus.EventBus.Abstractions;
global using Awc.BuildingBlocks.EventBus.EventBus.Events;
global using Awc.Services.Company.API;
global using Awc.Services.Company.API.Infrastructure;
global using Awc.Services.Company.API.Infrastructure.EntityConfigurations;
// global using Awc.Dapr.Services.Company.API.IntegrationEvents.EventHandling;
// global using Awc.Dapr.Services.Company.API.IntegrationEvents.Events;
global using Awc.Services.Company.API.Model.Company;
global using Awc.Services.Company.API.Model.Employees;
global using Awc.Services.Company.API.Model.Person;
// global using Awc.Services.Company.API.ViewModel;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Hosting;
global using Microsoft.OpenApi.Models;
global using Mapster;
global using MapsterMapper;
global using Polly;
global using System.Net;
global using Serilog;
global using System.Data;
global using Microsoft.Data.SqlClient;