﻿using CashFlow.Aplication.AutoMapper;
using CashFlow.Aplication.UseCases.Expenses.Delete;
using CashFlow.Aplication.UseCases.Expenses.GetAll;
using CashFlow.Aplication.UseCases.Expenses.GetById;
using CashFlow.Aplication.UseCases.Expenses.Register;
using CashFlow.Aplication.UseCases.Expenses.Reports.Excel;
using CashFlow.Aplication.UseCases.Expenses.Reports.Pdf;
using CashFlow.Aplication.UseCases.Expenses.Update;
using CashFlow.Aplication.UseCases.Login;
using CashFlow.Aplication.UseCases.Users.Register;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Aplication
{
    public static class DependencyInjectionExtension
    {
        public static void AddAplication(this IServiceCollection services)
        {
            AddUseCases(services);
            AddAutoMapper(services);
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
        }

        private static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
            services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
            services.AddScoped<IGetByIdUseCase, GetByIdUseCase>();
            services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
            services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
            services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
            services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        }
    }
}
