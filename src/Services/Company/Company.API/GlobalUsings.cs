﻿global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.Extensions.Hosting;
global using AWC.Shared.Kernel.Utilities;
global using Awc.Services.Company.API.Application.Abstractions.Messaging;
global using Awc.Services.Company.API.Infrastructure;
global using Awc.Services.Company.API.Model.Company;
global using Awc.Services.Company.API.Model.Employees;
global using Awc.Services.Company.API.Model.Person;
global using Awc.Services.Company.API.Services;
global using Awc.Services.Company.API.Services.Queries;
global using Awc.Services.Company.API.ViewModels;
global using Dapper;
global using Mapster;
global using MapsterMapper;
global using MediatR;
global using Serilog;
global using System.Data;
